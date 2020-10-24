using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterLevel
{
    public int value;
    public int curExp;
    public int maxExp;
    public float expGrowthPerLvl;

    public ParameterLevel(int v, int c = 0, int m = 100, float e = 2.0f)
    {
        value = v;
        curExp = c;
        maxExp = m;
        expGrowthPerLvl = e;
    }
}
public partial class PlayerControls : BasicMovement
{
    public ParameterLevel MindPower = new ParameterLevel(5);
    public ParameterLevel MindWit = new ParameterLevel(10);
    public ParameterLevel BodyEndurance = new ParameterLevel(10);
    public ParameterLevel ArmsStrength = new ParameterLevel(5);
    public ParameterLevel ArmsSpeed = new ParameterLevel(5);
    int baseArmsSpeed = 5;
    public ParameterLevel LegsStrength = new ParameterLevel(5);
    public ParameterLevel LegsSpeed = new ParameterLevel(5);
    int baseLegsSpeed = 5;

    public void RecalcCharacteristics()
    {
        RecalcAtkSpd();
        RecalcSpd();
    }

    public void RecalcAtk()
    {
    }
    public void RecalcAtkSpd()
    {
        anim.SetVar("AtkSpd", (float)(ArmsSpeed.value)/(float)(baseArmsSpeed));
    }
    public void RecalcSpd()
    {
        anim.SetVar("Spd", (float)(LegsSpeed.value) / (float)(baseLegsSpeed));
        move.walkSpeed = move.baseWalkSpeed * (float)(LegsSpeed.value) / (float)(baseLegsSpeed);
    }

    public float CalcWeaponPenalty(int weaponStr = 0)
    {
        return weaponStr > ArmsStrength.value ? ((float)weaponStr / (float)ArmsStrength.value) : 1.0f;
    }
}
