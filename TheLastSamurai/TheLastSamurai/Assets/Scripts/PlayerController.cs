using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float maxChargeTime = 2f; // Maximum charge time in seconds
    [SerializeField] private float chargeRate = 1f; // Rate at which the jump charge increases per second
    private float currentChargeTime = 0f; // Current charge time
    private bool isCharging = false; // Flag to indica

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isCharging = true;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            Jump();
        }

        if (isCharging)
        {
            currentChargeTime += Time.deltaTime * chargeRate;
            currentChargeTime = Mathf.Clamp(currentChargeTime, 0f, maxChargeTime);
        }
    }

    private void Jump()
    {
        // Calculate the jump strength based on charge time
        float jumpStrength = currentChargeTime;

        // Perform the jump action using the jumpStrength value
        // For example, you might apply a force to a Rigidbody2D component
        // attached to the player character.
        // Replace 'rigidbody2D' with your actual Rigidbody2D reference.
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpStrength), ForceMode2D.Impulse);

        // Reset the charge time and charging flag
        currentChargeTime = 0f;
        isCharging = false;
    }


}
