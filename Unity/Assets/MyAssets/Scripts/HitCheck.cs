using UnityEngine;

public class HitCheck : MonoBehaviour
{
    //this script requires a specified land trigger
    private void OnTriggerEnter2D(Collider2D c)
    {
        try
        {
            var parent = GetParent(gameObject);
            while (parent != null)
            {
                if (parent.GetComponent<Health>() == null || parent.GetComponent<Collider2D>() == null)
                {
                    parent = GetParent(parent);
                }
                else
                {
                    break;
                }
            }

            if (parent == null)
                return;
            if (parent.GetComponent<Collider2D>() == c)
            {
                return;
            }

            var hp = c.gameObject.GetComponent<Health>();
            var atk = parent.GetComponent<Health>().Damage();
            hp.Substract(atk);
            hp.UpdateHits();
        }
        catch
        {
        }
    }

    public GameObject GetParent(GameObject obj)
    {
        try
        {
            return obj.transform.parent.gameObject;
        }
        catch
        {
            return null;
        }
    }
}