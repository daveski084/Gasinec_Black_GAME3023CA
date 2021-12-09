using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMusic : MonoBehaviour
{
    AudioSource audioSrc;
    private float fadeVolume;

    // Start is called before the first frame update
    void Start()
    {
       audioSrc = GetComponent<AudioSource>();
       audioSrc.Play();
       fadeVolume = audioSrc.volume;
    }

    // Update is called once per frame
    void Update()
    {
        if(BattleSystem.currState == BattleState.LOST || BattleSystem.currState == BattleState.WON)
        {
            //StartCoroutine(StopMusic());

            if (fadeVolume <= 0)
                audioSrc.Stop();
            else
            {
                audioSrc.volume = fadeVolume;
                fadeVolume -= 0.1f * Time.deltaTime;
            }
        }      
    }

    //IEnumerator StopMusic()
    //{
    //    yield return new WaitForSeconds(2f);
    //    audioSrc.Stop();
    //}
}
