using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;
using UnityEngine;
using SimpleJSON;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Live17.LocalizationEditor
{
    public static class JsonOutput
    {
        public static void Execute(Dictionary<string, List<TranslatePairData>> langMap, string outputPath, bool isFormatJson)
        {
            JSONObject rootObj = new JSONObject();
            JSONObject localesObj = new JSONObject();
            JSONObject enObj = new JSONObject();
            JSONObject zhObj = new JSONObject();
            JSONObject jaObj = new JSONObject();
            JSONObject ja_jpObj = new JSONObject();
            JSONObject zh_jpObj = new JSONObject();
            JSONObject en_jpObj = new JSONObject();

            rootObj.Add("locales", localesObj);
            localesObj.Add("en", enObj);
            localesObj.Add("zh", zhObj);
            localesObj.Add("ja", jaObj);
            localesObj.Add("ja_jp", ja_jpObj);
            localesObj.Add("zh_jp", zh_jpObj);
            localesObj.Add("en_jp", en_jpObj);

            foreach (var langItem in langMap)
            {
                string langKey = langItem.Key;
                List<TranslatePairData> list = langItem.Value;

                JSONObject langObj = langKey switch
                {
                    "en_us" => enObj,
                    "zh_tw" => zhObj,
                    "ja_jp" => jaObj,
                    _ => throw new InvalidOperationException($"Invalid langKey:{langKey}"),
                };

                JSONObject keysObj = new JSONObject();

                foreach (TranslatePairData translateItem in list)
                {
                    // Debug.Log($"[{langKey}] {translateItem.Key}:{translateItem.Value}");
                    keysObj.Add(translateItem.Key, translateItem.Value);
                }

                langObj.Add($"keys", keysObj);
            }

            // Supplement ja_jp, zh_jp, en_jp
            ja_jpObj.Add("keys", new JSONObject());
            zh_jpObj.Add("keys", new JSONObject());
            en_jpObj.Add("keys", new JSONObject());

            string jsonString = rootObj.ToString();

            if (isFormatJson)
            {
                jsonString = JsonUtil.FormatJson(jsonString);
            }

            FileStream fs = new FileStream(outputPath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            sw.Write(jsonString);
            sw.Close();
            fs.Close();
        }
    }
}