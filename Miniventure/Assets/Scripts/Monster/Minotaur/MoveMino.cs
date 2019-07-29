using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private SpriteRenderer sprite;

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

    IEnumerator BossMovementPhase01()
    {
        anim.SetBool("isMoving", true);
        treeMove.StartAllTree();

        player.Pause();
        player.BackToOriginPos(originPos, 1.0f);
        player.ForcePlayWalkAnim();
        
        yield return new WaitForSeconds(3.0f);

        anim.SetTrigger("isAttack");
        treeMove.StopAllTree();

        player.Release();
        player.ForceStopWalkAnim();

        anim.SetBool("isMoving", false);

        yield return new WaitForSeconds(6.0f);

        StartCoroutine(BossMovementPhase01());
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