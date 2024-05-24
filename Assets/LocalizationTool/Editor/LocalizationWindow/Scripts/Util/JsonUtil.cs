using System;
using UnityEngine;
using Unity.Plastic.Newtonsoft.Json.Linq;

namespace Live17.LocalizationEditor
{
	public static class JsonUtil
	{
		public static string FormatJson(string jsonString)
		{
			// https://blog.csdn.net/qq_27410185/article/details/123040341
			string formatJson = jsonString.Trim();

			try
			{
				// 判读是数组还是对象
				if (formatJson.StartsWith("["))
				{
					// The JArray.ToString() method calls formatting internally, so just use it directly
					JArray jArray = JArray.Parse(formatJson);
					formatJson = jArray.ToString();
				}
				else if (formatJson.StartsWith("{"))
				{
					// The JObject.ToString() method calls formatting internally, so just use it directly
					JObject jObject = JObject.Parse(formatJson);
					formatJson = jObject.ToString();
				}
			}
			catch (Exception ex)
			{
				Debug.LogError($"Json format error:{ex}");
			}

			return formatJson;
		}

		/*public static string FormatJson(string jsonString)
		{
			// https://blog.csdn.net/qq_27410185/article/details/123040341
			string formatJson = jsonString.Trim();

			try
			{
				JsonSerializer serializer = new JsonSerializer();
				TextReader tr = new StringReader(formatJson);
				JsonTextReader jtr = new JsonTextReader(tr);
				object obj = serializer.Deserialize(jtr);
				if (obj != null)
				{
					StringWriter textWriter = new StringWriter();
					JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
					{
						Formatting = Formatting.Indented,
						Indentation = 4,//缩进字符数
						IndentChar = ' '//缩进字符
					};
					serializer.Serialize(jsonWriter, obj);
					formatJson = textWriter.ToString();
				}
			}
			catch (Exception ex)
			{
				Debug.LogError($"Json format error:{ex}");
			}

			return formatJson;
		}*/
	}
}