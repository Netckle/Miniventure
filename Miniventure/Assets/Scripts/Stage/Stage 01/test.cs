using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public Cutscene01 cutscene;
    public Transform SlimePhase02Pos;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "SlimeBoss")
        {
            other.gameObject.transform.position = SlimePhase02Pos.position;
        }

        else if (other.gameObject.tag == "Player")
        {
            cutscene.phase02start = true;
            this.gameObject.SetActive(false);
        }        
    }
}
