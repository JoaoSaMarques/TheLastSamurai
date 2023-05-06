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
        int spriteIndex = health / 20; // Divide health by 20 to get the corresponding sprite index

        // Ensure the sprite index is within the bounds of the array
        if (spriteIndex >= 0 && spriteIndex < healthSprites.Length)
        {
            healthImage.sprite = healthSprites[spriteIndex];
        }
    }
}
