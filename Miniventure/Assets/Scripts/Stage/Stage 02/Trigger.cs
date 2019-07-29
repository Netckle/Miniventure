using UnityEngine;

public class Trigger : MonoBehaviour {
    public Cutscene02 cutscene;
    public Transform pos;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player")
        {
            cutscene.phase_02_start = true;
            other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            other.gameObject.transform.position = pos.position;
        }
    }
}