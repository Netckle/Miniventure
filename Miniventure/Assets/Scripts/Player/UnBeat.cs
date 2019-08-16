using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnBeat : MonoBehaviour
{
    public float waitTime;
    public SpriteRenderer spriteRenderer;
    public SimpleCameraShakeInCinemachine cameraShake;

    private bool isUnbeatTime = false;

    private Rigidbody2D rigid;
    private PlayerMovement movement;
    private SoundManager soundManager;

    private bool isCollide = false;
    private bool canDamaged = true;

    public GameObject damageEffect;

    void ShowDamageEffect(Vector3 pos, float size = 1.0f)
    {
        damageEffect.SetActive(false);

        damageEffect.transform.position = pos;
        damageEffect.transform.localScale = new Vector3(size, size, size);
        
        damageEffect.SetActive(true);
    }

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();
        soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();
    }    

    private void StartUnBeat(Collider2D other)
    {
        // Attacked by Creature       
        --movement.HP;
        soundManager.PlaySfx(soundManager.EffectSounds[2]);
        cameraShake.ShakeCam();

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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "SlimeBoss" || other.gameObject.tag == "Slime" || other.gameObject.tag == "MinoBoss" || other.gameObject.tag == "Bat" || other.gameObject.tag == "BatBoss" && !other.isTrigger && !isUnbeatTime)
        {
            Debug.Log(other.gameObject.tag);
            isCollide = true;
        }
    }

    void OnTriggerStay2D(Collider2D other) 
    {
        if (isCollide && canDamaged)
        {
            ShowDamageEffect(this.gameObject.transform.position, 2);
            StartUnBeat(other);
            isCollide = false;
        }
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        if (other.gameObject.tag == "SlimeBoss" || other.gameObject.tag == "Slime" || other.gameObject.tag == "MinoBoss" && !other.isTrigger && !isUnbeatTime)
        {
            isCollide = false;
        }
    }

    public void UnBeatTime()
    {
        StartCoroutine(CorUnBeatTime());
    }    

    private IEnumerator CorUnBeatTime()
    {
        GetComponent<PlayerMovement>().canMove = false;
        canDamaged = false;

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
        canDamaged = true;

        yield return null;
    }
}
