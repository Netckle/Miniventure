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

    void Update()
    {
        

        if (timeBtwAttack <= 0)
        {
            // then you can attack
            if (Input.GetKey(KeyCode.Q))
            {
                playerAnim.SetBool("isAttacking", true);
                //playerAnim.SetTrigger("attack");

                timeBtwAttack = startTimeBtwAttack;
                Collider2D[] enemiesToDamage;
 
                // Collider2D[] ** = Phyiscs2D.OverlapBoxAll(attackPos.position, new Vector2(attackRangeX, attackRangeY), 0, whatIsEnemies);
                enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position , attackRange, whatIsEnemies);
                
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {                    
                    enemiesToDamage[i].GetComponent<MiniSlimeMove>().TakeDamage(damage);
                }
            }      
            else
            {
                playerAnim.SetBool("isAttacking", false);
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
