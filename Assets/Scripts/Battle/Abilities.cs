using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    public int escapeChance;
    public int StruggleChance;

    public void Heal()
    {

    }
    public void Attack()
    {

    }
    public void ProblemSolve()
    {
        //reduce damage
    }
    public void Struggle()
    {
        int randomNumber = Random.Range(1, 101);
        if (randomNumber <= StruggleChance)
        {
            //instant win
        }
    }
    public void Escape()
    {
        int randomNumber = Random.Range(1, 101);
        if (randomNumber <= escapeChance)
        {
            //ran away
        }
    }
}
