using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Live17.LocalizationEditor
{
	public static class ConfigUtil
	{
		private const string SETTING_NAME = "Live17Localization";

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

			if (string.IsNullOrEmpty(_configData.OutputPath))
			{
				editorWindow.ShowNotification(new GUIContent("OutputPath field can't empty"), fadeoutWait);
				return false;
			}

			string directoryPath = Path.GetDirectoryName(_configData.OutputPath);
			if (!Directory.Exists(directoryPath))
			{
				editorWindow.ShowNotification(new GUIContent("OutputPath isn't exist"), fadeoutWait);
				return false;
			}

			return true;
		}
	}

	public class ConfigData
	{
		public string CsvPath = "./csv/localization_xxxxx.csv";
		public string OutputPath = "./Assets/Resources/Localization/localization_xxxxx.txt";
		public bool IsFormatJson = true;
		public bool IsGenerateReport = false;

		public void Save()
		{
			ConfigUtil.Save(this);
		}
	}
}