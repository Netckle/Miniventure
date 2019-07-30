using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChangeForPhase02 : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachineCam;
    public Transform Phase02StagePos;

    public Transform Phase02SlimePos;

    public Cutscene01 cutscene;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "SlimeBoss")
        {
            other.gameObject.transform.position = Phase02SlimePos.position;
        }
        else if (other.gameObject.tag == "Player")
        {
            //cinemachineCam.Follow = Phase02StagePos;
            var transposer = cinemachineCam.GetCinemachineComponent<CinemachineTransposer>();
            transposer.m_FollowOffset = new Vector3(0, 0, -10);
            cutscene.phase02start = true;
            this.gameObject.SetActive(false);
        }
    }
}
