using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeMove : MonoBehaviour
{
    public GameObject[] trees;

    public Transform spawn_transform;

    private bool can_spawn;

    void Update()
    {
        if (!CheckActiveTree() && can_spawn)
        {
            StartCoroutine(SpawnTree());
            can_spawn = false;
        }
    }

    bool CheckActiveTree()
    {
        foreach (GameObject tree in trees)
        {
            if (tree.activeSelf)
            {
                return true;
            }
        }
        can_spawn = true;
        return false;
    }

    public void StopAllTree()
    {
        for (int i = 0; i < 3; i++)
        {
            trees[i].GetComponent<TreeMovement>().StopMove();
        }
    }

    public void StartAllTree()
    {
        for (int i = 0; i < 3; i++)
        {
            trees[i].GetComponent<TreeMovement>().StartMove();
        }
    }

    private bool spawn_tree_is_end = false;
    IEnumerator SpawnTree()
    {   
        spawn_tree_is_end = false;
        for (int i = 0; i < 3; i++)
        {
            int[] test = RandomInt.getRandomInt(1, 1, 4);
            Debug.Log(i + "번째 나무를 소환하고 기다리는 시간은 " + test[0] + "초입니다.");

            trees[i].transform.position = spawn_transform.position;
            trees[i].SetActive(true);
            trees[i].GetComponent<TreeMovement>().StartMove();

            yield return new WaitForSeconds(test[0]);
        }
        spawn_tree_is_end = true;
    }
}
