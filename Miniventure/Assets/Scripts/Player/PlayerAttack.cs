using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float timeBtwAttack;
    public float startTimeBtwAttack;

    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public Animator camAnim;
    public Animator playerAnim;

    public SpriteRenderer renderer;

    // public float attackRangeX, attackRangeY
    public float attackRange;
    public int damage;

    private bool attackIsRunning;

    private SoundManager soundManager;

    public GameObject damageEffect;

    private void Start() {
        soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();
        player = GetComponentInParent<PlayerMovement>();
    }
    

    public void OnClickAct()
    {
        attackIsRunning = true;
    }

    private PlayerMovement player;

    void Update()
    {        
        if (timeBtwAttack <= 0)
        {
            // then you can attack
            //if (Input.GetKeyDown(KeyCode.Q))
            if (attackIsRunning && player.canMove)
            {
                playerAnim.SetBool("isAttacking", true);
                //playerAnim.SetTrigger("attack");
                soundManager.PlaySfx(soundManager.EffectSounds[0]);
                timeBtwAttack = startTimeBtwAttack;
                Collider2D[] enemiesToDamage;
 
                // Collider2D[] ** = Phyiscs2D.OverlapBoxAll(attackPos.position, new Vector2(attackRangeX, attackRangeY), 0, whatIsEnemies);
                enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position , attackRange, whatIsEnemies);
                
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {       
                    if (enemiesToDamage[i].tag == "MinoBoss")
                    {
                        Debug.Log("보스 미노 데미지 준다");
                        enemiesToDamage[i].GetComponent<MoveMino>().TakeDamage(damage);
                        damageEffect.SetActive(false);
                        damageEffect.transform.position = enemiesToDamage[i].transform.position;
                        damageEffect.SetActive(true);
                    }
                    else if (enemiesToDamage[i].tag == "SlimeBoss")
                    {
                        enemiesToDamage[i].GetComponent<MoveSlimeDot>().TakeDamage(damage);
                        Debug.Log("보스 슬라임에게 데미지를 준다");
                        damageEffect.SetActive(false);
                        damageEffect.transform.position = enemiesToDamage[i].transform.position;
                        damageEffect.SetActive(true);
                    }
                    else if (enemiesToDamage[i].tag == "Slime")
                    {
                        enemiesToDamage[i].GetComponent<MiniSlimeMove>().TakeDamage(damage, transform.localScale.x);
                        Debug.Log("슬라임에게 데미지를 준다");
                        damageEffect.SetActive(false);
                        damageEffect.transform.position = enemiesToDamage[i].transform.position;
                        damageEffect.SetActive(true);
                    }       
                    else if (enemiesToDamage[i].tag == "BossBat" && enemiesToDamage[i].GetComponent<BossBatMovement>().canDamaged && !enemiesToDamage[i].GetComponent<BossBatMovement>().otherPatternOn)
                    {
                        soundManager.PlaySfx(soundManager.EffectSounds[4]);
                        enemiesToDamage[i].GetComponent<BossBatMovement>().AfterCollideToPlayer();
                        damageEffect.SetActive(false);
                        damageEffect.transform.position = enemiesToDamage[i].transform.position;
                        damageEffect.transform.localScale = new Vector3(3, 3, 3);
                        damageEffect.SetActive(true);
                    }    
                    else if (enemiesToDamage[i].tag == "BossBat" && enemiesToDamage[i].GetComponent<BossBatMovement>().canDamaged && enemiesToDamage[i].GetComponent<BossBatMovement>().otherPatternOn)
                    {
                        enemiesToDamage[i].GetComponent<BossBatMovement>().TakeDamage(1);
                        damageEffect.SetActive(false);
                        damageEffect.transform.position = enemiesToDamage[i].transform.position;
                        damageEffect.SetActive(true);
                    }  
                    else if (enemiesToDamage[i].tag == "MiniBat")
                    {
                        enemiesToDamage[i].GetComponent<MiniBatMovement>().TakeDamage();
                        damageEffect.SetActive(false);
                        damageEffect.transform.position = enemiesToDamage[i].transform.position;
                        damageEffect.SetActive(true);
                    }       
                    Debug.Log(enemiesToDamage[i].tag);                  
                }

                attackIsRunning = false;  
            }      
            else
            {
                playerAnim.SetBool("isAttacking", false);
                attackIsRunning = false;  
            }                
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
        }
    }

    

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        // Gizmos.DrawWireCube(attackPos.position, new Vector3(attackRangeX, attackRangeY, 1));
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
