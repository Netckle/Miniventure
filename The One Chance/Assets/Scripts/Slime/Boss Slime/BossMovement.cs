using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public StageController stage;
    public PlayerMovement player;

    [Range(0, 10)]
    public float moveSpeed;
    [Range(0, 20)]
    public float moveRange;
    [Range(0, 10)]
    public float jumpPower;
    [Range(0.0f, 1.0f)]
    public float waitTime;

    public int jumpCount;
    public int spawnCount;

    private Rigidbody2D rigid;
    private Animator anim;
    private bool spawnedSlimeIsExist = false;

    [Space]
    [Header("공용 플래그 변수")]
    public bool moveToNewLine = false;
    public bool moveToMiddle = false;
    public bool moveToSide = false;

    public bool blockDmgIsEnd = false;
    public bool spawnIsEnd = false;
    public bool jumpIsEnd = false;

    [Space]
    [Header("페이즈 2 플래그 변수")]
    public bool moveToLeftEnd = false;

    [Space]
    [Header("기타")]
    public bool isJumping = false;
    public bool pause = false;

    private bool mainCoroutineIsRunning = false;
    private IEnumerator bossPattern = null;

    private ParticleSystem particle;

    void Start()
    {
        rigid    = GetComponent<Rigidbody2D>();
        anim     = GetComponent<Animator>();
        particle = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        if (stage.CheckSlimeIsActive())
        {
            spawnIsEnd = true;
        }
    }

    void FixedUpdate()
    {
        Jump();
    }

    public void StartBossMove(int phase)
    {
        if (mainCoroutineIsRunning)
        {
            StopCoroutine(bossPattern);
            mainCoroutineIsRunning = false;
        }

        switch(phase)
        {
            case 1:
                bossPattern = CoBossPattern();
                break;
            case 2:
                bossPattern = CoBossPhase02Pattern();
                break;
        }

        StartCoroutine(bossPattern);
    }    

    // 보스 패턴 종합 코루틴.

    IEnumerator CoBossPattern()
    {
        mainCoroutineIsRunning = true;

        StartCoroutine(CoMoveToNewLine(player.currentLine, waitTime));
        yield return new WaitUntil(() => moveToNewLine);

        StartCoroutine(CoMoveToSide(moveSpeed, moveRange, waitTime));
        yield return new WaitUntil(() => moveToSide);

        StartCoroutine(CoMoveToMiddle(moveSpeed, waitTime));
        yield return new WaitUntil(() => moveToMiddle);

        StartCoroutine(CoJump(jumpCount, waitTime));
        yield return new WaitUntil(() => jumpIsEnd);

        StartCoroutine(CoSpawn(spawnCount, waitTime));
        yield return new WaitUntil(() => spawnIsEnd);

        StartCoroutine(CoBossPattern());
    }

    IEnumerator CoBossPhase02Pattern()
    {
        mainCoroutineIsRunning = true;

        int[] lineNumTemp = RandomInt.getRandomInt(3, 0, 3);

        for (int i = 0; i < 3; ++i)
        {           
            StartCoroutine(CoMoveToNewLine(lineNumTemp[i], waitTime, moveRange));            
            yield return new WaitUntil(() => moveToNewLine);

            StartCoroutine(CoMoveToLeftEnd(moveSpeed, waitTime, moveRange));
            yield return new WaitUntil(() => moveToLeftEnd);
        }

        StartCoroutine(CoMoveToMiddle(moveSpeed, waitTime));
        yield return new WaitUntil(() => moveToMiddle);

        StartCoroutine(CoSpawn(spawnCount, waitTime));
        yield return new WaitUntil(() => spawnIsEnd);

        StartCoroutine(CoBossPhase02Pattern());
    }

    // 공동으로 사용하는 세부 코루틴.

    IEnumerator CoMoveToNewLine(int lineNum, float waitTime, float range = 0)
    {
        // 파티클 및 페이드 인 효과 추가해야함.

        moveToNewLine = false;

        yield return new WaitForSeconds(waitTime);

        transform.position = stage.lineColliders[lineNum].transform.position + 
        new Vector3(range, 0, 0);
        particle.Play();

        moveToNewLine = true;
    }

    IEnumerator CoMoveToMiddle(float speed, float waitTime)
    {
        moveToMiddle = false;;

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

    IEnumerator CoMoveToSide(float speed, float range, float waitTime)
    {
        moveToSide = false;

        Vector3 leftPos  = transform.position - new Vector3(range, 0, 0);
        Vector3 rightPos = transform.position + new Vector3(range, 0, 0);

        transform.localScale = new Vector3(3, 3, 3);

        while (Vector3.Distance(transform.position, leftPos) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, leftPos, speed * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(waitTime);

        transform.localScale = new Vector3(-3, 3, 3);

        while (Vector3.Distance(transform.position, rightPos) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, rightPos, speed * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }

        moveToSide = true;
    }

    IEnumerator CoSpawn(int count, float waitTime)
    {
        spawnIsEnd = false;

        stage.SpawnMiniSlimes(count);
        yield return new WaitForSeconds(waitTime);
    }

    IEnumerator CoJump(int count, float waitTime)
    {
        jumpIsEnd = false;

        for (int i = 0; i < count; ++i)
        {
            isJumping = true;
            yield return new WaitForSeconds(waitTime);
        }

        isJumping = false; // 만약을 대비.
        
        jumpIsEnd = true;
    }

    // 페이즈 02에만 사용할 세부 코루틴.

    IEnumerator CoMoveToLeftEnd(float speed, float waitTime, float range)
    {
        moveToLeftEnd = false;

        Vector3 leftPos  = transform.position - new Vector3(range * 2, 0, 0);

        while (Vector3.Distance(transform.position, leftPos) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, leftPos, speed * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }

        moveToLeftEnd = true;
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

    void MatchStateWithPhase02(float speedIncrement, int spawnIncrement)
    {
        moveSpeed  += speedIncrement;
        spawnCount += spawnIncrement;
    }
}
