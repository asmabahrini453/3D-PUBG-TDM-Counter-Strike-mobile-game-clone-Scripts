using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class ScoreManager : MonoBehaviour
{
    [Header("Score Manager")]
    public int kills;
    public int enemyKills;
    public Text playerKillCounter;
    public Text enemyKillCounter;
    public Text MainText;


    public void Awake() //to initialize 0 to the phone everytime the game launches
    {
        if (PlayerPrefs.HasKey("kills"))
        {
            kills = PlayerPrefs.GetInt("0");
        }
        else if (PlayerPrefs.HasKey("enemyKills"))
        {


            enemyKills = PlayerPrefs.GetInt("0");

            
               
        }
    }
    private void Update()
    {
        StartCoroutine(WinOrLose());
    }
    IEnumerator WinOrLose()
    {
        playerKillCounter.text = "" + kills;
        enemyKillCounter.text = "" + enemyKills;

        if(kills >= 10)
        {
            MainText.text = "Blue Team Victory";
            PlayerPrefs.SetInt("kills", kills); //PlayerPrefs : a class that stores the player scores between game sessions
            Time.timeScale = 0f; //stop the time -> pause the game
            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene("TDMRoom"); //reload the scene after 5s
        }
        else if (enemyKills >= 10)
        {
            MainText.text = "Red Team Victory";
            PlayerPrefs.SetInt("enemyKills", enemyKills);
            Time.timeScale = 0f;
            yield return new WaitForSecondsRealtime(5f);
            SceneManager.LoadScene("TDMRoom");
        }
        yield return null;
    }
}
