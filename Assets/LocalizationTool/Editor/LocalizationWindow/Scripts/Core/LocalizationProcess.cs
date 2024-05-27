using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Live17.LocalizationEditor
{
    public static class LocalizationProcess
    {
        public static void Execute(ConfigData _configData, EditorWindow editorWindow)
        {
            if (!FileUtil.CheckConfigData(editorWindow, _configData))
            {
                return;
            }

            Dictionary<string, List<TranslatePairData>> translateMap = CSVParser.Execute(_configData);

            JsonOutput.Execute(translateMap, _configData.LocalizationPath, _configData.IsFormatJson);

            if (_configData.IsGenerateReport)
            {
                DebugReport.Execute(translateMap);
            }
        }
    }
}