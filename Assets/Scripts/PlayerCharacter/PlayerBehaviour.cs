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
    public static bool isMoving;

    public float playerSpeed = 1.0f;
    public Animator playerAnimator;
    public Animator transitionAnim;

    public int chance;
    public float transitionSpeed;



    //void Start()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        // Varibles to hold our movement data.
        float playerInputX = Input.GetAxisRaw("Horizontal");
        float playerInputY = Input.GetAxisRaw("Vertical");
        //Vector2 direction = Vector2.zero;
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
        //else if (playerInputX < 0)  //left
        //{
        //    isMoving = true;
        //    direction.x = -1;
        //    playerAnimator.SetInteger("playerAnimationState", 4);
        //}
        //else if (playerInputY > 0) // up
        //{
        //    isMoving = true;
        //    direction.y = 1;
        //    playerAnimator.SetInteger("playerAnimationState", 2);
        //}
        //else if (playerInputY < 0) //down 
        //{
        //    isMoving = true;
        //    direction.y = -1;
        //    playerAnimator.SetInteger("playerAnimationState", 1);
        //}

        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
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
        yield return new WaitForSeconds(transitionSpeed);
        SceneManager.LoadScene("BattleScene");
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "TallGrass")
        {
            RollForEncounter();
        }
    }
}
