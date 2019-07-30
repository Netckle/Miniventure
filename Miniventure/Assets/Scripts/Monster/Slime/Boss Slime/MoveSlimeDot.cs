using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MoveSlimeDot : MonoBehaviour
{
    private SoundManager soundManager;

    public StageController stage;
    public PlayerMovement player;

    public int HP = 10;
    public int maxHP;

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

    SpriteRenderer sprite;

    public GameObject Part;

    PauseManager pauseManager;

    void Start()
    {
        maxHP = HP;
        rigid    = GetComponent<Rigidbody2D>();
        anim     = GetComponentInChildren<Animator>();
        particle = GetComponentInChildren<ParticleSystem>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();
        pauseManager = GameObject.Find("Pause Manager").GetComponent<PauseManager>();
    }

    public SpriteRenderer spriteRenderer;

    public void Die()
    {
        // 파티클 효과
        // 서서히 사라짐 효과
        // 카메라 효과

        // 대화문 작성

        StartCoroutine(CoDie());
    }

    public Fade fade;
    public bool isDie = false;

    IEnumerator CoDie()
    {
        isDie = false;
        sprite.color = new Color32(255, 255, 255, 255);
        cameraShake.ShakeCam(2.0f);
        fade.FadeOutSprite(sprite, 2.0f);
        yield return new WaitForSeconds(2.0f);
        particle.Play();

        yield return new WaitUntil(()=>!particle.isPlaying);
        
        //this.gameObject.SetActive(false);

        fade.FadeIn(3.0f);
        yield return new WaitForSeconds(3.0f);

        SceneManager.LoadScene("Select Stage");
    }

    private IEnumerator CorUnBeatTime()
    {
        //GetComponent<BossMovement>().canMove = false;

        int countTime = 0;

        while (countTime < 10)
        {
            // Alpha Effect
            if (countTime % 2 == 0)
            {
                spriteRenderer.color = new Color32(255, 255, 255, 90);
            }
            else
            {
                spriteRenderer.color = new Color32(255, 255, 255, 180);
            }

            // Wait Update Frame
            yield return new WaitForSeconds(0.1f);

            countTime++;
        }

        // Alpha Effect End
        spriteRenderer.color = new Color32(255, 255, 255, 255);

        // UnBeatTime Off
        //isUnbeatTime = false;

        //GetComponent<PlayerMovement>().canMove = true;
        
        yield return null;
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
            .SetEase(Ease.InOutQuart)
            .OnComplete(EndMove);
    }

    void MoveToMiddle(float moveTime)
    {
        moveIsEnd = false;
        float endPos = 0;

        transform.DOMoveX(endPos, moveTime)
            .SetEase(Ease.InOutQuart)
            .OnComplete(EndMove);
    }

    void SpawnSlime(int count)
    {
        spawnIsEnd = false;
        stage.SpawnMiniSlimes(count);        
    }

    void Update()
    {
        if (HP <= 0)
        {
            
        }

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
        Debug.Log("페이즈1실행중");
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

        bossPattern = BossMovementPhase01();
        StartCoroutine(bossPattern);
    }

    IEnumerator BossMovementPhase02()
    {
        Debug.Log("페이즈2실행중");
        //int[] nums = RandomInt.getRandomInt(3, 0, 3);

        float tempTime = normalMoveTime * 0.75f;

        //

        transform.position = stage.linePos[0].position;
        particle.Play();
        MoveToMiddle(tempTime);
        yield return new WaitUntil(()=>moveIsEnd && !pause);

        MoveX(moveRange, tempTime);
        yield return new WaitUntil(()=>moveIsEnd && !pause);

        //

        transform.position = stage.linePos[1].position;
        particle.Play();
        MoveToMiddle(tempTime);
        yield return new WaitUntil(()=>moveIsEnd && !pause);

        MoveX(-moveRange, tempTime);
        yield return new WaitUntil(()=>moveIsEnd && !pause);

        //

        transform.position = stage.linePos[2].position;
        particle.Play();
        MoveToMiddle(tempTime);
        yield return new WaitUntil(()=>moveIsEnd && !pause);

        MoveX(moveRange, tempTime);
        yield return new WaitUntil(()=>moveIsEnd && !pause);


        yield return new WaitForSeconds(3.0f);

        bossPattern = BossMovementPhase02();
        StartCoroutine(bossPattern);
    }

    public void TakeDamage(int damage)
    {   
        //if (canDamaged)
            StartCoroutine(CoTakeDamage(damage));
    }

    public SimpleCameraShakeInCinemachine cameraShake;

    IEnumerator CoTakeDamage(int damage)
    {        
        soundManager.PlaySfx(soundManager.EffectSounds[1]);
        //Camera.main.GetComponent<CameraShake>().Shake(0.3f, 0.3f);
        
        cameraShake.ShakeCam();
        Part.SetActive(false);
        Part.transform.position = this.transform.position;
        Part.SetActive(true);

        StartCoroutine(CorUnBeatTime());
        
        HP -= damage;
        Debug.Log(this.gameObject.name + "이 " + damage + "의 데미지를 입었습니다.");

        yield return new WaitForSeconds(1.5f);                
    }
}
