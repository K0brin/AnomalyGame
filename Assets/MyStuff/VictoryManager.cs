using System;
using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    [SerializeField] private GameObject VictoryScreen;
    [SerializeField] private GameObject LossScreen;

    SAnomalySpawner sAnomalySpawner;

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

        sAnomalySpawner = GameObject.FindGameObjectWithTag("AnomalyManager").GetComponent<SAnomalySpawner>();

        VictoryScreen.SetActive(false);
        LossScreen.SetActive(false);
    }

    void Update()
    {
        if(HaveWon == true)
        {
            RunVictory(); //Handles winning the game
        }

        if(sAnomalySpawner.gameOver == true)
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
