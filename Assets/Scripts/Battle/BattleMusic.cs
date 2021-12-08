using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMusic : MonoBehaviour
{

    AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
       audioSrc = GetComponent<AudioSource>();
        audioSrc.Play(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(BattleSystem.currState == BattleState.LOST || BattleSystem.currState == BattleState.WON)
        {
            audioSrc.Stop();
        }
       
    }
}
