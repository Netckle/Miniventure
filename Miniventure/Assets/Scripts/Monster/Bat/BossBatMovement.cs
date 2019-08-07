using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossBatMovement : MonoBehaviour 
{
    private ParticleSystem particle;
    private PlayerMovement player;

    [HideInInspector]
    //public Rigidbody2D rigid;
    private Animator anim;
    private SpriteRenderer sprite;
    public Collider2D collider2d;

    [Range(0, 50)]
    public int HP;
    [Range(0, 50)]
    public int maxHP = 20;

    public bool isUnbeatTime = false;
    public bool moveIsEnd = false;

    public float normalMoveTime = 1.0f;

    public Transform originPos;
    public GameObject damageEffect;

    public float offsetX, offsetY;

    public bool phase02on = false;

    public MiniBatMovement[] miniBats = new MiniBatMovement[2];

    private bool canDamaged = false;

    public Rigidbody2D rigidbody2d;

    private void Start() 
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
        //StartBossMove();

        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    public void StartBossMove()
    {
        StartCoroutine(NormalPattern());
    }

    private void Update() 
    {
        canMoveNext = CheckAllMiniBatIsDeleted();
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

    public bool canMoveNext = false;

    private IEnumerator NormalPattern()
    {
        canDamaged = false;
        yield return new WaitForSeconds(2.0f);

        foreach (MiniBatMovement miniBat in miniBats)
        {
            miniBat.transform.position = this.transform.position + new Vector3(0, 2f, 0);
            miniBat.gameObject.SetActive(true);

            miniBat.StartMoving();
        }   

        canMoveNext = false;

        yield return new WaitUntil(()=>canMoveNext);
        canDamaged = true;

        Move(-offsetX, transform.position.y, normalMoveTime * 6);
        yield return new WaitUntil(()=>moveIsEnd);

        Move(offsetX, transform.position.y, normalMoveTime * 6);
        yield return new WaitUntil(()=>moveIsEnd);

        StartCoroutine(NormalPattern());
    }

    public void UnderPattern()
    {
        StartCoroutine(CoUnderPattern());
    }

    private IEnumerator CoUnderPattern()
    {
        yield return new WaitForSeconds(3.0f);
        Debug.Log("CoUnderPattern에 들어왔다.");
        Move(0, 4, normalMoveTime * 2);
        yield return new WaitUntil(()=>moveIsEnd);

        Move(13, 23, normalMoveTime * 6);
        yield return new WaitUntil(()=>moveIsEnd);

        yield return new WaitUntil(()=>player.transform.position.y > this.transform.position.y - 2);
        StartCoroutine(NormalPattern());
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player" && canDamaged)
        {
            Debug.Log("2페이즈 각");
            transform.DOPause();
            StopAllCoroutines();

            other.gameObject.GetComponent<PlayerMovement>().collider2d.isTrigger = true;
            other.gameObject.GetComponent<PlayerMovement>().rigidbody2d.gravityScale = 1;
            other.gameObject.GetComponent<PlayerMovement>().rigidbody2d.bodyType = RigidbodyType2D.Dynamic;
            rigidbody2d.bodyType = RigidbodyType2D.Dynamic;     
        }
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