using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage03Phase02Trigger : MonoBehaviour
{
    public Transform playerPos;
    public Transform bossPos;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "BossBat")
        {
            BossBatMovement bossBat = other.gameObject.GetComponent<BossBatMovement>();
            bossBat.AfterColliderToFloor();
            
            this.gameObject.SetActive(false);
        }

        else if (other.gameObject.tag == "Player")
        {
            PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();

            player.rigidbody2d.gravityScale = 3;
            player.rigidbody2d.velocity = Vector2.zero;

            player.transform.position = playerPos.position;

            player.collider2d.isTrigger = false;
        }
    }       
}
