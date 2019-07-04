using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    Vector3 originPos;
 
    void Start () 
    {
        originPos = transform.localPosition;
    }

    public void Shake(float _amount, float _duration)
    {
        originPos = transform.localPosition;
        StartCoroutine(CoShake(_amount, _duration));
    }
    
    IEnumerator CoShake(float _amount,float _duration)
    {
        GetComponent<MultipleTargetCamera>().canZoom = false;
        float timer=0;
        while(timer <= _duration)
        {
            transform.localPosition = (Vector3)Random.insideUnitCircle * _amount + originPos;
    
            timer += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originPos;    
        GetComponent<MultipleTargetCamera>().canZoom = true;
    }
}
