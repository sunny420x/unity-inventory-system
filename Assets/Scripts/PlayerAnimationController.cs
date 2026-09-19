using UnityEngine;
using Cinemachine;
using StarterAssets;

public class PlayerAnimationController : MonoBehaviour
{
    Animator playerAnimator;

    [SerializeField] private GameObject playerCapsule;
    private CapsuleCollider capsuleCollider;
    [SerializeField] private GameObject PlayerCameraRoot;
    [SerializeField] private FirstPersonController FirstPersonControllerScript;
    [SerializeField] private PlayerStatus PlayerStatus;

    float crouchX, crouchZ;
    float playerCamX, playerCamZ;
    float playerBodyZ;

    public bool isCrouch;
    public bool isCrouchWalking;

    public bool isMoving;
    public bool isRunning;

    Vector3 targetCenter;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        capsuleCollider = playerCapsule.GetComponent<CapsuleCollider>();

        playerCamX = PlayerCameraRoot.transform.localPosition.x;
        playerCamZ = PlayerCameraRoot.transform.localPosition.z;
        crouchX = playerCamX;
        crouchZ = playerCamZ;

        playerBodyZ = gameObject.transform.localPosition.z;
    }

    void Update()
    {
        isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D);
        isRunning = Input.GetKey(KeyCode.LeftShift) && isMoving && PlayerStatus.stamina >= 10;

        Vector3 collider_center = capsuleCollider.center;
        collider_center.y = 0f;

        Vector3 Crouch_collider_center = capsuleCollider.center;
        Crouch_collider_center.y = -0.6f;

        if(Input.GetKeyDown(KeyCode.C)) {
            if(isCrouch) {
                isCrouch = false;
            } else {
                isCrouch = true;
            }
        }
        isCrouchWalking = isMoving && isCrouch;

        float targetHeight = isCrouch ? 1f : 2f;
        capsuleCollider.height = Mathf.Lerp(capsuleCollider.height, targetHeight, Time.deltaTime * 5f);

        if(isCrouch) {
            //Set player camera lower at crouch.
            PlayerCameraRoot.transform.localPosition = new Vector3(crouchX, 0.7f, crouchZ);
            targetCenter = new Vector3(capsuleCollider.center.x, -0.6f, 0.4f);

            //Move Player Model A little bit backward.
            gameObject.transform.localPosition = new Vector3(0f, 0f, playerBodyZ - 0.58f);
        }

        if(!isCrouch) {
            //Set player camera back to normal.
            PlayerCameraRoot.transform.localPosition = new Vector3(playerCamX - 0.1f, 1.8633f, playerCamZ - 0.1f);
            targetCenter = new Vector3(capsuleCollider.center.x, 0f, 0f);

            //Move Player Model back to the starting position.
            gameObject.transform.localPosition = new Vector3(0f, 0f, playerBodyZ);
        }

        capsuleCollider.center = Vector3.Lerp(capsuleCollider.center, targetCenter, Time.deltaTime * 5f);

        playerAnimator.SetBool("isRunning", isRunning);
        playerAnimator.SetBool("isWalking", isMoving && !isRunning);
        playerAnimator.SetBool("isCrouching", isCrouch);
        playerAnimator.SetBool("isCrouchWalking", isCrouch && isCrouchWalking && isMoving);
    }
}
