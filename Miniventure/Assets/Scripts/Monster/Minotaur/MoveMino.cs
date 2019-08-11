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
    private PlayerMovement player;
    private BackgroundScroll backgroundScroll;
    public SimpleCameraShakeInCinemachine cameraShake;

    private Rigidbody2D rigid;
    private Animator anim;

    [HideInInspector]
    public SpriteRenderer sprite;

    [Range(0, 50)]
    public int HP;
    [Range(0, 50)]
    public int maxHP = 20;

    public bool isUnbeatTime = false;
    public bool moveIsEnd = false;

    [Range(0f, 1f)]
    public float playerPushTime = 1.0f;
    [Range(0f, 1f)]
    public float minoMoveTime = 0.5f;
    public float offsetX = 10;
    
    public Fade fade;
    public Transform playerOriginPos;

    private void Flip(string direction)
    {
        switch(direction)
        {
            case "RIGHT":
                transform.localScale = new Vector3(3, 3, 3);
                break;
            case "LEFT":
                transform.localScale = new Vector3(-3, 3, 3);
                break;
        }
    }

    private void Start() 
    {
        HP = maxHP;

        player   = GameObject.Find("Player").GetComponent<PlayerMovement>();

        rigid    = GetComponent<Rigidbody2D>();
        anim     = GetComponentInChildren<Animator>();
        particle = GetComponentInChildren<ParticleSystem>();
        sprite   = GetComponentInChildren<SpriteRenderer>();

        soundManager     = GameObject.Find("Sound Manager").GetComponent<SoundManager>();
        backgroundScroll = GameObject.Find("Background Scroll").GetComponent<BackgroundScroll>();

        cameraShake = Camera.main.GetComponentInChildren<SimpleCameraShakeInCinemachine>();
    }

    public void UnlockRigidbodyFreeze()
    {
        rigid.constraints = RigidbodyConstraints2D.None;
    }

    public void PlayAttackAnim()
    {
        anim.SetTrigger("isAttack");
    }

    private IEnumerator CorUnBeatTime()
    {
        isUnbeatTime = true;

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
        isUnbeatTime = false;
        
        yield return null;
    }

    void MoveOnlyX(float _xEndPos, float moveTime, Ease moveType = Ease.Linear)
    {
        moveIsEnd = false;
        float xEndPos = _xEndPos;

        transform
        .DOMoveX(_xEndPos, moveTime)
        .SetEase(moveType)
        .OnComplete(EndMove);
    }

    void EndMove()
    {
        moveIsEnd = true;
    }

    public void StartBossPattern()
    {   
        StartCoroutine(BossMovement01());
    }

    public void StartBossPattern02()
    {
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        StartCoroutine(BossMovement02());
    }          

    IEnumerator BossMovement01(float runningTime = 3.0f, float afterAttackTime = 5.2f)
    {
        // 배경 스크롤 시작.
        backgroundScroll.Move();

        anim.SetBool("isMoving", true);

        // 플레이어 원위치로 이동.
        player.ForcePlayMoveAnim();
        player.Flip("RIGHT");
        player.BackToOriginPos(playerOriginPos, playerPushTime);        
        player.Pause();
        
        yield return new WaitForSeconds(runningTime);

        // 배경 스크롤 멈추기.
        backgroundScroll.StopAllCoroutines();

        player.Release();
        player.ForceStopMoveAnim(); 

        anim.SetTrigger("isAttack");  
        anim.SetBool("isMoving", false);             

        yield return new WaitForSeconds(0.8f);

        // 공격 후 사운드 및 파티클 발생.
        soundManager.PlaySfx(soundManager.EffectSounds[0]);  
        particle.Play();   

        // 때릴 수 있는 쿨타임 시간.
        yield return new WaitForSeconds(afterAttackTime);

        StartCoroutine(BossMovement01());
    }      

    IEnumerator BossMovement02()
    {       
        for (int i = 0; i < 3; i++) // 파트 1
        {
            anim.SetBool("isMoving", true);
            Flip("RIGHT");

            MoveOnlyX(-10, (minoMoveTime * (10 - Mathf.Abs(transform.position.x)) / 2));
            yield return new WaitUntil(()=>moveIsEnd);
            anim.SetBool("isMoving", false);
            yield return new WaitUntil(()=>player.transform.position.x > transform.position.x + offsetX);
            anim.SetBool("isMoving", true);

            MoveOnlyX(player.transform.position.x - offsetX, (minoMoveTime * (10 - Mathf.Abs(player.transform.position.x - offsetX)) / 2));
            yield return new WaitUntil(()=>moveIsEnd);

            anim.SetTrigger("isAttack");       
            yield return new WaitForSeconds(0.8f);

            soundManager.PlaySfx(soundManager.EffectSounds[0]);            
            particle.Play();
            cameraShake.ShakeCam();

            // canDamaged = true;
            anim.SetBool("isMoving", false);
            yield return new WaitForSeconds(2.0f);
        }
        
        // 파트 2
        anim.SetBool("isMoving", true);
        MoveOnlyX(-10, (minoMoveTime * (10 - Mathf.Abs(transform.position.x)) / 2));
        yield return new WaitUntil(()=>moveIsEnd);        

        MoveOnlyX(10, 6.0f);
        yield return new WaitUntil(()=>moveIsEnd);

        Flip("LEFT");

        anim.SetBool("isMoving", false);
        yield return new WaitForSeconds(2.0f);
        
        for (int i = 0; i < 3; i++) // 파트 3
        {
            anim.SetBool("isMoving", true);
            Flip("LEFT");

            MoveOnlyX(10, (minoMoveTime * (10 - Mathf.Abs(transform.position.x)) / 2));
            yield return new WaitUntil(()=>moveIsEnd);
            anim.SetBool("isMoving", false);
            yield return new WaitUntil(()=>player.transform.position.x < transform.position.x - offsetX);
            anim.SetBool("isMoving", true);

            MoveOnlyX(player.transform.position.x + offsetX, (minoMoveTime * (10 - Mathf.Abs(player.transform.position.x + offsetX)) / 2));
            yield return new WaitUntil(()=>moveIsEnd);

            anim.SetTrigger("isAttack");       
            yield return new WaitForSeconds(0.8f);

            soundManager.PlaySfx(soundManager.EffectSounds[0]);            
            particle.Play();
            cameraShake.ShakeCam();

            // canDamaged = true;
            anim.SetBool("isMoving", false);
            yield return new WaitForSeconds(2.0f);
        }

        // 파트 4
        anim.SetBool("isMoving", true);
        MoveOnlyX(10, (minoMoveTime * (10 - Mathf.Abs(transform.position.x)) / 2));
        yield return new WaitUntil(()=>moveIsEnd);        

        MoveOnlyX(-10, 6.0f);
        yield return new WaitUntil(()=>moveIsEnd);

        Flip("RIGHT");

        anim.SetBool("isMoving", false);
        yield return new WaitForSeconds(2.0f);

        StartCoroutine(BossMovement02());
    }

    public void TakeDamage(int damage)
    {   
        StartCoroutine(CoTakeDamage(damage));
    }

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
        StartCoroutine(CoDie());
    }

    IEnumerator CoDie()
    {
        anim.SetTrigger("isDie");

        sprite.color = new Color32(255, 255, 255, 255);

        cameraShake.ShakeCam(2.0f);
        fade.FadeOutSprite(sprite, 2.0f);
        soundManager.PlaySfx(soundManager.EffectSounds[4]);
        yield return new WaitForSeconds(2.0f);
        
        particle.transform.position = new Vector3(transform.position.x, particle.transform.position.y, particle.transform.position.z);
        particle.Play();

        yield return new WaitUntil(()=>!particle.isPlaying);
        
        fade.transform.SetAsLastSibling();
        fade.FadeIn(3.0f);
        yield return new WaitForSeconds(3.0f);

        SceneManager.LoadScene("Select Stage");
    }
}