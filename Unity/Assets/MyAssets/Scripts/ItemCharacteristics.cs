using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCharacteristics
{
    public int number = 0;
    public string type = "";
    public string atk1 = "Atk1";
    public string atk2 = "Atk2";
    public string kick = "Atk3";
    public int maxStack = 64;
    private Buff atk1Buff = new Buff();
    private Buff atk2Buff = new Buff();
    private Buff atk3Buff = new Buff();
    private GameObject atk1Projectile;
    private GameObject atk2Projectile;
    private GameObject atk3Projectile;

    public void SetBuffs(Buff Atk1, Buff Atk2, Buff Atk3)
    {
        atk1Buff = new Buff(Atk1);
        atk2Buff = new Buff(Atk2);
        atk3Buff = new Buff(Atk3);      
    }

    public void SetProjectiles(string path1, string path2, string path3)
    {
        if (path1 != "")
        {
            atk1Projectile = (GameObject)Resources.Load(path1);
        }
        if (path2 != "")
        {
            atk2Projectile = (GameObject)Resources.Load(path2);
        }
        if (path3 != "")
        {
            atk3Projectile = (GameObject)Resources.Load(path3);
        }
    }

    public Buff GetBuff(int index)
    {
        switch (index)
            {
            case 1:
                return atk1Buff;
            case 2:
                return atk2Buff;
            case 3:
                return atk3Buff;
        }
        return new Buff();
    }
    public GameObject GetProjectile(int index)
    {
        switch (index)
        {
            case 1:
                return atk1Projectile;
            case 2:
                return atk2Projectile;
            case 3:
                return atk3Projectile;
        }
        return null;
    }
}
