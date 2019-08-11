using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterCollideFloor : MonoBehaviour
{
    private MoveManager moveManager;
    private Collider2D collider2d;

    public Transform playerUnderOriginPos;
    public Transform bossUnderOriginPos;
    public Transform bossUnderTargetPos;

    public bool playerMoveIsEnd, bossMoveIsEnd;

    private bool moveIsEnd;

    public float moveTime;

    private void Awake() 
    {
        moveManager = GameObject.Find("Move Manager").GetComponent<MoveManager>();
        collider2d = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player")        
        {            
            StartCoroutine(CoMovePlayerPos(other.gameObject));
        }
        else if (other.gameObject.tag == "BatBoss")
        {
            StartCoroutine(CoMoveBossPos(other.gameObject));
        }
    }

    private void Update() 
    {
        if (playerMoveIsEnd && bossMoveIsEnd)
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator CoMovePlayerPos(GameObject player)
    {
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();

        playerMovement.collider2d.isTrigger = false;
        //playerMovement.rigidbody2d.velocity = Vector2.zero;

        playerMovement.Pause();
        playerMovement.ForcePlayMoveAnim();

        moveManager.Move(player, playerUnderOriginPos.position, moveTime);
        yield return new WaitUntil(()=>moveIsEnd);

        playerMovement.Release();
        playerMovement.ForceStopMoveAnim();

        playerMoveIsEnd = true;
    }

    private IEnumerator CoMoveBossPos(GameObject boss)
    {
        BossBatMovement bossMovement = boss.GetComponent<BossBatMovement>();
        bossMovement.rigidbody2d.bodyType = RigidbodyType2D.Kinematic;
        bossMovement.rigidbody2d.velocity = Vector2.zero;
        bossMovement.underPhaseIsEnd = false;

        moveManager.Move(boss, bossUnderOriginPos.position, moveTime);
        yield return new WaitUntil(()=>moveIsEnd);

        bossMovement.underPhaseIsEnd = true;

        
        bossMoveIsEnd = true;
        bossMovement.AfterColliderToFloor();
    }
}
