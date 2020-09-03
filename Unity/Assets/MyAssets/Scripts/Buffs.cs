using System;
[Serializable]
public class Buff
{
    public int amountOfHits;
    public int atk;
    public int def;
    public int hp;
    public int maxHp;
    public int percentAtk;
    public int percentDef;
    public int percentHp;
    public int percentMaxHp;
    public float percentXSpd;
    public float percentYSpd;
    public int time;

    public Buff()
    {

    }
    public Buff(Buff newBuff)
    {
        amountOfHits = newBuff.amountOfHits;
        atk = newBuff.atk;
        def = newBuff.def;
        hp = newBuff.hp;
        maxHp = newBuff.maxHp;
        percentAtk = newBuff.percentAtk;
        percentDef = newBuff.percentDef;
        percentHp = newBuff.percentHp;
        percentMaxHp = newBuff.percentMaxHp;
        percentXSpd = newBuff.percentXSpd;
        percentYSpd = newBuff.percentYSpd;
        time = newBuff.time;
    }
    public Buff( int AmountOfHits = 1, int Atk = 0, int Def = 0, int Hp = 0, int MaxHp = 0,
                 int PercentAtk = 0, int PercentDef = 0, int PercentHp = 0, int PercentMaxHp = 0,
                 float PercentXSpd = 0.0f, float PercentYSpd = 0.0f, int Time = 0)
    {
        amountOfHits = AmountOfHits;
        atk = Atk;
        def = Def;
        hp = Hp;
        maxHp = MaxHp;
        percentAtk = PercentAtk;
        percentDef = PercentDef;
        percentHp = PercentHp;
        percentMaxHp = PercentMaxHp;
        percentXSpd = PercentXSpd;
        percentYSpd = PercentYSpd;
        time = Time;
    }
}
