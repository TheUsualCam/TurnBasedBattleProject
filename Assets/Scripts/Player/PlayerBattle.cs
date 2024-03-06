using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattle : MonoBehaviour
{
    private int maxHealth = 2000;
    private int maxMagic = 1000;
    private int currentHealth;
    private int currentMagic;
    private int basicAttackDamage = 50;
    private int fireballDamage = 100;
    private int lightningDamage = 200;
    private int hailstormDamage = 300;
    private int soulBombDamage = 500;
    private int thanatosDamage = 100000;

    public enum AttackTypes
    {
        BASIC = 0,
        FIRE_BALL = 1,
        LIGHTNING = 2,
        HAIL_STORM = 3,
        SOUL_BOMB = 4,
        THANATOS = 5
    };

    public AttackTypes attackType = AttackTypes.BASIC;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentMagic = maxMagic;
    }

    public void SetHealth(int health)
    {
        currentHealth += health;
    }

    public void SetMagic(int magic)
    {
        currentMagic += magic;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetMaxMagic()
    {
        return maxMagic;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetCurrentMagic()
    {
        return currentMagic;
    }

    public int GetAttackDamage()
    {
        switch(attackType)
        {
            case AttackTypes.BASIC:
                return basicAttackDamage * Random.Range(1, 4);
            case AttackTypes.FIRE_BALL:
                return fireballDamage * Random.Range(1, 4);
            case AttackTypes.HAIL_STORM:
                return hailstormDamage * Random.Range(1, 4);
            case AttackTypes.LIGHTNING:
                return lightningDamage * Random.Range(1, 4);
            case AttackTypes.SOUL_BOMB:
                return soulBombDamage * Random.Range(1, 4);
            case AttackTypes.THANATOS:
                return thanatosDamage * 10;
            default:
                return basicAttackDamage * Random.Range(1, 4);
        }
    }

    public int GetMPCost(AttackTypes attack)
    {
        switch(attack)
        {
            case AttackTypes.BASIC:
                return 0;
            case AttackTypes.FIRE_BALL:
                return 20;
            case AttackTypes.HAIL_STORM:
                return 70;
            case AttackTypes.LIGHTNING:
                return 50;
            case AttackTypes.SOUL_BOMB:
                return 100;
            case AttackTypes.THANATOS:
                return 300;
            default:
                return 20;
        }
    }

    public void SetAttackType(int attack)
    {
        attackType = (AttackTypes)attack;
    }
}
