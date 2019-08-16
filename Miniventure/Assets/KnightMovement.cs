using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class KnightMovement : MonoBehaviour
{
    public int hp;
    public const int maxHp = 20;

    public float moveRange;
    public float moveTime;

    private Rigidbody2D rigid;
    [HideInInspector]
    public Animator anim;

    private bool moveIsEnd = false;
    private bool attackIsEnd = false;
    private bool jumpIsEnd = false;

    public bool onGround = false;
    public bool isGuarding = false;

    public Collider2D groundCheck;
    public Collider2D guardCheck;

    private void Awake() 
    {
        rigid = GetComponent<Rigidbody2D>();
        anim  = GetComponentInChildren<Animator>();

        //groundCheck = GetComponentInChildren<Collider2D>();
    }
    
    private void Start() 
    {
        StartCoroutine(CoRepeatMove());
    }

    private void Update() 
    {
        anim.SetBool("onGround", onGround);

        if (Input.GetKeyDown(KeyCode.P))
        {
            anim.SetTrigger("startDamage");
        }

        anim.SetBool("isGuarding", isGuarding);

        if (Input.GetKeyDown(KeyCode.O))
        {
            transform.DOPause();
            StopAllCoroutines();
            
            StartCoroutine(CoDie());
        }
    }

    private IEnumerator CoDie()
    {

        anim.SetTrigger("startDie");

        yield return new WaitForSeconds(2.0f);
        this.gameObject.SetActive(false);
    }

    private void FixedUpdate() 
    {
        anim.SetFloat("verticalVelocity", rigid.velocity.y);
    }

    public void RepeatMove()
    {
        StartCoroutine(CoRepeatMove());
    }

    private IEnumerator CoRepeatMove()
    {
        anim.SetBool("isMoving", true);

        int firstNum  = Random.Range(1, 4);
        int secondNum = Random.Range(1, 4);        

        // Walk Right

        Flip("RIGHT");
        MoveX(0, moveTime); // Middle
        yield return new WaitUntil(()=>moveIsEnd);

        StartCoroutine(CoJump(3, 2.0f)); // Jump
        yield return new WaitUntil(()=>onGround && jumpIsEnd);
        guardCheck.gameObject.SetActive(true);

        MoveX(moveRange, moveTime);
        yield return new WaitUntil(()=>moveIsEnd);

        StartCoroutine(CoAttack(firstNum)); // Attack
        yield return new WaitUntil(()=>attackIsEnd);

        // Walk Left

        Flip("LEFT");
        MoveX(0, moveTime); // Middle
        yield return new WaitUntil(()=>moveIsEnd);

        StartCoroutine(CoJump(3, 2.0f)); // Jump
        yield return new WaitUntil(()=>onGround && jumpIsEnd);

        MoveX(-moveRange, moveTime);
        yield return new WaitUntil(()=>moveIsEnd);
        guardCheck.gameObject.SetActive(true);

        StartCoroutine(CoAttack(secondNum)); // Attack
        yield return new WaitUntil(()=>attackIsEnd);

        StartCoroutine(CoRepeatMove());
    }

    // DG Move Function

    void MoveX(float _xEndPos, float moveTime, Ease moveType = Ease.Linear)
    {
        moveIsEnd = false;
        float xEndPos = _xEndPos;

        transform
        .DOMoveX(_xEndPos, moveTime)
        .SetEase(moveType)
        .OnComplete(EndMove);
    }

    void MoveY(float _yEndPos, float moveTime, Ease moveType = Ease.Linear)
    {
        moveIsEnd = false;
        float yEndPos = _yEndPos;

        transform
        .DOMoveY(_yEndPos, moveTime)
        .SetEase(moveType)
        .OnComplete(EndMove);
    }

    void Move(Vector3 _endPos, float moveTime, Ease moveType = Ease.Linear)
    {
        moveIsEnd = false;
        Vector3 endPos = _endPos;

        transform
        .DOMove(endPos, moveTime)
        .SetEase(moveType)
        .OnComplete(EndMove);
    }

    private void EndMove()
    {
        moveIsEnd = true;
    }

    // Controller Animation Function

    private IEnumerator CoAttack(int attackType)
    {
        attackIsEnd = false;

        anim.SetBool("isAttacking", true);
        anim.SetInteger("attackType", attackType);

        yield return new WaitForSeconds(1.0f);

        anim.SetBool("isAttack", false);
        anim.SetInteger("attackType", 0);

        attackIsEnd = true;
    }

    private IEnumerator CoJump(float jumpHeight, float jumpTime)
    {
        groundCheck.gameObject.SetActive(false);
        guardCheck.gameObject.SetActive(false);

        onGround = false;
        jumpIsEnd = false;

        rigid.bodyType = RigidbodyType2D.Kinematic;

        anim.SetTrigger("startJumping");

        MoveY((-2.6f) + jumpHeight, jumpTime, Ease.OutQuint);
        yield return new WaitUntil(()=>moveIsEnd);

        rigid.bodyType = RigidbodyType2D.Dynamic;
        rigid.velocity = Vector2.zero;

        jumpIsEnd = true;

        groundCheck.gameObject.SetActive(true);
    }

    private void Flip(string dir)
    {
        switch (dir)
        {
            case "RIGHT":
                transform.localScale = new Vector3(1, 1, 1);
                break;
            case "LEFT":
                transform.localScale = new Vector3(-1, 1, 1);
                break;
        }        
    }
}
