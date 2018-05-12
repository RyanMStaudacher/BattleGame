using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobber : MonoBehaviour
{
    [Tooltip("How fast the head bobs")]
    [SerializeField] private float bobbingSpeed = 0.25f;

    [Tooltip("The size of the bob")]
    [SerializeField] private float bobbingAmount = 0.1f;

    [Tooltip("The midpoint of the bob")]
    [SerializeField] private float midpoint = 1.5f;

    private float timer = 0f;
	
	// Update is called once per frame
	private void Update ()
    {
        float waveslice = 0.0f;
        float horizontal = Input.GetAxisRaw("Horizontal Movement");
        float vertical = Input.GetAxisRaw("Vertical Movement");

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
