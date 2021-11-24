using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


// Essientially a state machine without the machine part :*)
public enum BattleState
{
    START, 
    PLAYERTURN,
    ENEMYTURN,
    WON,
    LOST
}

//Use an enum to determine which battle object the player has encountered. 
public enum BattleObjectCase
{
    DUMMY,
    ENEMYTYPE1,
    ENEMYTYPE2,
    ENEMYTYPE3,
    ENEMYTYPE4,
    ENEMYTYPE5,
    BOSS
}

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public Transform playerBattleLocation;
    public Transform enemyBattleLocation; 

    public BattleState currState;
    BattleObject playerBO;
    BattleObject enemyBO;
    public Text dialogueText;
    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;
    
    // Start is called before the first frame update
    void Start()
    {
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

        GameObject enemyGameObject = Instantiate(enemyPrefab, enemyBattleLocation);
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
        }
        else if (currState == BattleState.LOST)
        {
            dialogueText.text = "You have lost.";
        }
    }
    //2. Waits for the user to choose an action. At the time of this writing the player can only Attack and Heal. 
    void PlayerTurn()
    {
        dialogueText.text = "Choose an action: "; 
    }

    //3. Occurs after the user has choosen 'attack'.
    IEnumerator PlayerAttack()
    {
        // Actions towards enenmy (damage, paralize, sleep etc).

        bool isDead = enemyBO.TakeDamage(playerBO.damage);
        
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
        playerBO.Heal(5);
        playerHUD.UpdateHP(playerBO.currHP);
        dialogueText.text = "You healed yourself!";

        yield return new WaitForSeconds(2.0f);
        currState = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        dialogueText.text = enemyBO.objName + "Attacks!";


        yield return new WaitForSeconds(1.0f);

        // Update player HUD 
        bool isPlayerDead = playerBO.TakeDamage(enemyBO.damage);
        playerHUD.UpdateHP(enemyBO.damage);

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
}
