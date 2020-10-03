using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public partial class PlayerControls : BasicMovement
{
    Vector3 defaultBackAngle;
    Vector3 defaultHeadAngle;
    Vector3 defaultLAAngle;
    Vector3 defaultRAAngle;
    float preNormalizeAngle;
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
    Vector3 NormalAngleValue(Vector3 angle)
    {
        return angle.z >= 270.0f ? (new Vector3(0.0f, 0.0f, angle.z - 360.0f)) : angle;
    }
    void SetDefaultBoneAngles()
    {
        defaultBackAngle = NormalAngleValue(Back.transform.localEulerAngles);
        defaultHeadAngle = NormalAngleValue(Head.transform.localEulerAngles);
        defaultLAAngle = NormalAngleValue(leftArm.transform.localEulerAngles);
        defaultRAAngle = NormalAngleValue(rightArm.transform.localEulerAngles);
    }
    float ReturnCoeffLeft()
    {
        return ((float)currentNormalization / (float)tactsToNormalizeMax);
    }
    float ReturnCoeffPassed()
    {
        return (((float)tactsToNormalizeMax - (float)currentNormalization) / (float)tactsToNormalizeMax);
    }
    void ReturningStep()
    {
        if (currentNormalization == 0)
        {
            Back.transform.localEulerAngles = new Vector3(0.0f, 0.0f, defaultBackAngle.z);
            Head.transform.localEulerAngles = new Vector3(0.0f, 0.0f, defaultHeadAngle.z);
            leftArm.transform.localEulerAngles = new Vector3(0.0f, 0.0f, defaultLAAngle.z);
            rightArm.transform.localEulerAngles = new Vector3(0.0f, 0.0f, defaultRAAngle.z);
        }
        else
        {
            if (preNormalizeAngle <= 45.0f)
            {
                Back.transform.localEulerAngles = new Vector3(0.0f, 0.0f, preNormalizeAngle * ReturnCoeffLeft() + defaultBackAngle.z * ReturnCoeffPassed());
            }
            else
            {
                Back.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 45.0f* ReturnCoeffLeft() + defaultBackAngle.z * ReturnCoeffPassed());
                Head.transform.localEulerAngles = new Vector3(0.0f, 0.0f, (preNormalizeAngle - 45.0f) * ReturnCoeffLeft() + defaultHeadAngle.z);
                leftArm.transform.localEulerAngles = new Vector3(0.0f, 0.0f, (preNormalizeAngle - 45.0f) * ReturnCoeffLeft() + defaultLAAngle.z);
                rightArm.transform.localEulerAngles = new Vector3(0.0f, 0.0f, (preNormalizeAngle - 45.0f) * ReturnCoeffLeft() + defaultRAAngle.z);
            }
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
        if(!anim.a.GetBool("Grab"))
        if ((move.movementMultiplier != 0)&&(!attacking))
        {
            holdAngleFor = 0;
            currentNormalization = 0;
        }
        if (holdAngleFor > 0)
        {
            preNormalizeAngle = CalculateBeta();
            if (preNormalizeAngle <= 45.0f)
            {
                Back.transform.localEulerAngles = new Vector3(0.0f, 0.0f, preNormalizeAngle);
                Head.transform.localEulerAngles = new Vector3(0.0f, 0.0f, defaultHeadAngle.z);
                leftArm.transform.localEulerAngles = new Vector3(0.0f, 0.0f, defaultLAAngle.z);
                rightArm.transform.localEulerAngles = new Vector3(0.0f, 0.0f, defaultRAAngle.z);
            }
            else
            {
                Back.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 45.0f);
                Head.transform.localEulerAngles = new Vector3(0.0f, 0.0f, defaultHeadAngle.z + preNormalizeAngle - 45.0f);
                leftArm.transform.localEulerAngles = new Vector3(0.0f, 0.0f, defaultLAAngle.z + preNormalizeAngle - 45.0f);
                rightArm.transform.localEulerAngles = new Vector3(0.0f, 0.0f, defaultRAAngle.z + preNormalizeAngle - 45.0f);
            }
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
