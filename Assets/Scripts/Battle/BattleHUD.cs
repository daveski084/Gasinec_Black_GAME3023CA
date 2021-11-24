using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public Text nameText;
    public Text levelText;
    public Slider hpBar;

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
