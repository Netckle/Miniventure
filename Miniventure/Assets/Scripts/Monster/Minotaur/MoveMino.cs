using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

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

    public void FreeRigid()
    {
        rigid.constraints = RigidbodyConstraints2D.None;
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
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
        soundManager.PlaySfx(soundManager.EffectSounds[0]);
        treeMove.StopAllTree();             

        player.Release();
        player.ForceStopWalkAnim();

        anim.SetBool("isMoving", false);

        yield return new WaitForSeconds(0.8f);
        particle.Play();   

        yield return new WaitForSeconds(5.2f);

        StartCoroutine(BossMovementPhase01());
    }   

    IEnumerator BossMovementPhase02()
    {
        
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        
        anim.SetBool("isMoving", true);
        transform.localScale = new Vector3(3, 3, 3);

        MoveOnlyX(0, 2.5f);
        yield return new WaitUntil(()=>move_is_end);

        anim.SetTrigger("isAttack");
        soundManager.PlaySfx(soundManager.EffectSounds[0]);
        yield return new WaitForSeconds(0.8f);
        particle.Play();
        
        MoveOnlyX(8, 2.5f);
        yield return new WaitUntil(()=>move_is_end);
        anim.SetBool("isMoving", false);
        yield return new WaitForSeconds(2.0f);

        anim.SetBool("isMoving", true);

        transform.localScale = new Vector3(-3, 3, 3);
        MoveOnlyX(0, 2.5f);
        yield return new WaitUntil(()=>move_is_end);

        anim.SetTrigger("isAttack");
        soundManager.PlaySfx(soundManager.EffectSounds[0]);
        yield return new WaitForSeconds(0.8f);
        particle.Play();
        
        MoveOnlyX(-8, 2.5f);
        yield return new WaitUntil(()=>move_is_end);
        anim.SetBool("isMoving", false);
        yield return new WaitForSeconds(2.0f);

        anim.SetBool("isMoving", true);

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

    public void Die()
    {
        // 파티클 효과
        // 서서히 사라짐 효과
        // 카메라 효과

        // 대화문 작성

        StartCoroutine(CoDie());
    }

    IEnumerator CoDie()
    {
        

        anim.SetTrigger("isDie");

        sprite.color = new Color32(255, 255, 255, 255);
        cameraShake.ShakeCam(2.0f);
        fade.FadeOutSprite(sprite, 2.0f);
        yield return new WaitForSeconds(2.0f);
        particle.transform.position = new Vector3(transform.position.x, particle.transform.position.y, particle.transform.position.z);
        particle.Play();

        yield return new WaitUntil(()=>!particle.isPlaying);
        
        //this.gameObject.SetActive(false);

        fade.FadeIn(3.0f);
        yield return new WaitForSeconds(3.0f);

        SceneManager.LoadScene("Select Stage");
    }
}