using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public enum Language
{
    English,
    Spanish
}

public class Lang : MonoBehaviour
{

    public static Hashtable UIS;

    public static void InitLanguage(Language language)
    {
        if (UIS != null)
            return;

        SetLang(language);
    }

    public static void SetLang(Language newlang)
    {
        TextAsset textAsset = (TextAsset)Resources.Load("XML/UIStrings");
        XmlDocument xml = new XmlDocument();
        xml.LoadXml(textAsset.text);
        UIS = new Hashtable();
        //Load Strings

        XmlNodeList elements = xml.SelectNodes($"/languages/{newlang}/string");
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

        return UIS[key].ToString();
    }

    public static string GetItemName(int id)
    {
        string key = $"item_name_{id}";

        return GetText(key);
    }

    public static string GetItemDesc(int id)
    {
        string key = $"item_desc_{id}";

        return GetText(key);
    }
}
