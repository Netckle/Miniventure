using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class KnightGuardCheck : MonoBehaviour
{
    private KnightMovement knight;

    private void Start() 
    {
        knight = GetComponentInParent<KnightMovement>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player")
        {
            knight.transform.DOPause();
            knight.StopAllCoroutines();
            knight.anim.SetTrigger("startGuard");
        }
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player")
        {
            knight.isGuarding = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player")
        {
            knight.isGuarding = false;
            knight.RepeatMove();
        }
    }
}
