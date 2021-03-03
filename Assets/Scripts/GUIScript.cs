using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GUIScript : MonoBehaviour
{
    public Button tryAgain;
    public TextMeshProUGUI GameOverText;
    public TextMeshProUGUI youWinText;

    HealthSystem playerHealth;

    public Array bads;

    // Start is called before the first frame update
    void Start()
    {
        //Gets number of bad guys in scene based off prefab's tag set
        bads = GameObject.FindGameObjectsWithTag("Bad");
        //grab health system script from player
        playerHealth = GetComponent<HealthSystem>();

        //sets all text and button UI to false
        youWinText.gameObject.SetActive(false);
        tryAgain.gameObject.SetActive(false);
        GameOverText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // if player kills all the enemies you WIN
        if (PlayerTargeting.enemiesKilled >= bads.Length)
        {
            Cursor.lockState = CursorLockMode.None;
            youWinText.gameObject.SetActive(true);
            tryAgain.gameObject.SetActive(true);
        }
        //If player's health is 0 or less you LOSE
        if (playerHealth.isDying)
        {
            Cursor.lockState = CursorLockMode.None;
            GameOverText.gameObject.SetActive(true);
            tryAgain.gameObject.SetActive(true);
        }
    }

    //function used for tryAgain button's restart feature
    public void restartLevel()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
