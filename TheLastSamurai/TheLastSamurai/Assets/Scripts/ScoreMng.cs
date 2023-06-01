using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreMng : MonoBehaviour
{
    private int score;

    static private ScoreMng instance;

    private void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        score = 0;
        DontDestroyOnLoad(gameObject);
    }

    public void AddScore(int delta)
    {
        score = score + delta;
    }

    public int GetScore()
    {
        return score;
    }
}
