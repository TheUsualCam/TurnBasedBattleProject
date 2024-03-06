using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattle : MonoBehaviour
{
    public string enemyName;
    public int maxHealth;
    public int currentHealth;
    
    private int baseAttackDamage = 20;
    private int darkDamage = 150;
    private int voidDamage = 300;
    private int toTelosDamage = 700;

    public enum AttackTypes
    {
        BASIC = 0,
        DARKNESS = 1,
        VOID = 2,
        TO_TELOS = 3
    };

    public AttackTypes attackType = AttackTypes.BASIC;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public string GetName()
    {
        return enemyName;
    }

    public void SetHealth(int health)
    {
        currentHealth += health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetAttackDamage()
    {
        switch(attackType)
        {
            case AttackTypes.BASIC:
                return baseAttackDamage;
            case AttackTypes.DARKNESS:
                return darkDamage;
            case AttackTypes.VOID:
                return voidDamage;
            case AttackTypes.TO_TELOS:
                return toTelosDamage;
            default:
                return baseAttackDamage;
        }
    }
}
