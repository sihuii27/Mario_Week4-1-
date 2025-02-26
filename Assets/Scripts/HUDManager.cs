// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;
// using UnityEngine.SceneManagement;

// public class HUDManager : MonoBehaviour
// {
//     public TextMeshProUGUI finalScore;
//     public TextMeshProUGUI scoreText;
//     public GameObject gameOverUI;
//     public GameObject replayButton; 
//     public GameObject replayButton1; 
//     private int currentScore = 0;
//     private GameManager gameManager;
//     public GameObject highscoreText;
//     public IntVariable gameScore;

//     void Awake()
//     {
//         GameManager.instance.gameStart.AddListener(GameStart);
//         GameManager.instance.gameOver.AddListener(gameOver);
//         GameManager.instance.gameRestart.AddListener(GameStart);
//         GameManager.instance.scoreChange.AddListener(SetScore);

//     }

//     void Start()
//     {
//         gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
//     }

//     void Update()
//     {
//     }

//     public void GameStart()
//     {
//         gameOverUI.SetActive(false);
//         if (scoreText != null)
//         {
//             scoreText.gameObject.SetActive(true);
//         }
            
//         if (replayButton != null){
//             replayButton.SetActive(true);
//         }
            
//     }

//     public void SetScore(int score)
//     {
//         currentScore = score;
//         if (scoreText != null)
//         {
//             scoreText.text = "Score: " + score.ToString();
//         }
//         if (finalScore != null)
//         {
//             finalScore.text = "Score: " + score.ToString();
//         }
//     }

//     public void gameOver()
//     {
//         gameOverUI.SetActive(true);
//         finalScore.text = "Score: " + currentScore.ToString();
        
//         if (scoreText != null)
//         {
//             scoreText.gameObject.SetActive(false);
//         }
            
//         if (replayButton != null)
//         {
//             replayButton.SetActive(false);
//         }

//         if (replayButton1 != null)
//         {
//             replayButton1.SetActive(true);
//         }

//         // set highscore
//         highscoreText.GetComponent<TextMeshProUGUI>().text = "TOP- " + gameScore.previousHighestValue.ToString("D6");
//         // show
//         highscoreText.SetActive(true);
//     }
// }
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour
{
    public TextMeshProUGUI finalScore;
    public TextMeshProUGUI scoreText;
    public GameObject gameOverUI;
    public GameObject replayButton; 
    public GameObject replayButton1; 
    private GameManager gameManager;
    public GameObject highscoreText;
    public IntVariable gameScore;  // This will be our only score tracker

    void Awake()
    {
        GameManager.instance.gameStart.AddListener(GameStart);
        GameManager.instance.gameOver.AddListener(gameOver);
        GameManager.instance.gameRestart.AddListener(GameStart);
        GameManager.instance.scoreChange.AddListener(SetScore);
    }

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
    }

    public void GameStart()
    {
        gameOverUI.SetActive(false);
        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(true);
        }
            
        if (replayButton != null){
            replayButton.SetActive(true);
        }
    }

    public void SetScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + gameScore.Value.ToString();
        }
        if (finalScore != null)
        {
            finalScore.text = "Score: " + gameScore.Value.ToString();
        }
    }

    public void gameOver()
    {
        gameOverUI.SetActive(true);
        finalScore.text = "Score: " + gameScore.Value.ToString();
        
        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(false);
        }
            
        if (replayButton != null)
        {
            replayButton.SetActive(false);
        }

        if (replayButton1 != null)
        {
            replayButton1.SetActive(true);
        }

        // set highscore
        highscoreText.GetComponent<TextMeshProUGUI>().text = "TOP- " + gameScore.previousHighestValue.ToString("D6");
        // show
        highscoreText.SetActive(true);
    }
}