using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Live17.LocalizationEditor
{
    public static class DebugReport
    {
        public static void Execute(Dictionary<string, List<TranslatePairData>> langMap)
        {
            StringBuilder sb = new StringBuilder();
            int index;

            foreach (var langItem in langMap)
            {
                index = -1;
                string langKey = langItem.Key;
                List<TranslatePairData> list = langItem.Value;

                sb.Append($"[{langKey}] Count:{list.Count}").AppendLine();

                foreach (var translateItem in list)
                {
                    index++;
                    sb.Append($"[{index}]{translateItem.Key}:{translateItem.Value}").AppendLine();
                }

                sb.AppendLine();
            }

            File.WriteAllText("LocaizationReport.txt", sb.ToString());
        }
    }
}