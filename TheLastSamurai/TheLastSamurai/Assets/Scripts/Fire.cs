using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject armoredBulletPrefab; // New armored bullet prefab
    public float fireRate;
    public float armoredFireRate; // New armored fire rate
    private float nextFire;
    private bool canShoot = true;

    private Player player; // Reference to the Player script

    private void Awake()
    {
        player = GetComponent<Player>(); // Get the Player component
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && canShoot)
        {
            canShoot = false;
            Invoke("Shoot", 0.3f);
        }
    }

    void Shoot()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + GetFireRate(); // Use the appropriate fire rate
            GameObject bulletPrefabToInstantiate = GetBulletPrefab(); // Use the appropriate bullet prefab
            Instantiate(bulletPrefabToInstantiate, firePoint.position, firePoint.rotation);
        }

        // Enable shooting again after the animation has finished
        StartCoroutine(EnableShootingAfterAnimation());
    }

    float GetFireRate()
    {
        if (player.IsArmor())
        {
            return armoredFireRate; // Return the armored fire rate if the player has armor
        }
        else
        {
            return fireRate; // Return the regular fire rate if the player doesn't have armor
        }
    }

    GameObject GetBulletPrefab()
    {
        if (player.IsArmor())
        {
            return armoredBulletPrefab; // Return the armored bullet prefab if the player has armor
        }
        else
        {
            return bulletPrefab; // Return the regular bullet prefab if the player doesn't have armor
        }
    }

    IEnumerator EnableShootingAfterAnimation()
    {
        float waitDuration = GetWaitDuration(); // Get the appropriate wait duration based on armor status

        yield return new WaitForSeconds(waitDuration);
        canShoot = true;
    }

    float GetWaitDuration()
    {
        if (player.IsArmor())
        {
            return 0.1f; // Return the longer wait duration if the player has armor
        }
        else
        {
            return 0.8f; // Return the regular wait duration if the player doesn't have armor
        }
    }
}
