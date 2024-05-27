using System.Text.RegularExpressions;
using UnityEngine;

namespace Live17.LocalizationEditor
{
    public static class StringUtil
    {
        public static string RemovePrefix(string s, string prefix)
        {
            return Regex.Replace(s, $"^{prefix}", string.Empty);
        }
    }
}