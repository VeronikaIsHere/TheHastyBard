using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AS.MicControl;
using System.Diagnostics;
using UnityEngine.Audio;
using System.Runtime.InteropServices;

public class JumpingPitch : MonoBehaviour
{

    public GameObject controller;
    float getLoudness = 0.0f;
    public float amp = 1.0f;  // louder

    RaycastHit2D hit;
    Rigidbody2D rb;

    public LayerMask groundLayer;
    private bool isGrounded = false;
    //private float jumpingStrength = 0.1f;


    [DllImport("AudioPluginDemo")]
    private static extern float PitchDetectorGetFreq(int index);

    [DllImport("AudioPluginDemo")]
    private static extern int PitchDetectorDebug(float[] data);

    float[] history = new float[1000];
    float[] debug = new float[65536];

    string[] noteNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };

    public Material mat;
    public string frequency = "detected frequency";
    public string note = "detected note";
    public AudioMixer mixer;
    public InfoText pitchText;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // get rigidbody
    }

    // Update is called once per frame
    void Update()
    {
        float freq = PitchDetectorGetFreq(0), deviation = 0.0f;
        frequency = freq.ToString() + " Hz";
        

        if (freq > 0.0f)
        {
            float noteval = 57.0f + 12.0f * Mathf.Log10(freq / 440.0f) / Mathf.Log10(2.0f);
            float f = Mathf.Floor(noteval + 0.5f);
            deviation = Mathf.Floor((noteval - f) * 100.0f);
            int noteIndex = (int)f % 12;
            int octave = (int)Mathf.Floor((noteval + 0.5f) / 12.0f);
            note = noteNames[noteIndex] + " " + octave;
        }
        else
        {
            note = "unknown";
        }

        if (pitchText != null)
        {
            pitchText.text = "Detected frequency: " + frequency + "\nDetected note: " + note + " (deviation: " + deviation + " cents)";
            //UnityEngine.Debug.Log("Freq 1: " + frequency);  // hier kennt ers noch
        }
            

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

        // UnityEngine.Debug.Log("Freq 2: " + freq); // hier gehts
        getLoudness = controller.GetComponent<MicControlC>().loudness * amp;  // get the loudness;
        
        if (getLoudness > 0.1f && isGrounded)  // only jump if you make a noise and are not in the air;
        {
            UnityEngine.Debug.Log("Freq 3: " + freq);  // das kennt er auch noch
            float jumpingStrength = freq * 0.001f;
            //UnityEngine.Debug.Log("Str: " + jumpingStrength);
            //UnityEngine.Debug.Log("Loudness: " + getLoudness);
            //UnityEngine.Debug.Log("Strength: " + freq + "*" + 0.001 + "=" + jumpingStrength);

            rb.AddForce(new Vector2(0, jumpingStrength), ForceMode2D.Impulse);
        }


    }

}
