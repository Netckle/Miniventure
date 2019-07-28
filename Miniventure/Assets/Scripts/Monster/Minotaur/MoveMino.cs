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

    private void Start() 
    {
        maxHP    = HP;
        rigid    = GetComponent<Rigidbody2D>();
        anim     = GetComponentInChildren<Animator>();
        particle = GetComponentInChildren<ParticleSystem>();
        sprite   = GetComponentInChildren<SpriteRenderer>();

        soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();
        pauseManager = GameObject.Find("Pause Manager").GetComponent<PauseManager>();
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

    IEnumerator BossMovementPhase01()
    {
        treeMove.StartAllTree();
        yield return new WaitForSeconds(3.0f);

        anim.SetBool("isAttack", true);

        yield return new WaitForSeconds(2.0f);

        anim.SetBool("isAttack", false);

        yield return new WaitForSeconds(2.0f);

        treeMove.StopAllTree();
        anim.SetBool("isStop", true);

        // 공격받을때까지 대기



        StartCoroutine(BossMovementPhase01());
    }
}