using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    //for death audio
    public AudioSource stomp;
    private GameManager gameManager;

    // Delegate for Goomba stomping
    public delegate void GoombaStompHandler(GameObject goomba);
    // Event that will be triggered when a Goomba is stomped
    public static event GoombaStompHandler OnGoombaStomped;

    void Awake()
    {
        GameManager.instance.gameRestart.AddListener(GameRestart);
    }

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
    }

    public void HandleGoombaStomped(GameObject goomba)
    {
        // Play stomp sound
        if (stomp != null)
        {
            stomp.Play();
        }

        // Trigger the event
        if (OnGoombaStomped != null)
        {
            OnGoombaStomped(goomba);
        }

        // Increase score
        if (gameManager != null)
        {
            gameManager.IncreaseScore(1);
        }
    }

    public void GameRestart()
    {
        //GetComponentsInChildren<Transform>(true) to include inactive children
        Transform[] children = GetComponentsInChildren<Transform>(true); 
        foreach (Transform child in children)
        {
            if (child.gameObject != gameObject){
                child.gameObject.SetActive(true);
                EnemyMovement enemyMovement = child.GetComponent<EnemyMovement>();
                if (enemyMovement != null)
                {
                    enemyMovement.GameRestart();
                }
            }
        }
    }
}