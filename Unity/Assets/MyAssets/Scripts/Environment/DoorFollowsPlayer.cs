using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorFollowsPlayer : MonoBehaviour
{
    public GameObject door;

    // Update is called once per frame
    void Update()
    {
        GameObject g = GameObject.Find("Player");
        if ((g != null)&&(door!=null))
        {
            door.transform.position = new Vector3(door.transform.position.x, g.transform.position.y, door.transform.position.z);
        }
    }
}
