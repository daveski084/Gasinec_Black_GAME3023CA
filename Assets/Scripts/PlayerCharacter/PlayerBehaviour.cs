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
    public GameObject PauseMenu;
    public static bool isMoving;
    public static int ability1, ability2, ability3, ability4;

    public float playerSpeed = 1.0f;
    public Animator playerAnimator;
    public Animator transitionAnim;

    public int chance;
    public float transitionSpeed;


    private void Start()
    {
        PauseMenu.SetActive(false);
        ability1 = 1;
        ability2 = 2;
        ability3 = 3;
        ability4 = 4; 
    }

    void Update()
    {
        // Varibles to hold our movement data.
        float playerInputX = Input.GetAxisRaw("Horizontal");
        float playerInputY = Input.GetAxisRaw("Vertical");

        playerAnimator.SetFloat("Horizontal", playerInputX);
        playerAnimator.SetFloat("Vertical", playerInputY);


        if (playerInputX == 0 && playerInputY == 0) // Idle
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }

        if (Input.GetKeyDown("escape"))
        {
            Time.timeScale = 0;
            PauseMenu.SetActive(true);
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
            RollForEncounter();
        }
    }
}