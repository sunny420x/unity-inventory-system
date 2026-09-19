using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    [Header("Silder")]
    public Slider Volume_Slider;
    public Slider EnvironmentVolumeSlider;
    public Slider MouseSensitivitySlider;

    [Header("Canvas and TextMesh Pro")]
    [SerializeField] private GameObject InputSystem_Script;
    [SerializeField] private TMP_Text VolumeValue;
    [SerializeField] private TMP_Text EnvironmentVolumeValue;
    [SerializeField] private TMP_Text MouseSensitivityValue;

    [Header("Objects to Settings")]
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Environment_Sounds;

    void Start()
    {
        float Volume = Player.GetComponent<AudioSource>().volume;
        float EnvironmentVolume = Environment_Sounds.GetComponent<AudioSource>().volume;
        float Mouse_Sensitivity = Player.transform.Find("PlayerCapsule").GetComponent<StarterAssets.FirstPersonController>().RotationSpeed;

        Volume_Slider.value = Volume;
        EnvironmentVolumeSlider.value = EnvironmentVolume;
        MouseSensitivitySlider.value = Mouse_Sensitivity;
    }

    void Update()
    {
        //Set Variables Values to Slider Values.
        float Volume = (float)System.Math.Round((double)Volume_Slider.value, 1);
        float EnvironmentVolume = (float)System.Math.Round((double)EnvironmentVolumeSlider.value, 1);
        float Mouse_Sensitivity = (float)System.Math.Round((double)MouseSensitivitySlider.value, 1);

        if (InputSystem_Script.GetComponent<InputSystem>().pauseMenuPanelStatus != false)
        {
            //Set TextMesh to Values.
            VolumeValue.text = Volume.ToString();
            EnvironmentVolumeValue.text = EnvironmentVolume.ToString();
            MouseSensitivityValue.text = Mouse_Sensitivity.ToString();

            //Setting Goes Here...
            Player.GetComponent<AudioSource>().volume = Volume;
            Environment_Sounds.GetComponent<AudioSource>().volume = EnvironmentVolume;
            Player.transform.Find("PlayerCapsule").GetComponent<StarterAssets.FirstPersonController>().RotationSpeed = Mouse_Sensitivity;
        }
    }
}
