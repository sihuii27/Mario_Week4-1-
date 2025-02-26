using UnityEngine;

public class MagicStarPowerup : BasePowerup
{
    private Vector3 originalScale;
    private Vector3 originalPosition;
    private bool isGrowing = false;
    private float growthDuration = 0.5f;
    private float growthTimer = 0f;
    
    protected override void Start()
    {
        base.Start();
        this.type = PowerupType.StarMan;
        originalScale = transform.localScale;
        originalPosition = transform.localPosition;
        // Start hidden
        transform.localScale = Vector3.zero;
    }

    void Update()
    {
        if (isGrowing)
        {
            growthTimer += Time.deltaTime;
            float progress = growthTimer / growthDuration;
            
            if (progress <= 1f)
            {
                // Grow in size
                transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, progress);
                
                // Move upward from the block
                float yOffset = Mathf.Lerp(0, 1.2f, progress); // Slightly increase Y offset to fully exit the block
                transform.localPosition = originalPosition + new Vector3(0, yOffset, 0);
            }
            else
            {
                isGrowing = false;
                
                // Move the mushroom slightly above its final position before enabling physics
                transform.localPosition = originalPosition + new Vector3(0, 1.2f, 0); 

                // Enable physics and movement once grown
                rigidBody.bodyType = RigidbodyType2D.Dynamic;

                // Ensure it's not stuck by applying a slight upward force
                rigidBody.AddForce(Vector2.up * 2, ForceMode2D.Impulse); 

                // Start moving sideways
                rigidBody.velocity = new Vector2(3, rigidBody.velocity.y);
            }

        }
    }

    public override void SpawnPowerup()
    {
        Debug.Log("Spawning Magic Star Powerup");
        spawned = true;
        isGrowing = true;
        growthTimer = 0f;
        
        // Disable physics while growing
        rigidBody.bodyType = RigidbodyType2D.Kinematic;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && spawned && !isGrowing)
        {
            // TODO: Apply powerup effect to player
            DestroyPowerup();
        }
        else if (col.gameObject.layer == 10 && spawned && !isGrowing) 
        {
            goRight = !goRight;
            rigidBody.velocity = new Vector2(3 * (goRight ? 1 : -1), rigidBody.velocity.y);
        }
    }

    public override void ApplyPowerup(MonoBehaviour i)
    {
        // TODO: Implement power-up effect
    }
}