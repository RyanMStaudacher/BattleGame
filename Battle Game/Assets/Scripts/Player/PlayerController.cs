using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Tooltip("The camera of the player")]
    [SerializeField] private GameObject playerCameraObject;

    [Tooltip("Tyhe animator of the player")]
    [SerializeField] private Animator playerAnimator;

    [Tooltip("The HeadBobber script attached to the Main Camera")]
    [SerializeField] private HeadBobber playerHeadBobberScript;

    [Tooltip("The ChangePerspective script attached to the Main Camera")]
    [SerializeField] private ChangePerspective playerChangePerspectiveScript;

    [Tooltip("How fast or slow the player can look around")]
    [SerializeField] private float lookSpeed = 10f;

    [Tooltip("The walk speed of the player")]
    [SerializeField] private float walkSpeed = 300f;

    [Tooltip("How high the player can jump")]
    [SerializeField] private float jumpHeight = 8f;

    private Rigidbody playerRigidbody;
    private RaycastHit groundHit;
    private float maxVelocityChange = 10.0f;
    private float gravity = 9.8f;
    private float currentVelocitySpeed = 0f;
    private float inputH;
    private float inputV;
    private bool isCrouched = false;
    private bool grounded = true;
    private bool canJump = true;

    // Use this for initialization
    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        playerRigidbody.freezeRotation = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    private void Update()
    {

    }

    private void FixedUpdate()
    {
        PlayerLook();
        PlayerMovement();
    }

    /// <summary>
    /// Controls the player's ability to look around
    /// </summary>
    private void PlayerLook()
    {
        //Horizontal
        if (Input.GetAxisRaw("Mouse X") > 0 || Input.GetAxisRaw("Mouse X") < 0)
        {
            Vector3 playerHorizontalRotation = new Vector3(0f, Input.GetAxisRaw("Mouse X") * lookSpeed * Time.deltaTime, 0f);

            this.gameObject.transform.Rotate(playerHorizontalRotation);
        }

        //Vertical
        if (Input.GetAxisRaw("Mouse Y") > 0 || Input.GetAxisRaw("Mouse Y") < 0)
        {
            Vector3 playerVerticalRotation = new Vector3(Input.GetAxisRaw("Mouse Y") * lookSpeed * Time.deltaTime, 0f, 0f);

            playerCameraObject.transform.Rotate(playerVerticalRotation);
        }
    }

    /// <summary>
    /// Handles the player's movement
    /// </summary>
    private void PlayerMovement()
    {
        //Gets input from axis
        inputH = Input.GetAxisRaw("Horizontal Movement");
        inputV = Input.GetAxisRaw("Vertical Movement");

        //Calculate how fast we should be moving
        Vector3 targetVelocity = new Vector3(inputH, 0f, inputV);
        targetVelocity = transform.TransformDirection(targetVelocity);
        targetVelocity *= walkSpeed;

        //Apply a force that attempts to reach our target velocity
        Vector3 velocity = playerRigidbody.velocity;
        Vector3 velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0f;
        playerRigidbody.AddForce(velocityChange, ForceMode.VelocityChange);

        //Jump
        //Jump();

        //Handle locomotive animations
        playerAnimator.SetFloat("InputH", inputH);
        playerAnimator.SetFloat("InputV", inputV);
    }

    private void Jump()
    {
        if(Input.GetButtonDown("Jump"))
        {
            //Check if can jump
            Vector3 down = transform.TransformDirection(Vector3.down);
            canJump = Physics.Raycast(this.gameObject.transform.position, down, out groundHit, 1f);

#if UNITY_EDITOR
            Debug.DrawRay(this.gameObject.transform.position, down * 1f, Color.green);
#endif

            //Do the jump
            if (canJump)
            {
                playerRigidbody.AddForce(Vector3.up * jumpHeight);
                canJump = false;
            }
        }

        //Check if grounded
        Vector3 downDir = transform.TransformDirection(Vector3.down);
        grounded = Physics.Raycast(this.gameObject.transform.position, downDir, out groundHit, 1f);

#if UNITY_EDITOR
        Debug.DrawRay(this.gameObject.transform.position, downDir * 1f, Color.green);
#endif

        if (grounded)
        {
            playerHeadBobberScript.enabled = true;
        }
        else if (!grounded)
        {
            playerHeadBobberScript.enabled = false;
        }
    }
}
