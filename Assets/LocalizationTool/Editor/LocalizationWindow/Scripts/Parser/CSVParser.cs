using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using UnityEngine;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace Live17.LocalizationEditor
{
    public static class CSVParser
    {
        private const string UNITY_TAG = "Unity";

        private static readonly CsvConfiguration CSV_CONFIG = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            //PrepareHeaderForMatch = args => args.Header.ToLower(),
            HasHeaderRecord = true,
        };

        private static readonly HashSet<string> LANGUAGE_HASHSET = new HashSet<string>()
        {
            "en_us",
            "zh_tw",
            "ja_jp",
        };

        public static Dictionary<string, List<TranslatePairData>> Execute(ConfigData configData)
        {
            Dictionary<string, List<TranslatePairData>> translateMap = null;

            // using (TextReader reader = new StringReader(sourcePath))
            using (var reader = new StreamReader(configData.CsvPath))
            using (var csv = new CsvReader(reader, CSV_CONFIG))
            {
                bool isRead = csv.Read();
                List<string> tags = GetTags(csv);
                List<string> ids = GetIds(csv, configData);
                translateMap = GetTranslateMap(csv, tags, ids, configData);
            }

            return translateMap;
        }

        private static List<string> GetTags(CsvReader csv)
        {
            List<string> tags = new List<string>();

            if (csv.ReadHeader())
            {
                string[] headers = csv.HeaderRecord;
                // Debug.Log($"headers.Length:{headers.Length}");
                for (int colIndex = 0; colIndex < headers.Length; colIndex++)
                {
                    string header = headers[colIndex];
                    // Debug.Log($"headers[{colIndex}]:{header}");
                    tags.Add(header);
                }
            }

            return tags;
        }

        private static List<string> GetIds(CsvReader csv, ConfigData _configData)
        {
            bool isRead = csv.Read();

            List<string> ids = new List<string>();

            // Collect id
            for (int colIndex = 0; colIndex < csv.ColumnCount; colIndex++)
            {
                if (csv.TryGetField(colIndex, out string data))
                {
                    // Debug.Log($"[{colIndex}][{data}]");
                    if (_configData.IsRemovePrefixWord)
                    {
                        data = StringUtil.RemovePrefix(data, $"{_configData.PrefixWord}.");
                    }

                    ids.Add(data);
                }
            }

            return ids;
        }

        private static Dictionary<string, List<TranslatePairData>> GetTranslateMap(CsvReader csv, List<string> tags, List<string> ids, ConfigData configData)
        {
            // Debug.Log($"tags.Count:{tags.Count}");
            // Debug.Log($"ids.Count:{ids.Count}");

            bool isRead = csv.Read();
            int count = Mathf.Min(tags.Count, ids.Count);
            // Debug.Log($"count:{count}");
            var langMap = new Dictionary<string, List<TranslatePairData>>();

            // en_us, zh_tw, ja_jp
            while (isRead)
            {
                string langId = csv.GetField<string>(0);

                if (LANGUAGE_HASHSET.Contains(langId))
                {
                    if (!langMap.TryGetValue(langId, out List<TranslatePairData> translateList))
                    {
                        translateList = new List<TranslatePairData>();
                        langMap.Add(langId, translateList);
                    }

                    for (int colIndex = 1; colIndex < count; colIndex++)
                    {
                        if (csv.TryGetField(colIndex, out string value))
                        {
                            string tag = tags[colIndex];
                            string id = ids[colIndex];
                            // Debug.Log($"[{colIndex}] tag:{tag} id:{id} value:{value}");

                            if (configData.IsOnlyUnityTagToggle && !tag.Equals(UNITY_TAG))
                            {
                                continue;
                            }

                            if (translateList.Exists(data => data.Key == id))
                            {
                                Debug.LogWarning($"Duplicate key, id:{id} value:{value}");
                            }
                            else if (string.IsNullOrEmpty(value)) // TODO: 看有沒有辦法忽略隱藏欄位
                            {
                                Debug.LogWarning($"Empty value, id:{id} value:{value}");
                            }
                            else
                            {
                                translateList.Add(new TranslatePairData(id, value));
                            }
                        }
                    }
                }

                isRead = csv.Read();
            }

            return langMap;
        }
    }
}