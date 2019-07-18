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
    public bool canDamaged = false;

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

    private bool corIsRunning;
    private IEnumerator bossPattern;

    private ParticleSystem particle;

    void Start()
    {
        rigid    = GetComponent<Rigidbody2D>();
        anim     = GetComponentInChildren<Animator>();
        particle = GetComponentInChildren<ParticleSystem>();
    }

    Vector3 pausePos;

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
        Debug.Log("페이즈1 코루틴 가동중.");
        corIsRunning = true;

        yield return new WaitForSeconds(2.0f);

        canDamaged = false;

        StartCoroutine(CoMoveToMiddle(moveSpeed));
        yield return new WaitUntil(() => moveToMiddle && !pause);

        StartCoroutine(CoMoveToBoth(moveSpeed, moveRange, normalWaitTime));
        yield return new WaitUntil(() => moveToBoth && !pause);

        StartCoroutine(CoMoveToMiddle(moveSpeed));
        yield return new WaitUntil(() => moveToMiddle && !pause);

        //StartCoroutine(CoJump(jumpCount, normalWaitTime));
        //yield return new WaitUntil(() => jumpIsEnd && !pause);

        StartCoroutine(CoSpawnMiniSlime(spawnCount, normalWaitTime));
        yield return new WaitUntil(() => spawnIsEnd && !pause);

        canDamaged = true;

        StartCoroutine(CoBossPatternPhase01());
    }

    IEnumerator CoBossPatternPhase02()
    {
        Debug.Log("페이즈2 코루틴 가동중.");
        corIsRunning = true;

        //int[] lineNumTemp = RandomInt.getRandomInt(3, 0, 3);             

                   
            transform.position = stage.linePos[0].position;

            StartCoroutine(CoMoveToMiddle(moveSpeed));
            yield return new WaitUntil(() => moveToMiddle);

            StartCoroutine(CoMoveToRight(moveSpeed, moveRange));
            yield return new WaitUntil(() => moveToRight);

            transform.position = stage.linePos[1].position;

            StartCoroutine(CoMoveToMiddle(moveSpeed));
            yield return new WaitUntil(() => moveToMiddle);

            StartCoroutine(CoMoveToLeft(moveSpeed, moveRange));
            yield return new WaitUntil(() => moveToLeft);

            transform.position = stage.linePos[2].position;

            StartCoroutine(CoMoveToMiddle(moveSpeed));
            yield return new WaitUntil(() => moveToMiddle);

            StartCoroutine(CoMoveToRight(moveSpeed, moveRange));
            yield return new WaitUntil(() => moveToRight);
        

        StartCoroutine(CoMoveToMiddle(moveSpeed));
        yield return new WaitUntil(() => moveToMiddle);

        transform.localScale = new Vector3(-3, 3, 3);

        //StartCoroutine(CoShootMiniSlime(moveSpeed, normalWaitTime));
        //yield return new WaitUntil(() => spawnIsEnd);        

        StartCoroutine(CoBossPatternPhase02());
    }

    public void TakeDamage(int damage)
    {
        //dazedTime = startDazedTime;

        // play a hurt sound
        // show damage effect
        //Instantiate(bloodEffect, transform.position, Quaternion.identity);       

        StartCoroutine(CoTakeDamage(damage));
    }

    IEnumerator CoTakeDamage(int damage)
    {
        if (canDamaged)
        {
        Camera.main.GetComponent<CameraShake>().Shake(0.3f, 0.3f);
        
        HP -= damage;
        Debug.Log("damage TAKEN !");

        //Vector2 attackedVelocity = Vector2.zero;
            
        //attackedVelocity = new Vector2 (3f * this.transform.localScale.x, 3f);
            
        //myBody.AddForce(attackedVelocity, ForceMode2D.Impulse);

        yield return new WaitForSeconds(1.5f);
        }
        
    }

    // 세부 코루틴

    IEnumerator CoMoveToNewLine(int lineIndex, float time, float padding = 0)
    {
        // 파티클 및 페이드 인 효과 추가해야함.

        moveToNewLine = false;

        yield return new WaitForSeconds(time);

        //transform.position = stage.lineColliders[lineIndex].transform.position + new Vector3(padding, 0, 0);
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
            yield return new WaitUntil(()=>!pause);
            yield return new WaitForSeconds(0.01f);
        }        
        transform.position = new Vector3(0, transform.position.y, 0);   

        moveToMiddle = true;  
    }

    IEnumerator CoMoveToLeft(float speed, float xPos)
    {
        moveToLeft = false;

        Vector3 leftPos  = transform.position - new Vector3(xPos, 0, 0);

        while (Vector3.Distance(transform.position, leftPos) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, leftPos, speed * Time.deltaTime);
            yield return new WaitUntil(()=>!pause);
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
            yield return new WaitUntil(()=>!pause);
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
            yield return new WaitUntil(()=>!pause);
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(time);

        transform.localScale = new Vector3(-3, 3, 3);

        while (Vector3.Distance(transform.position, rightPos) > 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, rightPos, speed * Time.deltaTime);
            yield return new WaitUntil(()=>!pause);
            yield return new WaitForSeconds(0.01f);
        }

        moveToBoth = true;
    }

    IEnumerator CoSpawnMiniSlime(int count, float time)
    {
        spawnIsEnd = false;

        stage.SpawnMiniSlimes(count);
        yield return new WaitUntil(()=>!pause);
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
            yield return new WaitUntil(()=>!pause);
            yield return new WaitForSeconds(1.0f);
        }
    }

    IEnumerator CoJump(int count, float time)
    {
        jumpIsEnd = false;

        for (int i = 0; i < count; ++i)
        {
            isJumping = true;
            yield return new WaitUntil(()=>!pause);
            yield return new WaitForSeconds(time);
        }

        isJumping = false; // 만약을 대비.
        
        jumpIsEnd = true;
    }
}
