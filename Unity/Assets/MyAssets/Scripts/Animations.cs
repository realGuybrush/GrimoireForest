using UnityEngine;

public class Animations
{
    public Animator a;

    public void SetVar(string name, bool value)
    {
        try
        {
            a.SetBool(name, value);
        }
        catch
        {
        }
    }

    public void SetVar(string name, int value)
    {
        try
        {
            a.SetInteger(name, value);
        }
        catch
        {
        }
    }

    public void SetVar(string name, float value)
    {
        try
        {
            a.SetFloat(name, value);
        }
        catch
        {
        }
    }

    public void SetVar(string name, double value)
    {
        try
        {
            a.SetFloat(name, (float) value);
        }
        catch
        {
        }
    }
}