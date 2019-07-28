using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMino : MonoBehaviour 
{
    private SoundManager soundManager;
    private PauseManager pauseManager;

    private ParticleSystem particle;

    public PlayerMovement player;

    public int HP = 10;
    public int maxHP;

    [Range(0, 10)]
    public float moveSpeed;
    public float normalMoveTime;
    public float moveRange;

    private Rigidbody2D rigid;
    private Animator anim;
    private SpriteRenderer sprite;

    public Fade fade;

    public TreeMove treeMove;

    public Transform originPos;

    private void Start() 
    {
        maxHP    = HP;
        rigid    = GetComponent<Rigidbody2D>();
        anim     = GetComponentInChildren<Animator>();
        particle = GetComponentInChildren<ParticleSystem>();
        sprite   = GetComponentInChildren<SpriteRenderer>();

        soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();
        pauseManager = GameObject.Find("Pause Manager").GetComponent<PauseManager>();

        //player.ForcePlayWalkAnim(true);
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
        //isUnbeatTime = false;

        //GetComponent<PlayerMovement>().canMove = true;
        
        yield return null;
    }

    private void Update() 
    {
        if (HP <= 0)
        {

        }
    }

    public void StartBossPattern()
    {
        StartCoroutine(BossMovementPhase01());
    }

    IEnumerator BossMovementPhase01()
    {
        //player.Pause();
        player.BackToOriginPos(originPos, 1.0f);
        player.ForcePlayWalkAnim();
        
        yield return new WaitForSeconds(3.0f);

        anim.SetBool("isAttack", true);
        treeMove.StopAllTree();

        player.Release();
        player.ForceStopWalkAnim();

        yield return new WaitForSeconds(6.0f);

        anim.SetBool("isAttack", false);  
        treeMove.StartAllTree();    

        StartCoroutine(BossMovementPhase01());
    }    

    public void TakeDamage(int damage)
    {   
        Debug.Log("데미지 들어간다");
        //if (canDamaged)
            StartCoroutine(CoTakeDamage(damage));
    }

    public SimpleCameraShakeInCinemachine cameraShake;

    IEnumerator CoTakeDamage(int damage)
    {        
        soundManager.PlaySfx(soundManager.EffectSounds[1]);
        //Camera.main.GetComponent<CameraShake>().Shake(0.3f, 0.3f);
        
        cameraShake.ShakeCam();
        //Part.SetActive(false);
        //Part.transform.position = this.transform.position;
        //Part.SetActive(true);

        StartCoroutine(CorUnBeatTime());
        
        HP -= damage;
        Debug.Log(this.gameObject.name + "이 " + damage + "의 데미지를 입었습니다.");

        yield return new WaitForSeconds(1.5f);                
    }
}