using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingLayer : MonoBehaviour
{
    public string sortingLayerName = string.Empty; // 메소드들 앞의 초기화 
    public int orderInLayer = 0; 
    public Renderer MyRenderer; 

    void Start()
    {
        SetSortingLayer();
    }

    void SetSortingLayer () 
    { 
        if (sortingLayerName != string.Empty) 
        { 
            MyRenderer.sortingLayerName = sortingLayerName; 
            MyRenderer.sortingOrder = orderInLayer; 
        } 
    } 
}
