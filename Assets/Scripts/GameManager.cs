using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    // events
    public UnityEvent gameStart;
    public UnityEvent gameRestart;
    public UnityEvent<int> scoreChange;
    public UnityEvent gameOver;
    private int score = 0;
    public IntVariable gameScore;

    void Start()
    {
        gameScore.SetValue(0);
        SetScore();
        gameStart.Invoke();
        Time.timeScale = 1.0f;
        //subscribe to scene manager scene change
        SceneManager.activeSceneChanged += SceneSetup;
    }

    public void SceneSetup(Scene current, Scene next){
        gameStart.Invoke();
        SetScore();
        Time.timeScale = 1.0f;
    }

    void Update()
    {
    }

    public void GameRestart()
    {
        Debug.Log("GameRestart method called");
        CoinPowerup[] coins = FindObjectsOfType<CoinPowerup>();
        foreach (CoinPowerup coin in coins)
        {
            coin.ResetCoins();
            Debug.Log("Coin reset");
        }
        QuestionBoxPowerupController[] questionBoxes = FindObjectsOfType<QuestionBoxPowerupController>();
        foreach (QuestionBoxPowerupController questionBox in questionBoxes)
        {
            questionBox.ResetQuestionBox();
            Debug.Log("Question box reset");
        }
        MagicMushroomPowerup[] mushrooms = FindObjectsOfType<MagicMushroomPowerup>();
        foreach (MagicMushroomPowerup mushroom in mushrooms)
        {
            mushroom.ResetPowerup();
            Debug.Log("Mushroom reset");
        }
        gameRestart.Invoke();
        gameScore.SetValue(0);
        SetScore();
        Time.timeScale = 1.0f;
    }

    public void IncreaseScore(int increment)
    {
        Debug.Log($"Score increased by {increment}");
        gameScore.ApplyChange(increment);
        Debug.Log($"New Score: {gameScore.Value}");
        SetScore();
    }

    public void SetScore()
    {
        scoreChange.Invoke(gameScore.Value);
    }

    public void GameOver()
    {
        Time.timeScale = 0.0f;
        gameOver.Invoke();
    }
}
