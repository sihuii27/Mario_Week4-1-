using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private float originalX;
    private float maxOffset = 5.0f;
    private float enemyPatroltime = 2.0f;
    private int moveRight = -1;
    private Vector2 velocity;
    private Rigidbody2D enemyBody;
    private bool isDead = false;
    private Animator goombaAnimator;
    private EnemyManager enemyManager;
    private Vector3 originalposition;

    void Awake()
    {
        GameManager.instance.gameRestart.AddListener(GameRestart);
    }

    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        originalX = transform.position.x;
        originalposition = transform.position;
        ComputeVelocity();
        goombaAnimator = transform.parent.GetComponent<Animator>();
        
        // Get EnemyManager from the NPC GameObject (two levels up)
        Debug.Log("Looking for EnemyManager...");
        if (transform.parent != null && transform.parent.parent != null)
        {
            enemyManager = transform.parent.parent.GetComponent<EnemyManager>();
        }
        else
        {
            Debug.LogError("Parent hierarchy is not correct! Need Goomba -> Enemies -> NPC");
        }
    }

    void ComputeVelocity()
    {
        velocity = new Vector2((moveRight) * maxOffset / enemyPatroltime, 0);
    }

    void Movegoomba()
    {
        if (!isDead){
            enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
        }
    }

    void Update()
    {
        if (Mathf.Abs(enemyBody.position.x - originalX) < maxOffset)
        {
            Movegoomba();
        }
        else
        {
            moveRight *= -1;
            ComputeVelocity();
            Movegoomba();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger detected with: " + other.gameObject.name);
        
        if (other.CompareTag("Obstacles") || other.CompareTag("Pipe") || other.CompareTag("Enemy"))
        {
            moveRight *= -1;
            ComputeVelocity();
        }

        if (!isDead && other.CompareTag("Player"))
        {            
            // Calculate vertical difference
            float marioY = other.transform.position.y;
            float goombaY = transform.position.y;
            float verticalDifference = marioY - goombaY;
                        
            // If Mario is above the Goomba
            if (verticalDifference > 0.5f)
            {
                Stomped();
                
                // Add upward force to Mario
                Rigidbody2D marioBody = other.GetComponent<Rigidbody2D>();
                if (marioBody != null)
                {
                    marioBody.velocity = new Vector2(marioBody.velocity.x, 10);
                }
            }
        }
    }

    public void Stomped()
    {
        if (!isDead)
        {
            isDead = true;
            enemyBody.velocity = Vector2.zero;
            GetComponent<Collider2D>().enabled = false;

            // Play animation
            if (goombaAnimator != null)
            {
                Debug.Log("stomp!");
                goombaAnimator.SetTrigger("stomp");
            }

            if (enemyManager != null)
            {
                enemyManager.HandleGoombaStomped(gameObject);
            }
            Invoke("removeEnemy", 0.5f);
        }
    }

    void removeEnemy()
    {
        gameObject.SetActive(false);
    }

    public void GameRestart()
    {
        CancelInvoke("removeEnemy");
        isDead = false;
        GetComponent<Collider2D>().enabled = true;
        gameObject.SetActive(true);
        transform.position = originalposition;
        originalX = transform.position.x;
        moveRight = -1;
        ComputeVelocity();
        
        if (goombaAnimator != null)
        {
            goombaAnimator.SetTrigger("gameRestart");
        }
    }
}