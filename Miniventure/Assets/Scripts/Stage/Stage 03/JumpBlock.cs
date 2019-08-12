using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JumpBlock : MonoBehaviour
{
    private SoundManager soundManager;
    public Transform playerOriginPos;

    public bool moveIsEnd = false;

    private void Awake() 
    {
        soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();
    }

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
            soundManager.PlaySfx(soundManager.EffectSounds[6]);
            other.gameObject.GetComponent<PlayerMovement>().rigidbody2d.bodyType = RigidbodyType2D.Kinematic;
            other.gameObject.GetComponent<PlayerMovement>().rigidbody2d.velocity = Vector2.zero;
            StartCoroutine(CoMove(other.gameObject));
        }
    }

    IEnumerator CoMove(GameObject obj) 
    {
        PlayerMovement player = obj.GetComponent<PlayerMovement>();
        player.Pause();

        Move(obj, playerOriginPos.position, 3.0f, Ease.OutQuart);
        yield return new WaitUntil(()=>moveIsEnd);

        player.Release();

        player.rigidbody2d.bodyType = RigidbodyType2D.Dynamic;
        player.rigidbody2d.velocity = Vector2.zero;
    }
}
