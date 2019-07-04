using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniSlimeMove : MonoBehaviour
{
    [Header("Test")]
    public int HP;
    private Animator anim;

    public ParticleSystem particle;

    public LayerMask enemyMask;
    public float speed;

    Rigidbody2D myBody;
    Transform myTrans;

    float myWidth, myHeight;

    private bool canMove = true;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();

        myTrans = this.transform;
        myBody = this.GetComponent<Rigidbody2D>();
        SpriteRenderer mySprite = this.GetComponentInChildren<SpriteRenderer>();
        myWidth = mySprite.bounds.extents.x;
        myHeight = mySprite.bounds.extents.y;
    }

    void Update()
    {
        if (HP <= 0)
        {
            FindObjectOfType<PlayerMovement>().catchedSlimes++;
            gameObject.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            Move();
        }
    }

    void Move()
    {
        // Check to see if there's ground in front of us before moving forward
        Vector2 lineCastPos = myTrans.position.toVector2();// - myTrans.right.toVector2() * myWidth + Vector2.up * myHeight;

        Debug.DrawLine(lineCastPos, lineCastPos + Vector2.down * 0.3f, Color.blue);
        bool isGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + Vector2.down * 0.3f, enemyMask);

        Debug.DrawLine(lineCastPos, lineCastPos - myTrans.right.toVector2() * 0.3f, Color.red);        
        bool isBlocked = Physics2D.Linecast(lineCastPos, lineCastPos - myTrans.right.toVector2() * 0.3f, enemyMask);

        // if theres no ground, turn around, Or if I'm blocked, turn around
        if (!isGrounded || isBlocked)
        {
            Vector3 currRot = myTrans.eulerAngles;
            currRot.y += 180;
            myTrans.eulerAngles = currRot;
        }
        
        //Always move forward
        Vector2 myVel = myBody.velocity;
        myVel.x = -myTrans.right.x * speed;
        myBody.velocity = myVel;
    }

    public void TakeDamage(int damage)
    {
        //dazedTime = startDazedTime;

        // play a hurt sound
        // show damage effect
        //Instantiate(bloodEffect, transform.position, Quaternion.identity);

        
        
    }

    IEnumerator CoTakeDamage(int damage)
    {
        canMove = false;
        Camera.main.GetComponent<CameraShake>().Shake(0.3f, 0.3f);
        
        HP -= damage;
        Debug.Log("damage TAKEN !");
        yield return new WaitForSeconds(0.3f);
        canMove = true;
    }
}
