using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReduceFallVelocity : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerMovement>().rigidbody2d.velocity = Vector2.zero;
        }
    }
}
