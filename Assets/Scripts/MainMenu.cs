using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject highscoreText;
    public IntVariable gameScore;

    void Start()
    {
        SetHighScore();
    }
    
    public void GoToLoadScene()
    {
        SceneManager.LoadSceneAsync("World-1-1", LoadSceneMode.Single);
    }

    void SetHighScore(){
        highscoreText.GetComponent<TextMeshProUGUI>().text = "TOP-" + gameScore.previousHighestValue.ToString("D6");
    }
    public void ResetHighScore()
    {
        GameObject eventSystem = GameObject.Find("EventSystem");
        eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
        gameScore.ResetHighestValue();
        SetHighScore();
    }
}
