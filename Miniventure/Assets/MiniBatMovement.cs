using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MiniBatMovement : MonoBehaviour
{
    private ParticleSystem particle;

    public Transform playerPos;
    public Transform originPos;

    private bool moveIsEnd = false;

    public float damage;  

    [Space]
    public float offsetX;
    public float offsetY;

    public float normalMoveTime;

    private IEnumerator mainCor;

    public float addOffsetY01, addOffsetY02, addOffsetY03;

    private Vector2 originPos01, originPos02, originPos03;
    private Vector2 tempPos01, tempPos02, tempPos03;

    private Vector2 topPos;

    private float tempX, tempY;
    private float originX, originY;

    private void Start() 
    {
        particle = GetComponentInChildren<ParticleSystem>();
        
        originX = offsetX / 4; originY = offsetY / 4;
        tempX = offsetX / 4; tempY = (offsetY / 4) - 1.5f;       

           
        originPos01 = new Vector2(transform.position.x - originX, transform.position.y + originY);
        tempPos01 = new Vector2(transform.position.x - tempX, transform.position.y + tempY + addOffsetY01);

        originPos02 = new Vector2(originPos01.x - originX, originPos01.y + originY);
        tempPos02 = new Vector2(tempPos01.x - tempX, tempPos01.y + tempY + addOffsetY02);

        originPos03 = new Vector2(originPos02.x - tempX, originPos02.y);
        tempPos03 = new Vector2(tempPos02.x - tempX, tempPos02.y + tempY + addOffsetY03);

        topPos = new Vector2(transform.position.x - offsetX, transform.position.y + offsetY); 
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(tempPos01, 0.3f);
        Gizmos.DrawWireSphere(tempPos02, 0.3f);
        Gizmos.DrawWireSphere(tempPos03, 0.3f);
        Gizmos.DrawWireSphere(topPos, 0.3f);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(originPos01, 0.3f);
        Gizmos.DrawWireSphere(originPos02, 0.3f);
        Gizmos.DrawWireSphere(originPos03, 0.3f);
        Gizmos.DrawWireSphere(topPos, 0.3f);
    }

    public void StartMoving()
    {
        originX = offsetX / 4; originY = offsetY / 4;
        tempX = offsetX / 4; tempY = (offsetY / 4) - 1.5f;       

           
        originPos01 = new Vector2(transform.position.x - originX, transform.position.y + originY);
        tempPos01 = new Vector2(transform.position.x - tempX, transform.position.y + tempY + addOffsetY01);

        originPos02 = new Vector2(originPos01.x - originX, originPos01.y + originY);
        tempPos02 = new Vector2(tempPos01.x - tempX, tempPos01.y + tempY + addOffsetY02);

        originPos03 = new Vector2(originPos02.x - tempX, originPos02.y);
        tempPos03 = new Vector2(tempPos02.x - tempX, tempPos02.y + tempY + addOffsetY03);

        topPos = new Vector2(transform.position.x - offsetX, transform.position.y + offsetY); 
        StartCoroutine(CoMoving());
    }

    private IEnumerator CoMoving()
    {
        Move(tempPos01, normalMoveTime);
        yield return new WaitUntil(()=>moveIsEnd);

        Move(tempPos02, normalMoveTime);
        yield return new WaitUntil(()=>moveIsEnd);

        Move(tempPos03, normalMoveTime);
        yield return new WaitUntil(()=>moveIsEnd);

        Move(topPos, normalMoveTime);
        yield return new WaitUntil(()=>moveIsEnd);

        yield return new WaitForSeconds(2.0f);

        Move(topPos.x + (playerPos.position.x - topPos.x), topPos.y + (playerPos.position.y - topPos.y), normalMoveTime * 3);
        yield return new WaitUntil(()=>moveIsEnd);

        Move(transform.position.x + ((playerPos.position.x - topPos.x)), transform.position.y + ((playerPos.position.y - topPos.y)), normalMoveTime * 3);
        yield return new WaitUntil(()=>moveIsEnd);
    }

    

    

    private void Move(float _offsetX, float _offsetY, float _moveTime, Ease moveType = Ease.Linear)
    {
        moveIsEnd = false;
        Vector3 endPos = new Vector3(_offsetX, _offsetY, 0f);

        transform
            .DOMove(endPos, _moveTime)
            .SetEase(moveType)
            .OnComplete(EndMove);
    }

    private void Move(Vector2 _offset, float _moveTime, Ease moveType = Ease.Linear)
    {
        moveIsEnd = false;
        Vector3 endPos = new Vector3(_offset.x, _offset.y, 0f);

        transform
            .DOMove(endPos, _moveTime)
            .SetEase(moveType)
            .OnComplete(EndMove);
    }

    private void EndMove()
    {
        moveIsEnd = true;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player")
        {
            StopAllCoroutines();
            //other.gameObject.GetComponent<PlayerMovement>().TakeDamage(damage);
            StartCoroutine(Die());
        }

        if (other.gameObject.tag == "DestroyCollider")
        {
            StopAllCoroutines();
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        //StopCoroutine(mainCor);

        //particle.Play();
        //yield return new WaitUntil(()=>!particle.isPlaying);
        yield return null;
        //Fade?

        this.gameObject.SetActive(false);
    }
}
