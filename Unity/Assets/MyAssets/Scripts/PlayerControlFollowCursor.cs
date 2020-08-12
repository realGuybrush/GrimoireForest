using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public partial class PlayerControls : BasicMovement
{
    Vector3 defaultBackAngle;
    float CalculateBeta()
    {
        float dX, dY;
        float gipotenuza;
        float preBeta;
        //it should be middle of the back bone, but here would be base, because whatever
        dX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - this.transform.position.x;
        dY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - this.transform.position.y;
        gipotenuza = (float)Math.Sqrt(dX * dX + dY * dY);
        preBeta = (float)(Mathf.Rad2Deg*Mathf.Asin(dY/gipotenuza));
        if (flip.facingRight == (dX < 0))
        {
            flip.Flip();//preBeta = Mathf.Sign(preBeta)*180 - preBeta;
        }
        return preBeta;
    }
    void ReturnToNormal()
    {

    }
    void FollowCursor()
    {
        //float beta = CalculateBeta();
        //Weapon.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 360.0f - turnAngleRight);
        Back.transform.localEulerAngles = new Vector3(0.0f, 0.0f, CalculateBeta());//Quaternion.Euler(0.0f, 0.0f, );
        //Debug.Log(Back.transform.eulerAngles.z);
    }
}
