using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MiniBatMovement : MonoBehaviour
{
    public SimpleCameraShakeInCinemachine cameraShake;
    public ParticleSystem particle;

    private SoundManager soundManager;
    private MoveManager moveManager;
    private Animator animator;

    private GameObject player;    

    public Transform originPos;   

    public float damage; 
    public float idleMoveTime;   

    public float waitTime;

    private Vector3 middlePos;
    private Vector3 fromMiddleToPlayer;
    private Vector3 playerPos;

    public ParticleSystem followParticle;

    private void Awake() 
    {
        soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();
        moveManager = GameObject.Find("Move Manager").GetComponent<MoveManager>();
        animator = GetComponentInChildren<Animator>();

        player = GameObject.Find("Player");

        middlePos = new Vector3(0, originPos.position.y, 0);
    }

    public void StartMove()
    {
        soundManager.PlaySfx(soundManager.EffectSounds[5]);
        StartCoroutine(CoStartMove());
    }

    private IEnumerator CoStartMove()
    {
        followParticle.gameObject.SetActive(true);
        followParticle.Play();
        moveManager.Move(this.gameObject, middlePos, idleMoveTime);
        yield return new WaitUntil(()=>moveManager.moveIsEnd);
        followParticle.gameObject.SetActive(false);
        yield return new WaitForSeconds(waitTime);
        particle.Play();
        followParticle.gameObject.SetActive(true);
        followParticle.Play();
        soundManager.PlaySfx(soundManager.EffectSounds[5]);

        fromMiddleToPlayer = player.transform.position - middlePos;
        playerPos = player.transform.position;

        moveManager.Move(this.gameObject, player.transform.position, idleMoveTime * 0.75f);
        yield return new WaitUntil(()=>moveManager.moveIsEnd);

        moveManager.Move(this.gameObject, playerPos + fromMiddleToPlayer + fromMiddleToPlayer, idleMoveTime * 0.75f * 2f);
        yield return new WaitUntil(()=>moveManager.moveIsEnd);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player")
        {
            transform.DOPause();
            cameraShake.ShakeCam();

            gameObject.SetActive(false);
        }

        if (other.gameObject.tag == "DestroyCollider")
        {
            transform.DOPause();
            gameObject.SetActive(false);
        }

        transform.position = originPos.position;
    }
}
