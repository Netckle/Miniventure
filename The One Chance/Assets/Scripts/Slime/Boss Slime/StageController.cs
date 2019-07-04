using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    public PlayerMovement player;

    public GameObject[] miniSlimes = new GameObject[6];
    public GameObject[] lineColliders = new GameObject[3];

    private Vector3[] spawnedPos = new Vector3[6];

    void Start()
    {
        SaveMiniSlimesPos();
    }

    void SaveMiniSlimesPos()
    {
        for (int i = 0; i < miniSlimes.Length; ++i)
        {
            spawnedPos[i] = miniSlimes[i].transform.position;
            miniSlimes[i].SetActive(false);
        }
    }

    public bool CheckSlimeIsActive()
    {
        for (int i = 0; i < miniSlimes.Length; i++)
        {
            if (miniSlimes[i].activeSelf)
            {
                return false;
            }            
        }
        return true;
    }

    public void SpawnMiniSlimes(int count)
    { 
        Debug.Log("소환할 갯수 : " + count);
        // 남아있는 슬라임 오브젝트 Off.
        for (int i = 0; i < miniSlimes.Length; ++i)
        {
            miniSlimes[i].SetActive(false);
        }

        int[] pos = RandomInt.getRandomInt(count, 0, miniSlimes.Length);        

        foreach (int num in pos)
        {
            Debug.Log("소환할 녀석 : " + num);
            miniSlimes[num].transform.position = spawnedPos[num];
            miniSlimes[num].SetActive(true);

            Camera.main.GetComponent<MultipleTargetCamera>().targets.Add(miniSlimes[num].transform);
            
            miniSlimes[num].GetComponent<MiniSlimeMove>().particle.Play();
        }
    }
}
