using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class ResoBucket : MonoBehaviour {

    private static ResoBucket _instance;

    public Dictionary<string, GameObject> menuPrefabs = new Dictionary<string, GameObject>();
    private Transform canvasTransform;
    private GameObject mapBackGroundPrefab;
    private GameObject planetPrefab;
    private GameObject starPrefab;
    private GameObject menuBackGroundPrefab;
    private Text galaxySizeText;
    [SerializeField]
    private TextAsset defaultConfig;

    public void loadDefaultConfig ()
    {
        defaultConfig = Resources.Load<TextAsset>("defaultConfig");
    }

    public TextAsset getDefaultConfig()
    {
        return defaultConfig;
    }


    public static ResoBucket Instance
    {

        get
        {
            if (_instance == null)
            {
                GameObject gm = new GameObject("ResoBucket");
                gm.AddComponent<ResoBucket>();
            }
            return _instance;
        }
    }


    public void loadPrefabs()
    {
        menuBackGroundPrefab = Resources.Load<GameObject>("Prefabs/backGround/menuBackground");
        planetPrefab = Resources.Load<GameObject>("Prefabs/Planet");
        starPrefab = Resources.Load<GameObject>("Prefabs/Star");
        mapBackGroundPrefab = Resources.Load<GameObject>("Prefabs/backGround/mapBackGround");
        menuPrefabs.Add("mainMenu", Resources.Load<GameObject>("Prefabs/MenuPanels/mainMenu"));
        menuPrefabs.Add("loadGameMenu", Resources.Load<GameObject>("Prefabs/MenuPanels/loadGameMenu"));
        menuPrefabs.Add("optionsMenu", Resources.Load<GameObject>("Prefabs/MenuPanels/optionsMenu"));
        menuPrefabs.Add("newGameMenu", Resources.Load<GameObject>("Prefabs/MenuPanels/newGameMenu"));
    }

    public GameObject getMenuBackgroundPrefab()
    {
        return menuBackGroundPrefab;
    }


    public GameObject getBackGroundPrefab()
    {
        return mapBackGroundPrefab;
    }

    public GameObject getPlanetPrefab()
    {
        return planetPrefab;
    }

    public GameObject getStarPrefab()
    {
        return starPrefab;
    }

    private void findCanvas()
    {
        canvasTransform = GameObject.Find("Canvas").transform;
    }

    public Transform getCanvas()
    {
        return canvasTransform;
    }

    public void StartResoBucket()
    {

    }


    void Awake()
    {
        _instance = this;
        findCanvas();
        loadPrefabs();
        loadDefaultConfig();
        DontDestroyOnLoad(gameObject);
    }

    public void setGalaxyTextObject(Text text)
    {
        galaxySizeText = text;
    }

    public Text getGalaxyTextObject()
    {
        return galaxySizeText;
    }
}
