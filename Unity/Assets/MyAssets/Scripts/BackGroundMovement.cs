using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMovement : MonoBehaviour
{
    public GameObject mainCamera;
    public float movementCoefficient = 1.0f;
    public float tileWidth = 25.0f;
    public int tileOffset = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = GameObject.Find("Main Camera");
        }
    }

    // Update is called once per frame
    void Update()
    {
        SetPosition();
    }

    public void SetPosition()
    {
        this.gameObject.transform.position = new Vector3((mainCamera.transform.position.x + tileOffset*tileWidth)*movementCoefficient, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
    }
}
