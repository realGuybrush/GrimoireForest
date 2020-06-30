using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCharacteristics
{
    public string type = "";
    public string atk1 = "Atk1";
    public string atk2 = "Atk2";
    public string kick = "Atk3";
    private Buff atk1Buff = new Buff();
    private Buff atk2Buff = new Buff();
    private Buff atk3Buff = new Buff();

    public void SetBuffs(Buff Atk1, Buff Atk2, Buff Atk3)
    {
        atk1Buff = new Buff(Atk1);
        atk2Buff = new Buff(Atk2);
        atk3Buff = new Buff(Atk3);      
    }
}
