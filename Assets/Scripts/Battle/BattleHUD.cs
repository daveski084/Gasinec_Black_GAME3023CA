/***********************************************************************;
* Project            : Untiled
*
* Author             : David Gasinec, 
* 
* Student Number     : 101187910
*
* Date created       : 21/11/2
*
* Description        : Controls the data that is displayed on the HUD.
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
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public Text nameText;
    public Text levelText;
    public Slider hpBar;
    public float healSpeed;

    public void SetHUD(BattleObject obj)
    {
        nameText.text = obj.objName;
        levelText.text = "Level " + obj.objLevel;
        hpBar.maxValue = obj.maxHP;
        hpBar.value = obj.currHP;
    }

    public void UpdateHP(int HP)
    {
        hpBar.value = HP;
    }

    
}
