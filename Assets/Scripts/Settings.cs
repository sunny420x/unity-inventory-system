using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    [Header("Silder")]
    public Slider Volume_Slider;
    public Slider Environment_Volume_Slider;
    public Slider Mouse_Sensitivity_Slider;

    [Header("Canvas and TextMesh Pro")]
    [SerializeField] private GameObject InputSystem_Script;
    [SerializeField] private TMP_Text Volume_Value;
    [SerializeField] private TMP_Text Environment_Volume_Value;
    [SerializeField] private TMP_Text Mouse_Sensitivity_Value;

    [Header("Objects to Settings")]
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Environment_Sounds;

    void Start()
    {
        float Volume = Player.GetComponent<AudioSource>().volume;
        float Environment_Volume = Environment_Sounds.GetComponent<AudioSource>().volume;
        float Mouse_Sensitivity = Player.transform.Find("PlayerCapsule").GetComponent<StarterAssets.FirstPersonController>().RotationSpeed;

        Volume_Slider.value = Volume;
        Environment_Volume_Slider.value = Environment_Volume;
        Mouse_Sensitivity_Slider.value = Mouse_Sensitivity;
    }

    void Update()
    {
        //Set Variables Values to Slider Values.
        float Volume = (float)System.Math.Round((double)Volume_Slider.value, 1);
        float Environment_Volume = (float)System.Math.Round((double)Environment_Volume_Slider.value, 1);
        float Mouse_Sensitivity = (float)System.Math.Round((double)Mouse_Sensitivity_Slider.value, 1);

        if (InputSystem_Script.GetComponent<InputSystem>().pauseMenuPanelStatus != false)
        {
            //Set TextMesh to Values.
            Volume_Value.text = Volume.ToString();
            Environment_Volume_Value.text = Environment_Volume.ToString();
            Mouse_Sensitivity_Value.text = Mouse_Sensitivity.ToString();

            //Setting Goes Here...
            Player.GetComponent<AudioSource>().volume = Volume;
            Environment_Sounds.GetComponent<AudioSource>().volume = Environment_Volume;
            Player.transform.Find("PlayerCapsule").GetComponent<StarterAssets.FirstPersonController>().RotationSpeed = Mouse_Sensitivity;
        }
    }
}
