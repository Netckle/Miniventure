using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnBeat : MonoBehaviour
{
    public float waitTime;
    public SpriteRenderer spriteRenderer;
    private bool isUnbeatTime = false;
    private Rigidbody2D rigid;

    private PlayerMovement movement;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();
    }

    // Attacked by Creature
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!movement.isDashing && (other.gameObject.tag == "SlimeBoss" || other.gameObject.tag == "Slime") && !other.isTrigger && !(rigid.velocity.y < -10f) && !isUnbeatTime)
        {
            SoundManager.instance.PlaySfx(SoundManager.instance.EffectSounds[2]);

            // Bouncing
            Vector2 attackedVelocity = Vector2.zero;

            if (other.gameObject.transform.position.x > transform.position.x)
            {
                attackedVelocity = new Vector2 (-3f, 3f);
            }
            else
            {
                attackedVelocity = new Vector2 (3f, 3f);
            }
            rigid.AddForce(attackedVelocity, ForceMode2D.Impulse);

            GetComponent<UnBeat>().UnBeatTime();
        }
    }

    public void UnBeatTime()
    {
        StartCoroutine(CorUnBeatTime());
    }

    private IEnumerator CorUnBeatTime()
    {
        GetComponent<PlayerMovement>().canMove = false;

        int countTime = 0;

        while (countTime < 10)
        {
            // Alpha Effect
            if (countTime % 2 == 0)
            {
                spriteRenderer.color = new Color32(255, 255, 255, 90);
            }
            else
            {
                spriteRenderer.color = new Color32(255, 255, 255, 180);
            }

            // Wait Update Frame
            yield return new WaitForSeconds(waitTime);

            countTime++;
        }

        // Alpha Effect End
        spriteRenderer.color = new Color32(255, 255, 255, 255);

        // UnBeatTime Off
        isUnbeatTime = false;

        GetComponent<PlayerMovement>().canMove = true;
        
        yield return null;
    }
}
