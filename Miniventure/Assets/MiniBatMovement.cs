using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MiniBatMovement : MonoBehaviour
{
    public SimpleCameraShakeInCinemachine cameraShakeInCinemachine;

    private SoundManager soundManager;
    private Animator animator;
    public ParticleSystem particle;
    private bool moveIsEnd = false;

    public GameObject player;
    public Transform originPos;   

    public float damage;  

    public float offsetX;
    public float offsetY;

    public float normalMoveTime;    

    public float offset01, offset02, offset03;

    private Vector2 originPos01, originPos02, originPos03;
    private Vector2 appliedPos01, appliedPos02, appliedPos03;

    private Vector2 topPos;

    private Vector2 originOneThirdOffset;
    private Vector2 appliedOneThirdOffset;

    private void Awake() 
    {
        animator = GetComponentInChildren<Animator>();
        soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();
    }

    private void Start() 
    {
        DefineAllPos();   
    }

    private void DefineAllPos()
    {
        originOneThirdOffset.x = offsetX / 4; 
        originOneThirdOffset.y = offsetY / 4;

        appliedOneThirdOffset.x = offsetX / 4; 
        appliedOneThirdOffset.y = (offsetY / 4) - 1.5f;

        topPos = new Vector2(transform.position.x - offsetX, transform.position.y + offsetY); 

        // Define Origin Pos;
        originPos01 = new Vector2(transform.position.x - originOneThirdOffset.x, transform.position.y + originOneThirdOffset.y);
        originPos02 = new Vector2(originPos01.x - originOneThirdOffset.x, originPos01.y + originOneThirdOffset.y);
        originPos03 = new Vector2(originPos02.x - originOneThirdOffset.x, originPos02.y + originOneThirdOffset.y);

        // Define Applied Pos;
        appliedPos01 = new Vector2(transform.position.x - appliedOneThirdOffset.x, transform.position.y + appliedOneThirdOffset.y + offset01);        
        appliedPos02 = new Vector2(appliedPos01.x - appliedOneThirdOffset.x, appliedPos01.y + appliedOneThirdOffset.y + offset02);
        appliedPos03 = new Vector2(appliedPos02.x - appliedOneThirdOffset.x, appliedPos02.y + appliedOneThirdOffset.y + offset03);
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(originPos01, 0.3f);
        Gizmos.DrawWireSphere(originPos02, 0.3f);
        Gizmos.DrawWireSphere(originPos03, 0.3f);

        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(appliedPos01, 0.3f);
        Gizmos.DrawWireSphere(appliedPos02, 0.3f);
        Gizmos.DrawWireSphere(appliedPos03, 0.3f);
    }

    public void StartMoving()
    { 
        animator.SetBool("isDie", false);
        particle.Play();
        soundManager.PlaySfx(soundManager.EffectSounds[5]);

        DefineAllPos();
        StartCoroutine(CoStartMoving());
    }

    private IEnumerator CoStartMoving()
    {
        Move(appliedPos01, normalMoveTime);
        yield return new WaitUntil(()=>moveIsEnd);

        Move(appliedPos02, normalMoveTime);
        yield return new WaitUntil(()=>moveIsEnd);

        Move(appliedPos03, normalMoveTime);
        yield return new WaitUntil(()=>moveIsEnd);

        Move(topPos, normalMoveTime);
        yield return new WaitUntil(()=>moveIsEnd);

        yield return new WaitForSeconds(3.0f);

        particle.Play();
        soundManager.PlaySfx(soundManager.EffectSounds[5]);

        Move(topPos + ((Vector2)player.transform.position - topPos) * 3f, normalMoveTime * 8.0f, Ease.OutQuart);
        yield return new WaitUntil(()=>moveIsEnd);
    }  

    private void Move(float _offsetX, float _offsetY, float _moveTime, Ease moveType = Ease.Linear)
    {
        moveIsEnd = false;
        Vector3 endPos = new Vector3(_offsetX, _offsetY, 0f);

        transform
            .DOMove(endPos, _moveTime)
            .SetEase(moveType)
            .OnComplete(EndMove);
    }

    private void Move(Vector2 _offset, float _moveTime, Ease moveType = Ease.Linear)
    {
        moveIsEnd = false;
        Vector3 endPos = new Vector3(_offset.x, _offset.y, 0f);

        transform
            .DOMove(endPos, _moveTime)
            .SetEase(moveType)
            .OnComplete(EndMove);
    }

    private void EndMove()
    {
        moveIsEnd = true;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "DestroyCollider")
        {
            transform.DOPause();
            StopAllCoroutines();

            StartCoroutine(CoDie());
        }
        if (other.gameObject.tag == "Player")
        {
            cameraShakeInCinemachine.ShakeCam();
            soundManager.PlaySfx(soundManager.EffectSounds[2]);
            transform.DOPause();
            StopAllCoroutines();

            StartCoroutine(CoDie());
        }
    }

    public void TakeDamage()
    {

        StartCoroutine(CoDie());
    }

    private IEnumerator CoDie()
    {
        animator.SetBool("isDie", true);
        GameObject effect = GameObject.Find("Damage Effect");
        effect.SetActive(false);
        effect.transform.position = this.transform.position;
        effect.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        effect.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        

        this.gameObject.SetActive(false);
    }
}
