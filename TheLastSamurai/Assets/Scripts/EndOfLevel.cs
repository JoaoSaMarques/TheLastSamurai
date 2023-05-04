using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfLevel : Pickup
{
    public string nextSceneName;

    protected override bool PickupObject(Player player)
    {
        SceneManager.LoadScene(nextSceneName);

        return false;
    }
}
