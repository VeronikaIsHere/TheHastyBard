using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AS.MicControl;
using System.Diagnostics;
using TMPro;

public class Jumping : MonoBehaviour
{
    public GameObject controller;
    float getLoudness = 0.0f;
    public float amp = 1.0f;  // louder (1.0)
    
    RaycastHit2D hit;
    Rigidbody2D rb;

    float jumpForce = 0.000000000000001f;
    public LayerMask groundLayer;
    private bool isGrounded = false;
    private float jumpingStrength;

    float theVol;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // get rigidbody
        theVol = PlayerPrefs.GetFloat("Volume");
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


        getLoudness = controller.GetComponent<MicControlC>().loudness * amp * theVol;  // get the loudness;
        if (getLoudness > 0.1f * theVol && isGrounded)  // only jump if you make a noise and are not in the air;
        {
            UnityEngine.Debug.Log(getLoudness);
            
            if (getLoudness < 3.5f * theVol)
            {
                
                jumpingStrength = Mathf.Clamp(getLoudness, 0.4f * theVol, 0.8f * theVol);  // min and max strength
            } 
            else
            {
                jumpingStrength = 1.0f * theVol;  // super loud scream
            }
            
            //jumpingStrength = 0.3f;
            
            
            
            rb.AddForce(new Vector2(0, jumpingStrength), ForceMode2D.Impulse);
        }


    }

}
