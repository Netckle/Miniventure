using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class BossBatMovement : MonoBehaviour 
{
    public GameObject canDamagedIcon;

    public SimpleCameraShakeInCinemachine cameraShake;

    private MoveManager move;
    private BackgroundScroll backgroundScroll;
    private SpriteUnBeat unBeat;

    private ParticleSystem particle;
    private PlayerMovement player;

    private Animator anim;
    private SpriteRenderer sprite;
    [HideInInspector]
    public Collider2D collider2d;
    [HideInInspector]
    public Rigidbody2D rigidbody2d;

    [Range(0, 50)]
    public int HP;
    [Range(0, 50)]
    public int maxHP = 20;

    public bool isUnbeatTime = false;
    public float normalMoveTime = 1.0f;

    public Transform playerUnderOriginPos;

    public Transform originPos;
    public Transform underOriginPos;
    public Transform underTargetPos;

    public bool otherPatternOn = false;

    public MiniBatMovement[] miniBats = new MiniBatMovement[2];

    public bool canDamaged = false;

    public Vector3 boxOffset;    
    public GameObject jumpBlock;

    private Vector3 leftEndPos;
    private Vector3 rightEndPos;

    public bool isDead = false;

    private void Awake() 
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        collider2d  = GetComponent<Collider2D>();

        leftEndPos  = new Vector3(-(originPos.position.x), 20, 0);
        rightEndPos = new Vector3(originPos.position.x, 20, 0);
        soundManager        = GameObject.Find("Sound Manager").GetComponent<SoundManager>();

        anim = GetComponentInChildren<Animator>();

        sprite = GetComponentInChildren<SpriteRenderer>();
        unBeat              = GameObject.Find("Sprite UnBeat").GetComponent<SpriteUnBeat>();
        player              = GameObject.Find("Player").GetComponent<PlayerMovement>();
        move                = GameObject.Find("Move Manager").GetComponent<MoveManager>();
        backgroundScroll    = GameObject.Find("Background Scroll").GetComponent<BackgroundScroll>();

        particle = GetComponentInChildren<ParticleSystem>();
    }

    private void Update() 
    {
        if (canDamaged)
        {
            canDamagedIcon.gameObject.SetActive(true);
        }
        else if (!canDamaged)
        {
            canDamagedIcon.gameObject.SetActive(false);
        }
    }

    public BoxCollider2D boxCollider;

    public void StartBossMove()
    {
        StartCoroutine(NormalPattern());
    }

    private IEnumerator NormalPattern()
    {
        boxCollider.isTrigger = true;
        otherPatternOn = false;
        canDamaged = false;

        yield return new WaitForSeconds(2.0f);
        jumpBlock.SetActive(false);

        foreach (MiniBatMovement miniBat in miniBats)
        {
            miniBat.transform.position = this.transform.position;
            miniBat.gameObject.SetActive(true);
            //miniBat.particle.Play();

            miniBat.StartMove();

            yield return new WaitUntil(()=>!miniBat.gameObject.activeSelf);
        }   

        Move(this.gameObject, rightEndPos, normalMoveTime);
        yield return new WaitUntil(()=>moveIsEnd);

        canDamaged = true; 
        boxCollider.isTrigger = false;       

        Move(this.gameObject, leftEndPos, normalMoveTime * 4);
        yield return new WaitUntil(()=>moveIsEnd);

        Move(this.gameObject, rightEndPos, normalMoveTime * 4);
        yield return new WaitUntil(()=>moveIsEnd);

        Move(this.gameObject, originPos.position, normalMoveTime);
        yield return new WaitUntil(()=>moveIsEnd);

        StartCoroutine(NormalPattern());
    }

    public void Move(GameObject obj, Vector3 _endPos, float moveTime, Ease moveType = Ease.Linear)
    {
        moveIsEnd = false;
        Vector3 endPos = new Vector3(_endPos.x, _endPos.y, 0);

        obj.transform
            .DOMove(endPos, moveTime)
            .SetEase(moveType)
            .OnComplete(EndMove);
    }
    public bool moveIsEnd;
    private void EndMove()
    {
        moveIsEnd = true;
    }

    public void AfterCollideToPlayer()
    {
        transform.DOPause();
        StopAllCoroutines();
        boxCollider.isTrigger = true;

        StartCoroutine(CoAfterCollideToPlayer());
    }

    private IEnumerator CoAfterCollideToPlayer()
    {
        backgroundScroll.Stop();

        player.Pause();
        player.ForcePlayFallAnim();

        move.Move(player.gameObject, new Vector3(player.transform.position.x, 9, 0), normalMoveTime * 3, DG.Tweening.Ease.OutQuart);
        move.Move(this.gameObject, new Vector3(transform.position.x, 10, 0), normalMoveTime * 3, DG.Tweening.Ease.OutQuart);        
        yield return new WaitUntil(()=>move.moveIsEnd);        
        player.ForceStopFallAnim();   
        player.Release();     

        AfterColliderToFloor();
    }
    
    public void AfterColliderToFloor()
    {        
        boxCollider.isTrigger = false;
        StartCoroutine(CoAfterColliderToFloor());
    }

    private IEnumerator CoAfterColliderToFloor()
    {
        otherPatternOn = true;
        anim.speed = 0.5f;

        canDamaged = true;
        yield return new WaitForSeconds(4.0f); // 데미지를 입힐 수 있는 시간.
        canDamaged = false;

        anim.speed = 1;

        move.Move(this.gameObject, underTargetPos.position, normalMoveTime * 2);
        yield return new WaitUntil(()=>move.moveIsEnd);

        boxCollider.isTrigger = true;

        move.Move(this.gameObject, originPos.position, normalMoveTime * 3);
        yield return new WaitUntil(()=>move.moveIsEnd);

        jumpBlock.SetActive(true);
        

        yield return new WaitUntil(()=>player.transform.position.y >= this.transform.position.y - 5f);

        backgroundScroll.Move();
        otherPatternOn = false;

        StartCoroutine(NormalPattern());
    }



    public void TakeDamage(int damage)
    {  
        StartCoroutine(CoTakeDamage(damage));
    }

    private IEnumerator CoTakeDamage(int damage)
    {
        soundManager.PlaySfx(soundManager.EffectSounds[1]);
        cameraShake.ShakeCam();
        HP -= damage;
        unBeat.UnBeat(sprite);
        
        yield return new WaitUntil(()=>!unBeat.isUnBeatTime);
    }
    SoundManager soundManager;

    public void Die()
    {
        StartCoroutine(CoDie());
    }
    public Fade fade;

    private IEnumerator CoDie()
    {
        anim.SetBool("isDie", true);
        canDamaged = false;
        soundManager.PlaySfx(soundManager.EffectSounds[4]);
        cameraShake.ShakeCam(1.0f);
        fade.FadeOutSprite(sprite, 1.0f);

        yield return new WaitForSeconds(1.0f);

        particle.Play();
        yield return new WaitUntil(()=>!particle.isPlaying);

        isDead = true;
    }
}