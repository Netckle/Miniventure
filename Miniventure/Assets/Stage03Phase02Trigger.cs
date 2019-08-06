using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage03Phase02Trigger : MonoBehaviour
{
    public Transform playerPos;
    public Transform bossPos;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Bat")
        {
            BossBatMovement bossBat = other.gameObject.GetComponent<BossBatMovement>();
            //bossBat.collider2d.isTrigger = false;
            // 파티클 재생
            bossBat.rigid.velocity = Vector2.zero;
            
            bossBat.TakeDamage(5.0f);

            bossBat.UnderPattern();
        }

        else if (other.gameObject.tag == "Player")
        {
            this.gameObject.SetActive(false);
        }
    }
}
