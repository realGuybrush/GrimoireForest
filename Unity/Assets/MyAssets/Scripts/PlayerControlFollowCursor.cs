using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public partial class PlayerControls : BasicMovement
{
    Vector3 defaultBackAngle;
    private int tactsToNormalizeMax = 100;
    private int currentNormalization = 0;
    private float normalizationStep;
    private int tactsToHoldMax = 200;
    private int holdAngleFor = 0;
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
        if ((flip.facingRight == (dX < 0)) && !(currentNormalization < tactsToNormalizeMax))
        {
            flip.Flip();//preBeta = Mathf.Sign(preBeta)*180 - preBeta;
        }
        return preBeta;
    }
    void ReturnToNormal()
    {
        //if (currentNormalization == tactsToNormalizeMax)
        //{
            //normalizationStep = ((Back.transform.localEulerAngles.z >= 270.0f ? Back.transform.localEulerAngles.z - 360.0f : Back.transform.localEulerAngles.z) + 9.47f) / (float)tactsToNormalizeMax;
        //}
        ReturningStep();
    }
    void ReturningStep()
    {
        if (currentNormalization == 0)
        {
            Back.transform.localEulerAngles = new Vector3(0.0f, 0.0f, -9.47f);
        }
        else
        {
            Back.transform.localEulerAngles = new Vector3(0.0f, 0.0f, (CalculateBeta()-9.47f)*((float)currentNormalization/(float)tactsToNormalizeMax));//new Vector3(0.0f, 0.0f, Back.transform.localEulerAngles.z - normalizationStep);
        }
        currentNormalization--;
    }
    public void StartFollowingCursor()
    {
        holdAngleFor = tactsToHoldMax;
        currentNormalization = tactsToNormalizeMax;
    }
    void FollowCursor()
    {
        //float beta = CalculateBeta();
        //Weapon.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 360.0f - turnAngleRight);
        if ((move.movementMultiplier != 0)&&(!attacking))
        {
            holdAngleFor = 0;
            currentNormalization = 0;
        }
        if (holdAngleFor > 0)
        {
            Back.transform.localEulerAngles = new Vector3(0.0f, 0.0f, CalculateBeta());
            holdAngleFor--;
        }
        else
        {
            //ReturnToNormal();
            if(currentNormalization>=0)
            ReturningStep();
        }
        //Quaternion.Euler(0.0f, 0.0f, );
        //Debug.Log(Back.transform.eulerAngles.z);
    }
}
