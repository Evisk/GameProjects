using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class InterfaceManager : MonoBehaviour {

    public enum menuType
    {
        MainMenu,
        NewGame,
        LoadMenu,
        OptionsMenu,
        Exit,
        StartGame,
        GalaxySizeForward,
        GalaxySizeBackward
    }
   
    

    private GameObject currentlyOpenedMenu;
    private GameObject menuBackground;
    private Animation animationHolder;


   

    private static InterfaceManager _instance;
    public static InterfaceManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject gm = new GameObject("InterfaceManager");
                gm.AddComponent<InterfaceManager>();
            }
            return _instance;
        }
    }
    public void resetMenuBackgroundSize()
    {
        menuBackground.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
    }


    public IEnumerator toggleMenuVisibility(menuType show)
    {

        if (currentlyOpenedMenu.GetComponent<Animation>())
        {
            animationHolder = currentlyOpenedMenu.GetComponent<Animation>();
            if (animationHolder.isPlaying)
            {
                switch (show) {
                    case menuType.MainMenu:
                        animationHolder["newGameFadeOut"].time = animationHolder["newGameFadeIn"].length - animationHolder["newGameFadeIn"].time;
                        animationHolder.Play("newGameFadeOut");
                        yield return new WaitForSeconds(animationHolder["newGameFadeOut"].length - animationHolder["newGameFadeOut"].time + 0.2f);
                        DestroyObject(currentlyOpenedMenu);
                        break;
                    default:
                        animationHolder["menuFadeOut"].time = animationHolder["menuFadeIn"].length - animationHolder["menuFadeIn"].time;
                        animationHolder.Play("menuFadeOut");
                        yield return new WaitForSeconds(animationHolder["menuFadeOut"].length - animationHolder["menuFadeOut"].time + 0.2f);
                        DestroyObject(currentlyOpenedMenu);
                        break;

                }
            }
            else
            {
                switch (show)
                {
                    case menuType.MainMenu:
                        animationHolder.Play("newGameFadeOut");
                        yield return new WaitForSeconds(animationHolder["newGameFadeOut"].length + 0.2f);
                        DestroyObject(currentlyOpenedMenu);
                        break;
                    default:
                        animationHolder.Play("menuFadeOut");
                        yield return new WaitForSeconds(animationHolder["menuFadeOut"].length + 0.2f);
                        DestroyObject(currentlyOpenedMenu);
                        break;

                }
                
            }
        }else
        {
            DestroyObject(currentlyOpenedMenu);
        }
        

        switch (show)
        {
            case menuType.MainMenu:
                currentlyOpenedMenu = Instantiate(ResoBucket.Instance.menuPrefabs["mainMenu"], ResoBucket.Instance.getCanvas(), false) as GameObject;
                break;
            case menuType.LoadMenu:
                currentlyOpenedMenu = Instantiate(ResoBucket.Instance.menuPrefabs["loadGameMenu"], ResoBucket.Instance.getCanvas(), false) as GameObject;
                break;
            case menuType.OptionsMenu:
                currentlyOpenedMenu = Instantiate(ResoBucket.Instance.menuPrefabs["optionsMenu"], ResoBucket.Instance.getCanvas(), false) as GameObject;
                break;
            case menuType.NewGame:
                currentlyOpenedMenu = Instantiate(ResoBucket.Instance.menuPrefabs["newGameMenu"], ResoBucket.Instance.getCanvas(), false) as GameObject;
                ResoBucket.Instance.setGalaxyTextObject(GameObject.Find("galaxySelectorText").GetComponent<Text>());
                GalaxyManager.Instance.setGalaxySelectorText();
                break;
        }
    }

   

    public void exitGame()
    {
        Application.Quit();
    }


    public void startGame()
    {
        SceneManager.LoadScene("mapLevel");
    }

    public void StartManager()
    {

    }

    void Awake()
    {
        _instance = this;
        menuBackground = Instantiate(ResoBucket.Instance.getMenuBackgroundPrefab(), ResoBucket.Instance.getCanvas(), false) as GameObject;
        resetMenuBackgroundSize();
        currentlyOpenedMenu = Instantiate(ResoBucket.Instance.menuPrefabs["mainMenu"], ResoBucket.Instance.getCanvas(), false) as GameObject;
        DontDestroyOnLoad(gameObject);
    }

}
