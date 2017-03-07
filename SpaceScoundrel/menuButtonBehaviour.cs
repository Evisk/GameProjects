using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class menuButtonBehaviour : MonoBehaviour, IPointerClickHandler {

    public InterfaceManager.menuType buttonType;
    private Dropdown galaxySizeDropDown;

	public void OnPointerClick(PointerEventData data)
    {
        switch (buttonType)
        {
            case InterfaceManager.menuType.NewGame :
                StartCoroutine(InterfaceManager.Instance.toggleMenuVisibility(buttonType));
                break;
            case InterfaceManager.menuType.LoadMenu:
                StartCoroutine(InterfaceManager.Instance.toggleMenuVisibility(buttonType));
                break;
            case InterfaceManager.menuType.OptionsMenu:
                StartCoroutine(InterfaceManager.Instance.toggleMenuVisibility(buttonType));
                break;
            case InterfaceManager.menuType.MainMenu:
                StartCoroutine(InterfaceManager.Instance.toggleMenuVisibility(buttonType));
                break;
            case InterfaceManager.menuType.Exit:
                InterfaceManager.Instance.exitGame();
                break;
            case InterfaceManager.menuType.StartGame:
                InterfaceManager.Instance.startGame();
                break;
            case InterfaceManager.menuType.GalaxySizeForward:
                GalaxyManager.Instance.increaseGalaxySize();
                break;
            case InterfaceManager.menuType.GalaxySizeBackward:
                GalaxyManager.Instance.decreaseGalaxySize();
                break;
        }
        
    }


       
}
