using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeathDisplay : MonoBehaviour
{
    [SerializeField] private Sprite[] healthSprites;
    [SerializeField] private Image healthImage;
    [SerializeField] private Player player;

    // Update is called once per frame
    void Update()
    {
        int health = player.GetHealth();
        int spriteIndex;

        if (player.IsArmor()) // Check if armor is active
        {
            spriteIndex = health / 40; // Divide health by 40 when armor is active
        }
        else
        {
            spriteIndex = health / 20; // Divide health by 20 when armor is not active
        }

        // Ensure the sprite index is within the bounds of the array
        if (spriteIndex >= 0 && spriteIndex < healthSprites.Length)
        {
            healthImage.sprite = healthSprites[spriteIndex];
        }
    }
}