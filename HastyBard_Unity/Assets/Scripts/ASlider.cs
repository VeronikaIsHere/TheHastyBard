using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class ASlider : MonoBehaviour
{
    public Slider mainSlider;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetFloat("Volume", 1.0f);
        mainSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Invoked when the value of the slider changes.
    public void ValueChangeCheck()
    {
        UnityEngine.Debug.Log(mainSlider.value);
        PlayerPrefs.SetFloat("Volume", mainSlider.value);
    }

}
