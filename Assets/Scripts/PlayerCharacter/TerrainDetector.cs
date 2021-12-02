/***********************************************************************;
* Project            : Untiled
*
* Author             : David Gasinec, 
* 
* Student Number     : 101187910
*
* Date created       : 21/11/2
*
* Description        : Controls player movement and player charater animation states.
*
* Last modified      : 21/11/30
*
* Revision History   :
*
*Date        Author Ref    Revision (Date in YYYY/MM/DD format) 
*21/11/30    David Gasinec        Created script. 
*
*
|**********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainDetector : MonoBehaviour
{

    //Audio
    Rigidbody2D rb;
    AudioSource audioSrc;


    bool hasStepPlayed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSrc = GetComponent<AudioSource>();
        hasStepPlayed = false;
    }


    // Player walks onto grass play steps.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && PlayerBehaviour.isMoving == true)
        {
            audioSrc.Play();
            print("WalkingOn grass");
            hasStepPlayed = true;
        }

    }

    //Player is currently standing on the grass, Play or stop the sound based on isMoving.
    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (collision.tag == "Player" && PlayerBehaviour.isMoving == false && hasStepPlayed)
        {
            audioSrc.Stop();
            Debug.Log("Stop walking");
            hasStepPlayed = false;
        }
        else if (collision.tag == "Player" && PlayerBehaviour.isMoving == true && !hasStepPlayed)
        {
            audioSrc.Play();
            hasStepPlayed = true; 
        }

    }

    //Player moves off the grass, stop playing the grass sounds. 
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            audioSrc.Stop();
            hasStepPlayed = false;
        }
    }


}
