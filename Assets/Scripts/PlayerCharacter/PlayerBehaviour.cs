/***********************************************************************;
* Project            : Untiled
*
* Author             : David Gasinec, Harrison Black
* 
* Student Number     : 101187910, 
*
* Date created       : 21/11/2
*
* Description        : Controls player movement and player charater animation states.
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
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour
{
    public GameObject saveData;
    public GameObject PauseMenu;
    public static bool isMoving;
    public static bool hasBeatWeeklyEx, hasBeatWeeklyVid, hasBeatMidterm, hasBeatweeklyQuiz,
                        hasBeatFinal;
    public static bool  hasAbilityTwo, hasAbilityThree, hasAbilityFour;
    public static int enemyGrass;

    public float playerSpeed = 1.0f;
    public Animator playerAnimator;
    public Animator transitionAnim;

    public int chance;
    public float transitionSpeed;
    bool isGamePaused;

    public int abilityTwo, abilityThree, abilityFour;
    public float playerX, playerY, playerZ;
    public int playerHealth;
    public GameObject playerCharacter;
    public AudioSource audioSrc;


    private void Awake()
    {
        PlayerPrefs.DeleteAll();

        if (BattleSystem.exitingBattle)
        {
            LoadLocation();
            LoadAbilities();
        }
        else
            
            LoadLocation();
            LoadAbilities(); 

        // LoadAbilities(); 
    }
   
    private void Start()
    {
        PauseMenu.SetActive(false);
        audioSrc = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Varibles to hold our movement data.
        float playerInputX = Input.GetAxisRaw("Horizontal");
        float playerInputY = Input.GetAxisRaw("Vertical");

        playerAnimator.SetFloat("Horizontal", playerInputX);
        playerAnimator.SetFloat("Vertical", playerInputY);


        //if (BattleSystem.enteringBattle)
        //{
        //    SaveLocation();
        //}

        if(BattleSystem.exitingBattle == true)
        {
            SaveAbilities();
        }

        if (playerInputX == 0 && playerInputY == 0) // Idle
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }

        if(Input.GetKeyDown("space"))
        {
            playerSpeed = 5.5f;
        }
        else if (Input.GetKeyUp("space"))
        {
            playerSpeed = 3.5f;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
           if(isGamePaused)
           {
               ResumeGame();
           }
           else
           {
               PauseGame();
           }
            //Application.Quit();
        }

        


        // Move the player.
        transform.Translate(new Vector3(playerInputX, playerInputY, 0) * playerSpeed * Time.deltaTime, Space.World);
    }

    private void RollForEncounter()
    {
        Debug.Log("touching");
        if (Random.Range(1, 100) < chance)
        {
            SaveLocation();
            StartCoroutine(LoadBattle());
            Debug.Log("do battle");
        }
    }

    IEnumerator LoadBattle()
    {
        transitionAnim.SetTrigger("Start");
        //PlayerPrefs.SetFloat("preBattleX", transform.position.x);
        //PlayerPrefs.SetFloat("preBattleY", transform.position.y);
        //PlayerPrefs.SetFloat("preBattleZ", transform.position.z);
        yield return new WaitForSeconds(transitionSpeed);
        SceneManager.LoadScene("BattleScene");
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "TallGrass" && isMoving)
        {
           // RollForEncounter(); 
        }
        if(col.gameObject.tag == "Enemy1Grass" && isMoving)
        {
            if(!hasAbilityTwo)
            {
                RollForEncounter();
                enemyGrass = 1;
            }
            else if (hasAbilityTwo)
            {
                print("this spot is done");
            }
             
        }
        if(col.gameObject.tag =="Enemy2Grass" && isMoving)
        {
            if(!hasAbilityThree)
            {
                RollForEncounter();
                enemyGrass = 2;
            }
            else if (hasAbilityThree)
            {
                print("this spot is done"); 
            }
        }
        if(col.gameObject.tag == "Enemy3Grass" && isMoving)
        {
            if(!hasAbilityFour)
            {
                RollForEncounter();
                enemyGrass = 3;
            }
            else if (hasAbilityFour)
            {
                print("this spot is done"); 
            }
        }
        if (col.gameObject.tag == "Enemy4Grass" && isMoving)
        {
            if(hasAbilityTwo && hasAbilityThree && hasAbilityFour)
            {
                RollForEncounter();
                enemyGrass = 4; 
            }
            else
            {
                print("not yet!"); 
            }
        }

    }

    void ResumeGame()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
        isGamePaused = false;
    }

    void PauseGame()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0;
        isGamePaused = true;
    }

    public void SaveLocation()
    {
        audioSrc.Play();

        playerX = playerCharacter.transform.position.x;
        playerY = playerCharacter.transform.position.y;
        playerZ = playerCharacter.transform.position.z;

        if(BattleSystem.hasBattled == true)
        {
            playerHealth = BattleSystem.playerBO.currHP;
        }


        PlayerPrefs.SetFloat("Xposition", playerX);
        PlayerPrefs.SetFloat("Yposition", playerY);
        PlayerPrefs.SetFloat("Zposition", playerZ);
        PlayerPrefs.SetInt("PlayerHealth", playerHealth);



        PlayerPrefs.Save();
        print("Game Saved.");
    }


    public void SaveAbilities()
    {
        print("Saving abilities"); 
        if (hasAbilityTwo)
        {
            abilityTwo = 1;
        }
        if (hasAbilityThree)
        {
            abilityThree = 1;
        }
        if(hasAbilityFour)
        {
            abilityFour = 1; 
        }

        //TODO: Save abilities. 
        PlayerPrefs.SetInt("AbilityTwo", abilityTwo);
        PlayerPrefs.SetInt("AbilityThree", abilityThree);
        PlayerPrefs.SetInt("AbilityFour", abilityFour);
    }

    public void LoadAbilities()
    {
       
        abilityTwo = PlayerPrefs.GetInt("AbilityTwo");
        abilityThree = PlayerPrefs.GetInt("AbilityThree");
        abilityFour = PlayerPrefs.GetInt("AbilityFour");

        if (abilityTwo == 1)
        {
            hasAbilityTwo = true; 
        }
        if(abilityThree == 1)
        {
            hasAbilityThree = true;
        }
        if(abilityFour == 1)
        {
            hasAbilityFour = true; 
        }
        
    }

    public void LoadLocation()
    {
        playerX = PlayerPrefs.GetFloat("Xposition");
        playerY = PlayerPrefs.GetFloat("Yposition");
        playerZ = PlayerPrefs.GetFloat("Zposition");
        playerHealth = PlayerPrefs.GetInt("PlayerHealth");

       
        Vector3 loadPlacement = new Vector3(playerX, playerY, playerZ);
        playerCharacter.transform.position = loadPlacement;

        if (BattleSystem.hasBattled == true)
        BattleSystem.playerBO.currHP = playerHealth;

       
    }


}