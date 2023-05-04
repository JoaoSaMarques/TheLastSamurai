using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            if (PickupObject(player))
            {
                Destroy(gameObject);
            }
        }
    }

    abstract protected bool PickupObject(Player player);
}
