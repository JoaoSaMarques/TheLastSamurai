using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    private ScoreMng        scoreMng;

    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        scoreMng = FindObjectOfType<ScoreMng>();
    }

    void Update()
    {
        // Get score
        int score = scoreMng.GetScore();
        // Set text
        scoreText.text = $"Score: {score}";
    }
}
