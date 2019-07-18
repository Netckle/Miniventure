using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveSlimeDot : MonoBehaviour
{
    public StageController stage;
    public PlayerMovement player;

    public int HP = 10;

    [Range(0, 10)]
    public float moveSpeed;
    public float normalMoveTime;
    public float moveRange;

    Rigidbody2D rigid;
    Animator anim;
    
    bool slimeIsExist;
    bool canDamaged;

    bool moveIsEnd;
    bool spawnIsEnd;
    bool pause;
    bool corIsRunning;

    ParticleSystem particle;
    Vector3 pausePos;

    IEnumerator bossPattern;

    void Start()
    {
        rigid    = GetComponent<Rigidbody2D>();
        anim     = GetComponentInChildren<Animator>();
        particle = GetComponentInChildren<ParticleSystem>();
    }

    public void Pause()
    {
        pausePos = transform.position;
        pause = true;
        anim.StartPlayback();
    }

    public void Release()
    {
        pause = false;
        anim.StopPlayback();
    }

    void EndMove()
    {
        moveIsEnd = true;
    }

    void MoveX(float moveDistance, float moveTime)
    {
        moveIsEnd = false;

        float endPos = moveDistance;
        transform.DOMoveX(endPos, moveTime)
            .SetEase(Ease.OutQuart)
            .OnComplete(EndMove);
    }

    void MoveToMiddle(float moveTime)
    {
        moveIsEnd = false;
        float endPos = 0;

        transform.DOMoveX(endPos, moveTime)
            .SetEase(Ease.OutQuart)
            .OnComplete(EndMove);
    }

    void SpawnSlime(int count)
    {
        spawnIsEnd = false;
        stage.SpawnMiniSlimes(count);        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Pause();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Release();
        }
        spawnIsEnd = stage.CheckSlimeIsActive();        

        if (pause)
        {
            transform.position = pausePos;
        }
    }

    public void StopCor()
    {
        StopCoroutine(bossPattern);
    }

    public void StartBossMove(int phase)
    {
        if (corIsRunning && bossPattern != null)
        {
            // 이미 실행중인 코루틴 중단 및 새로운 코루틴으로 교체.
            Debug.Log("보스패턴코루틴을 중단합니다.");

            StopCoroutine(bossPattern);
            corIsRunning = false;
        }

        switch(phase)
        {
            case 1:
                bossPattern = BossMovementPhase01();
                break;
            case 2:
                bossPattern = BossMovementPhase02();
                break;
        }

        StartCoroutine(bossPattern);
    }

    IEnumerator BossMovementPhase01()
    {
        canDamaged = false;

        MoveToMiddle(normalMoveTime);
        yield return new WaitUntil(()=>moveIsEnd && !pause);

        Debug.Log("1단계 통과");

        MoveX(-moveRange, normalMoveTime); // Move Left
        yield return new WaitUntil(()=>moveIsEnd && !pause);

        Debug.Log("2단계 통과");

        MoveToMiddle(normalMoveTime);
        yield return new WaitUntil(()=>moveIsEnd && !pause);

        Debug.Log("3단계 통과");

        MoveX(moveRange, normalMoveTime); // Move Right
        yield return new WaitUntil(()=>moveIsEnd && !pause);

        Debug.Log("4단계 통과");

        SpawnSlime(2);
        yield return new WaitUntil(()=>spawnIsEnd && !pause);

        canDamaged = true;

        yield return new WaitForSeconds(2.0f);

        StartCoroutine(BossMovementPhase01());
    }

    IEnumerator BossMovementPhase02()
    {
        int[] nums = RandomInt.getRandomInt(3, 0, 3);

        for (int i = 0; i < 3; i++)
        {
            transform.position = stage.linePos[nums[i]].position;

            MoveX(normalMoveTime * 2, normalMoveTime * 2);
            yield return new WaitUntil(()=>moveIsEnd && !pause);
        }

        yield return new WaitForSeconds(3.0f);

        StartCoroutine(BossMovementPhase02());
    }

    public void TakeDamage(int damage)
    {   
        if (canDamaged)
            StartCoroutine(CoTakeDamage(damage));
    }

    IEnumerator CoTakeDamage(int damage)
    {        
        Camera.main.GetComponent<CameraShake>().Shake(0.3f, 0.3f);
        
        HP -= damage;
        Debug.Log(this.gameObject.name + "이 " + damage + "의 데미지를 입었습니다.");

        yield return new WaitForSeconds(1.5f);                
    }
}
