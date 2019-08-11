using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BackgroundScroll : MonoBehaviour
{
    public float goalXPos;

    private Vector3 backgroundTrans;

    public float moveTime;
    private bool moveIsEnd = false;

    void Start() 
    {
        backgroundTrans = transform.position;
    }

    public void Stop()
    {
        transform.DOPause();
        StopAllCoroutines();
    }

    public void Move()
    {
        StartCoroutine(CoMove(goalXPos, moveTime));
    }

    private void ResetBackgroundPos()
    {
        transform.position = backgroundTrans;
    }

    IEnumerator CoMove(float goalXPos, float moveTime)
    {
        ResetBackgroundPos();

        MoveOnlyX(goalXPos, moveTime);
        yield return new WaitForSeconds(moveTime);        

        StartCoroutine(CoMove(goalXPos, moveTime));
    }
    
    void MoveOnlyX(float goalXPos, float moveTime)
    {
        moveIsEnd = false;

        float tempGoalXPos = goalXPos;

        transform.DOMoveX(tempGoalXPos, moveTime)
            .SetEase(Ease.Linear)
            .OnComplete(EndMove);

        moveIsEnd = true;
    }

    void EndMove()
    {
        moveIsEnd = true;
    }
}
