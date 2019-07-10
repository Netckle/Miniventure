using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniSlimeMoveOneDir : MonoBehaviour
{
    public int health;
    public float speed;
    private float dazedTime;
    public float startDazedTime;

    private Animator anim;
    public GameObject bloodEffect;

    void Start()
    {
        //anim = GetComponent<Animator>();
        //anim.SetBool("isRunning", true);
    }

    void Update()
    {   
        /*
        if (dazedTime <= 0)
        {
            speed = 5;
        }
        else
        {
            speed = 0;
            dazedTime -= Time.deltaTime;
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }

        transform.Translate(Vector2.left * speed * Time.deltaTime);
        */
    }

    public void TakeDamage(int damage)
    {
        dazedTime = startDazedTime;

        // play a hurt sound
        // show damage effect
        //Instantiate(bloodEffect, transform.position, Quaternion.identity);

        Camera.main.GetComponent<CameraShake>().Shake(0.3f, 0.3f);
        
        health -= damage;
        Debug.Log("damage TAKEN !");
    }
}
