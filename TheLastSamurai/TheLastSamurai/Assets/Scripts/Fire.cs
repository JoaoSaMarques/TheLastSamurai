using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float firerate;
    float nextFire;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Invoke("Shoot", 1.2f);

        }
    }

    void Shoot ()
    {
        if (Time.time > nextFire) 
        {
            
            nextFire = Time.time + firerate;
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
        
    }    
}
