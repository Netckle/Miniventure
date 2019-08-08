using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class BossBatMovement : MonoBehaviour 
{
    public Image eventBox;
    public TextMeshProUGUI text;

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
    public bool moveIsEnd = false;

    public float normalMoveTime = 1.0f;

    public Transform originPos;

    public float offsetX;
    public float offsetY;

    public bool otherPatternOn = false;

    public MiniBatMovement[] miniBats = new MiniBatMovement[2];

    private bool canDamaged = false;
    public bool canContinueNormalPattern = false;

    public float boxOffset;

    private void Start() 
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();

        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    public void StartBossMove()
    {
        StartCoroutine(NormalPattern());
    }

    private void Update() 
    {


        canContinueNormalPattern = CheckAllMiniBatIsDeleted(); // 미니 박쥐가 다 없어지면 TRUE;
    }

    private void FixedUpdate() 
    {
        if (canDamaged)
        {
            eventBox.gameObject.SetActive(true);
            Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
            float x = screenPos.x + boxOffset;
            eventBox.transform.position = new Vector3(x, screenPos.y, 0);
            text.text = "공격 가능";
        }
        else
        {
            eventBox.gameObject.SetActive(false);
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

    private IEnumerator NormalPattern()
    {
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

        Move(-offsetX, transform.position.y, normalMoveTime * 6);
        yield return new WaitUntil(()=>moveIsEnd);

        Move(offsetX, transform.position.y, normalMoveTime * 6);
        yield return new WaitUntil(()=>moveIsEnd);

        StartCoroutine(NormalPattern());
    }

    public void StartOtherPatternOn()
    {
        StartCoroutine(CoStartOtherPatternOn());
    }

    private IEnumerator CoStartOtherPatternOn()
    {
        otherPatternOn = true;

        canDamaged = true;
        yield return new WaitForSeconds(3.0f);
        canDamaged = false;

        Move(0, 4, normalMoveTime * 2);
        yield return new WaitUntil(()=>moveIsEnd);

        Move(13, 23, normalMoveTime * 6);
        yield return new WaitUntil(()=>moveIsEnd);

        yield return new WaitUntil(()=>player.transform.position.y > this.transform.position.y - 2);

        otherPatternOn = false;

        StartCoroutine(NormalPattern());
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player")
        {
            // player.TakeDamage(damage);
        }
    }

    public void AfterCollideToPlayer()
    {
        if (!otherPatternOn && canDamaged) // 노말 패턴이었다는 조건...
        {
            transform.DOPause();
            StopAllCoroutines();

            // Change Bat State
            rigidbody2d.bodyType = RigidbodyType2D.Dynamic; 

            // Change Player State
            player.collider2d.isTrigger = true;

            player.rigidbody2d.gravityScale = 1f;
            player.rigidbody2d.velocity = Vector2.zero;
            player.rigidbody2d.bodyType = RigidbodyType2D.Dynamic;    
        }
    }

    public void AfterColliderToFloor(Vector3 pos)
    {
        collider2d.isTrigger = false;

        rigidbody2d.velocity = Vector2.zero;
        rigidbody2d.bodyType = RigidbodyType2D.Kinematic;

        transform.position = pos;            

        StartOtherPatternOn();
    }

    private void Move(float offsetX, float offsetY, float moveTime, Ease moveType = Ease.Linear)
    {
        moveIsEnd = false;
        Vector2 endPos = new Vector3(offsetX, offsetY);

        transform
            .DOMove(endPos, moveTime)
            .SetEase(moveType)
            .OnComplete(EndMove);
    }

    private void EndMove()
    {
        moveIsEnd = true;
    }

    public void TakeDamage(float damage)
    {
        StartCoroutine(CoTakeDamage(damage));
    }

    private IEnumerator CoTakeDamage(float damage)
    {
        yield return null;
    }
}