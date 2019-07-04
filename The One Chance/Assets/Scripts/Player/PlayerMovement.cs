using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class PlayerMovement : MonoBehaviour
{   
    public int catchedSlimes = 0;

    [HideInInspector]
    public Rigidbody2D rigidbody2d;
    private Collision collision;
    private Animator animator;
    private AnimationScript animationScript;

    [Space]
    [Header("Move & Jump")]    
    public float speed        = 10;
    public float jumpForce    = 50;
    public float slideSpeed   = 5;
    public float wallJumpLerp = 10;
    public float dashSpeed    = 20;

    private bool groundTouch;
    private bool hasDashed;

    [Space]
    [Header("State Flags")]

    public bool wallGrab;
    public bool wallJumped;
    public bool wallSlide;
    public bool isDashing;    

    [Space]
    public int side = 1;
    public int currentLine = 0;

    [Space]
    [Header("Particles")]
    public ParticleSystem dashParticle;
    public ParticleSystem jumpParticle;
    public ParticleSystem wallJumpParticle;
    public ParticleSystem slideParticle;

    [Space]
    [Header("Platformer Effector")]
    public bool canGoDown = false;
    public PlatformEffector2D effector;

    [Space]
    [Header("Atttack")]    
    public float            cooldown = 0.5f;   // Combo Attack Cooldown
    public float            maxTime = 0.8f;    // Accepted Combo Limit Time
    public int              maxCombo;          // Combo Attack Max Count
    private int             combo = 0;         // Current Combo Count
    private float           lastTime;          // Last Attack Time

    public int              maxHealth = 6;
    public int              health = 6;

    public bool canMove = false;
    public bool pause = false;
    public bool isTalking = false;

    public bool isJumping = false;

    public void Pause()
    {
        canMove = false;
    }

    public void Release()
    {
        canMove = true;
    }

    public void ChangeTransform(Vector3 pos)
    {
        this.gameObject.transform.position = pos;
    }

    void Start()
    {
        collision = GetComponent<Collision>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        animationScript = GetComponentInChildren<AnimationScript>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");

        Vector2 dir = new Vector2(x, y);

        Walk(dir);        
        animationScript.SetHorizontalMovement(x, y, rigidbody2d.velocity.y);

        if (isTalking && Input.GetButtonDown("Interact"))
        {
            DialogueManager.instance.DisplayNextSentence();
        }
        
        if (isJumping)
        {
            animationScript.SetTrigger("jump");

            if (collision.onGround)
            {
                Jump(Vector2.up, false);
            }

            if (collision.onWall && !collision.onGround)
            {
                WallJump();
            }
        }
        
        if (isDashing && !hasDashed)
        {
            if (xRaw != 0 || yRaw != 0)
            {
                Dash(xRaw, yRaw);
            }
        }

        if (collision.onWall && Input.GetButton("Interact") && canMove)
        {
            if (side != collision.wallSide)
                animationScript.Flip(side * -1);

            wallGrab  = true;
            wallSlide = false;
        }
        
        if (Input.GetButtonUp("Interact") || !collision.onWall || !canMove)
        {
            wallGrab  = false;
            wallSlide = false;
        }

        if (Input.GetButtonDown("Interact") && !canGoDown)
        {
            StartCoroutine(CoRotatePlatform(0.5f));
        }

        if (pause)
        {
            PauseManager.instance.Pause(this.gameObject, true);            
        }

        if (collision.onGround && !isDashing)
        {
            wallJumped = false;
            GetComponent<BetterJumping>().enabled = true;
        }

        if (wallGrab && !isDashing)
        {
            rigidbody2d.gravityScale = 0;
            if (x > 0.2f || x < -0.2f)
            {
                rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, 0);
            }

            float speedModifier = y > 0 ? 0.5f : 1;

            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, y * (speed * speedModifier));
        }
        else
        {
            rigidbody2d.gravityScale = 3;
        }

        if (collision.onWall && !collision.onGround)
        {
            if (x != 0 && !wallGrab)
            {
                wallSlide = true;
                WallSlide();
            }
        }

        if (!collision.onWall || collision.onGround)
        {
            wallSlide = false;
        }        

        if (collision.onGround && !groundTouch)
        {
            GroundTouch();
            groundTouch = true;
        }

        if (!collision.onGround && groundTouch)
        {
            groundTouch = false;
        }

        WallParticle(y);

        if (wallGrab || wallSlide || !canMove)
            return;

        if (x > 0)
        {
            side = 1;
            animationScript.Flip(side);
        }

        if (x < 0)
        {
            side = -1;
            animationScript.Flip(side);
        }        
    }

    void Walk(Vector2 dir)
    {
        if (!canMove)
            return;

        if (wallGrab)
            return;

        if (!wallJumped)
        {
            rigidbody2d.velocity = new Vector2(dir.x * speed, rigidbody2d.velocity.y);
        }
        else
        {
            rigidbody2d.velocity = Vector2.Lerp(rigidbody2d.velocity, (new Vector2(dir.x * speed, rigidbody2d.velocity.y)), wallJumpLerp * Time.deltaTime);
        }
    }

    void Dash(float x, float y)
    {
        Camera.main.transform.DOComplete();
        Camera.main.transform.DOShakePosition(.2f, .5f, 14, 90, false, true);
        FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));

        hasDashed = true;

        animationScript.SetTrigger("dash");

        rigidbody2d.velocity = Vector2.zero;
        Vector2 dir = new Vector2(x, y);

        rigidbody2d.velocity += dir.normalized * dashSpeed;
        StartCoroutine(CoDashWait());
    }

    void Jump(Vector2 dir, bool wall)
    {
        slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
        ParticleSystem particle = wall ? wallJumpParticle : jumpParticle;

        rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, 0);
        rigidbody2d.velocity += dir * jumpForce;

        particle.Play();
    }

    void WallJump()
    {
        if ((side == 1 && collision.onRightWall) || (side == -1 && !collision.onRightWall))
        {
            side *= -1;
            animationScript.Flip(side);
        }

        StopCoroutine(CoDisableMovement(0));
        StartCoroutine(CoDisableMovement(0.1f));

        Vector2 wallDir = collision.onRightWall ? Vector2.left : Vector2.right;

        Jump((Vector2.up / 1.5f + wallDir / 1.5f), true);

        wallJumped = true;
    }

    void WallSlide()
    {
        if (collision.wallSide != side)
        {
            animationScript.Flip(side * -1);
        }

        if (!canMove)
            return;

        bool pushingWall = false;
        if ((rigidbody2d.velocity.x > 0 && collision.onRightWall) || (rigidbody2d.velocity.x < 0 && collision.onLeftWall))
        {
            pushingWall = true;
        }
        float push = pushingWall ? 0 : rigidbody2d.velocity.x;

        rigidbody2d.velocity = new Vector2(push, -slideSpeed);
    }

    void GroundTouch()
    {
        hasDashed = false;
        isDashing = false;

        side = animationScript.sr.flipX ? -1 : 1;

        jumpParticle.Play();
    }    

    void RigidbodyDrag(float x)
    {
        rigidbody2d.drag = x;
    }

    int ParticleSide()
    {
        int particleSide = collision.onRightWall ? 1 : -1;
        return particleSide;
    }

    void WallParticle(float vertical)
    {
        var main = slideParticle.main;

        if (wallSlide || (wallGrab && vertical < 0))
        {
            slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
            main.startColor = Color.white;
        }
        else
        {
            main.startColor = Color.clear;
        }
    }

    IEnumerator CoRotatePlatform(float waitTime)
    {
        canGoDown = true;
        effector.rotationalOffset = 180;

        yield return new WaitForSeconds(waitTime);

        effector.rotationalOffset = 0;
        canGoDown = false;
    }

    IEnumerator CoDisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    IEnumerator CoDashWait()
    {
        FindObjectOfType<GhostTrail>().ShowGhost();

        StartCoroutine(CoGroundDash());

        DOVirtual.Float(14, 0, .8f, RigidbodyDrag);

        dashParticle.Play();
        rigidbody2d.gravityScale = 0;
        GetComponent<BetterJumping>().enabled = false;
        wallJumped = true;
        isDashing = true;

        yield return new WaitForSeconds(.3f);

        dashParticle.Stop();
        rigidbody2d.gravityScale = 3;
        GetComponent<BetterJumping>().enabled = true;
        wallJumped = false;
        isDashing = false;
    }

    IEnumerator CoGroundDash()
    {
        yield return new WaitForSeconds(0.15f);

        if (collision.onGround)
            hasDashed = false;
    }

    IEnumerator CoMeleeAttack()
    {
        // Constantly loops so you only have to call it once
        while(true)
        {
            // Checks if attacking and then starts of the combo
            if (Input.GetKeyDown(KeyCode.Q))
            {
                canMove = false;
                combo++;

                animator.SetBool("isAttacking", true);

                animator.SetInteger("attackCount", combo);
                lastTime = Time.time;

                //Combo loop that ends the combo if you reach the maxTime between attacks, or reach the end of the combo
                while((Time.time - lastTime) < maxTime && combo < maxCombo)
                {
                    // Attacks if your cooldown has reset
                    if (Input.GetKeyDown(KeyCode.Q) && (Time.time - lastTime) > cooldown)
                    {
                        combo++;

                        animator.SetInteger("attackCount", combo);
                        lastTime = Time.time;
                    }
                    yield return null;
                }                
                // Resets combo and waits the remaining amount of cooldown time before you can attack again to restart the combo
                canMove = true;

                combo = 0;
                animator.SetBool("isAttacking", false);
                animator.SetInteger("attackCount", combo);
                
                yield return new WaitForSeconds(cooldown - (Time.time - lastTime));
            }
            yield return null;
        }
    }
}
