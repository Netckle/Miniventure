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
            bossBat.collider2d.isTrigger = false;
            bossBat.rigidbody2d.velocity = Vector2.zero;

            bossBat.rigidbody2d.bodyType = RigidbodyType2D.Kinematic;

            bossBat.transform.position = bossPos.position;

            

            bossBat.UnderPattern();
            this.gameObject.SetActive(false);
        }

        else if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerMovement>().rigidbody2d.gravityScale = 3;
            other.gameObject.GetComponent<PlayerMovement>().rigidbody2d.velocity = Vector2.zero;
            other.gameObject.transform.position = playerPos.position;
            other.gameObject.GetComponent<PlayerMovement>().collider2d.isTrigger = false;
            
        }
    }

        

    
}
