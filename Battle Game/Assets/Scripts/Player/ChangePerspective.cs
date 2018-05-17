using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePerspective : MonoBehaviour
{
    [Tooltip("The PlayerController script attached to the player gameobject")]
    [SerializeField] private PlayerController playerControllerScript;

    public static event Action<bool> SwitchedPerspectives;

    private HeadBobber headBobberScript;
    private Camera playerCamera;
    private bool isInFirstPerson = false;

	// Use this for initialization
	void Start ()
    {
        headBobberScript = GetComponent<HeadBobber>();
        playerCamera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        HandlePerspectiveToggle();
	}

    private void HandlePerspectiveToggle()
    {
        if(!isInFirstPerson && Input.GetButtonDown("ChangePerspective"))
        {
            SwitchToFirstPerson();
        }
        else if (isInFirstPerson && Input.GetButtonDown("ChangePerspective"))
        {
            SwitchToThirdPerson();
        }
    }

    private void SwitchToFirstPerson()
    {
        headBobberScript.enabled = true;
        playerCamera.cullingMask ^= 1 << LayerMask.NameToLayer("Player");
        Vector3 cameraPosition = new Vector3(0f, 1.5f, 0f);
        Vector3 cameraRotation = new Vector3(0f, 0f, 0f);
        this.gameObject.transform.localPosition = cameraPosition;
        this.gameObject.transform.localRotation = Quaternion.Euler(cameraRotation);
        isInFirstPerson = true;
    }

    private void SwitchToThirdPerson()
    {
        headBobberScript.enabled = false;
        playerCamera.cullingMask ^= 1 << LayerMask.NameToLayer("Player");
        Vector3 newCameraPosition = new Vector3(0f, 2.25f, -3f);
        Vector3 newCameraRotation = new Vector3(10f, 0f, 0f);
        this.gameObject.transform.localPosition = newCameraPosition;
        this.gameObject.transform.localRotation = Quaternion.Euler(newCameraRotation);
        isInFirstPerson = false;
    }
}
