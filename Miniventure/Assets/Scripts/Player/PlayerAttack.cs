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

    public void OnClickAct()
    {
        attackIsRunning = true;
    }

    void Update()
    {        
        if (timeBtwAttack <= 0)
        {
            // then you can attack
            //if (Input.GetKeyDown(KeyCode.Q))
            if (attackIsRunning)
            {
                playerAnim.SetBool("isAttacking", true);
                //playerAnim.SetTrigger("attack");
                SoundManager.instance.PlaySfx(SoundManager.instance.EffectSounds[0]);
                timeBtwAttack = startTimeBtwAttack;
                Collider2D[] enemiesToDamage;
 
                // Collider2D[] ** = Phyiscs2D.OverlapBoxAll(attackPos.position, new Vector2(attackRangeX, attackRangeY), 0, whatIsEnemies);
                enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position , attackRange, whatIsEnemies);
                
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {       
                    if (enemiesToDamage[i].tag == "SlimeBoss")
                    {
                        enemiesToDamage[i].GetComponent<MoveSlimeDot>().TakeDamage(damage);
                        Debug.Log("보스 슬라임에게 데미지를 준다");
                    }
                    else if (enemiesToDamage[i].tag == "Slime")
                    {
                        enemiesToDamage[i].GetComponent<MiniSlimeMove>().TakeDamage(damage, transform.localScale.x);
                        Debug.Log("슬라임에게 데미지를 준다");
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
