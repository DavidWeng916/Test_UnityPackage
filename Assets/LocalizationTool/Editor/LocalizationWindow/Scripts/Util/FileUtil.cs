using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Live17.LocalizationEditor
{
    public static class FileUtil
    {
        public static bool CheckConfigData(EditorWindow editorWindow, ConfigData _configData, double fadeoutWait = 2f)
        {
            if (string.IsNullOrEmpty(_configData.CsvPath))
            {
                editorWindow.ShowNotification(new GUIContent("CsvPath field can't empty"), fadeoutWait);
                return false;
            }

            if (!File.Exists(_configData.CsvPath))
            {
                editorWindow.ShowNotification(new GUIContent("CsvPath isn't exist"), fadeoutWait);
                return false;
            }

            if (string.IsNullOrEmpty(_configData.LocalizationPath))
            {
                editorWindow.ShowNotification(new GUIContent("LocalizationPath field can't empty"), fadeoutWait);
                return false;
            }

            if (!File.Exists(_configData.LocalizationPath))
            {
                editorWindow.ShowNotification(new GUIContent("LocalizationPath isn't exist"), fadeoutWait);
                return false;
            }

            return true;
        }

        public static TextAsset GetTextAsset(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            TextAsset textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);

            if (textAsset == null)
            {
                return null;
            }

            return textAsset;
        }
    }
}