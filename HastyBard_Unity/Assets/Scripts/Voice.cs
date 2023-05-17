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
    public GameObject cooldownIndicator; // Reference to the cooldown indicator GameObject

    private bool isInitialized = false; // Flag to track if the script has been initialized

    // Start is called before the first frame update
    void Start()
    {
        if (isInitialized)
            return;

        actions.Add("protect", Protect);

        if (keywordRecognizer != null)
        {
            keywordRecognizer.Stop();
            keywordRecognizer.Dispose();
        }

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();

        invulnerabilityScript = GetComponentInParent<CharacterInvulnerability>();
        rendererComponent = GetComponent<Renderer>();

        cooldownIndicator.SetActive(false);

        isInitialized = true;
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

        cooldownIndicator.SetActive(true); // Enable the cooldown indicator

        yield return new WaitForSeconds(protectCooldownDuration);

        isProtectOnCooldown = false;

        cooldownIndicator.SetActive(false); // Disable the cooldown indicator
    }
}
