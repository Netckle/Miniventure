using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossBatMovement : MonoBehaviour 
{
    private ParticleSystem particle;

    private PlayerMovement player;

    private Rigidbody2D rigid;
    private Animator anim;
    private SpriteRenderer sprite;

    [Range(0, 50)]
    public int HP;
    [Range(0, 50)]
    public int maxHP = 20;

    public bool isUnbeatTime = false;
    public bool moveIsEnd = false;

    public float normalMoveTime = 1.0f;

    public Transform originPos;
    public GameObject damageEffect;

    public float offsetX, offsetY;

    public Transform phase02originPos;

    public bool phase02on = false;

    void Start()
    {
        //phase02originPos.position = new Vector3(originPos.position.x, originPos.position.y + 3.5f, 0);
        if (phase02on)
        {
            StartCoroutine(BossMovement03());
        }
        else
        {
                    StartBossPhase02();
        }

    }

    private void Move(float offsetX, float offsetY, float moveTime, Ease moveType = Ease.Linear)
    {
        moveIsEnd = false;
        Vector2 endPos = new Vector3(offsetX, offsetY);

        transform
            .DOMove(endPos, moveTime)
            .SetEase(moveType)
            .OnComplete(EndMove);
    }

    private void EndMove()
    {
        moveIsEnd = true;
    }

    public void StartBossPattern()
    {   
        StartCoroutine(BossMovement01());
    }

    public void StartBossPhase02()
    {
        originPos = phase02originPos;
        StartCoroutine(BossMovement02());
    }

    private IEnumerator BossMovement01()
    {
        Move(offsetX, originPos.position.y + offsetY, normalMoveTime);
        yield return new WaitUntil(()=>moveIsEnd);

        Move(-offsetX, originPos.position.y - offsetY, normalMoveTime * 1.5f);
        yield return new WaitUntil(()=>moveIsEnd);

        Move(-offsetX, originPos.position.y + offsetY, normalMoveTime * 2f);
        yield return new WaitUntil(()=>moveIsEnd);

        Move(offsetX, originPos.position.y - offsetY, normalMoveTime * 1.5f);
        yield return new WaitUntil(()=>moveIsEnd);

        Move(originPos.position.x, originPos.position.y, normalMoveTime);
        yield return new WaitUntil(()=>moveIsEnd);

        Move(-offsetX, originPos.position.y, normalMoveTime * 1.5f);
        yield return new WaitUntil(()=>moveIsEnd);

        Move(originPos.position.x, originPos.position.y, normalMoveTime * 1.5f);
        yield return new WaitUntil(()=>moveIsEnd);

        yield return new WaitForSeconds(2.0f);

        StartCoroutine(BossMovement01());
    }

    private IEnumerator BossMovement02()
    {
        yield return new WaitForSeconds(2.0f);

        Move(originPos.position.x, originPos.position.y, normalMoveTime);
        yield return new WaitUntil(()=>moveIsEnd);

        Move(originPos.position.x - offsetX, originPos.position.y + (offsetY / 2), normalMoveTime * 2f, Ease.InQuad);
        yield return new WaitUntil(()=>moveIsEnd);

        Move(originPos.position.x - offsetX * 2, originPos.position.y, normalMoveTime * 2f, Ease.OutQuad);
        yield return new WaitUntil(()=>moveIsEnd);

        yield return new WaitForSeconds(2.0f);

        Move(originPos.position.x, originPos.position.y, normalMoveTime * 2f);
        yield return new WaitUntil(()=>moveIsEnd);

        Move(originPos.position.x - offsetX, originPos.position.y - (offsetY / 2), normalMoveTime * 2f, Ease.InQuad);
        yield return new WaitUntil(()=>moveIsEnd);

        Move(originPos.position.x - offsetX * 2, originPos.position.y, normalMoveTime * 2f, Ease.OutQuad);
        yield return new WaitUntil(()=>moveIsEnd);

        yield return new WaitForSeconds(2.0f);

        Move(originPos.position.x, originPos.position.y, normalMoveTime * 2f);
        yield return new WaitUntil(()=>moveIsEnd);

        StartCoroutine(BossMovement02());
    }

    private IEnumerator BossMovement03()
    {
        yield return new WaitForSeconds(2.0f);

        Move(originPos.position.x, originPos.position.y, normalMoveTime);
        yield return new WaitUntil(()=>moveIsEnd);

        Move(originPos.position.x - offsetX, originPos.position.y - (offsetY / 2), normalMoveTime * 2f, Ease.InQuad);
        yield return new WaitUntil(()=>moveIsEnd);

        Move(originPos.position.x - offsetX * 2, originPos.position.y, normalMoveTime * 2f, Ease.OutQuad);
        yield return new WaitUntil(()=>moveIsEnd);

        yield return new WaitForSeconds(2.0f);

        Move(originPos.position.x, originPos.position.y, normalMoveTime * 2f);
        yield return new WaitUntil(()=>moveIsEnd);

        Move(originPos.position.x - offsetX, originPos.position.y + (offsetY / 2), normalMoveTime * 2f, Ease.InQuad);
        yield return new WaitUntil(()=>moveIsEnd);

        Move(originPos.position.x - offsetX * 2, originPos.position.y, normalMoveTime * 2f, Ease.OutQuad);
        yield return new WaitUntil(()=>moveIsEnd);

        yield return new WaitForSeconds(2.0f);

        Move(originPos.position.x, originPos.position.y, normalMoveTime * 2f);
        yield return new WaitUntil(()=>moveIsEnd);

        StartCoroutine(BossMovement03());
    }
}