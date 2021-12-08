using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveScript : MonoBehaviour
{

    public void EnterBattleSave()
    {
        PlayerPrefs.SetFloat("preBattleX", transform.position.x);
        PlayerPrefs.SetFloat("preBattleY", transform.position.y);
        PlayerPrefs.SetFloat("preBattleZ", transform.position.z);
    }
    public void LeaveBattleLoad()
    {
        PlayerPrefs.GetFloat("preBattleX", transform.position.x);
        PlayerPrefs.GetFloat("preBattleY", transform.position.y);
        PlayerPrefs.GetFloat("preBattleZ", transform.position.z);
    }
}
