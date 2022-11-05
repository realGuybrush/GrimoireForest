using System;
using UnityEngine;

[Serializable]
public class BackgroundMovement:MonoBehaviour {
    //todo: later replace every layer background with a single sprite

    [SerializeField]
    private float width;

    private float speed;
    private Camera mainCamera;
    private float prevX;
    private Vector3 widthVector;
    private float halfWidth;

    public void Init(Camera main, float Speed) {
        speed = Speed;
        mainCamera = main;
        prevX = mainCamera.transform.position.x;
        widthVector = new Vector3(width, 0, 0);
        halfWidth = width / 2;
    }

    private void Update() {
        transform.position += new Vector3(-(mainCamera.transform.position.x - prevX) * speed, 0, 0);
        prevX = mainCamera.transform.position.x;
        if (transform.position.x > mainCamera.transform.position.x + halfWidth) {
            transform.position -= widthVector;
        } else if (transform.position.x < mainCamera.transform.position.x - halfWidth) {
            transform.position += widthVector;
        }
    }
}
