using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    public GameObject[] backgroundOBJ = new GameObject[4];
    public Vector3[] paddingZ = new Vector3[4];

    public float time;

    void Start()
    {
        
    }

    void LateUpdate()
    {
        for (int i = 0; i < 4; ++i)
        {
            backgroundOBJ[i].transform.position = 
            Vector3.Lerp(backgroundOBJ[i].transform.position, Camera.main.transform.position + paddingZ[i], time * (i + 0.5f) * Time.deltaTime);
        }
    }
}
