using System;
using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    [SerializeField] private GameObject VictoryScreen;
    [SerializeField] private GameObject LossScreen;
    [SerializeField] private SAnomalySpawner AnomalySpawner;

    // SAnomalySpawner sAnomalySpawner;

    public bool HaveWon = false; 
    public bool HaveLost = false;

    void Awake()
    {
        Time.timeScale = 1f;
    }
    void Start()
    {
        HaveWon = false;
        HaveLost = false;

        //sAnomalySpawner = GameObject.FindGameObjectWithTag("AnomalyManager").GetComponent<SAnomalySpawner>(); //JB reload issues with tags

        VictoryScreen.SetActive(false);
        LossScreen.SetActive(false);
    }

    void Update()
    {
        if(HaveWon == true)
        {
            RunVictory(); //Handles winning the game
        }

        if(AnomalySpawner.gameOver == true)
        {
            RunLoss(); //Handles when the player loses
        }
    }

    private void RunLoss()
    {
        Time.timeScale = 0f;
        SPauseMenu.GameIsPaused = true;
        LossScreen.SetActive(true);
    }

    private void RunVictory()
    {
        Time.timeScale = 0f;
        SPauseMenu.GameIsPaused = true;
        VictoryScreen.SetActive(true);
    }
}
