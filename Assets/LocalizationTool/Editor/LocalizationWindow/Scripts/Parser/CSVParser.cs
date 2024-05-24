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

        public static Dictionary<string, List<TranslatePairData>> Execute(ConfigData _configData)
        {
            Dictionary<string, List<TranslatePairData>> translateMap = null;

            // using (TextReader reader = new StringReader(sourcePath))
            using (var reader = new StreamReader(_configData.CsvPath))
            using (var csv = new CsvReader(reader, CSV_CONFIG))
            {
                bool isRead = csv.Read();

                List<string> ids = GetIds(csv);
                translateMap = GetTranslateMap(csv, ids);
            }

            return translateMap;
        }

        private static List<string> GetIds(CsvReader csv)
        {
            bool isRead = csv.Read();

            //Debug.Log($"===== csv.ColumnCount:{csv.ColumnCount}");

            /*if (csv.ReadHeader())
            {
                string[] headers = csv.HeaderRecord;
                Debug.Log($"===== headers.Length:{headers.Length}");
                for (int i = 1; i < headers.Length; i++)
                {
                    string header = headers[i];
                    Debug.Log($"headers[{i}]:{header}");
                    ids.Add(header);
                }
            }*/

            List<string> ids = new List<string>();

            // Collect id
            for (int colIndex = 0; colIndex < csv.ColumnCount; colIndex++)
            {
                if (csv.TryGetField(colIndex, out string data))
                {
                    //Debug.Log($"[{colIndex}][{data}]");
                    ids.Add(data);
                }
            }

            return ids;
        }

        private static Dictionary<string, List<TranslatePairData>> GetTranslateMap(CsvReader csv, List<string> ids)
        {
            bool isRead = csv.Read();

            int idCount = ids.Count;
            Debug.Log($"===== idCount:{idCount}");

            var langMap = new Dictionary<string, List<TranslatePairData>>();

            // en_us, zh_tw, ja_jp
            while (isRead)
            {
                string langId = csv.GetField<string>(0);

                if (LANGUAGE_HASHSET.Contains(langId))
                {
                    Debug.Log($"===== langId:{langId} ColumnCount:{csv.ColumnCount}");

                    if (!langMap.TryGetValue(langId, out List<TranslatePairData> translateList))
                    {
                        translateList = new List<TranslatePairData>();
                        langMap.Add(langId, translateList);
                    }

                    for (int colIndex = 1; colIndex < idCount; colIndex++)
                    {
                        if (csv.TryGetField(colIndex, out string value))
                        {
                            string id = ids[colIndex];
                            //Debug.Log($"[{colIndex}] id:{id} value:{value}");

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