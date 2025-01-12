using UnityEngine;

public enum ChampionBonusType
{
    Damage,
    Defense,
    Stun,
    Heal
};

public enum BonusTarget
{
    Self,
    Enemy
};

[System.Serializable]
public class ChampionBonus
{
    public int championCount = 0;

    public ChampionBonusType championBonusType;

    public BonusTarget bonusTarget;

    public float bonusValue = 0;

    public float duration;

    public GameObject effectPrefab;

    public float ApplyOnAttack(ChampionController champion, ChampionController targetChampion)
    {
        float bonusDamage = 0;
        bool addEffect = false;
        switch (championBonusType)
        {
            case ChampionBonusType.Damage:
                bonusDamage += bonusValue;
                break;
            case ChampionBonusType.Stun:
                int rand = Random.Range(0, 100);
                if (rand < bonusValue)
                {
                    targetChampion.OnGotStun(duration);
                    addEffect = true;
                }

                break;
            case ChampionBonusType.Heal:
                champion.OnGotHeal(bonusValue);
                addEffect = true;
                break;
            default:
                break;
        }

        if (addEffect)
        {
            if (bonusTarget == BonusTarget.Self)
                champion.AddEffect(effectPrefab, duration);
            else if (bonusTarget == BonusTarget.Enemy)
                targetChampion.AddEffect(effectPrefab, duration);
        }

        return bonusDamage;
    }

    public float ApplyOnGotHit(ChampionController champion, float damage)
    {
        switch (championBonusType)
        {
            case ChampionBonusType.Defense:
                damage = ((100 - bonusValue) / 100) * damage;
                break;
            default:
                break;
        }

        return damage;
    }
}