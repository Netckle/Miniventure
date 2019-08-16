using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightGroundCheck : MonoBehaviour
{
    private KnightMovement knight;

    private void Start() 
    {
        knight = GetComponentInParent<KnightMovement>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.layer == 8)
        {
            knight.onGround = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if (other.gameObject.layer == 8)
        {
            knight.onGround = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.gameObject.layer == 8)
        {
            knight.onGround = false;
        }
    }
}
