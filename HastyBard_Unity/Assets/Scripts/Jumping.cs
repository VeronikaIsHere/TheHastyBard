using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AS.MicControl;
using System.Diagnostics;

public class Jumping : MonoBehaviour
{

    public GameObject controller;
    float getLoudness = 0.0f;
    public float amp = 100000000000000f;  // louder
    
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


        getLoudness = controller.GetComponent<MicControlC>().loudness * amp;  // get the loudness;

        if (getLoudness > 0.0001f && isGrounded)  // only jump if you make a noise and are not in the air;
        {
            UnityEngine.Debug.Log(getLoudness);
            if (getLoudness < 3.5f )
            {
                
                jumpingStrength = Mathf.Clamp(getLoudness, 0.4f, 0.8f);  // min and max strength
            } 
            else
            {
                jumpingStrength = 1.0f;  // super loud scream
            }
            
            jumpingStrength = 1.0f;
            
            
            rb.AddForce(new Vector2(0, jumpingStrength), ForceMode2D.Impulse);
        }


    }

}
