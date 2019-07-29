using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveMino : MonoBehaviour 
{
    private SoundManager soundManager;
    private PauseManager pauseManager;

    private ParticleSystem particle;

    public PlayerMovement player;

    public int HP = 10;
    public int maxHP;

    [Range(0, 10)]
    public float moveSpeed;
    public float normalMoveTime;
    public float moveRange;

    private Rigidbody2D rigid;
    private Animator anim;
    [HideInInspector]
    public SpriteRenderer sprite;

    public Fade fade;

    public TreeMove treeMove;

    public Transform originPos;


    private void Start() 
    {
        maxHP    = HP;
        rigid    = GetComponent<Rigidbody2D>();
        anim     = GetComponentInChildren<Animator>();
        particle = GetComponentInChildren<ParticleSystem>();
        sprite   = GetComponentInChildren<SpriteRenderer>();

        soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();
        pauseManager = GameObject.Find("Pause Manager").GetComponent<PauseManager>();

        //player.ForcePlayWalkAnim(true);
    }

    public void PlayAttack()
    {
        anim.SetTrigger("isAttack");
    }

    private IEnumerator CorUnBeatTime()
    {
        //GetComponent<BossMovement>().canMove = false;
        anim.SetTrigger("isDamage");
        int countTime = 0;

        while (countTime < 10)
        {
            // Alpha Effect
            if (countTime % 2 == 0)
            {
                sprite.color = new Color32(255, 255, 255, 90);
            }
            else
            {
                sprite.color = new Color32(255, 255, 255, 180);
            }

            // Wait Update Frame
            yield return new WaitForSeconds(0.1f);

            countTime++;
        }

        // Alpha Effect End
        sprite.color = new Color32(255, 255, 255, 255);
        

        // UnBeatTime Off
        //isUnbeatTime = false;

        //GetComponent<PlayerMovement>().canMove = true;
        
        yield return null;
    }

    private void Update() 
    {
        if (HP <= 0)
        {

        }
    }

    public void StartBossPattern()
    {   
        StartCoroutine(BossMovementPhase01());
    }

    public void StartBossPattern02()
    {
        StartCoroutine(BossMovementPhase02());
    }

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

    IEnumerator BossMovementPhase01()
    {
        anim.SetBool("isMoving", true);
        treeMove.StartAllTree();

        player.ForcePlayWalkAnim();
        player.transform.localScale = new Vector3(1, 1, 1);
        player.BackToOriginPos(originPos, 1.0f);        
        player.Pause();
        
        yield return new WaitForSeconds(3.0f);

        anim.SetTrigger("isAttack");
        treeMove.StopAllTree();

        player.Release();
        player.ForceStopWalkAnim();

        anim.SetBool("isMoving", false);

        yield return new WaitForSeconds(6.0f);

        StartCoroutine(BossMovementPhase01());
    }   

    IEnumerator BossMovementPhase02()
    {
        anim.SetBool("isMoving", true);
        transform.localScale = new Vector3(3, 3, 3);
        MoveOnlyX(0, 3.0f);
        yield return new WaitUntil(()=>move_is_end);
        
        MoveOnlyX(6, 3.0f);
        yield return new WaitUntil(()=>move_is_end);

        anim.SetTrigger("isAttack");

        yield return new WaitForSeconds(1.0f);
        transform.localScale = new Vector3(-3, 3, 3);
        MoveOnlyX(0, 3.0f);
        yield return new WaitUntil(()=>move_is_end);
        
        MoveOnlyX(-5, 3.0f);
        yield return new WaitUntil(()=>move_is_end);

        anim.SetTrigger("isAttack");

        yield return new WaitForSeconds(1.0f);

        StartCoroutine(BossMovementPhase02());
    } 

    public AnimationClip ani;

    public void TakeDamage(int damage)
    {   
        StartCoroutine(CoTakeDamage(damage));
    }

    public SimpleCameraShakeInCinemachine cameraShake;

    IEnumerator CoTakeDamage(int damage)
    {        
        
        soundManager.PlaySfx(soundManager.EffectSounds[1]);
        
        cameraShake.ShakeCam();

        StartCoroutine(CorUnBeatTime());
        
        HP -= damage;

        yield return new WaitForSeconds(1.5f);                     
    }
}