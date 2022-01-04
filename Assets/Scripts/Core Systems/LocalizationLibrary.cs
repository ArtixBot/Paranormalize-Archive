using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

// Currently only handles cards. TODO: Add other localization handling later. Singleton.
public class LocalizationLibrary
{
    public static readonly LocalizationLibrary Instance = new LocalizationLibrary();
    private JObject arguments = null;
    private JObject cards = null;
    private JObject ambience = null;
    private JObject keywords = null;
    private JObject ui = null;

    private LocalizationLibrary(){
        string path = "Localization/" + Application.systemLanguage.ToString() + "/cards";
        TextAsset targetFile = Resources.Load<TextAsset>(path);
        cards = JObject.Parse(targetFile.ToString());

        path = "Localization/" + Application.systemLanguage.ToString() + "/arguments";
        targetFile = Resources.Load<TextAsset>(path);
        arguments = JObject.Parse(targetFile.ToString());

        path = "Localization/" + Application.systemLanguage.ToString() + "/ambience";
        targetFile = Resources.Load<TextAsset>(path);
        ambience = JObject.Parse(targetFile.ToString());

        path = "Localization/" + Application.systemLanguage.ToString() + "/keywords";
        targetFile = Resources.Load<TextAsset>(path);
        keywords = JObject.Parse(targetFile.ToString());

        path = "Localization/" + Application.systemLanguage.ToString() + "/ui";
        targetFile = Resources.Load<TextAsset>(path);
        ui = JObject.Parse(targetFile.ToString());
    }

    public Dictionary<string, string> GetArgumentStrings(string ID){
        try {
            return arguments[ID].ToObject<Dictionary<string, string>>();
        } catch { //(Exception ex) {
            Dictionary<string, string> notFound = new Dictionary<string, string>();
            notFound.Add("NAME", "Missing name: " + ID);
            notFound.Add("DESC", "Missing desc: " + ID);
            return notFound;
        }
    }

    public Dictionary<string, string> GetCardStrings(string ID){
        try {
            return cards[ID].ToObject<Dictionary<string, string>>();
        } catch { //(Exception ex) {
            Dictionary<string, string> notFound = new Dictionary<string, string>();
            notFound.Add("NAME", "Missing name: " + ID);
            notFound.Add("DESC", "Missing desc: " + ID);
            return notFound;
        }
    }

    public Dictionary<string, string> GetAmbienceStrings(string ID){
        try {
            return ambience[ID].ToObject<Dictionary<string, string>>();
        } catch { //(Exception ex) {
            Dictionary<string, string> notFound = new Dictionary<string, string>();
            notFound.Add("NAME", "Missing name: " + ID);
            notFound.Add("FLAVOR", "Missing name: " + ID);
            notFound.Add("DESC", "Missing desc: " + ID);
            return notFound;
        }
    }

    public string GetKeywordString(string ID){
        try {
            return keywords[ID].ToString();
        } catch {
            return "No description found for " + ID;
        }
    }

    public string GetUIString(string ID){
        try {
            return ui[ID].ToString();
        } catch {
            return "No description found for " + ID;
        }
    }
}