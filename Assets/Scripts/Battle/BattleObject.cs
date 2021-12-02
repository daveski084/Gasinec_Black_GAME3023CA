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
