// https://github.com/GlitchEnzo/NuGetForUnity
// https://forum.unity.com/threads/possibilities-to-load-uss-uxml-when-distributed-as-package.905618/

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

            // return;
            // Import UXML
            // var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/LocalizationTool/Editor/UI Toolkit/VisualTree/LocalizationWindow.uxml");
            VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>("UI Toolkit/VisualTree/LocalizationWindow");
            VisualElement labelFromUXML = visualTree.Instantiate();
            // Debug.Log($"===== labelFromUXML:{labelFromUXML}");
            rootElement.Add(labelFromUXML);

            // Import Style
            // var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/LocalizationWindow/UI Toolkit/StyleSheet/LocalizationWindow.uss");

            // TextField - CSV Path
            TextField csvPathTextField = rootElement.Query<TextField>("CsvPath");
            csvPathTextField.value = _configData.CsvPath;
            csvPathTextField.RegisterValueChangedCallback((ChangeEvent<string> evt) =>
            {
                _configData.CsvPath = evt.newValue;
                _configData.Save();
            });

            // TextField - Output Path
            TextField outputPathTextField = rootElement.Query<TextField>("OutputPath");
            outputPathTextField.value = _configData.OutputPath;
            outputPathTextField.RegisterValueChangedCallback((ChangeEvent<string> evt) =>
            {
                _configData.OutputPath = evt.newValue;
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

            // Button - Execute
            Button parseButton = rootElement.Query<Button>("ExecuteButton");
            parseButton.clicked += OnProcess;

            // Button - Execute
            Button testButton = rootElement.Query<Button>("TestButton");
            testButton.clicked += OnTest;
        }

        private void OnProcess()
        {
            LocalizationProcess.Execute(_configData, this);
        }

        private void OnTest()
        {
            // ConfigUtil.DeleteAll();

            VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>("UI Toolkit/VisualTree/LocalizationWindow");
            Debug.Log($"===== visualTree:{visualTree}");

            // var aaa = Resources.Load<TextAsset>("json");
            // Debug.Log($"===== aaa:{aaa}");
        }
    }
}