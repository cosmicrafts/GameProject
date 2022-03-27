using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public enum Language
{
    English,
    Spanish,
    Arabian,
    Chinese,
    German,
    French,
    Hindu,
    Japanese,
    Portuguese,
    Russian,
    Vietnamese,
    Korean
}

public class Lang : MonoBehaviour
{
    public static string[] FilesNames = new string[12]
    {
        "ENG","ESP","ARA","CHA","DEU","FRA","HIN","JPN","POR","RUS","VIE","KOR"
    };

    public static Hashtable UIS;

    public static void InitLanguage(Language language)
    {
        if (UIS != null)
            return;

        SetLang(language);
    }

    public static void SetLang(Language newlang)
    {
        TextAsset textAsset = (TextAsset)Resources.Load($"XML/Langs/LNG_{FilesNames[(int)newlang]}");
        XmlDocument xml = new XmlDocument();
        xml.LoadXml(textAsset.text);
        UIS = new Hashtable();
        //Load Strings

        XmlNodeList elements = xml.SelectNodes($"/Strings/string");
        if (elements != null)
        {
            IEnumerator elemEnum = elements.GetEnumerator();
            while (elemEnum.MoveNext())
            {
                XmlElement xmlItem = (XmlElement)elemEnum.Current;
                UIS.Add(xmlItem.GetAttribute("name"), xmlItem.InnerText);
            }

            foreach (UILang lg in FindObjectsOfType<UILang>())
            {
                lg.SetMyText();
            }
        }
        else
        {
            Debug.LogError($"The specified language does not exist: {newlang}");
        }
    }

    public static string GetText(string key)
    {
        if (UIS == null)
            return string.Empty;

        if (!UIS.ContainsKey(key))
            return "[NO FOUND]";

        return UIS[key].ToString();
    }

    public static string GetEntityName(string key)
    {
        if (UIS == null)
            return string.Empty;

        if (!UIS.ContainsKey(key))
            return "[NO FOUND]";

        string data = UIS[key].ToString();

        return data.Substring(0, data.IndexOf(':'));
    }

    public static string GetEntityDescription(string key)
    {
        if (UIS == null)
            return string.Empty;

        if (!UIS.ContainsKey(key))
            return "[NO FOUND]";

        string data = UIS[key].ToString();

        return data.Substring(data.IndexOf(':')+1);
    }
}
