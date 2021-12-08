using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGame : MonoBehaviour
{
    public float playerX, playerY, playerZ;
    public GameObject playerCharacter;
    public AudioSource audioSrc;

    private void Awake()
    {
        LoadLocation(); 
    }

    private void Start()
    {
        audioSrc = GetComponent<AudioSource>(); 
    }

    public void SaveLocation()
    {
        audioSrc.Play(); 

        playerX = playerCharacter.transform.position.x;
        playerY = playerCharacter.transform.position.y;
        playerZ = playerCharacter.transform.position.z;

        PlayerPrefs.SetFloat("Xposition", playerX);
        PlayerPrefs.SetFloat("Yposition", playerY);
        PlayerPrefs.SetFloat("Zposition", playerZ); 

        PlayerPrefs.Save();
        print("Game Saved.");
    }

    public void LoadLocation()
    {
        playerX = PlayerPrefs.GetFloat("Xposition");
        playerY = PlayerPrefs.GetFloat("Yposition");
        playerZ = PlayerPrefs.GetFloat("Zposition");

        Vector3 loadPlacement = new Vector3(playerX, playerY, playerZ);
        playerCharacter.transform.position = loadPlacement;
    }

}
