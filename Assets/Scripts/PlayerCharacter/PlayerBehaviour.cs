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
    public static int ability1, ability2, ability3, ability4;
    public static int enemyGrass;

    public float playerSpeed = 1.0f;
    public Animator playerAnimator;
    public Animator transitionAnim;

    public int chance;
    public float transitionSpeed;
    bool isGamePaused;


    public float playerX, playerY, playerZ;
    public int playerHealth;
    public GameObject playerCharacter;
    public AudioSource audioSrc;


    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
        if (BattleSystem.exitingBattle)
        {
            LoadLocation();
        }
        else
            LoadLocation(); 
        
       // LoadAbilities(); 
    }
   
    private void Start()
    {
        PauseMenu.SetActive(false);
        ability1 = 1;
        ability2 = 2;
        ability3 = 3;
        ability4 = 4;
        audioSrc = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Varibles to hold our movement data.
        float playerInputX = Input.GetAxisRaw("Horizontal");
        float playerInputY = Input.GetAxisRaw("Vertical");

        playerAnimator.SetFloat("Horizontal", playerInputX);
        playerAnimator.SetFloat("Vertical", playerInputY);


        if (BattleSystem.enteringBattle)
        {
            SaveLocation();
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
            print("in tall grass");
            RollForEncounter();
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

        //TODO: Save abilities. 


        PlayerPrefs.Save();
        print("Game Saved.");
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