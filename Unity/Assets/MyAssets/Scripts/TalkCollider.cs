using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerControls>() != null)
        {
            collision.gameObject.GetComponent<PlayerControls>().IncludeTalk(transform.parent.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerControls>() != null)
        {
            collision.gameObject.GetComponent<PlayerControls>().ExcludeTalk(transform.parent.gameObject);
        }
    }
}
