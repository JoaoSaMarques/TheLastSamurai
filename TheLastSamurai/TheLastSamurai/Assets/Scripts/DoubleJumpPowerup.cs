using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class DoubleJumpPowerup : Pickup
{
    protected override bool PickupObject(Player player)
    {
        player.SetMaxJumps(2);
        return true;
    }
}
