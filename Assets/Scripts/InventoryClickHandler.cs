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

    private GameObject current_hold_item;
    private int current_hold_item_slot_id;

    void Start()
    {
        useBtn.SetActive(false);
        dropBtn.SetActive(false);
        equipBtn.SetActive(false);
        itemsIcon.SetActive(false);

        Button useBtn_action = useBtn.GetComponent<Button>();
        Button dropBtn_action = dropBtn.GetComponent<Button>();
        Button equipBtn_action = equipBtn.GetComponent<Button>();

        //Get usable state of items.

        useBtn_action.onClick.AddListener(() => {
            UseItem(Player, current_selected_slot, current_selected_item);
        });

        dropBtn_action.onClick.AddListener(() => {
            DropItem(Player, current_selected_slot, current_selected_item);
        });

        equipBtn_action.onClick.AddListener(() =>
        {
            EquipItem(Player, current_selected_slot, current_selected_item);
        });

        slots = Player.GetComponent<Inventory>().inventory_slots;
    }

    void Update()
    {

    }

    public void OnPointerClick(PointerEventData PointerEvent)
    {
        if (PointerEvent.button == PointerEventData.InputButton.Left) {

        }
    }

    void UseItem(GameObject Inventory, int slot_id, int obj_id)
    {
        var inventory_data = Player.GetComponent<Inventory>().inventory_data;
        var inventory_items_amounts = Player.GetComponent<Inventory>().inventory_items_amounts;
        if (inventory_items_amounts[slot_id] > 1)
        {
            inventory_items_amounts[slot_id] -= 1;
        }
        else
        {
            if (inventory_items_amounts[slot_id] == 1)
            {
                inventory_data[slot_id] = 0;
                inventory_items_amounts[slot_id] = 0;
                SetInventoryItemDetail(0);
            }
        }

        Debug.Log("You use item id: "+obj_id);
        if(obj_id == 1) {
            Player.GetComponent<AudioSource>().PlayOneShot(taking_pill_sound, PlayerCam.GetComponent<InputSystem>().UISoundVolume);
        }
        Player.GetComponent<Inventory>().DrawIcon();
    }

    void DropItem(GameObject Inventory, int slot_id, int obj_id)
    {
        var inventory_data = Player.GetComponent<Inventory>().inventory_data;
        var inventory_items_amounts = Player.GetComponent<Inventory>().inventory_items_amounts;

        if (slot_id == current_hold_item_slot_id)
        {
            foreach (Transform child in HoldPosition)
            {
                Destroy(child.gameObject);
            }
            current_hold_item = null;
        }

        if (inventory_items_amounts[slot_id] > 1)
        {
            inventory_items_amounts[slot_id] -= 1;
        }
        else
        {
            if (inventory_items_amounts[slot_id] == 1)
            {
                inventory_data[slot_id] = 0;
                inventory_items_amounts[slot_id] = 0;
                SetInventoryItemDetail(0);
            }
        }

        Debug.Log("You drop item from slot id: "+slot_id);

        Player.GetComponent<Inventory>().DrawIcon();

        //Spawning Object.
        ObjectSpawner.GetComponent<ObjectSpawner>().Spawner(obj_id);
    }

    void EquipItem(GameObject Inventory, int slot_id, int obj_id)
    {
        Items itemsComponent = itemsManager.GetComponent<Items>();

        if (itemsComponent != null && obj_id >= 0)
        {
            Item itemData = itemsComponent.items[obj_id];

            if (itemData.prefab != null)
            {
                if (current_hold_item != null)
                {
                    foreach (Transform child in HoldPosition)
                    {
                        Destroy(child.gameObject);
                    }
                    current_hold_item = null;
                    equipBtn.GetComponentInChildren<TMP_Text>().text = "Equip";
                    return;
                }
                // Proceed with instantiation
                current_hold_item = Instantiate(itemData.prefab, HoldPosition.position, HoldPosition.rotation);
                current_hold_item.transform.SetParent(HoldPosition);
                current_hold_item.layer = LayerMask.NameToLayer("Item");

                foreach (Transform child in current_hold_item.transform)
                {
                    child.gameObject.layer = LayerMask.NameToLayer("Item");
                }

                // Access the Rigidbody component (if exists)
                Rigidbody rb = current_hold_item.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    // Disable the Rigidbody's physics influence by making it kinematic
                    rb.isKinematic = true;

                    // Optionally, lock the position and rotation using constraints
                    rb.constraints = RigidbodyConstraints.FreezeAll;
                }

                // Optionally, keep the item at the hold position in case any physics changes it
                Vector3 lockedPosition = current_hold_item.transform.position;
                lockedPosition.y = HoldPosition.position.y;  // Lock to the HoldPosition's Y
                current_hold_item.transform.position = lockedPosition;


                Collider[] colliders = current_hold_item.GetComponents<Collider>();
                foreach (Collider col in colliders)
                {
                    col.enabled = false;  // Disable the collider
                }

                if (current_hold_item != null)
                {
                    equipBtn.GetComponentInChildren<TMP_Text>().text = "Unequip";
                }

                current_hold_item_slot_id = slot_id;
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
        var item_id = Player.GetComponent<Inventory>().inventory_data[slot_id];
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
