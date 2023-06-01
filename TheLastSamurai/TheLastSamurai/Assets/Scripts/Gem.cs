using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : Pickup
{
    [SerializeField] private int score = 1;

    override protected bool PickupObject(Player player)
    {
        ScoreMng scoreMng = FindObjectOfType<ScoreMng>();
        scoreMng.AddScore(score);

        return true;
    }
}
