// https://github.com/GlitchEnzo/NuGetForUnity
// https://forum.unity.com/threads/possibilities-to-load-uss-uxml-when-distributed-as-package.905618/
// https://warl.top/posts/Unity-Package/

using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Live17.LocalizationEditor
{
    public class LocalizationWindow : EditorWindow
    {
        private ConfigData _configData = null;

        [MenuItem("17Live/Localization/Parse Localization")]
        static void Init()
        {
            LocalizationWindow wnd = GetWindow<LocalizationWindow>();
            wnd.titleContent = new GUIContent("LocalizationTool");
        }

        void CreateGUI()
        {
            _configData = ConfigUtil.Load();

            VisualElement rootElement = rootVisualElement;

            // Import UXML
            // var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/LocalizationTool/Editor/UI Toolkit/VisualTree/LocalizationWindow.uxml");
            VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>("UI Toolkit/VisualTree/LocalizationWindow");
            VisualElement labelFromUXML = visualTree.Instantiate();
            rootElement.Add(labelFromUXML);

            // Import Style
            // var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/LocalizationWindow/UI Toolkit/StyleSheet/LocalizationWindow.uss");

            // TextField - CSV Path
            TextField csvPathTF = rootElement.Query<TextField>("CsvPath");
            csvPathTF.value = _configData.CsvPath;
            csvPathTF.RegisterValueChangedCallback((ChangeEvent<string> evt) =>
            {
                _configData.CsvPath = evt.newValue;
                _configData.Save();
            });

            // ObjectField - Localizatio Path
            ObjectField localizationPathOF = rootVisualElement.Query<ObjectField>("LocalizationPath");
            localizationPathOF.value = FileUtil.GetTextAsset(_configData.LocalizationPath);
            localizationPathOF.RegisterValueChangedCallback((ChangeEvent<Object> evt) =>
            {
                if (evt.newValue == null)
                {
                    return;
                }

                _configData.LocalizationPath = AssetDatabase.GetAssetPath(evt.newValue);
                _configData.Save();
            });

            // Toggle - Only Unity Tag
            Toggle onlyUnityTagToggle = rootElement.Query<Toggle>("OnlyUnityTagToggle");
            onlyUnityTagToggle.value = _configData.IsOnlyUnityTagToggle;
            onlyUnityTagToggle.RegisterValueChangedCallback((ChangeEvent<bool> evt) =>
            {
                _configData.IsOnlyUnityTagToggle = evt.newValue;
                _configData.Save();
            });

            // Toggle - Is format json
            Toggle jsonFormatToggle = rootElement.Query<Toggle>("JsonFormatToggle");
            jsonFormatToggle.value = _configData.IsFormatJson;
            jsonFormatToggle.RegisterValueChangedCallback((ChangeEvent<bool> evt) =>
            {
                _configData.IsFormatJson = evt.newValue;
                _configData.Save();
            });

            // Toggle - Is generate report
            Toggle generateReportToggle = rootElement.Query<Toggle>("GenerateReportToggle");
            generateReportToggle.value = _configData.IsGenerateReport;
            generateReportToggle.RegisterValueChangedCallback((ChangeEvent<bool> evt) =>
            {
                _configData.IsGenerateReport = evt.newValue;
                _configData.Save();
            });

            // TextField - Prefix
            TextField prefixInputTF = rootElement.Query<TextField>("PrefixInput");
            prefixInputTF.style.display = CheckIsDisplay(_configData.IsRemovePrefixWord);
            prefixInputTF.value = _configData.PrefixWord;
            prefixInputTF.RegisterValueChangedCallback((ChangeEvent<string> evt) =>
            {
                _configData.PrefixWord = evt.newValue;
                _configData.Save();
            });

            // Toggle - Prefix
            Toggle prefixToggleToggle = rootElement.Query<Toggle>("PrefixToggle");
            prefixToggleToggle.value = _configData.IsRemovePrefixWord;
            prefixToggleToggle.RegisterValueChangedCallback((ChangeEvent<bool> evt) =>
            {
                _configData.IsRemovePrefixWord = evt.newValue;
                _configData.Save();

                prefixInputTF.style.display = CheckIsDisplay(_configData.IsRemovePrefixWord);
            });

            // Button - Execute
            Button parseButton = rootElement.Query<Button>("ExecuteButton");
            parseButton.clicked += OnProcess;

            // Button - Test
            /* Button testButton = rootElement.Query<Button>("TestButton");
            testButton.clicked += OnTest; */
        }

        private static DisplayStyle CheckIsDisplay(bool isDisplay)
        {
            return isDisplay ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void OnProcess()
        {
            LocalizationProcess.Execute(_configData, this);
        }

        /* private void OnTest()
        {
            ConfigUtil.DeleteAll();
        } */
    }
}