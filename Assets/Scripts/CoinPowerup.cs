using UnityEngine;
using System.Collections;

public class CoinPowerup : BasePowerup
{
    public int maxHits = 1;
    public int startingHits = 1;
    public Sprite emptyBlock;
    public Animator questionBoxAnimator;
    //for audio
    public AudioSource coinAudio;
    public GameManager gameManager;
    
    protected override void Start()
    {
        base.Start();
        this.type = PowerupType.Coin;
        startingHits = maxHits;
        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 normal = collision.contacts[0].normal;
            //detect if block is hit from below not above
            if (normal.y > 0.4f)
            {
                Hit();
            }
        }
    }

    void Hit()
    {
        if (maxHits>0)
        {
            maxHits--;
            Debug.Log("Triggering hit animation");
            questionBoxAnimator.SetTrigger("hit");
            PlayCoinSound();
            gameManager.IncreaseScore(1);
            if (maxHits == 0)
            {
                StartCoroutine(SetEmptyAfterLongerDelay());
            }
        }
    }

    IEnumerator SetEmptyAfterLongerDelay()
    {
        yield return new WaitForSeconds(1.0f); // Longer delay
        questionBoxAnimator.SetTrigger("Empty");
    }

    public override void SpawnPowerup()
    {
        spawned = true;
    }

    // IEnumerator SetEmptyAfterDelay()
    // {
    //     yield return new WaitForSeconds(0.5f);
    //     questionBoxAnimator.SetTrigger("Empty");
    // }

    public void ResetCoins()
    {
        // Reset max hits to original value
        maxHits = startingHits;
        questionBoxAnimator.SetTrigger("gameRestart");
    }

    void PlayCoinSound()
    {
        //play mario jump sound
        coinAudio.PlayOneShot(coinAudio.clip);
    }

    public override void ApplyPowerup(MonoBehaviour i)
    {
        // TODO: Implement power-up effect
    }
}