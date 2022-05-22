using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobber : MonoBehaviour
{
    [Tooltip("The PlayerController script attached to the player gameobject")]
    [SerializeField] private PlayerController playerControllerScript;

    public float bobbingSpeed = 0.06f;
    public float bobbingAmount = 0.1f;
    public float midpoint = 1.5f;

    private float timer = 0f;
	
	// Update is called once per frame
	private void Update ()
    {
        float waveslice = 0.0f;
        float horizontal = Input.GetAxis("Horizontal Movement");
        float vertical = Input.GetAxis("Vertical Movement");

        if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
        {
            timer = 0.0f;
        }
        else
        {
            waveslice = Mathf.Sin(timer);
            timer = timer + bobbingSpeed;

            if(timer > Mathf.PI * 2)
            {
                timer = timer - (Mathf.PI * 2);
            }
        }

        if (waveslice != 0)
        {
            float translateChange = waveslice * bobbingAmount;
            float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);

            totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
            translateChange = totalAxes * translateChange;

            transform.position = new Vector3(transform.position.x, midpoint + translateChange, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, midpoint, transform.position.z);
        }

	}
}
