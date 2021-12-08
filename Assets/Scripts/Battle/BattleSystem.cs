/***********************************************************************;
* Project            : Untiled
*
* Author             : David Gasinec, 
* 
* Student Number     : 101187910
*
* Date created       : 21/11/2
*
* Description        : Bread and butter of the encounters. 
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
using TMPro;

// TODO: Nov 29, left of owrking on the HP bar. 

// Essientially a state machine without the machine part :*)
public enum BattleState
{
    START, 
    PLAYERTURN,
    ENEMYTURN,
    WON,
    LOST
}

//Use an enum to determine which battle object to load based on the one the player has encountered. 
public enum BattleObjectCase
{
    DUMMY,
    WEEKLYEXERCISES,
    WEEKLYQUIZZES,
    MIDTERM,
    CULMINATINGASSIGNMENT,
    FINALEXAM
}

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab, enemyPrefab2, enemyPrefab3, enemyPrefab4, enemyPrefab5;
    public Transform playerBattleLocation;
    public Transform enemyBattleLocation; 

    public BattleState currState;
    BattleObject playerBO;
    BattleObject enemyBO;
    public Text dialogueText;
    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public GameObject abilityOne, abilityTwo, abilityThree, abilityFour;
    public ParticleSystem playerParticle, enemyParticle;
    
    private int whichEnemy;
    private GameObject enemyGameObject;
    private bool isPlayerDead;

    // Start is called before the first frame update
    void Start()
    {
        whichEnemy = Random.Range(1, 6);
        Debug.Log(whichEnemy);
        HideActions();
        currState = BattleState.START;
        StartCoroutine(SetUpBattle());
    }

    // Set up as a coroutine 

    // 1.
    IEnumerator SetUpBattle()
    {

        //TODO: Switch statement to determine which battle object to load in.

        GameObject playerGameObject = Instantiate(playerPrefab, playerBattleLocation);
        playerBO = playerGameObject.GetComponent<BattleObject>();

        switch (whichEnemy)
        {
            case 1:
                enemyGameObject = Instantiate(enemyPrefab, enemyBattleLocation);
                break;
            case 2:
                enemyGameObject = Instantiate(enemyPrefab, enemyBattleLocation);//enemyPrefab2
                break;
            case 3:
                enemyGameObject = Instantiate(enemyPrefab, enemyBattleLocation);//enemyPrefab3
                break;
            case 4:
                enemyGameObject = Instantiate(enemyPrefab, enemyBattleLocation);//enemyPrefab4
                break;
            case 5:
                enemyGameObject = Instantiate(enemyPrefab, enemyBattleLocation);//enemyPrefab5
                break;
        }
        enemyBO = enemyGameObject.GetComponent<BattleObject>();

        
        dialogueText.text = "A wild " + enemyBO.objName + " approaches...";

        playerHUD.SetHUD(playerBO);
        enemyHUD.SetHUD(enemyBO);

        yield return new WaitForSeconds(2.0f);
        //set up complete now hand control to the user. 

        currState = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void EndBattle()
    {
        //TODO: User gains a new ability or is loaded into a previous state. 
        if (currState == BattleState.WON)
        {
            dialogueText.text = "You Won!";
            //
        }
        else if (currState == BattleState.LOST)
        {
            dialogueText.text = "You have lost.";
            //load last save
        }
    }
    //2. Waits for the user to choose an action. At the time of this writing the player can only Attack and Heal. 
    void PlayerTurn()
    {
        ShowActions();
        dialogueText.text = "Choose an action: "; 
    }

    //3. Occurs after the user has choosen 'attack'.
    IEnumerator PlayerAttack()
    {
        // Actions towards enenmy (damage, paralize, sleep etc).

        bool isDead = enemyBO.TakeDamage(playerBO.damage);
        HideActions();
        //Update Enemy HUD.
        enemyHUD.UpdateHP(enemyBO.currHP);
        dialogueText.text = "The attack landed!"; 

        yield return new WaitForSeconds(2f);

        // Is the player alive still?
        if(isDead)
        {
            currState = BattleState.WON;
            EndBattle();
        }
        else
        {
            currState = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }

    }

    // TODO: Change this to the item system. 
    IEnumerator PlayerHeal()
    {
        HideActions();
        playerParticle.Play();
        playerBO.Heal(5);
        playerHUD.UpdateHP(playerBO.currHP);
        dialogueText.text = "You healed yourself!";

        yield return new WaitForSeconds(2.0f);
        currState = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        if (enemyBO.currHP < (enemyBO.maxHP / 2)) // Enemy heals if there is less than half of health. 
        {
            enemyParticle.Play();
            dialogueText.text = enemyBO.objName + " has healed a little bit.";
            enemyBO.Heal(enemyBO.healAmount);
            enemyHUD.UpdateHP(enemyBO.currHP);
        }
        else
        {
            dialogueText.text = enemyBO.objName + "Attacks!";
            isPlayerDead = playerBO.TakeDamage(enemyBO.damage);
            playerHUD.UpdateHP(enemyBO.damage);
        }

        yield return new WaitForSeconds(1.0f);

        if (isPlayerDead)
        {
            currState = BattleState.LOST;
            EndBattle();
        }
        else
        {
            currState = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    // Ability button logic:
    public void OnAttackButtonPressed()
    {
        // If it's not the player's turn, GTFO. 
         if (currState != BattleState.PLAYERTURN)
         {
            return;
         }

         StartCoroutine(PlayerAttack());
    }

    public void OnHealButtonPressed()
    {
        // Heal the player.
        StartCoroutine(PlayerHeal());
    }

    private void HideActions()
    {
        abilityOne.SetActive(false);
        abilityTwo.SetActive(false);
        abilityThree.SetActive(false);
        abilityFour.SetActive(false);
    }
    private void ShowActions()
    {
        abilityOne.SetActive(true);

        //check script where playerprefs save loaded these bools into

        //if(hasAbilityTwo)
            abilityTwo.SetActive(true);
        //if (hasAbilityThree)
            abilityThree.SetActive(true);
       // if (hasAbilityFour)
            abilityFour.SetActive(true);
    }

}
