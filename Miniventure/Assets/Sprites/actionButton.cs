using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class actionButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public PlayerMovement player;

    

    public void OnPointerDown(PointerEventData ped)
    {
        player.jumpFlag = true;
        player.jumpIsRunning = true;
    }

    public void OnPointerUp(PointerEventData ped)
    {
        player.jumpFlag = false;
        player.jumpIsRunning = false;
    }
}
