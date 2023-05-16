using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class Voice : MonoBehaviour
{

    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();
    private CharacterInvulnerability invulnerabilityScript; // Reference to the CharacterInvulnerability script
    private Renderer rendererComponent;

    // Start is called before the first frame update
    void Start()
    {
        actions.Add("block", Block);
        actions.Add("forward", Forward);

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();

        invulnerabilityScript = GetComponentInParent<CharacterInvulnerability>(); // Get the CharacterInvulnerability script attached to the parent GameObject
        rendererComponent = GetComponent<Renderer>();


    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        actions[speech.text].Invoke();
    }

    private void Forward()
    {
        invulnerabilityScript.StartInvulnerability();
    }

    private void Block()
    {
        transform.Translate(0, 5, 0);
    }



    // Update is called once per frame
    void Update()
    {

    }
}
