using System;
using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    [SerializeField] private GameObject VictoryScreen;
    [SerializeField] private GameObject LossScreen;

    public bool HaveWon = false; 
    public bool HaveLost = false;
    void Start()
    {
        HaveWon = false;
        HaveLost = false;

        VictoryScreen.SetActive(false);
        LossScreen.SetActive(false);
    }

    void Update()
    {
        if(HaveWon == true)
        {
            RunVictory(); //Handles winning the game
        }

        if(HaveLost == true)
        {
            RunLoss(); //Handles when the player loses
        }
    }

    private void RunLoss()
    {
        Time.timeScale = 0f;
        LossScreen.SetActive(true);
    }

    private void RunVictory()
    {
        Time.timeScale = 0f;
        VictoryScreen.SetActive(true);
    }
}
