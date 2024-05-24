using UnityEngine;

namespace Live17.LocalizationEditor
{
	[SerializeField]
	public struct TranslatePairData
	{
		public string Key;
		public string Value;

		public TranslatePairData(string key, string value)
		{
			Key = key;
			Value = value;
		}
	}
}