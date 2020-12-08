using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeCover : MonoBehaviour
{
    FakeCoverMovement FCM;
    private void Start()
    {
        FCM = transform.parent.gameObject.GetComponent<FakeCoverMovement>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerControls>() != null)
        {
            collision.gameObject.GetComponent<PlayerControls>().IncludeCover(this.gameObject);
            if(FCM != null)
                FCM.SetWake(2);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerControls>() != null)
        {
            if (FCM != null)
            {
                if (!FCM.hidden)
                {
                    collision.gameObject.GetComponent<PlayerControls>().ExcludeCover(this.gameObject);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerControls>() != null)
        {
            collision.gameObject.GetComponent<PlayerControls>().ExcludeCover(this.gameObject);
        }
    }
}
