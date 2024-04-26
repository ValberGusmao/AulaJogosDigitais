using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    public Text scoreText;

    private int score = 0;

    void Awake(){
        instance = this;
    }

    void Start()
    {
        scoreText.text = scoreText.text + score.ToString();
    }

    public void UpdateScore(int valor)
    {
        scoreText.text = scoreText.text.Replace(score.ToString(), valor.ToString());
        score = valor;
    }
}
