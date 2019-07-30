using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform player_transform;
    
    private void LateUpdate() 
    {
        Vector3 newPosition = new Vector3(0, player_transform.position.y, 0);

        this.transform.position = newPosition;
    }
}
