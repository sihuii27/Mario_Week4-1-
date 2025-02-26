using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpOverGoomba : MonoBehaviour
{
    private bool countScoreState = false;
    private PlayerMovement mario;
    private Animator goombaAnimator;
    private GameManager gameManager;
    private EnemyMovement enemyMovement;
    private Rigidbody2D marioBody;

    void Start()
    {
        // Get references to required components
        mario = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        if (mario != null)
        {
            marioBody = mario.GetComponent<Rigidbody2D>();
        }
        
        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        
        // Get the enemy movement component from this GameObject
        enemyMovement = GetComponent<EnemyMovement>();
        goombaAnimator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && mario != null)
        {
            // Calculate the vertical distance between Mario and Goomba
            float marioY = other.transform.position.y;
            float goombaY = transform.position.y;
            float verticalDifference = marioY - goombaY;

            // If Mario is above the Goomba and falling down
            if (verticalDifference > 0.5f && marioBody.velocity.y <= 0)
            {
                Stomp();
                // Add a small upward force to Mario
                marioBody.velocity = new Vector2(marioBody.velocity.x, 10);
            }
        }
    }

    void Stomp()
    {
        if (goombaAnimator != null)
        {
            goombaAnimator.SetTrigger("stomp");
        }
        
        if (enemyMovement != null)
        {
            enemyMovement.Stomped();
        }
        
        if (gameManager != null)
        {
            countScoreState = true;
            gameManager.IncreaseScore(1);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position - transform.up * 0.5f, new Vector3(1, 1, 1));
    }
}
