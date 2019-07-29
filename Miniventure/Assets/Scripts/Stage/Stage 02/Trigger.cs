using UnityEngine;

public class Trigger : MonoBehaviour 
{
    public Cutscene02 cutscene_02;

    public Transform trans_for_player;
    public Transform trans_for_mino;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Player")
        {
            //cutscene_02.phase_02_start = true;
            other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            other.gameObject.transform.position = trans_for_player.position;

            
        }
        else if (other.gameObject.tag == "MinoBoss")
        {
            cutscene_02.phase_02_start = true;
            other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            other.gameObject.transform.position = trans_for_mino.position;

            this.gameObject.SetActive(false);
        }
    }
}