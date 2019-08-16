using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickTest : MonoBehaviour
{
    // 공개
    public Transform stick;         // 조이스틱.
 
    // 비공개
    private RectTransform stickRect;

    private Vector3 stickFirstPos;  // 조이스틱의 처음 위치.
    private Vector3 stickVector;    // 조이스틱의 방향 벡터.
    private float radius;           // 조이스틱 뒷 배경의 반지름.
    private bool moveFlag;          // 플레이어 움직임 스위치.

    public float canvasLocalX;
 
    void Awake()
    {
        radius = GetComponent<RectTransform>().sizeDelta.y * 0.5f;

        stickRect = stick.GetComponent<RectTransform>();

        stickFirstPos = stickRect.localPosition;
        // stickFirstPos = Vector3.zero;
 
        // 캔버스 크기에대한 반지름 조절.
        // float canvasLocalX = transform.parent.GetComponent<RectTransform>().localScale.x;
        radius *= canvasLocalX;
 
        moveFlag = false;
    }

    private Vector2 offset = new Vector3(-245, -240);
    Vector3 pointPos;
 
    // 드래그
    public void Drag(BaseEventData data)
    {
        moveFlag = true;

        PointerEventData pointData = data as PointerEventData;
        pointPos = pointData.position + offset;
        
        // 조이스틱을 이동시킬 방향 벡터를 구함. (오른쪽,왼쪽,위,아래)
        stickVector = (pointPos - stickFirstPos).normalized;

        // 조이스틱의 처음 위치와 현재 내가 터치하고있는 위치의 거리를 구한다.
        float distance = Vector2.Distance(pointPos, stickFirstPos);

        // 거리가 반지름보다 작으면 조이스틱을 현재 터치하고 있는 곳으로 이동.
        if (distance < radius)
        {
            stickRect.localPosition = stickFirstPos + stickVector * distance;
        }
        // 거리가 반지름보다 커지면 조이스틱을 반지름의 크기만큼만 이동.
        else
        {
            stickRect.localPosition = stickFirstPos + stickVector * radius;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(pointPos, 1);
    }
 
    // 드래그 끝.
    public void DragEnd()
    {
        stickVector = Vector3.zero;                 // 방향을 0으로.
        stickRect.localPosition = stickFirstPos;    // 스틱을 원래의 위치로.   

        Debug.Log(stickFirstPos);    

        moveFlag = false;
    }

    public float GetHorizontalValue()
    {
        return stickVector.x;
    }

    public float GetVerticalValue()
    {
        return stickVector.y;
    }
}
