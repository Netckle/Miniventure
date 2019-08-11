using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class BossBatMovement : MonoBehaviour 
{
    public GameObject HPBar;

    public SimpleCameraShakeInCinemachine cameraShakeInCinemachine;

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

    public float offsetX;
    public float offsetY;

    public bool otherPatternOn = false;

    public MiniBatMovement[] miniBats = new MiniBatMovement[2];

    public bool canDamaged = false;
    public bool canContinueNormalPattern = false;

    public bool underPhaseIsEnd;

    public Vector3 boxOffset;

        Fade fade;

    private bool boxIsOpen = false;

    private void Start() 
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();

        sprite = GetComponentInChildren<SpriteRenderer>();
        unBeat = GameObject.Find("Sprite UnBeat").GetComponent<SpriteUnBeat>();
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        move = GameObject.Find("Move Manager").GetComponent<MoveManager>();
        backgroundScroll = GameObject.Find("Background Scroll").GetComponent<BackgroundScroll>();
        fade = GameObject.Find("Fade").GetComponent<Fade>();
        particle = GetComponentInChildren<ParticleSystem>();
    }

    private void Update() 
    {
        canContinueNormalPattern = CheckAllMiniBatIsDeleted(); // 미니 박쥐가 다 없어지면 TRUE;
    }

    private void FixedUpdate() 
    {
        if (canDamaged && !otherPatternOn)
        {
            HPBar.gameObject.SetActive(true);

        }
        else if (canDamaged && otherPatternOn)
        {
            HPBar.gameObject.SetActive(true);
        }
        else if (!canDamaged)
        {
            HPBar.gameObject.SetActive(false);

            
        }
    }


    private bool CheckAllMiniBatIsDeleted()
    {
        foreach (MiniBatMovement miniBat in miniBats)
        {
            if (miniBat.gameObject.activeSelf) // 활성화된 오브젝트 있으면
            {
                return false;
            }
        }
        return true;
    }

    public void StartBossMove()
    {
        StartCoroutine(NormalPattern());
    }

    private IEnumerator NormalPattern()
    {
        underPhaseIsEnd = false;
        otherPatternOn = false;
        canDamaged = false;
        yield return new WaitForSeconds(2.0f);

        foreach (MiniBatMovement miniBat in miniBats)
        {
            miniBat.transform.position = this.transform.position + new Vector3(0, 2f, 0);
            miniBat.gameObject.SetActive(true);

            miniBat.StartMoving();

            yield return new WaitForSeconds(4.0F);
        }   

        canContinueNormalPattern = false;

        yield return new WaitUntil(()=>canContinueNormalPattern); // 미니 박쥐가 다 없어질 때까지...
        canDamaged = true;

        move.Move(this.gameObject, -offsetX, transform.position.y, normalMoveTime * 6);
        yield return new WaitUntil(()=>move.moveIsEnd);

        move.Move(this.gameObject, offsetX, transform.position.y, normalMoveTime * 6);
        yield return new WaitUntil(()=>move.moveIsEnd);

        StartCoroutine(NormalPattern());
    }

    

    public void AfterCollideToPlayer()
    {
        transform.DOPause();
        StopAllCoroutines();

        player.collider2d.isTrigger = true;

        rigidbody2d.bodyType = RigidbodyType2D.Dynamic;
        rigidbody2d.velocity = Vector2.zero;
    }

    private IEnumerator CoAfterCollideToPlayer()
    {
        backgroundScroll.Stop();

        player.ForcePlayFallAnim();

        move.Move(player.gameObject, playerUnderOriginPos.position, normalMoveTime * 5, DG.Tweening.Ease.OutQuart);
        move.Move(this.gameObject, underOriginPos.position, normalMoveTime * 5, DG.Tweening.Ease.OutQuart);        
        yield return new WaitUntil(()=>move.moveIsEnd);        
        player.ForceStopFallAnim();
        yield return new WaitForSeconds(1.0f);
        

        AfterColliderToFloor();
    }
    
    public void AfterColliderToFloor()
    {        
        StartCoroutine(CoAfterColliderToFloor());
    }

    private IEnumerator CoAfterColliderToFloor()
    {
        otherPatternOn = true;

        canDamaged = true;
        yield return new WaitForSeconds(4.0f); // 데미지를 입힐 수 있는 시간.
        canDamaged = false;

        move.Move(this.gameObject, underTargetPos.position, normalMoveTime * 2);
        yield return new WaitUntil(()=>move.moveIsEnd);

        move.Move(this.gameObject, originPos.position, normalMoveTime * 6);
        yield return new WaitUntil(()=>move.moveIsEnd);

        yield return new WaitUntil(()=>player.transform.position.y >= this.transform.position.y);

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
        cameraShakeInCinemachine.ShakeCam();
        HP -= damage;
        unBeat.UnBeat(sprite);
        
        yield return new WaitUntil(()=>!unBeat.isUnBeatTime);
    }


    public void Die()
    {
        StartCoroutine(CoDie());
    }

    public bool isDead = false;
    private IEnumerator CoDie()
    {
        cameraShakeInCinemachine.ShakeCam(1.0f);
        fade.FadeOutSprite(sprite, 1.0f);
        yield return new WaitForSeconds(1.0f);

        particle.Play();
        yield return new WaitUntil(()=>!particle.isPlaying);

        isDead = true;
    }
}