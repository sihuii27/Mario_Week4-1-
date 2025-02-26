using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public GameConstants gameConstants;
    private Rigidbody2D marioBody;
    float speed;
    float upSpeed;
    float maxSpeed;
    float deathImpulse;
    private bool onGroundState = true;
    public bool OnGroundState { get { return onGroundState; } }
    private SpriteRenderer marioSprite;
    private bool faceRightState = true; //by default Mario is facing right
    public TextMeshProUGUI scoreText;
    public GameObject enemies;
    
    //for death audio
    public AudioSource deathAudio;
    //for jump audio
    public AudioSource jumpAudio;

    public AudioClip marioDeath;
    public AudioClip marioJump;

    public Transform gameCamera;
    int collisionLayerMask = (1<<3) | (1<<6) | (1<<7);

    //state
    [System.NonSerialized]
    public bool alive = true;

    //Week2
    public Animator marioAnimator;
    private bool moving = false;
    private bool jumpedState = false;

    void Awake(){
        GameManager.instance.gameRestart.AddListener(RestartButtonCallback);
    }

    // Start is called before the first frame update
    void Start()
    {
        speed = gameConstants.speed;
        maxSpeed = gameConstants.maxSpeed;
        deathImpulse = gameConstants.deathImpulse;
        upSpeed = gameConstants.upSpeed;
        //Set to be 30 FPS
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
        marioAnimator.SetBool("onGround", onGroundState);
        // subscribe to scene manager scene change
        SceneManager.activeSceneChanged += SetStartingPosition;
    }

    public void SetStartingPosition(Scene current, Scene next)
    {
        if (this == null || gameObject == null) return;
        if (next.name == "World-1-2")
        {
            // change the position accordingly in your World-1-2 case
            this.transform.position = new Vector3(-6.54f, -5.47f, 0.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));
    }

    void FlipMarioSprite(int value)
    {
        if (value == -1 && faceRightState)
        {
            faceRightState = false;
            marioSprite.flipX = true;
            if (marioBody.velocity.x > 0.05f)
            {
                marioAnimator.SetTrigger("onSkid");
            }
        }
        else if (value == 1 && !faceRightState)
        {
            faceRightState=true;
            marioSprite.flipX = false; //flip sprite to face right
            if (marioBody.velocity.x < -0.05f)
            {
                marioAnimator.SetTrigger("onSkid");
            }
        }
    }

    // Collider callback function, OnCollision2D.
    void OnCollisionEnter2D(Collision2D col)
    {
        if (((collisionLayerMask & (1 << col.transform.gameObject.layer)) > 0) & !onGroundState)
        {
            if(!alive){
                return;
            }
            //Mario is alive
            float blockEntityY = col.transform.position.y;
            float marioY = transform.position.y;
            float verticalDifference = marioY - blockEntityY;

            if ((col.gameObject.CompareTag("Obstacles") && verticalDifference >= -3f) ||
            (!col.gameObject.CompareTag("Obstacles") && marioBody.velocity.y <= 0))
            {
                onGroundState = true;
                jumpedState = false;
                //update animator state
                marioAnimator.SetBool("onGround", onGroundState);
            }
        }
    }

    // FixedUpdate is called 50 times a second and it is about Physics Engine
    void FixedUpdate()
    {
        if (alive && moving)
        {
            Move(faceRightState == true ? 1 : -1);
        }
    }

    void Move(int value)
    {
        Vector2 movement = new Vector2(value, 0);
        //check if it doesn't go beyond maxSpeed
        if (marioBody.velocity.magnitude < maxSpeed)
        {
            marioBody.AddForce(movement * speed);
        }
    }

    public void MoveCheck(int value)
    {
        if (value == 0)
        {
            moving = false;
        }
        else
        {
            FlipMarioSprite(value);
            moving = true;
            Move(value);
        }
    }

    public void Jump()
    {
        Debug.Log("Jump method called");
        if (alive && onGroundState)
        {
            // jump
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            jumpedState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);

        }
    }

    public void JumpHold()
    {
        if (alive && jumpedState)
        {
            // jump higher
            marioBody.AddForce(Vector2.up * upSpeed * 30, ForceMode2D.Force);
            jumpedState = false;

        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy") && alive)
        {
            Vector2 marioCenter = transform.position;
            Vector2 goombaCenter = other.transform.position;
            if (marioCenter.y <= goombaCenter.y)
            {
                Debug.Log("Collided with goomba!");
                // play death animation
                marioAnimator.Play("mario-die");
                if (deathAudio != null)
                {
                    deathAudio.PlayOneShot(marioDeath);
                }
                PlayDeathImpulse();
                onGroundState = false; //mario not standing on ground
                alive = false;
                GameManager.instance.GameOver();
            }
            
        }
    }

    public void RestartButtonCallback()
    {
        Debug.Log("Restart!");
        // reset everything
        ResetGame();
        if (marioBody != null)
        {
            marioBody.velocity = Vector2.zero;
        }
        // resume time
        Time.timeScale = 1.0f;
    }

    private void ResetGame()
    {
        // reset position
        marioBody.transform.position = new Vector3(-9.26f, -4.9f, 0.0f);
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;

        // reset animation
        marioAnimator.SetTrigger("gameRestart");
       
        alive = true;
        //reset camera position
        gameCamera.position = new Vector3(0, 0, -10);
    }

    void PlayJumpSound()
    {
        //play mario jump sound
        jumpAudio.PlayOneShot(marioJump);
    }

    //death
    void PlayDeathImpulse()
    {
        marioBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);
    }

    void OnDestroy()
    {
        SceneManager.activeSceneChanged -= SetStartingPosition;
    }

}