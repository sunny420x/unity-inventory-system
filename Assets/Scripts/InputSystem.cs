using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputSystem : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject interactPanel;
    public TMP_Text interactionText;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private GameObject ItemHoldPosition;

    [Header("Pause Menu")]
    public GameObject pauseMenu;
    public GameObject Background;
    public GameObject pauseMenuPanel;
    public GameObject settingsMenu;
    public Slider volumeSlider;
    public TMP_Text volumeValue;

    [Header("UI Status")]
    public bool pauseMenuStatus = false;
    public bool pauseMenuPanelStatus = false;
    public bool noteStatus = false;
    public bool dialogStatus = false;

    [Header("Note Canvas")]
    [SerializeField] private GameObject notePanel;
    [SerializeField] private TMP_Text noteContent;

    [Header("Dialog Canvas")]
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private TMP_Text dialogContent;

    [Header("Sounds Effects")]
    [SerializeField] private AudioClip clickSoundEffect;
    [SerializeField] private AudioClip itemPickUpClip;
    [SerializeField] private AudioClip notePickUpClip;

    [Header("Player Audio Source")]
    public AudioSource playerAudio;
    public float UISoundVolume = 1f;

    [Header("Developer Options")]
    [SerializeField] private bool checkHit = true;

    private bool isHold = false;
    private GameObject carryingObj;

    void Start()
    {
        interactPanel.SetActive(false);

        pauseMenu.SetActive(pauseMenuStatus);
        pauseMenuPanel.SetActive(pauseMenuPanelStatus);
        notePanel.SetActive(noteStatus);
        Background.SetActive(false);
        dialogPanel.SetActive(dialogStatus);
    }

    void ShowNote(string Contents)
    {
        PauseGame();
        Cursor.lockState = CursorLockMode.Locked;

        //Check if Dialog Panel is Active.
        if(dialogStatus == false) 
        {
            if(noteStatus == false)
            {
                playerAudio.PlayOneShot(notePickUpClip, UISoundVolume); //Play Note Pickup Sound Effect.
                noteStatus = true;
            }
            notePanel.SetActive(noteStatus);
            noteContent.text = Contents.ToString();
        } else
        {
            dialogStatus = false;
            dialogPanel.SetActive(dialogStatus);
        }
    }

    void Dialog(string content)
    {
        PauseGame();

        Cursor.lockState = CursorLockMode.Locked;

        dialogStatus = true;
        dialogPanel.SetActive(dialogStatus);
        // dialogTitle.text = title.ToString();
        dialogContent.text = content.ToString();
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //Raycasting to check interactable objects.
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            switch (hit.transform.gameObject.tag)
            {
                case "Interact":
                    var distance = Vector3.Distance(hit.transform.gameObject.GetComponent<Collider>().bounds.center, transform.position);
                    if (distance < 1.414)
                    {
                        var ObjScript = hit.transform.GetComponent<ObjectDetail>();

                        if(ObjScript.type == "Holdable") {
                            interactPanel.SetActive(true);
                            if(!isHold) {
                                interactionText.text = "Press [LeftClick] to Hold.";
                            }

                            if(Input.GetMouseButtonDown(0)) {
                                if(!isHold) {
                                    carryingObj = hit.transform.gameObject;
                                    isHold = true;
                                }
                            }
                        }                        

                        //Items
                        if (ObjScript.type == "Item")
                        {
                            interactPanel.SetActive(true);
                            interactionText.text = "Press [E] to Pick up.";

                            if (Input.GetKeyDown(KeyCode.E))
                            {
                                Destroy(hit.transform.gameObject);
                                playerAudio.PlayOneShot(itemPickUpClip, UISoundVolume);
                                player.GetComponent<Inventory>().AddtoInventory(ObjScript.obj_id);
                            }
                        }

                        //Note
                        if (ObjScript.type == "Note")
                        {
                            interactPanel.SetActive(true);
                            interactionText.text = "Press [E] to Read.";

                            if(pauseMenuStatus == false)
                            {
                                if (Input.GetKeyDown(KeyCode.E))
                                {
                                    ShowNote(ObjScript.read_contents);
                                    if(hit.transform.gameObject.GetComponent<ObjectDetail>().obj_id == 5) {
                                    }
                                }
                            }
                        }

                        //Dialog
                        if (ObjScript.type == "Talk")
                        {
                            interactPanel.SetActive(true);
                            interactionText.text = "Press [E] to Talk.";

                            if (pauseMenuStatus == false) //Check if Pause Menu is Active.
                            {
                                if (Input.GetKeyDown(KeyCode.E))
                                {
                                    Dialog(ObjScript.read_contents);
                                }
                            }
                        }
                    } else {
                        interactionText.text = "";
                    }
                    //Close
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        noteStatus = false;
                        dialogStatus = false;
                        notePanel.SetActive(noteStatus);
                        Background.SetActive(noteStatus);
                        dialogPanel.SetActive(dialogStatus);
                        ResumeGame();
                        Cursor.lockState = CursorLockMode.Locked;
                    }

                    //Developer Option
                    if (checkHit == true)
                    {
                        Debug.Log("Hitting --> Interectable Object");
                    }
                    break;

                default:
                    //e_btn_status = false;
                    interactPanel.SetActive(false);
                    interactionText.text = "";

                    if (checkHit == true) 
                    {
                        Debug.Log("Hitting --> " + hit.transform.gameObject.tag);
                    }

                    break;
            }
        }

        //Holding Item
        if(isHold) {
            interactPanel.SetActive(true);
            interactionText.text = "Press [Q] to Drop.";
            if(Input.GetKeyDown(KeyCode.Q)) {
                isHold = false;
                carryingObj.GetComponent<Collider>().isTrigger = false;
                carryingObj = null;
                return;
            }
            carryingObj.GetComponent<Collider>().isTrigger = true;
            carryingObj.transform.position = new Vector3(ItemHoldPosition.transform.position.x, ItemHoldPosition.transform.position.y, ItemHoldPosition.transform.position.z);
        }

        //Pause Menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(dialogStatus == false && noteStatus == false)
            {
                if (pauseMenuStatus == false)
                {
                    pauseMenuStatus = true;
                    pauseMenu.SetActive(pauseMenuStatus);
                    PauseGame();
                }
                else
                {
                    pauseMenuStatus = false;
                    pauseMenu.SetActive(pauseMenuStatus);
                    ResumeGame();
                }
            }
        }
    }
    public void playSoundEffect() {
        playerAudio.PlayOneShot(clickSoundEffect, UISoundVolume);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        playerCamera.SetActive(false); //Prevent player camera to move
        player.transform.Find("PlayerCapsule").gameObject.SetActive(false); //Prevent player body to move with cursor.
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        playerCamera.SetActive(true); //Allow player camera to move
        player.transform.Find("PlayerCapsule").gameObject.SetActive(true); //Allow player body to move.
        Cursor.lockState = CursorLockMode.Locked;
    }
}
