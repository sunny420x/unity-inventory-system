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
    private bool inventoryStatus = false;

    private GameObject PlayerCam;
    private GameObject PlayerFollowCamera;

    [Header("Inventory Data")]
    public int[] inventoryData = new int[]{
        0,0,0,0,0,0,0,0,0,0,0,0,0,0
    };
    public int[] inventoryItemsAmounts = new int[]{
        0,0,0,0,0,0,0,0,0,0,0,0,0,0
    };

    [Header("Inventory Amount Text Display (TextMeshPro)")]
    [Tooltip("Each slot textmesh goes here !")]
    [SerializeField] private TMP_Text[] inventoryItemsAmounts_tmp = new TMP_Text[]{
    };

    [Header("Inventory Slots")]
    [Tooltip("Each slot of inventory canvas goes here !")]
    public GameObject[] inventorySlots = new GameObject[]{
    };

    private Item[] items;

    [Header("Player Audio Source")]
    private AudioSource playerAudio;
    private float UISoundVolume;
    [SerializeField] private AudioClip inventoryClip;

    [SerializeField] private int currentEquippedItem = 0;
    [SerializeField] private GameObject currentEquippedItemText;

    void Start()
    {
        PlayerCam = GameObject.Find("MainCamera");
        PlayerFollowCamera = GameObject.Find("PlayerFollowCamera");
        items = items_manager.GetComponent<Items>().items;
        inventory.SetActive(inventoryStatus);
        Background.SetActive(inventoryStatus);
        UISoundVolume = PlayerCam.GetComponent<InputSystem>().UISoundVolume;
        playerAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            DrawIcon();
            if (inventoryStatus == false)
            {
                PlayerCam.GetComponent<InputSystem>().PauseGame();
                inventoryStatus = true;
                Cursor.lockState = CursorLockMode.Confined;
                Background.SetActive(inventoryStatus);
                inventory.SetActive(inventoryStatus);
                playerAudio.PlayOneShot(inventoryClip, UISoundVolume);

            } else
            {
                PlayerCam.GetComponent<InputSystem>().ResumeGame();
                inventoryStatus = false;
                Cursor.lockState = CursorLockMode.Locked;
                Background.SetActive(inventoryStatus);
                inventory.SetActive(inventoryStatus);
                playerAudio.PlayOneShot(inventoryClip, UISoundVolume);
            }
        }
    }

    public void DrawIcon()
    {
        for (int i = 0; i < inventoryData.Length; i++)
        {
            var obj_id = inventoryData[i];
            inventorySlots[i].GetComponent<RawImage>().texture = items[obj_id].icon;
            if(inventoryItemsAmounts[i] != 0 && inventoryItemsAmounts[i] != 1)
            {
                inventoryItemsAmounts_tmp[i].text = inventoryItemsAmounts[i].ToString();
            } else {
                inventoryItemsAmounts_tmp[i].text = "";
            }
        }
    }

    public void AddtoInventory(int obj_id)
    {
        if(CheckExistingItem(inventoryData, obj_id) == false)
        {
            for (int i = 0; i < inventoryData.Length; i++)
            {
                if (inventoryData[i] == 0)
                {
                    inventoryData[i] = obj_id;
                    inventoryItemsAmounts[i] += 1;
                    break;
                }
            }
        } else
        {
            for (int i = 0; i < inventoryData.Length; i++)
            {
                if (inventoryData[i] == obj_id)
                {
                    inventoryItemsAmounts[i] += 1;
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

    public void EquipItem(int slot_index)
    {
        if (slot_index >= 0 && slot_index < inventoryData.Length)
        {
            currentEquippedItem = inventoryData[slot_index];
        }
    }

    public int GetCurrentEquippedItemIndex()
    {
        return currentEquippedItem;
    }
}
