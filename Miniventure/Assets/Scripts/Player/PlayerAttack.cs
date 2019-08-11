using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private SoundManager soundManager;
    private PlayerMovement player;

    private float timeBtwAttack;
    public float  startTimeBtwAttack;

    public Transform attackPos;
    public LayerMask whatIsEnemies;
    private Animator  playerAnim;

    // public float attackRangeX, attackRangeY
    public float attackRange;
    public int damage;

    private bool attackIsRunning;    

    public GameObject damageEffect;

    private void Start() 
    {
        soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();

        player = GetComponentInParent<PlayerMovement>();
        playerAnim = player.GetComponentInChildren<Animator>();
    }    

    public void OnClickAttack()
    {
        attackIsRunning = true;
    }

    void ShowDamageEffect(Vector3 pos, float size = 1.0f)
    {
        damageEffect.SetActive(false);

        damageEffect.transform.position = pos;
        damageEffect.transform.localScale = new Vector3(size, size, size);
        
        damageEffect.SetActive(true);
    }

    void Update()
    {        
        if (timeBtwAttack <= 0)
        {
            if (attackIsRunning && player.canMove)
            {
                soundManager.PlaySfx(soundManager.EffectSounds[0]);
                playerAnim.SetBool("isAttacking", true);                
                
                timeBtwAttack = startTimeBtwAttack;
                Collider2D[] enemiesToDamage;
 
                // Collider2D[] = Phyiscs2D.OverlapBoxAll(attackPos.position, new Vector2(attackRangeX, attackRangeY), 0, whatIsEnemies);
                enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position , attackRange, whatIsEnemies);
                
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {       
                    if (enemiesToDamage[i].tag == "MinoBoss")
                    {
                        enemiesToDamage[i].GetComponent<MoveMino>().TakeDamage(damage);
                        ShowDamageEffect(enemiesToDamage[i].transform.position, 3.0f);                        
                    }
                    else if (enemiesToDamage[i].tag == "SlimeBoss")
                    {
                        enemiesToDamage[i].GetComponent<MoveSlimeDot>().TakeDamage(damage);
                        ShowDamageEffect(enemiesToDamage[i].transform.position); 
                    }
                    else if (enemiesToDamage[i].tag == "Slime")
                    {
                        enemiesToDamage[i].GetComponent<MiniSlimeMove>().TakeDamage(damage, transform.localScale.x);
                        ShowDamageEffect(enemiesToDamage[i].transform.position); 
                    }       
                    else if (enemiesToDamage[i].tag == "BatBoss")
                    {
                        BossBatMovement batBoss = enemiesToDamage[i].GetComponent<BossBatMovement>();

                        if (batBoss.canDamaged && !batBoss.otherPatternOn)
                        {
                            batBoss.TakeDamage(damage);
                            batBoss.AfterCollideToPlayer();
                            ShowDamageEffect(enemiesToDamage[i].transform.position, 3.0f); 
                        }
                        else if (batBoss.canDamaged && batBoss.otherPatternOn)
                        {
                            batBoss.TakeDamage(damage);
                            ShowDamageEffect(enemiesToDamage[i].transform.position, 3.0f); 
                        }
                    }  
                    else if (enemiesToDamage[i].tag == "Bat")
                    {
                        enemiesToDamage[i].GetComponent<MiniBatMovement>().TakeDamage();
                        ShowDamageEffect(enemiesToDamage[i].transform.position); 
                    }                     
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
        
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
        // Gizmos.DrawWireCube(attackPos.position, new Vector3(attackRangeX, attackRangeY, 1));
    }
}
