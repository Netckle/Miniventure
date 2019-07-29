using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NiddleControll : MonoBehaviour
{
    public GameObject niddleParent;

    bool move_is_end = false;

    void OnTriggerEnter2D(Collider2D other) 
    {
        if ((other.gameObject.tag == "Player") &&
            other.gameObject.GetComponent<Rigidbody2D>().velocity.y < 0)
            {
                StartCoroutine(MoveNiddle());
            }
    }

    void MoveNiddleOnlyY(float y_destination, float move_time)
    {
        move_is_end = false;

        float y_end_pos = y_destination;

        niddleParent.transform.DOMoveY(y_destination, move_time)
            .SetEase(Ease.InOutQuart)
            .OnComplete(EndMove);
    }

    void EndMove()
    {
        move_is_end = true;
    }

    IEnumerator MoveNiddle()
    {
        niddleParent.transform.position = Vector3.zero;

        MoveNiddleOnlyY(1, 0.5f);
        yield return new WaitUntil(()=>move_is_end);

        MoveNiddleOnlyY(0, 0.5f);
        yield return new WaitUntil(()=>move_is_end);
    }
}
