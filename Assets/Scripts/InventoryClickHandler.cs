using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventoryClickHandler : MonoBehaviour, IPointerClickHandler
{
    private int current_selected_item;
    private int current_selected_slot;

    [Header("Settings")]
    public GameObject itemsIcon;
    public TMP_Text itemsTitleText;
    public TMP_Text itemsContentText;

    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject PlayerCam;
    [SerializeField] private GameObject itemsManager;

    [Header("Equip, Use and drop buttons")]
    [SerializeField] private GameObject equipBtn;
    [SerializeField] private GameObject useBtn;
    [SerializeField] private GameObject dropBtn;

    [Header("Object Spawner")]
    [SerializeField] private GameObject ObjectSpawner;
    [SerializeField] private Transform HoldPosition;
    
    [Header("Sound Effects")]
    [SerializeField] private AudioClip taking_pill_sound;

    private GameObject[] slots;

    private int currentEquippedItem;
    private int currentEquippedItemSlotId;

    void Start()
    {
        useBtn.SetActive(false);
        dropBtn.SetActive(false);
        equipBtn.SetActive(false);
        itemsIcon.SetActive(false);

        Button useBtn_action = useBtn.GetComponent<Button>();
        Button dropBtn_action = dropBtn.GetComponent<Button>();
        Button equipBtn_action = equipBtn.GetComponent<Button>();

        currentEquippedItem = Player.GetComponent<Inventory>().getCurrentEquippedItemIndex();
        currentEquippedItemSlotId = Player.GetComponent<Inventory>().getCurrentEquippedItemSlotId();

        //Get usable state of items.

        useBtn_action.onClick.AddListener(() => {
            UseItem(current_selected_slot, current_selected_item);
        });

        dropBtn_action.onClick.AddListener(() => {
            DropItem(current_selected_slot, current_selected_item);
        });

        equipBtn_action.onClick.AddListener(() =>
        {
            if(currentEquippedItem != 0)
            {
                UnequipItem();
            }
            else
            {
                EquipItem(current_selected_slot, current_selected_item);
            }
        });

        slots = Player.GetComponent<Inventory>().inventorySlots;
    }

    public void OnPointerClick(PointerEventData PointerEvent)
    {
        if (PointerEvent.button == PointerEventData.InputButton.Left) {

        }
    }

    void UseItem(int slot_id, int obj_id)
    {
        var inventoryData = Player.GetComponent<Inventory>().inventoryData;
        var inventoryItemsAmounts = Player.GetComponent<Inventory>().inventoryItemsAmounts;
        if (inventoryItemsAmounts[slot_id] > 1)
        {
            inventoryItemsAmounts[slot_id] -= 1;
        }
        else
        {
            if (inventoryItemsAmounts[slot_id] == 1)
            {
                inventoryData[slot_id] = 0;
                inventoryItemsAmounts[slot_id] = 0;
                SetInventoryItemDetail(0);
            }
        }

        Debug.Log("You use item id: "+obj_id);
        if(obj_id == 1) {
            Player.GetComponent<AudioSource>().PlayOneShot(taking_pill_sound, PlayerCam.GetComponent<InputSystem>().UISoundVolume);
        }
        Player.GetComponent<Inventory>().DrawIcon();
    }

    void DropItem(int slot_id, int obj_id)
    {
        var inventoryData = Player.GetComponent<Inventory>().inventoryData;
        var inventoryItemsAmounts = Player.GetComponent<Inventory>().inventoryItemsAmounts;

        if (slot_id == currentEquippedItemSlotId)
        {
            foreach (Transform child in HoldPosition)
            {
                Destroy(child.gameObject);
            }
            currentEquippedItem = 0;
        }

        if (inventoryItemsAmounts[slot_id] > 1)
        {
            inventoryItemsAmounts[slot_id] -= 1;
        }
        else
        {
            if (inventoryItemsAmounts[slot_id] == 1)
            {
                inventoryData[slot_id] = 0;
                inventoryItemsAmounts[slot_id] = 0;
                SetInventoryItemDetail(0);
            }
        }

        Debug.Log("You drop item from slot id: "+slot_id);

        Player.GetComponent<Inventory>().DrawIcon();

        //Spawning Object.
        ObjectSpawner.GetComponent<ObjectSpawner>().Spawner(obj_id);
    }

    void EquipItem(int slot_id, int obj_id)
    {
        Items itemsComponent = itemsManager.GetComponent<Items>();

        if (itemsComponent != null && obj_id >= 0)
        {
            Item itemData = itemsComponent.items[obj_id];

            if (itemData.prefab != null)
            {
                if (currentEquippedItem != 0)
                {
                    foreach (Transform child in HoldPosition)
                    {
                        Destroy(child.gameObject);
                    }
                    currentEquippedItem = 0;
                    equipBtn.GetComponentInChildren<TMP_Text>().text = "Equip";
                    return;
                }
                Player.GetComponent<Inventory>().EquipItem(slot_id);
                currentEquippedItem = obj_id;
                equipBtn.GetComponentInChildren<TMP_Text>().text = "Unequip";
            }
            else
            {
                Debug.LogWarning("Prefab is null for item ID: " + obj_id);
            }
        }
        else
        {
            Debug.LogWarning("Invalid item ID or Items component is missing.");
        }
    }

    void UnequipItem()
    {
        if (currentEquippedItem != 0)
        {
            foreach (Transform child in HoldPosition)
            {
                Destroy(child.gameObject);
            }
            currentEquippedItem = 0;
            equipBtn.GetComponentInChildren<TMP_Text>().text = "Equip";
            Player.GetComponent<Inventory>().UnequipItem();
        }
    }

    public void SetInventoryItemDetail(int item_id)
    {
        if (item_id != 0)
        {
            itemsIcon.SetActive(true);
            itemsIcon.GetComponent<RawImage>().texture = itemsManager.GetComponent<Items>().items[item_id].icon;
            itemsTitleText.text = itemsManager.GetComponent<Items>().items[item_id].title;
            itemsContentText.text = itemsManager.GetComponent<Items>().items[item_id].description;

            useBtn.SetActive(true);
            dropBtn.SetActive(true);
            equipBtn.SetActive(true);

            if (itemsManager.GetComponent<Items>().items[item_id].usable == true)
            {
                useBtn.GetComponent<Button>().interactable = true;
            }
            else
            {
                useBtn.GetComponent<Button>().interactable = false;
            }

            if (itemsManager.GetComponent<Items>().items[item_id].equipable == true)
            {
                equipBtn.GetComponent<Button>().interactable = true;
            }
            else
            {
                equipBtn.SetActive(true);
                equipBtn.GetComponent<Button>().interactable = false;
            }
        }
        else
        {
            itemsIcon.SetActive(false);
            itemsIcon.GetComponent<RawImage>().texture = itemsManager.GetComponent<Items>().items[0].icon;
            itemsTitleText.text = "";
            itemsContentText.text = "";

            useBtn.SetActive(false);
            dropBtn.SetActive(false);
            equipBtn.SetActive(false);
        }
    }

    public void ClickOnSlot(int slot_id)
    {
        var item_id = Player.GetComponent<Inventory>().inventoryData[slot_id];
        Debug.Log("Selecting: " + slot_id + " is " + item_id);
        if (item_id != 0)
        {
            SetInventoryItemDetail(item_id);

            //Update Selected Slot and Item variables.
            current_selected_item = item_id;
            current_selected_slot = slot_id;
        }
    }
}
