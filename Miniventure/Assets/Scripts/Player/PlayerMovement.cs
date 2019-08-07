using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{    
    public Collider2D collider2d;

    [HideInInspector]
    public Rigidbody2D rigidbody2d;

    private Collision collision;
    private Animator animator;
    private AnimationScript animationScript;
    private SpriteRenderer sprite;

    private DialogueManager dialogueManager;
    private SoundManager soundManager;
    private PauseManager pauseManager;
    private Fade fade;
        
    public JoystickTest joystick;

    private Vector3 moveVector; // 플레이어 이동벡터

    [Space]
    [Header("움직임")]    
    public float moveSpeed    = 10;
    public float jumpPower    = 50;

    public bool jumpFlag;
    public bool jumpIsRunning;

    public bool canDoubleJump = true;

    public bool canMove = false;
    public bool pause = false;

    private bool groundTouch;
    private bool isDie;

    private bool pauseFlag = false;
    public bool forceMoveMode = false;
    public bool moveIsEnd = false;

    #region TEMP TRASH
    /*
    public float slideSpeed   = 5;
    public float wallJumpLerp = 10;
    public float dashSpeed    = 20;

    public bool hasDashed;

    public bool wallGrab;
    public bool wallJumped;
    public bool wallSlide;
    public bool isDashing;  

    public ParticleSystem dashParticle;
    public ParticleSystem wallJumpParticle;
    public ParticleSystem slideParticle;
    */
    #endregion

    public int side = 1;

    public ParticleSystem jumpParticle;
    
    public bool canGoDown = false;
    public PlatformEffector2D effector;

    [Space]
    [Header("공격")]    
    public float cooldown   = 0.5f; // Combo Attack Cooldown
    public float maxTime    = 0.8f; // Accepted Combo Limit Time
    public int maxCombo;            // Combo Attack Max Count
    private int combo       = 0;    // Current Combo Count
    private float lastTime;         // Last Attack Time

    [Space]
    [Header("체력")]
    public int maxHP  = 10;
    public int HP = 10;

    [Space]
    [Header("대화")]
    public bool isTalking = false;

    private bool nextDialogue;     

    void Start()
    {
        collision       = GetComponent<Collision>();
        rigidbody2d     = GetComponent<Rigidbody2D>();

        animationScript = GetComponentInChildren<AnimationScript>();
        animator        = GetComponentInChildren<Animator>();
        sprite          = GetComponentInChildren<SpriteRenderer>();
        
        soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();
        dialogueManager = GameObject.Find("Canvas").GetComponent<DialogueManager>();
        pauseManager = GameObject.Find("Pause Manager").GetComponent<PauseManager>();

        fade = GameObject.Find("Fade").GetComponent<Fade>();

        collider2d = GetComponent<Collider2D>();
    }   

    #region ONCLICK EVENT       

    public void OnClickJump()
    {
        jumpFlag = true;
        jumpIsRunning = true;
    }

    public void OnClickNextDialogue()
    {
        nextDialogue = true;        
    }

    public void OnClickRotateEffector()
    {
        StartCoroutine(CoRotatePlatform(0.5f));
    }

    #endregion

    void Update()
    {
        #region TEMP TRASH
        /*
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");

        Vector2 dir = new Vector2(x, y);
        */
        #endregion

        HandleInput(); // 조이스틱 입력 받기.

        Walk(moveVector);   

        if (!forceMoveMode)
        {   
            animationScript.SetHorizontalMovement(moveVector.x, moveVector.y, rigidbody2d.velocity.y);
        }

        if (pause)
        {
            //pauseManager.Pause(this.gameObject, false);  
            canMove = false;  
            pauseFlag = true;
        }
        else if (!pause && pauseFlag)
        {
            //pauseManager.Release(this.gameObject, "Player");
            canMove = true;
            pauseFlag = false;
        }

        if (isTalking && nextDialogue)
        {
            nextDialogue = false;
            dialogueManager.DisplayNextSentence();            
        }

        if (HP == 0)
        {
            Pause();
            
            // Die Coroutine and Game End...
        }

        if (jumpFlag)
        {
            animationScript.SetTrigger("jump");
            
            if (!collision.onGround && canDoubleJump)
            {                
                Jump(Vector2.up, false);
                canDoubleJump = false;
            }

            if (collision.onGround) // 바닥에서 시작할때
            {
                Jump(Vector2.up, false);              
            }          
            jumpFlag = false;

            #region TEMP TRASH
            /*
            if (collision.onWall && !collision.onGround)
            {
                WallJump();
            }
            */
            #endregion
        }
        
        #region TEMP TRASH
        /*
        if (Input.GetButtonDown("Dash"))//&& !hasDashed)
        {
            if (xRaw != 0 || yRaw != 0)
            {
                Debug.Log(xRaw + " " + yRaw);
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
        */
        #endregion

        if (collision.onGround)
        {
            canDoubleJump = true;
        }

        #region TEMP TRASH
        /*
        if (collision.onGround && !isDashing)
        {
            wallJumped = false;
        }

        if (wallGrab && !isDashing)
        {
            rigidbody2d.gravityScale = 0;
            if (x > 0.2f || x < -0.2f)
            {
                rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, 0);
            }

            float speedModifier = y > 0 ? 0.5f : 1;

            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, y * (moveSpeed * speedModifier));
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
        */
        #endregion

        if (collision.onGround && !groundTouch)
        {
            GroundTouch();
            groundTouch = true;
        }

        if (!collision.onGround && groundTouch)
        {
            groundTouch = false;
        }

        #region TEMP TRASH
        /*
        WallParticle(y);

        if (wallGrab || wallSlide)
            return;
        */ 
        #endregion

        if (moveVector.x > 0)
        {
            side = 1;
            animationScript.Flip(side);
        }

        if (moveVector.x < 0)
        {
            side = -1;
            animationScript.Flip(side);
        }        
    }

    void Walk(Vector2 dir)
    {
        if (!canMove)
            return;

        #region TEMP TRASH

        /*
            if (wallGrab)
                return;

            if (!wallJumped)
            {
                // 원래 여깄었음.
            }
            else
            {
                rigidbody2d.velocity = Vector2.Lerp(rigidbody2d.velocity, (new Vector2(dir.x * moveSpeed, rigidbody2d.velocity.y)), wallJumpLerp * Time.deltaTime);
            }
        */
            
        #endregion

        rigidbody2d.velocity = new Vector2(dir.x * moveSpeed, rigidbody2d.velocity.y);
    }

    #region TEMP TRASH
    /*
    private void Dash(float x, float y)
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

    private void WallJump()
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

    private void WallSlide()
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

    private void RigidbodyDrag(float x)
    {
        rigidbody2d.drag = x;
    }

    private void WallParticle(float vertical)
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

    private IEnumerator CoDashWait()
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

    private IEnumerator CoGroundDash()
    {
        yield return new WaitForSeconds(0.15f);

        if (collision.onGround)
            hasDashed = false;
    }
    */
    #endregion

    private void Jump(Vector2 dir, bool wall)
    {
        soundManager.PlaySfx(soundManager.EffectSounds[3]);

        #region TEMP TRASH
        /*
        slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
        ParticleSystem particle = wall ? wallJumpParticle : jumpParticle;
        */
        #endregion

        ParticleSystem particle = jumpParticle;

        rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, 0);
        rigidbody2d.velocity += dir * jumpPower;

        particle.Play();
    }    

    public void HandleInput()
    {
        moveVector = PoolInput();
    }

    public Vector3 PoolInput()
    {
        float horizontal = joystick.GetHorizontalValue();
        float vertical   = joystick.GetVerticalValue();
        Vector3 moveDir = new Vector3(horizontal, vertical, 0).normalized;

        return moveDir;
    }

    public void Flip(string direction)
    {
        switch(direction)
        {
            case "RIGHT":
                transform.localScale = new Vector3(1, 1, 1);
                break;
            case "LEFT":
                transform.localScale = new Vector3(-1, 1, 1);
                break;
        }
    }

    public void ForcePlayMoveAnim()
    {
        forceMoveMode = true;
        animator.SetFloat("HorizontalAxis", 1);
    }

    public void ForceStopMoveAnim()
    {
        forceMoveMode = false;
        animator.SetFloat("HorizontalAxis", 0);
    }

    private void MoveOnlyX(float _xEndPos, float moveTime)
    {
        moveIsEnd = false;

        float xEndPos = _xEndPos;

        transform
        .DOMoveX(xEndPos, moveTime)
        .SetEase(Ease.InOutQuart)
        .OnComplete(EndMove);
    }

    void EndMove()
    {
        moveIsEnd = true;
    }

    public void BackToOriginPos(Transform origin, float move_time)
    {
        StartCoroutine(CoBackToOriginPos(origin.position.x, move_time));
    }

    private IEnumerator CoBackToOriginPos(float _xEndPos, float moveTime)
    {
        MoveOnlyX(_xEndPos, moveTime);
        yield return new WaitUntil(()=>moveIsEnd);
    }

    public void Pause()
    {
        canMove = false;
    }

    public void Release()
    {
        canMove = true;
    }

    public void ChangePos(Vector3 pos)
    {
        gameObject.transform.position = pos;
    }    

    private void GroundTouch()
    {
        // hasDashed = false;
        // isDashing = false;

        side = animationScript.sr.flipX ? -1 : 1;

        jumpParticle.Play();
    }    

    private int ParticleSide()
    {
        int particleSide = collision.onRightWall ? 1 : -1;
        return particleSide;
    }
        
    private IEnumerator CoRotatePlatform(float waitTime)
    {
        canGoDown = true;
        effector.rotationalOffset = 180;

        yield return new WaitForSeconds(waitTime);

        effector.rotationalOffset = 0;
        canGoDown = false;
    }

    private IEnumerator CoDisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }    

    private IEnumerator CoMeleeAttack()
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
