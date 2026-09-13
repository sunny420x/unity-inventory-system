using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using StarterAssets;

public class PlayerStatus : MonoBehaviour
{
    [Header("Settings")]
    public float health = 100f;
    public float stamina = 100f;
    [SerializeField] private float staminaDecreaseRate = 25f;
    [SerializeField] private float staminaIncreaseRate = 20f;

    [Header("Prefebs")]

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject PlayerCam;
    private GameObject player_controller;
    [SerializeField] private Animator player_animator;
    [SerializeField] private Slider stamina_bar;
    [SerializeField] private TMP_Text health_text;

    [SerializeField] private PlayerAnimationController PlayerAnimationController;
    [SerializeField] private Footsteps Footsteps;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip hit_sound_effect;
    private float UISoundVolume;

    private float MoveSpeed;
    private float SprintSpeed;
    private float stepRate;
    private bool isCrouch;
    private bool isCrouchWalking;

    void Start() {
        player_controller = player.transform.Find("PlayerCapsule").gameObject;
        UISoundVolume = PlayerCam.GetComponent<InputSystem>().UISoundVolume;
        MoveSpeed = player_controller.GetComponent<FirstPersonController>().MoveSpeed;
        SprintSpeed = player_controller.GetComponent<FirstPersonController>().SprintSpeed;
        stepRate = Footsteps.GetComponent<Footsteps>().stepRate;
    }

    void Update() {
        stamina_bar.value = stamina;
        health_text.text = health.ToString();

        isCrouch = PlayerAnimationController.GetComponent<PlayerAnimationController>().isCrouch;
        isCrouchWalking = PlayerAnimationController.GetComponent<PlayerAnimationController>().isCrouchWalking;

        if (stamina < 10)
        {
            player_controller.GetComponent<FirstPersonController>().SprintSpeed = MoveSpeed;
            Footsteps.GetComponent<Footsteps>().stepRate = stepRate;
        }
        else
        {
            if (isCrouch)
            {
                player_controller.GetComponent<FirstPersonController>().MoveSpeed = MoveSpeed / 2;
                player_controller.GetComponent<FirstPersonController>().SprintSpeed = MoveSpeed / 2;
                Footsteps.GetComponent<Footsteps>().stepRate = stepRate * 2.0f;
            }
            else
            {
                player_controller.GetComponent<FirstPersonController>().MoveSpeed = MoveSpeed;
                player_controller.GetComponent<FirstPersonController>().SprintSpeed = SprintSpeed;
                Footsteps.GetComponent<Footsteps>().stepRate = stepRate;
            }
        }

        if(Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) 
        || Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.S)
        || Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.A)
        || Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.D)
        ) {
            Running();
        } else {
            Resting();
        }

        if(health == 0) {
            Debug.Log("[!] Player died...");
        }
    }

    public void Running() {
        if (isCrouch && isCrouchWalking) return;
        if (stamina >= 10)
        {
            Footsteps.GetComponent<Footsteps>().stepRate = stepRate / 1.5f;
        }
        stamina_bar.transform.gameObject.SetActive(true);
        this.stamina -= (float) this.staminaDecreaseRate * (Time.deltaTime);
        this.stamina = Mathf.Clamp(this.stamina, 0f, 100f);
    }

    public void Resting() {
        Footsteps.GetComponent<Footsteps>().stepRate = stepRate;
        stamina_bar.transform.gameObject.SetActive(false);
        this.stamina += (float) this.staminaIncreaseRate * (Time.deltaTime);
        this.stamina = Mathf.Clamp(this.stamina, 0f, 100f);
    }
}