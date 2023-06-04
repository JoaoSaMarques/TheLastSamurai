using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float fireRate;
    private float nextFire;
    private bool canShoot = true;

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
            nextFire = Time.time + fireRate;
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }

        // Enable shooting again after the animation has finished
        StartCoroutine(EnableShootingAfterAnimation());
    }

    IEnumerator EnableShootingAfterAnimation()
    {
        // Assuming the "PlayerAttack1" animation has a duration of 1 second
        yield return new WaitForSeconds(0.8f);
        canShoot = true;
    }
}