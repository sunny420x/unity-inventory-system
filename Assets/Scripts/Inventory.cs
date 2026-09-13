using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{

    [Header("Inventory Setting")]
    [SerializeField] private GameObject items_manager;
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject Background;
    private bool inventory_status = false;

    private GameObject PlayerCam;
    private GameObject PlayerFollowCamera;

    [Header("Inventory Data")]
    public int[] inventory_data = new int[]{
        0,0,0,0,0,0,0,0,0,0,0,0,0,0
    };
    public int[] inventory_items_amounts = new int[]{
        0,0,0,0,0,0,0,0,0,0,0,0,0,0
    };

    [Header("Inventory Amount Text Display (TextMeshPro)")]
    [Tooltip("Each slot textmesh goes here !")]
    [SerializeField] private TMP_Text[] inventory_items_amounts_tmp = new TMP_Text[]{
    };

    [Header("Inventory Slots")]
    [Tooltip("Each slot of inventory canvas goes here !")]
    public GameObject[] inventory_slots = new GameObject[]{
    };

    private Item[] items;

    [Header("Player Audio Source")]
    private AudioSource player_audio;
    private float UISoundVolume;
    [SerializeField] private AudioClip inventory_clip;

    void Start()
    {
        PlayerCam = GameObject.Find("MainCamera");
        PlayerFollowCamera = GameObject.Find("PlayerFollowCamera");
        items = items_manager.GetComponent<Items>().items;
        inventory.SetActive(inventory_status);
        Background.SetActive(inventory_status);
        UISoundVolume = PlayerCam.GetComponent<InputSystem>().UISoundVolume;
        player_audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            DrawIcon();
            if (inventory_status == false)
            {
                PlayerCam.GetComponent<InputSystem>().PauseGame();
                inventory_status = true;
                Cursor.lockState = CursorLockMode.Confined;
                Background.SetActive(inventory_status);
                inventory.SetActive(inventory_status);
                player_audio.PlayOneShot(inventory_clip, UISoundVolume);

            } else
            {
                PlayerCam.GetComponent<InputSystem>().ResumeGame();
                inventory_status = false;
                Cursor.lockState = CursorLockMode.Locked;
                Background.SetActive(inventory_status);
                inventory.SetActive(inventory_status);
                player_audio.PlayOneShot(inventory_clip, UISoundVolume);
            }
        }
    }

    public void DrawIcon()
    {
        for (int i = 0; i < inventory_data.Length; i++)
        {
            var obj_id = inventory_data[i];
            inventory_slots[i].GetComponent<RawImage>().texture = items[obj_id].icon;
            if(inventory_items_amounts[i] != 0 && inventory_items_amounts[i] != 1)
            {
                inventory_items_amounts_tmp[i].text = inventory_items_amounts[i].ToString();
            } else {
                inventory_items_amounts_tmp[i].text = "";
            }
        }
    }

    public void AddtoInventory(int obj_id)
    {
        if(CheckExistingItem(inventory_data, obj_id) == false)
        {
            for (int i = 0; i < inventory_data.Length; i++)
            {
                if (inventory_data[i] == 0)
                {
                    inventory_data[i] = obj_id;
                    inventory_items_amounts[i] += 1;
                    break;
                }
            }
        } else
        {
            for (int i = 0; i < inventory_data.Length; i++)
            {
                if (inventory_data[i] == obj_id)
                {
                    inventory_items_amounts[i] += 1;
                    break;
                }
            }
        }
    }

    bool CheckExistingItem(int[] player_inventory, int obj_id)
    {
        for (int i = 0; i < player_inventory.Length; i++)
        {
            if (player_inventory[i] == obj_id)
            {
                return true;
            }
        }
        return false;
    }
}
