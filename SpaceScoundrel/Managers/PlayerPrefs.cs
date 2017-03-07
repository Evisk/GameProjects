using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

public class PlayerPrefs : MonoBehaviour {

    private static PlayerPrefs _instance;

    public static PlayerPrefs Instance
    {

        get
        {
            if (_instance == null)
            {
                GameObject pp = new GameObject("PlayerPrefs");
                pp.AddComponent<PlayerPrefs>();
            }
            return _instance;
        }
    }


    public Dictionary<string, KeyCode> keyBinds = new Dictionary<string, KeyCode>();
    public Dictionary<string, int> options = new Dictionary<string, int>();
    public string playerName = "JoJo The Space Clown";
    private INIParser iniParse = new INIParser();



    public void LoadKeyBinds()
    {
       
        iniParse.Open(ResoBucket.Instance.getDefaultConfig());
        keyBinds.Add("Forward", (KeyCode)Enum.Parse(typeof(KeyCode), iniParse.ReadValue("Keybinds","Forward","W")));
        keyBinds.Add("Backward", (KeyCode)Enum.Parse(typeof(KeyCode), iniParse.ReadValue("Keybinds", "Backward", "S")));
        keyBinds.Add("Up", (KeyCode)Enum.Parse(typeof(KeyCode), iniParse.ReadValue("Keybinds", "Up", "R")));
        keyBinds.Add("Down", (KeyCode)Enum.Parse(typeof(KeyCode), iniParse.ReadValue("Keybinds", "Down", "F")));
        iniParse.Close();

        foreach (KeyValuePair<string, KeyCode> entry in keyBinds)
        {
            UnityEngine.Debug.Log(entry.Value);
        }


    }


    void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);

    }

}
