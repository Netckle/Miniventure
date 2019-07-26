using UnityEngine;

public class TreeMovement : MonoBehaviour {
    
    private Rigidbody2D rigid;
    private bool is_active = true;

    private void Start() 
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() 
    {
        if (is_active)
        {
            rigid.velocity = Vector2.left * 5;
        }
        else 
        {
            rigid.velocity = Vector2.zero;
        }
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if (other.gameObject.tag == "StopCollider")
        {
            is_active = false;
            this.gameObject.SetActive(false);
        }
    }

    public void StartMove()
    {
        is_active = true;
    }
}