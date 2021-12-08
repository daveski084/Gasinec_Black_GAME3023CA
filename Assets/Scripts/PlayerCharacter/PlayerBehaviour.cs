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

public class PlayerBehaviour : MonoBehaviour
{
    public static bool isMoving;

    public float playerSpeed = 1.0f;
    public Animator playerAnimator;

  

    //void Start()
    //{
       
    //}

    // Update is called once per frame
    void Update()
    {
        // Varibles to hold our movement data.
        float playerInputX = Input.GetAxisRaw("Horizontal");
        float playerInputY = Input.GetAxisRaw("Vertical");
        Vector2 direction = Vector2.zero;

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

        //if (playerInputX == 0 && playerInputY == 0) // Idle
        //{
        //    isMoving = false;
        //    playerAnimator.SetInteger("playerAnimationState", 0);
        //}
        //else if (playerInputX > 0)  //right
        //{
        //    isMoving = true;
        //    direction.x = 1;
        //    playerAnimator.SetInteger("playerAnimationState", 3);

        //}
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
}
