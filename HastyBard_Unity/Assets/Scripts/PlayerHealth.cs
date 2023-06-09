using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{

    public GameObject DieBlock;
    public GameObject[] hearts;
    int heartnumber;
    bool deleteheart;

    CharacterInvulnerability invulnerabilityScript; // Reference to the CharacterInvulnerability script

    // Start is called before the first frame update
    void Start()
    {
        heartnumber = hearts.Length;
        deleteheart = false;

        invulnerabilityScript = GetComponent<CharacterInvulnerability>(); // Get the CharacterInvulnerability script attached to Player GameObject
    }

    // Update is called once per frame
    void Update()
    {
        if(deleteheart == true)
        {
            hearts[heartnumber].gameObject.SetActive(false);  // remove ui heart
            deleteheart=false;

            if (heartnumber <= 0)
            {
                SceneManager.LoadScene("replayscreen");  // game over
            }

        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "DieGround")  // if colliding with dieground
        {
            SceneManager.LoadScene("replayscreen");  // game over
        } 
        else if (other.gameObject.tag == "Win")  // if colliding with win
        {
            SceneManager.LoadScene("winscreen");  // win
        } 
        else if (other.gameObject.tag == "nextLevel2")
        {
            SceneManager.LoadScene("theLevel_2");  // win
        }
        else if (other.gameObject.tag == "nextLevel3")
        {
            SceneManager.LoadScene("theLevel_3");  // win
        }
        else if(other.gameObject.tag == "enemy")  // collision with enemy
        {

            if (!invulnerabilityScript.IsInvulnerable()) // Check if the character is currently invulnerable
            {
                heartnumber--;
                deleteheart = true;

            }


        }

    }

}
