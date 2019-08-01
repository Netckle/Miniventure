using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForPhase02 : MonoBehaviour
{
    

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player")
        {
            
        }
    }
}
