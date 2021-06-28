using UnityEngine.UI;
using UnityEngine;
using System;

public class DisplayHighScore : MonoBehaviour
{
    public Text highScore;

    void Update()
    {
        highScore.text = FindObjectOfType<Collision>().highCoinScores[Convert.ToInt32(FindObjectOfType<Collision>().newStageNr)].ToString();
    }
}
