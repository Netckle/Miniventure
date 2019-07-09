using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnBeatEffect : MonoBehaviour
{  
    string type;

    public float waitTime = 0.1f;
    public SpriteRenderer spriteRenderer;

    bool isUnbeatTime = false;
    Rigidbody2D rigidbody2d;

    PlayerMovement playerMovement = null;
    BossMovement bossMovement = null;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
    }

    void Knockback(Collider2D other, string type)
    {
        switch(type)
        {
            case "플레이어":
            playerMovement = GetComponent<PlayerMovement>();
            break;
            case "보스슬라임":
            bossMovement = GetComponent<BossMovement>();
            break;
        }

        //if ((!playerMovement.isDashing && other.gameObject.tag == "SlimeBoss"))
    }
}
