using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Tooltip("The camera of the player")]
    [SerializeField] private GameObject playerCameraObject;

    [Tooltip("How fast or slow the player can look around")]
    [SerializeField] private float lookSpeed = 10f;

    [Tooltip("The walk speed of the player")]
    [SerializeField] private float walkSpeed = 300f;

    [Tooltip("How high the player can jump")]
    [SerializeField] private float jumpSpeed = 8f;

    private CharacterController playerCharacterController;
    private float gravity = 9.8f;
    private float currentVelocitySpeed = 0f;

    // Use this for initialization
    private void Start()
    {
        playerCharacterController = GetComponent<CharacterController>();
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
        if(Input.GetKeyDown(KeyCode.T))
        {
            currentVelocitySpeed = 0f;
            currentVelocitySpeed = jumpSpeed;
        }
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
        if(Input.GetAxisRaw("Mouse Y") > 0 || Input.GetAxisRaw("Mouse Y") < 0)
        {
            Vector3 playerVerticalRotation = new Vector3(Input.GetAxisRaw("Mouse Y") * lookSpeed * Time.deltaTime, 0f,0f);

            playerCameraObject.transform.Rotate(playerVerticalRotation);
        }
    }

    private void PlayerMovement()
    {
        Vector3 horizontalDir = transform.TransformDirection(Vector3.right);
        Vector3 verticalDir = transform.TransformDirection(Vector3.forward);
        Vector3 jumpVelocity = new Vector3(0,0,0);

        //Horizontal
        if (Input.GetAxisRaw("Horizontal Movement") > 0)
        {
            playerCharacterController.SimpleMove(horizontalDir * walkSpeed * Time.deltaTime);
        }
        else if (Input.GetAxisRaw("Horizontal Movement") < 0)
        {
            playerCharacterController.SimpleMove(-horizontalDir * walkSpeed * Time.deltaTime);
        }

        //Vertical
        if (Input.GetAxisRaw("Vertical Movement") > 0)
        {
            playerCharacterController.SimpleMove(verticalDir * walkSpeed * Time.deltaTime);
        }
        else if (Input.GetAxisRaw("Vertical Movement") < 0)
        {
            playerCharacterController.SimpleMove(-verticalDir * walkSpeed * Time.deltaTime);
        }

        //Applies gravity to the player
        currentVelocitySpeed -= gravity * Time.deltaTime;
        verticalDir.y = currentVelocitySpeed;
        playerCharacterController.Move(verticalDir * Time.deltaTime);
    }
}
