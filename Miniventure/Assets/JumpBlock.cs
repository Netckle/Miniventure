using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JumpBlock : MonoBehaviour
{
    public Transform playerOriginPos;

    private bool moveIsEnd = false;

    private void Move(GameObject obj, Vector2 _endPos, float moveTime, Ease moveType = Ease.Linear)
    {
        moveIsEnd = false;
        Vector2 endPos = _endPos;

        obj.transform
            .DOMove(endPos, moveTime)
            .SetEase(moveType)
            .OnComplete(EndMove);
    }

    private void EndMove()
    {
        moveIsEnd = true;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<Rigidbody2D>().velocity.y < 0)
        {
            other.gameObject.GetComponent<PlayerMovement>().rigidbody2d.bodyType = RigidbodyType2D.Kinematic;
            other.gameObject.GetComponent<PlayerMovement>().rigidbody2d.velocity = Vector2.zero;
            StartCoroutine(CoMove(other.gameObject));
        }
    }

    IEnumerator CoMove(GameObject obj) 
    {
        obj.GetComponent<PlayerMovement>().Pause();

        Move(obj, playerOriginPos.position, 3.0f, Ease.OutQuart);
        yield return new WaitUntil(()=>moveIsEnd);

        obj.gameObject.GetComponent<PlayerMovement>().rigidbody2d.bodyType = RigidbodyType2D.Dynamic;
        obj.gameObject.GetComponent<PlayerMovement>().rigidbody2d.velocity = Vector2.zero;

        obj.GetComponent<PlayerMovement>().Release();
    }
}
