using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MiniBatMovement : MonoBehaviour
{
    private ParticleSystem particle;
    private bool moveIsEnd = false;

    public GameObject player;
    public Transform originPos;   

    public float damage;  

    public float offsetX;
    public float offsetY;

    public float normalMoveTime;    

    public float offset01, offset02, offset03;

    private Vector2 originPos01, originPos02, originPos03;
    private Vector2 appliedPos01, appliedPos02, appliedPos03;

    private Vector2 topPos;

    private Vector2 originOneThirdOffset;
    private Vector2 appliedOneThirdOffset;

    private void Start() 
    {
        particle = GetComponentInChildren<ParticleSystem>();

        DefineAllPos();   
    }

    private void DefineAllPos()
    {
        originOneThirdOffset.x = offsetX / 4; 
        originOneThirdOffset.y = offsetY / 4;

        appliedOneThirdOffset.x = offsetX / 4; 
        appliedOneThirdOffset.y = (offsetY / 4) - 1.5f;

        topPos = new Vector2(transform.position.x - offsetX, transform.position.y + offsetY); 

        // Define Origin Pos;
        originPos01 = new Vector2(transform.position.x - originOneThirdOffset.x, transform.position.y + originOneThirdOffset.y);
        originPos02 = new Vector2(originPos01.x - originOneThirdOffset.x, originPos01.y + originOneThirdOffset.y);
        originPos03 = new Vector2(originPos02.x - originOneThirdOffset.x, originPos02.y + originOneThirdOffset.y);

        // Define Applied Pos;
        appliedPos01 = new Vector2(transform.position.x - appliedOneThirdOffset.x, transform.position.y + appliedOneThirdOffset.y + offset01);        
        appliedPos02 = new Vector2(appliedPos01.x - appliedOneThirdOffset.x, appliedPos01.y + appliedOneThirdOffset.y + offset02);
        appliedPos03 = new Vector2(appliedPos02.x - appliedOneThirdOffset.x, appliedPos02.y + appliedOneThirdOffset.y + offset03);
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(originPos01, 0.3f);
        Gizmos.DrawWireSphere(originPos02, 0.3f);
        Gizmos.DrawWireSphere(originPos03, 0.3f);

        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(appliedPos01, 0.3f);
        Gizmos.DrawWireSphere(appliedPos02, 0.3f);
        Gizmos.DrawWireSphere(appliedPos03, 0.3f);
    }

    public void StartMoving()
    { 
        DefineAllPos();
        StartCoroutine(CoStartMoving());
    }

    private IEnumerator CoStartMoving()
    {
        Move(appliedPos01, normalMoveTime);
        yield return new WaitUntil(()=>moveIsEnd);

        Move(appliedPos02, normalMoveTime);
        yield return new WaitUntil(()=>moveIsEnd);

        Move(appliedPos03, normalMoveTime);
        yield return new WaitUntil(()=>moveIsEnd);

        Move(topPos, normalMoveTime);
        yield return new WaitUntil(()=>moveIsEnd);

        yield return new WaitForSeconds(3.0f);

        Debug.Log(player.transform.position);

        Move(player.transform.position, normalMoveTime * 3.0f, Ease.OutQuart);
        yield return new WaitUntil(()=>moveIsEnd);

        Move(((Vector2)transform.position + (Vector2)transform.position - topPos).normalized * 10.0f, normalMoveTime * 3.0f + 10.0f);
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
        if (other.gameObject.tag == "DestroyCollider")
        {
            transform.DOPause();
            StopAllCoroutines();

            StartCoroutine(CoDie());
        }
    }

    public void TakeDamage()
    {
        StartCoroutine(CoDie());
    }

    private IEnumerator CoDie()
    {
        //StopCoroutine(mainCor);

        //particle.Play();
        //yield return new WaitUntil(()=>!particle.isPlaying);
        yield return null;
        //Fade?

        this.gameObject.SetActive(false);
    }
}
