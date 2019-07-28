using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{   
    public bool move_is_end = false;

    void MoveOnlyX(float x_destination, float move_time)
    {
        move_is_end = false;

        float x_end_pos = x_destination;

        transform.DOMoveX(x_destination, move_time)
            .SetEase(Ease.InOutQuart)
            .OnComplete(EndMove);
    }

    void EndMove()
    {
        move_is_end = true;
    }

    private IEnumerator CoBackToOriginPos(float x_destination, float move_time)
    {
        MoveOnlyX(x_destination, move_time);
        yield return new WaitUntil(()=>move_is_end);
    }

    public void BackToOriginPos(Transform origin, float move_time)
    {
        StartCoroutine(CoBackToOriginPos(origin.position.x, move_time));
    }

    

    //-----

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
    public bool hasDashed;

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
    public float cooldown = 0.5f; // Combo Attack Cooldown
    public float maxTime = 0.8f; // Accepted Combo Limit Time
    public int maxCombo; // Combo Attack Max Count
    private int combo = 0; // Current Combo Count
    private float lastTime; // Last Attack Time

    public int maxHealth = 6;
    public int health = 6;

    public bool isTalking = false;

    public bool jumpFlag;

    public bool jumpIsRunning;
    bool nextDialogue;  
    
    public int jumpCount = 2;

    public bool canDoubleJump = true;

    public Fade fade;
    bool isDie;

    public bool canMove = false;

    public bool pause = false;

    public DialogueManager dialogueManager;

    private SoundManager soundManager;
    private PauseManager pauseManager;

    #region JOYSTICK
        
    public JoystickTest joystick;
    private Vector3 _moveVector; // 플레이어 이동벡터

    SpriteRenderer sprite;

    public void HandleInput()
    {
        _moveVector = PoolInput();
    }

    public Vector3 PoolInput()
    {
        float h = joystick.GetHorizontalValue();
        float v = joystick.GetVerticalValue();
        Vector3 moveDir = new Vector3(h, v, 0).normalized;

        return moveDir;
    }

    public bool force_move_mode = false;

    public void ForcePlayWalkAnim()
    {
        force_move_mode = true;
        animator.SetFloat("HorizontalAxis", 1);
    }
    public void ForceStopWalkAnim()
    {
        force_move_mode = false;
        animator.SetFloat("HorizontalAxis", 0);
    }

    #endregion

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
        sprite = GetComponentInChildren<SpriteRenderer>();
        
        soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();
        pauseManager = GameObject.Find("Pause Manager").GetComponent<PauseManager>();
    }   

    #region ONCLICK
        
    public void OnClickJump()
    {
        jumpFlag = true;
        jumpIsRunning = true;
    }

    public void OnClickDialogue()
    {
        nextDialogue = true;        
    }

    public void OnClickRotateEffector()
    {
        StartCoroutine(CoRotatePlatform(0.5f));
    }

    #endregion
    bool pauseFlag = false;
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");

        Vector2 dir = new Vector2(x, y);

        HandleInput();

        Walk(_moveVector);     
        if (!force_move_mode)
        {   
            animationScript.SetHorizontalMovement(_moveVector.x, _moveVector.y, rigidbody2d.velocity.y);
        }

        if (pause)
        {
            pauseManager.Pause(this.gameObject, false);  
            canMove = false;  
            pauseFlag = true;
        }
        else if (!pause && pauseFlag)
        {
            pauseManager.Release(this.gameObject, "Player");
            canMove = true;
            pauseFlag = false;
        }

        if (pause)
        {
             
                   
        }

        if (isTalking && nextDialogue)
        {
            nextDialogue = false;
            dialogueManager.DisplayNextSentence();            
        }

        if (health == 0)
        {
            Pause();
            //fade.FadeOutSprite(renderer, 2.0f);
            fade.FadeIn(3.0f);
        }

        if (jumpFlag && canMove)
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

            if (collision.onWall && !collision.onGround)
            {
                WallJump();
            }
            jumpFlag = false;
        }
        
        if (Input.GetButtonDown("Dash"))//&& !hasDashed)
        {
            //if (xRaw != 0 || yRaw != 0)
            //{
                Debug.Log(xRaw + " " + yRaw);
                Dash(xRaw, yRaw);
            //}
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

        if (collision.onGround)
        {
            canDoubleJump = true;
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

        // x가 0 이상일 경우...
        if (_moveVector.x > 0)
        {
            side = 1;
            animationScript.Flip(side);
        }

        // x가 0 이하일 경우...
        if (_moveVector.x < 0)
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
        soundManager.PlaySfx(soundManager.EffectSounds[3]);

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

    #region COROUTINE
        
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

    #endregion
}
