using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public StageController stage;
    public PlayerMovement player;

    public int HP = 100;

    [Range(0, 10)]
    public float moveSpeed;
    [Range(0, 20)]
    public float moveRange;
    [Range(0, 10)]
    public float jumpPower;
    [Range(0.0f, 1.0f)]
    public float normalWaitTime;

    public int jumpCount;
    public int spawnCount;

    private Rigidbody2D rigid;
    private Animator anim;
    private bool spawnedSlimeIsExist;

    [Space]
    [Header("행동 플래그")]
    public bool moveToNewLine;
    public bool moveToMiddle;
    public bool moveToBoth;
    
    public bool spawnIsEnd;
    public bool jumpIsEnd;

    public bool moveToLeft;
    public bool moveToRight;

    [Space]
    [Header("기타")]
    public bool isJumping;
    public bool pause;

    private bool mainCorIsRunning;
    private IEnumerator bossPattern;

    private ParticleSystem particle;

    void Start()
    {
        rigid    = GetComponent<Rigidbody2D>();
        anim     = GetComponent<Animator>();
        particle = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        if (stage.CheckSlimeIsActive()) // 스테이지에 활성화되어있는 슬라임이 없는 경우...
        {
            spawnIsEnd = true;
        }
    }

    void FixedUpdate()
    {
        Jump();
    }

    void Jump()
    {
        if (!isJumping)
            return;

        rigid.velocity = Vector2.zero;

        Vector2 jumpVelocity = new Vector2(0, jumpPower);
        rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);

        isJumping = false;
    }

    public void StartBossMove(int phase)
    {
        if (mainCorIsRunning)
        {
            StopCoroutine(bossPattern);
            mainCorIsRunning = false;
        }

        switch(phase)
        {
            case 1:
                bossPattern = CoBossPatternPhase01();
                break;
            case 2:
                bossPattern = CoBossPatternPhase02();
                break;
        }

        StartCoroutine(bossPattern);
    }    

    // 종합 코루틴

    IEnumerator CoBossPatternPhase01()
    {
        mainCorIsRunning = true;

        StartCoroutine(CoMoveToNewLine(player.currentLine, normalWaitTime));
        yield return new WaitUntil(() => moveToNewLine);

        StartCoroutine(CoMoveToBoth(moveSpeed, moveRange, normalWaitTime));
        yield return new WaitUntil(() => moveToBoth);

        StartCoroutine(CoMoveToMiddle(moveSpeed));
        yield return new WaitUntil(() => moveToMiddle);

        StartCoroutine(CoJump(jumpCount, normalWaitTime));
        yield return new WaitUntil(() => jumpIsEnd);

        StartCoroutine(CoSpawnMiniSlime(spawnCount, normalWaitTime));
        yield return new WaitUntil(() => spawnIsEnd);

        StartCoroutine(CoBossPatternPhase01());
    }

    IEnumerator CoBossPatternPhase02()
    {
        mainCorIsRunning = true;

        int[] lineNumTemp = RandomInt.getRandomInt(3, 0, 3);

        for (int i = 0; i < 3; ++i)
        {           
            StartCoroutine(CoMoveToNewLine(lineNumTemp[i], normalWaitTime, moveRange));            
            yield return new WaitUntil(() => moveToNewLine);

            StartCoroutine(CoMoveToLeft(moveSpeed, moveRange));
            yield return new WaitUntil(() => moveToLeft);
        }

        StartCoroutine(CoMoveToMiddle(moveSpeed));
        yield return new WaitUntil(() => moveToMiddle);

        StartCoroutine(CoMoveToRight(moveSpeed, moveRange));
        yield return new WaitUntil(() => moveToRight);

        transform.localScale = new Vector3(-3, 3, 3);

        StartCoroutine(CoShootMiniSlime(moveSpeed, normalWaitTime));
        yield return new WaitUntil(() => spawnIsEnd);        

        StartCoroutine(CoBossPatternPhase02());
    }

    // 세부 코루틴

    IEnumerator CoMoveToNewLine(int lineIndex, float time, float padding = 0)
    {
        // 파티클 및 페이드 인 효과 추가해야함.

        moveToNewLine = false;

        yield return new WaitForSeconds(time);

        transform.position = stage.lineColliders[lineIndex].transform.position + new Vector3(padding, 0, 0);
        particle.Play();

        moveToNewLine = true;
    }

    IEnumerator CoMoveToMiddle(float speed)
    {
        moveToMiddle = false;

        Vector3 endPos = new Vector3(0, transform.position.y, 0);

        // 슬라임 방향 설정.
        if (transform.position.x > 0)
        {
            transform.localScale = new Vector3(-3, 3, 3);
        }
        else 
        {
            transform.localScale = new Vector3(3, 3, 3);
        }

        // 중간으로 이동. 
        while (Vector3.Distance(transform.position, endPos) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, speed * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }        
        transform.position = new Vector3(0, transform.position.y, 0);   

        moveToMiddle = true;  
    }

    IEnumerator CoMoveToLeft(float speed, float xPos)
    {
        moveToLeft = false;

        Vector3 leftPos  = transform.position - new Vector3(xPos * 2, 0, 0);

        while (Vector3.Distance(transform.position, leftPos) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, leftPos, speed * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }

        moveToLeft = true;
    }

    IEnumerator CoMoveToRight(float speed, float xPos)
    {
        moveToRight = false;
        Vector3 endPos = transform.position + new Vector3(xPos, 0, 0);

        transform.localScale = new Vector3(-3, 3, 3);

        while (Vector3.Distance(transform.position, endPos) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, speed * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }
        moveToRight = true;
    }

    IEnumerator CoMoveToBoth(float speed, float range, float time)
    {
        moveToBoth = false;

        Vector3 leftPos  = transform.position - new Vector3(range, 0, 0);
        Vector3 rightPos = transform.position + new Vector3(range, 0, 0);

        transform.localScale = new Vector3(3, 3, 3);

        while (Vector3.Distance(transform.position, leftPos) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, leftPos, speed * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(time);

        transform.localScale = new Vector3(-3, 3, 3);

        while (Vector3.Distance(transform.position, rightPos) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, rightPos, speed * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }

        moveToBoth = true;
    }

    IEnumerator CoSpawnMiniSlime(int count, float time)
    {
        spawnIsEnd = false;

        stage.SpawnMiniSlimes(count);
        yield return new WaitForSeconds(time);
    }

    IEnumerator CoShootMiniSlime(float speed, float time)
    {
        spawnIsEnd = false;
        for (int i = 0; i < 2; i++)
        {
            stage.miniSlimes[i].transform.position = transform.position - new Vector3(2, 0, 0);
            stage.miniSlimes[i].SetActive(true);
            stage.miniSlimes[i].GetComponent<MiniSlimeMove>().shootingMode = true;
            yield return new WaitForSeconds(time);
        }
    }

    IEnumerator CoJump(int count, float time)
    {
        jumpIsEnd = false;

        for (int i = 0; i < count; ++i)
        {
            isJumping = true;
            yield return new WaitForSeconds(time);
        }

        isJumping = false; // 만약을 대비.
        
        jumpIsEnd = true;
    }
}
