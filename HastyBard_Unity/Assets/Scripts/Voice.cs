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

    private bool isProtectOnCooldown = false; // Flag to track if Protect is on cooldown
    public float protectCooldownDuration = 10f; // Cooldown duration for the Protect method
    private Coroutine protectCooldownCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        actions.Add("protect", Protect);

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

    private void Protect()
    {
        if (isProtectOnCooldown)
        {
            Debug.Log("Protect is on cooldown");
            return;
        }

        invulnerabilityScript.StartInvulnerability();
        StartCoroutine(ProtectCooldownCoroutine());
    }

    private IEnumerator ProtectCooldownCoroutine()
    {
        isProtectOnCooldown = true;
        yield return new WaitForSeconds(protectCooldownDuration);
        isProtectOnCooldown = false;
    }
}
