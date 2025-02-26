using UnityEngine;

public class QuestionBoxPowerupController : MonoBehaviour, IPowerupController
{
    public BasePowerup powerup; // reference to this question box's powerup
    private Animator boxAnimator; // reference to the question box's animator
    public AudioSource PowerupAudio;

    void Start()
    {
        boxAnimator = GetComponent<Animator>();
    }

    // 
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Vector2 normal = other.contacts[0].normal;
            Debug.Log($"Collision normal: {normal}");
            if (normal.y > 0.4f)
            {
                // Stop Mario's upward movement
                Rigidbody2D playerRb = other.gameObject.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    Vector2 velocity = playerRb.velocity;
                    velocity.y = 0;
                    playerRb.velocity = velocity;
                }
                
                // Set both triggers at the same time
                if (!powerup.hasSpawned)
                {
                    boxAnimator.SetTrigger("bounce");
                    
                    // Use an animation event instead of direct call
                    // Or spawn the powerup here directly
                    powerup.SpawnPowerup();
                    PowerupAudio.Play();
                    boxAnimator.SetTrigger("Empty");
                }
                else
                {
                    // Still bounce if hit again after powerup is gone
                    boxAnimator.SetTrigger("bounce");
                }
            }
        }
    }

    // used by animator
    public void Disable()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        transform.localPosition = Vector3.zero;
    }
    public void ResetQuestionBox()
    {
        boxAnimator.ResetTrigger("bounce");
        boxAnimator.SetTrigger("gameRestart");
    }
}