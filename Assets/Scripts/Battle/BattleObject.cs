/***********************************************************************;
* Project            : Untiled
*
* Author             : David Gasinec, 
* 
* Student Number     : 101187910
*
* Date created       : 21/11/2
*
* Description        : Gameobject that controls the properties of a enemy or player.
*
* Last modified      : 21/11/01
*
* Revision History   :
*
*Date        Author Ref    Revision (Date in YYYY/MM/DD format) 
*21/11/2    David Gasinec        Created script. 
*
*
|**********************************************************************/
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
    public int healAmount;

    public int escapeChance;
    public int StruggleChance;
    public int damageIncrease;





    public bool TakeDamage(int damage)
    {
        Debug.Log("hit");
        currHP -= damage;
        if ( currHP <= 0)
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
        if (currHP > maxHP)
        {
            currHP = maxHP;
        }
    }

    public void ProblemSolve(BattleObject BO)
    {
        BO.damage += damageIncrease;
    }
    public bool Struggle()
    {
        int randomNumber = Random.Range(1, 101);
        if (randomNumber <= StruggleChance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool Escape()
    {
        int randomNumber = Random.Range(1, 101);
        if (randomNumber <= escapeChance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
