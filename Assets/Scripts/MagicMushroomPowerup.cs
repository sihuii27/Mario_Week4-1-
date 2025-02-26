using UnityEngine;

public class MagicMushroomPowerup : BasePowerup
{
    private Vector3 originalScale;
    private Vector3 originalPosition;
    private bool isGrowing = false;
    private float growthDuration = 0.5f;
    private float growthTimer = 0f;
    
    protected override void Start()
    {
        base.Start();
        this.type = PowerupType.MagicMushroom;
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
                float yOffset = Mathf.Lerp(0, 1.1f, progress);
                transform.localPosition = originalPosition + new Vector3(0, yOffset, 0);
            }
            else
            {
                isGrowing = false;
                transform.localPosition = originalPosition + new Vector3(0, 1.2f, 0); 
                rigidBody.bodyType = RigidbodyType2D.Dynamic;
                rigidBody.AddForce(Vector2.up * 2, ForceMode2D.Impulse); 
                rigidBody.velocity = new Vector2(3, rigidBody.velocity.y);
            }
        }
    }

    public override void SpawnPowerup()
    {
        Debug.Log("Spawning Magic Mushroom Powerup");
        spawned = true;
        isGrowing = true;
        growthTimer = 0f;
        rigidBody.bodyType = RigidbodyType2D.Kinematic;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && spawned && !isGrowing)
        {
            DestroyPowerup();
        }
        else if (col.gameObject.layer == 6 && spawned && !isGrowing) 
        {
            goRight = !goRight;
            rigidBody.velocity = new Vector2(3 * (goRight ? 1 : -1), rigidBody.velocity.y);
        }
    }

    public override void ApplyPowerup(MonoBehaviour i)
    {
        // TODO: Implement power-up effect
    }
    public void ResetPowerup()
    {
        spawned = false;
        isGrowing = false;
        transform.localScale = Vector3.zero;
        transform.localPosition = originalPosition;
        rigidBody.bodyType = RigidbodyType2D.Static;
        gameObject.SetActive(true); // Ensure the power-up is active
    }
}