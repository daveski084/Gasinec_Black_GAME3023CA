/***********************************************************************;
* Project            : Untiled
*
* Author             : David Gasinec
* 
* Student Number     : 101187910
*
* Date created       : 21/10/06
*
* Description        : Controls button behaviour and scene managment.
*
* Last modified      : 21/11/01
*
* Revision History   :
*
*Date        Author Ref    Revision (Date in YYYYMMDD format) 
*21/10/06    David Gasinec        Created script. 
*
*
|**********************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class MenuButtonsManager : MonoBehaviour
{
    private int scene;

    public void OnStartButtonPressed()
    {
        Debug.Log("Start Button Pressed!");
        SceneManager.LoadScene("MainMenu");
    }

    // Main menu button functionality. 
    public void OnLoadButtonPressed()
    {
        //TODO: Will load the player's save file. 

    }

    public void OnOptionsButtonPressed()
    {
        Debug.Log("Options button pressed");
        SceneManager.LoadScene("Options");
    }

    public void OnStartGameButtonPressed()
    {
        scene = 2;
        StartCoroutine(Loader());
    }


    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }
    public void EscapeGame()
    {
        scene = 1;
        StartCoroutine(Loader());
    }
    IEnumerator Loader()
    {
        switch (scene)
        {
            case 1:
                Time.timeScale = 1;
                GameObject.Find("Blocker").GetComponent<Animator>().SetTrigger("Start");
                yield return new WaitForSeconds(1.0f);
                SceneManager.LoadScene("MainMenu");
                break;
            case 2:
                GameObject.Find("Blocker").GetComponent<Animator>().SetTrigger("Start");
                yield return new WaitForSeconds(1.0f);
                SceneManager.LoadScene("OverWorldScene");
                break;
        }
    }
}
