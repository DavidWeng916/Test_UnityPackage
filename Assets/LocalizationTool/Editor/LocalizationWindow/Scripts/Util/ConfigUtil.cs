using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Live17.LocalizationEditor
{
	public static class ConfigUtil
	{
		private static readonly string SETTING_NAME = $"{Application.productName}_Live17Localization";

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
		public string CsvPath = $"./csv/localization_{Application.productName}.csv";
		public string LocalizationPath = $"Assets/Resources/Localization/localization_{Application.productName}.txt";
		public bool IsFormatJson = true;
		public bool IsGenerateReport = false;

		public void Save()
		{
			ConfigUtil.Save(this);
		}
	}
}