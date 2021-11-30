using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleObject : MonoBehaviour
{
    public string objName;
    public int objLevel;
    public int damage;
    public int maxHP;
    public int currHP;
    



    public bool TakeDamage(int damage)
    {
        currHP -= damage;

        if( currHP <= 0)
        {
            return true;
        }
        else
        {
            return false; 
        }
    }

    public void Heal(int amount)
    {
        currHP += amount;
        if(currHP > maxHP)
        {
            currHP = maxHP;
        }


    }
}
