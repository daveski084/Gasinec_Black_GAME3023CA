/***********************************************************************;
* Project            : Untiled
*
* Author             : David Gasinec, Harrison Black
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
using UnityEngine.SceneManagement;
using TMPro;

// 

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
    public GameObject saveData;
    public AudioClip enemyMove1;
    //public AudioClip a4SFX
    //public AudioClip a3SFX
    public AudioClip attackSFX;
    public AudioClip healSFX;
    public AudioSource audSrc;


    public GameObject playerPrefab;
    public GameObject enemyPrefab, enemyPrefab2, enemyPrefab3, enemyPrefab4, enemyPrefab5, essanceObj;
    public Transform playerBattleLocation;
    public Transform enemyBattleLocation; 

    public static BattleState currState;
    public static bool enteringBattle, hasBattled, hasBattledEnded;
    public static bool exitingBattle;
    public static bool ftWkyVideo, ftWkyQuiz, ftWkyExercises, ftMidterm, ftFinal;
   public static BattleObject playerBO;
    BattleObject enemyBO;
    public Text dialogueText;
    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public GameObject abilityOne, abilityTwo, abilityThree, abilityFour;
    public ParticleSystem playerParticle, enemyParticle, playerParticle2, enemyParticle2, essence, 
                            playerParticle3, enemyParticle3, playerParticle4, enemyParticle4;
    
    private int whichEnemy;
    private GameObject enemyGameObject;
    private bool isPlayerDead;
    private Animator playerAnim, enemyAnim;


    // Start is called before the first frame update
    void Start()
    {
        audSrc = GetComponent<AudioSource>();
        //whichEnemy = Random.Range(1, 5);
        whichEnemy = PlayerBehaviour.enemyGrass;
        HideActions();
        currState = BattleState.START;
        StartCoroutine(SetUpBattle());
    }


  
    // Set up as a coroutine 

    // 1.
    IEnumerator SetUpBattle()
    {
        exitingBattle = false; 
        //TODO: Switch statement to determine which battle object to load in.

        GameObject playerGameObject = Instantiate(playerPrefab, playerBattleLocation);
        playerAnim = playerGameObject.GetComponent<Animator>();
        playerBO = playerGameObject.GetComponent<BattleObject>();

        switch (whichEnemy)
        {
            case 1:
                enemyGameObject = Instantiate(enemyPrefab, enemyBattleLocation);
                ftWkyVideo = true;
                break;
            case 2:
                enemyGameObject = Instantiate(enemyPrefab2, enemyBattleLocation);//enemyPrefab2
                ftWkyQuiz = true;
                break;
            case 3:
                enemyGameObject = Instantiate(enemyPrefab3, enemyBattleLocation);//enemyPrefab3
                ftWkyExercises = true; 
                break;
            case 4:
                enemyGameObject = Instantiate(enemyPrefab4, enemyBattleLocation);//enemyPrefab4
                ftFinal = true; 
                break;
            //case 5:
            //    enemyGameObject = Instantiate(enemyPrefab5, enemyBattleLocation);//enemyPrefab5
               // break;
        }
        enemyAnim = enemyGameObject.GetComponent<Animator>();
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
        hasBattled = true;
        exitingBattle = true;
        //TODO: User gains a new ability or is loaded into a previous state. 
        if (currState == BattleState.WON)
        {
            dialogueText.text = "You Won!";
            enemyAnim.Play("DeathAnim");

            if(ftWkyVideo == true)
            {
                PlayerBehaviour.hasAbilityTwo = true; 
            }

            if(ftWkyQuiz == true)
            {
                PlayerBehaviour.hasAbilityThree = true; 
            }

            if(ftWkyExercises == true)
            {
                PlayerBehaviour.hasAbilityFour = true; 
            }
            if(ftFinal == true)
            {
                SceneManager.LoadScene("EndScene"); 
            }


            StartCoroutine(GetAbility());
            
        }
        else if (currState == BattleState.LOST)
        {
            dialogueText.text = "You have lost.";
            playerAnim.Play("DeathAnim");
            StartCoroutine(Respawn());

        }
    }
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2f);
        exitingBattle = true;
        GameObject.Find("Blocker").GetComponent<Animator>().SetTrigger("Start");
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("OverWorldScene");
    }
    IEnumerator GetAbility()
    {
        essanceObj.SetActive(true);
        yield return new WaitForSeconds(1f);
        essence.Play();
        yield return new WaitForSeconds(1.9f);
        dialogueText.text = enemyBO.objName + "'s ability is now yours!";
        essanceObj.SetActive(false);
        //add ability bool to player prefs **UPDATE BUTTONS AT BOTTOM OF SCRIPT** and button text
        yield return new WaitForSeconds(2f);
        GameObject.Find("Blocker").GetComponent<Animator>().SetTrigger("Start");
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("OverWorldScene");

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
        enemyAnim.Play("DamagedAnim");
        enemyHUD.UpdateHP(enemyBO.currHP);
        dialogueText.text = "The attack landed!";
        audSrc.PlayOneShot(attackSFX); 

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
        audSrc.PlayOneShot(healSFX);


        yield return new WaitForSeconds(2.0f);
        currState = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        switch (whichEnemy)
        {
            case 1: //weekly Videos
                if (enemyBO.currHP < (enemyBO.maxHP / 2)) // Enemy heals if there is less than half of health. 
                {
                    dialogueText.text = enemyBO.objName + " is looking hurt...";
                    yield return new WaitForSeconds(2.0f);
                    enemyParticle.Play();
                    dialogueText.text = enemyBO.objName + " has healed a little bit.";
                    enemyBO.Heal(enemyBO.healAmount);
                    enemyHUD.UpdateHP(enemyBO.currHP);
                }
                else
                {
                    dialogueText.text = enemyBO.objName + " prepares.";
                    yield return new WaitForSeconds(2.0f);
                    playerAnim.Play("DamagedAnim");
                    dialogueText.text = enemyBO.objName + "Attacks!";
                    isPlayerDead = playerBO.TakeDamage(enemyBO.damage);
                    playerHUD.UpdateHP(playerBO.currHP);
                    audSrc.PlayOneShot(enemyMove1);
                }
                break;
            case 2://WeeklyQuizes
                if (enemyBO.currHP < (enemyBO.maxHP * 0.5)) 
                {
                    if(Random.Range(1, 101) < 75)
                    {
                        dialogueText.text = enemyBO.objName + " looks scared...";
                        yield return new WaitForSeconds(2.0f);
                        if (enemyBO.Escape())
                        {
                            dialogueText.text = enemyBO.objName + " ran away safely";
                            enemyAnim.Play("EnemyEscAnim");
                        }
                        else
                        {
                            dialogueText.text = enemyBO.objName + " tried to run away, but was frozen in fear";
                        }
                    }
                    else
                    {
                        dialogueText.text = enemyBO.objName + " is looking hurt...";
                        yield return new WaitForSeconds(2.0f);
                        enemyParticle.Play();
                        dialogueText.text = enemyBO.objName + " has healed a little bit.";
                        enemyBO.Heal(enemyBO.healAmount);
                        enemyHUD.UpdateHP(enemyBO.currHP);
                    }
                }
                else
                {
                    dialogueText.text = enemyBO.objName + " prepares.";
                    yield return new WaitForSeconds(2.0f);
                    playerAnim.Play("DamagedAnim");
                    dialogueText.text = enemyBO.objName + "Attacks!";
                    isPlayerDead = playerBO.TakeDamage(enemyBO.damage);
                    playerHUD.UpdateHP(playerBO.currHP);
                    audSrc.PlayOneShot(enemyMove1);
                }
                break;
            case 3://WeeklyExercises
                if (enemyBO.currHP < (enemyBO.maxHP * 0.25))
                {
                    dialogueText.text = enemyBO.objName + " looks scared...";
                    yield return new WaitForSeconds(2.0f);
                    if (enemyBO.Escape())
                    {
                        dialogueText.text = enemyBO.objName + " ran away safely";
                        enemyAnim.Play("EnemyEscAnim");
                    }
                    else
                    {
                        dialogueText.text = enemyBO.objName + " tried to run away, but was frozen in fear";
                    }
                }
                else
                {
                    if (Random.Range(1, 101) < 50)
                    {
                        dialogueText.text = enemyBO.objName + " seems to be thinking hard.";
                        yield return new WaitForSeconds(2.0f);
                        enemyBO.ProblemSolve(enemyBO);
                        enemyParticle2.Play();
                        dialogueText.text = enemyBO.objName + " has found your weak spot";
                    }
                    else
                    {
                        dialogueText.text = enemyBO.objName + " prepares.";
                        yield return new WaitForSeconds(2.0f);
                        playerAnim.Play("DamagedAnim");
                        dialogueText.text = enemyBO.objName + "Attacks!";
                        isPlayerDead = playerBO.TakeDamage(enemyBO.damage);
                        playerHUD.UpdateHP(playerBO.currHP);
                        audSrc.PlayOneShot(enemyMove1);
                    }
                }
                break;
            case 4://Midterm
                dialogueText.text = enemyBO.objName + " seems to be thinking hard.";
                yield return new WaitForSeconds(2.0f);
                enemyBO.ProblemSolve(enemyBO);
                enemyParticle2.Play();
                dialogueText.text = enemyBO.objName + " has found your weak spot";
                if (Random.Range(1, 101) < 50)
                {
                    dialogueText.text = enemyBO.objName + " prepares.";
                    yield return new WaitForSeconds(2.0f);
                    playerAnim.Play("DamagedAnim");
                    dialogueText.text = enemyBO.objName + "Attacks!";
                    isPlayerDead = playerBO.TakeDamage(enemyBO.damage);
                    playerHUD.UpdateHP(playerBO.currHP);
                    audSrc.PlayOneShot(enemyMove1);
                }
                else
                {
                    dialogueText.text = enemyBO.objName + " seems to be ascending!";
                    yield return new WaitForSeconds(2.0f);
                    if (enemyBO.Struggle())
                    {
                        enemyAnim.Play("AcsendAnim");
                        yield return new WaitForSeconds(1.0f);
                        dialogueText.text = enemyBO.objName + " smites thou mortal!";
                        playerParticle3.Play();
                        playerAnim.Play("DamagedAnim");
                        currState = BattleState.LOST;
                        EndBattle();
                    }
                    else
                    {
                        dialogueText.text = enemyBO.objName + " no, wait... just gas.";
                        enemyParticle4.Play();
                    }
                }
                    break;
            case 5://FinalExam
                //enemyPrefab5
                break;
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

    public void OnStrugglePressed()
    {
        // struggle
        // StartCoroutine(Struggle());
    }

    public void OnUnityExperiencePressed()
    {
        // You used your unity experiance and defaeted the culminating assignment!
       // StartCoroutine(UnityExp());
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

        if(PlayerBehaviour.hasAbilityTwo)
            abilityTwo.SetActive(true);
        if (PlayerBehaviour.hasAbilityThree)
           abilityThree.SetActive(true);
        if (PlayerBehaviour.hasAbilityFour) 
           abilityFour.SetActive(true);
    }

}
