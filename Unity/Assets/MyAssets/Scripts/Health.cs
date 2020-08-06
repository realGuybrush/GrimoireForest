using UnityEngine;

public class Health : MonoBehaviour
{
    public Characteristics values = new Characteristics();

    private void Update()
    {
        UpdateTimes();
    }

    public string Name()
    {
        return values.name;
    }

    public int HealthAmount()
    {
        return values.GetHealth();
    }

    public int Damage()
    {
        return values.GetDamage();
    }

    public int Defense()
    {
        return values.GetDefense();
    }

    public void Substract(int damage)
    {
        values.SetHealth(values.GetHealth() - damage * (100 / (values.GetDefense() + 100)));
        if (values.GetHealth() <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void AddBuff(int time, int amountOfHits, int atk, int percentAtk, float percentXSpd, float percentYSpd,
        int def, int percentDef, int hp, int percentHp, int maxHp, int percentMaxHp)
    {
        values.AddBuff(time, amountOfHits, atk, percentAtk, percentXSpd, percentYSpd, def, percentDef, hp, percentHp,
            maxHp, percentMaxHp);
    }
    public void AddBuff(Buff buff)
    {
        values.AddBuff(buff);
    }

    public void UpdateTimes()
    {
        values.UpdateTimes();
    }

    public void UpdateHits()
    {
        values.UpdateHits();
    }
}
