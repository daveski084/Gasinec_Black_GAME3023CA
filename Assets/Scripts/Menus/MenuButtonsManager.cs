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
    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    //void Update()
    //{
        
    //}

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
        SceneManager.LoadScene("OverWorldScene");
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }
}
