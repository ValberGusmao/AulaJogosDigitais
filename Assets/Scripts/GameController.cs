using UnityEngine;

public class GameController : MonoBehaviour
{
    private int score = 0;
    public static GameController instance;

    void Awake()
    {
        instance = this;
    }

    public void AddPoints(int quant)
    {
        score += quant;
        UIController.instance.UpdateScore(score);
    }
}