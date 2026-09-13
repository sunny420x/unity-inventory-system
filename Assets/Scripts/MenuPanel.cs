using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    public GameObject InputScript;

    public void OpenPanel(string name)
    {
        switch (name)
        {
            case "settings":
                if(InputScript.GetComponent<InputSystem>().pauseMenuPanelStatus == false)
                {
                    InputScript.GetComponent<InputSystem>().pauseMenuPanelStatus = true;

                    InputScript.GetComponent<InputSystem>().pauseMenuPanel.SetActive(true);
                    InputScript.GetComponent<InputSystem>().settingsMenu.SetActive(true);
                } else
                {
                    InputScript.GetComponent<InputSystem>().pauseMenuPanelStatus = false;

                    InputScript.GetComponent<InputSystem>().pauseMenuPanel.SetActive(false);
                    InputScript.GetComponent<InputSystem>().settingsMenu.SetActive(false);
                }
                break;
        }
    }
}
