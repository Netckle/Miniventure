using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private Vector3 savedVelocity;

    public static PauseManager instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }   

    public void Pause(GameObject obj, bool animeStop)
    {
        Animator animator = obj.GetComponentInChildren<Animator>();
        Rigidbody2D rigid = obj.GetComponentInChildren<Rigidbody2D>();
        Transform transform = obj.transform;

        if (animeStop)
            animator.speed = 0f;

        savedVelocity = rigid.velocity;

        rigid.velocity = Vector3.zero;
        rigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY; 
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void Release(GameObject obj, string tag)
    {
        Animator animator = obj.GetComponentInChildren<Animator>();
        Rigidbody2D rigid = obj.GetComponentInChildren<Rigidbody2D>();
        Transform transform = obj.transform;

        animator.speed = 1f;

        rigid.velocity = savedVelocity;
        savedVelocity = Vector3.zero;

        rigid.constraints = RigidbodyConstraints2D.None;
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        
        ChangeOBJMove(obj, tag, false);
    }

    void ChangeOBJMove(GameObject obj, string tag, bool flag)
    {
        switch (tag)
        {
            case "Player":
                obj.GetComponent<PlayerMovement>().pause = flag;
                break;
            case "BossSlime":
                obj.GetComponent<BossMovement>().pause = flag;
                break;
            case "NPC":
                obj.GetComponent<NPCMovement>().pause = flag;
                break;
        }
    }
}
