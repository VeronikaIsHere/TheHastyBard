using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AS.MicControl;
using System.Diagnostics;

public class JumpingPitch : MonoBehaviour
{

    public GameObject controller;
    //next we need a float array for easy acces to our spectrumData values. (Alternativly, you can also call the loudness value directly).
    AnimationCurve getSpectrumCurve;
    public float amp = 18f;  // louder           
    public Transform[] objectList = null; //we need a container to place all our objects in.
    float getLoudness = 0.0f;

    RaycastHit2D hit;
    Rigidbody2D rb;

    public float jumpForce = 0.000000000000001f;
    public LayerMask groundLayer;
    private bool isGrounded = false;
    private float jumpingStrength;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // get rigidbody
    }

    // Update is called once per frame
    void Update()
    {
        // Überprüfe, ob der Charakter den Boden berührt
        hit = Physics2D.Raycast(transform.position, Vector2.down, 2.0f, groundLayer);
        if (hit.collider != null)
        {
            float distanceToGround = hit.distance;
            if (distanceToGround < 1.0f) 
            {
                isGrounded = true;
            }
        }
        else
        {
            isGrounded = false;
        }


        //getLoudness = controller.GetComponent<MicControlC>().loudness * amp;  // get the loudness;

        if (controller.GetComponent<MicControlC>().enableSpectrumData && controller.GetComponent<MicControlC>().Initialized && isGrounded)  // only jump if you make a noise and are not in the air;
        {
            //update our float array every frame with the new input value. Use this value in your code.
            getSpectrumCurve = controller.GetComponent<MicControlC>().spectrumCurve;

            if (getLoudness < 3.5f )
            {
                UnityEngine.Debug.Log(getLoudness);
                jumpingStrength = Mathf.Clamp(jumpingStrength, 0.4f, 0.8f);  // min and max strength
            } 
            else
            {
                jumpingStrength = 1.0f;  // super loud scream
            }
            
            
            rb.AddForce(new Vector2(0, jumpingStrength), ForceMode2D.Impulse);
        }


    }

}
