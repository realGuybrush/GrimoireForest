using UnityEngine;

public class Passage : MonoBehaviour
{
    private Collider2D thisCollider;
    public DirectionType LocalDirection;
    //onCollision
    // search for hp script and extract hp.
    void Start()
    {
        thisCollider = this.gameObject.GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerControls>() != null)
        {
            collision.gameObject.GetComponent<PlayerControls>().IncludePassage(this.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerControls>() != null)
        {
            collision.gameObject.GetComponent<PlayerControls>().ExcludePassage(this.gameObject);
        }
    }
}
