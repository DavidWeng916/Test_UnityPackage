using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Live17.LocalizationEditor
{
    public static class ConfigUtil
    {
        public static readonly string PROJECT_NAME = Application.productName.ToLower();
        private static readonly string SETTING_NAME = $"{PROJECT_NAME}_17live_localization";

        private static readonly string DEFAULT_VALUE = JsonUtility.ToJson(new ConfigData());

        public static ConfigData Load()
        {
            string data = EditorPrefs.GetString(SETTING_NAME, DEFAULT_VALUE);
            return JsonUtility.FromJson<ConfigData>(data);
        }

        public static void Save(ConfigData recordData)
        {
            string jsonString = JsonUtility.ToJson(recordData);
            EditorPrefs.SetString(SETTING_NAME, jsonString);
        }

        public static void DeleteAll()
        {
            EditorPrefs.DeleteAll();
        }
    }

    public class ConfigData
    {
        public string CsvPath = $"./csv/localization_{ConfigUtil.PROJECT_NAME}.csv";
        public string LocalizationPath = $"Assets/Resources/Localization/localization_{ConfigUtil.PROJECT_NAME}.txt";
        public bool IsFormatJson = true;
        public bool IsGenerateReport = false;

        public void Save()
        {
            ConfigUtil.Save(this);
        }
    }
}