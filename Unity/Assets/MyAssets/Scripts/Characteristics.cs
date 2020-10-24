using System;
using System.Collections.Generic;

[Serializable]
public class Characteristics
{
    private readonly List<Buff> buffs = new List<Buff>();
    public int damage = 4;
    public int defense = 0;
    public int maxHealth = 10;
    public bool attacking = false;
    private int health = 10;
    public string name;

    public void AddBuff(Buff buff)
    {
        buffs.Add(new Buff(buff));
    }
    public void AddBuff(int time, int amountOfHits, int atk, int percentAtk, float percentXSpd, float percentYSpd,
        int def, int percentDef, int hp, int percentHp, int maxHp, int percentMaxHp)
    {
        buffs.Add(new Buff());
        buffs[buffs.Count - 1].time = time;
        buffs[buffs.Count - 1].amountOfHits = amountOfHits;
        buffs[buffs.Count - 1].atk = atk;
        buffs[buffs.Count - 1].percentAtk = percentAtk;
        buffs[buffs.Count - 1].percentXSpd = percentXSpd;
        buffs[buffs.Count - 1].percentYSpd = percentYSpd;
        buffs[buffs.Count - 1].def = def;
        buffs[buffs.Count - 1].percentDef = percentDef;
        buffs[buffs.Count - 1].hp = hp;
        buffs[buffs.Count - 1].percentHp = percentHp;
        buffs[buffs.Count - 1].maxHp = maxHp;
        buffs[buffs.Count - 1].percentMaxHp = percentMaxHp;
    }

    public void SetHealth(int value)
    {
        health = value;
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetDamage()
    {
        var sum = GetBuffSum();
        if (attacking)
            return (damage + sum.atk) * (100 + sum.percentAtk) / 100;
        return 0;
    }

    public int GetDefense()
    {
        var sum = GetBuffSum();
        return (defense + sum.def) * (100 + sum.percentDef) / 100;
    }

    public int GetMaxHealth()
    {
        var sum = GetBuffSum();
        return (maxHealth + sum.maxHp) * (100 + sum.percentMaxHp) / 100;
    }

    public void UpdateTimes()
    {
        for (var i = 0; i < buffs.Count; i++)
        {
            health += buffs[i].hp;
            health += health * buffs[i].percentHp / 100;
            buffs[i].time--;
            if (buffs[i].time == 0) buffs.RemoveAt(i);
        }

        var curMaxHealth = GetMaxHealth();
        if (health > curMaxHealth)
            health = curMaxHealth;
    }

    public void UpdateHits()
    {
        for (var i = 0; i < buffs.Count; i++)
        {
            health += buffs[i].hp;
            health += health * buffs[i].percentHp / 100;
            buffs[i].amountOfHits--;
            if (buffs[i].amountOfHits == 0) buffs.RemoveAt(i);
        }

        var curMaxHealth = GetMaxHealth();
        if (health > curMaxHealth)
            health = curMaxHealth;
    }

    private Buff GetBuffSum()
    {
        var result = new Buff();
        for (var i = 0; i < buffs.Count; i++)
        {
            result.atk += buffs[i].atk;
            result.percentAtk += buffs[i].percentAtk;
            result.percentXSpd += buffs[i].percentXSpd;
            result.percentYSpd += buffs[i].percentYSpd;
            result.def += buffs[i].def;
            result.percentDef += buffs[i].percentDef;
            result.hp += buffs[i].hp;
            result.percentHp += buffs[i].percentHp;
        }

        return result;
    }
}
