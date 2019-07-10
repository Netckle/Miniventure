using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniSlime : MonoBehaviour
{
    [Header("State")]
    public int HP;

    [Header("Movement")]
    public int movePower;

    private Vector3 move;
    private int movementFlag = 0;    
    private Animator anim;

    public ParticleSystem particle;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();

        StartCoroutine("ChangeMovement");

        //particle = GetComponentInChildren<ParticleSystem>();
    }

    IEnumerator ChangeMovement()
    {
        movementFlag = Random.Range(0, 3);

        if (movementFlag == 0)
            anim.SetBool("isMoving", false);
        else
            anim.SetBool("isMoving", true);

        yield return new WaitForSeconds(3.0f);

        StartCoroutine("ChangeMovement");
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;

        if (movementFlag == 1)
        {
            moveVelocity = Vector3.left;
            transform.localScale = new Vector3 (1, 1, 1);
        }
        else if (movementFlag == 2)
        {
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3 (-1, 1, 1);
        }
        transform.position += moveVelocity * movePower * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 임시로 해놓음.
        if (other.gameObject.tag == "Player")
        {
            gameObject.SetActive(false);
        }
    }
}
