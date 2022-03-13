using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace CNC.Common.Localization
{
    public class CNCLocalization
    {
        public static string ModKey => "Mods.CNC.";
        public static Dictionary<string, string>[] Culture => _Culture;
        private static Dictionary<string, string>[] _Culture = new Dictionary<string, string>[10];
        public static int language => Language.ActiveCulture.LegacyId;
        public static readonly bool isChinese = language == 7;
        
        private static string _GetText(string key, int culture = 0)
        {
            if (!key.StartsWith(ModKey)) key = ModKey + key;
            string text = key;
            if (culture == 0) culture = language;
            if (Culture == null || Culture[culture] == null) return text;
            if (Culture[culture].ContainsKey(key))
            {
                text = Culture[culture][text];
                if (text.Contains("{$" + ModKey)) 
                {
                    int start = text.IndexOf("{$" + ModKey);
                    int end = text.IndexOf('}');
                    while (end >= 0 && end <= start && text.Substring(end + 1).Contains("}")) end = text.IndexOf('}', end + 1);
                    if (start >= 0 && end >= 0 && start < end)
                    {
                        string replace = text.Substring(start, end - start + 1);
                        string key2 = replace.Substring(2, replace.Length - 3);
                        text = text.Replace(replace, _GetText(key2, culture));
                    }
                }
            }
            return text;
        }

        public static bool TextExist(string key) => _GetText(key) != (key.StartsWith(ModKey) ? key : ModKey + key);

        public static string GetText(string key, int culture = 0) => _GetText(key, culture);
        public static string GetText(string key, object[] args, int culture = 0) => string.Format(_GetText(key, culture), args);
        public static string GetText(string key, object arg0, int culture = 0) => string.Format(_GetText(key, culture), arg0);
        public static string GetText(string key, object arg0, object arg1, int culture = 0) => string.Format(_GetText(key, culture), arg0, arg1);
        public static string GetText(string key, object arg0, object arg1, object arg2, int culture = 0) => string.Format(_GetText(key, culture), arg0, arg1, arg2);

        public static string LegacyGetText(string key) => Language.GetTextValue(ModKey + key);
        public static string LegacyGetText(string key, object[] args) => Language.GetTextValue(ModKey + key, args);
        public static string LegacyGetText(string key, object arg0) => Language.GetTextValue(ModKey + key, arg0);
        public static string LegacyGetText(string key, object arg0, object arg1) => Language.GetTextValue(ModKey + key, arg0, arg1);
        public static string LegacyGetText(string key, object arg0, object arg1, object arg2) => Language.GetTextValue(ModKey + key, arg0, arg1, arg2);
        
        public static Dictionary<string, string>[] Load(string mod, TmodFile modFiles)
        {
            string[] language = { "", "en-US", "de-DE", "it-IT", "fr-FR", "es-ES", "ru-RU", "zh-Hans", "pt-BR", "pl-PL" };
            string extension = ".hjson";
            for (int i = 1; i < 10; i++)
            {
                if (i != 1 && i != 7) continue;
                string name = language[i] + extension;
                _Culture[i] = new Dictionary<string, string>();
                _Culture[i].Clear();
                foreach (TmodFile.FileEntry file in modFiles)
                {
                    if (Path.GetFileNameWithoutExtension(file.Name) == language[i] && file.Name.EndsWith(extension))
                    {
                        string key = "";
                        string line = "";
                        string[] lines = Encoding.UTF8.GetString(ModLoader.GetMod(mod).GetFileBytes(file.Name)).Split('\r');
                        for (int j = 0; j < lines.Length; j++) if (lines[j].Contains("#")) lines[j].Remove(lines[j].IndexOf('#'));
                        for (int j = 0; j < lines.Length; j++)
                        {

                            int start = 0;
                            char[] _line = lines[j].ToCharArray();
                            while (start < _line.Length && (_line[start] == ' ' || _line[start] == '\t' || _line[start] == '\n' || _line[start] == '\r')) start++;
                            line = lines[j].Substring(start);
                            if (line.StartsWith("}"))
                            {
                                if (key.IndexOf('.') == key.LastIndexOf('.')) key = "";
                                else
                                {
                                    key = key.Remove(key.LastIndexOf('.'));
                                    key = key.Remove(key.LastIndexOf('.'));
                                    key += '.';
                                }
                            }
                            else if (line.Contains(": "))
                            {
                                string _key = line.Substring(0, line.IndexOf(": "));
                                string _value = line.Substring(line.IndexOf(": "));
                                string value = _value.Substring(2);
                                if (_value.StartsWith(": {")) key += _key + '.';
                                else {
                                    string temp = "~!@#$%^&*()_+";
                                    int idx = value.IndexOf("\\\\");
                                    while (idx >= 0)
                                    {
                                        value = value.Substring(0, idx) + temp + value.Substring(idx + 2);
                                        idx = value.IndexOf("\\\\");
                                    }
                                    value = value.Replace("\\\"", "\"");
                                    value = value.Replace("\\\'", "\'");
                                    value = value.Replace("\\0", "");//"\0"
                                    value = value.Replace("\\a", "\a");
                                    value = value.Replace("\\b", "\b");
                                    value = value.Replace("\\f", "\f");
                                    value = value.Replace("\\n", "\n");
                                    value = value.Replace("\\r", "\r");
                                    value = value.Replace("\\t", "\t");
                                    value = value.Replace("\\v", "\v");
                                    value = value.Replace(temp, "\\");
                                    _Culture[i].Add(key + _key, value);
                                }
                            }
                        }
                    }
                }
            }
            List<Dictionary<string, string>> _culture = new List<Dictionary<string, string>>();
            _culture.AddRange(_Culture);
            _culture.RemoveAt(0);
            return _culture.ToArray();
        }

        public static void Unload()
        {
            if (_Culture != null)
            {
                for (int i = 0; i < _Culture.Length; i++)
                {
                    if (_Culture[i] != null) { _Culture[i].Clear(); _Culture[i] = null; }
                    if (Culture[i] != null) { Culture[i].Clear(); Culture[i] = null; }
                }
            }
        } 
    }
}