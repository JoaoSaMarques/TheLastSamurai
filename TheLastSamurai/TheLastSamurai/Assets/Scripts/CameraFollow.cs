using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform  target;

    [SerializeField] private float      maxSpeed = 1.0f;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            Player player = FindObjectOfType<Player>();
            if (player != null)
            {
                target = player.transform;
            }
            else
            {
                return;
            }
        }

        Vector3 targetPosition = target.position;
        targetPosition.z = transform.position.z;

        Vector3 error = targetPosition - transform.position;

        //transform.position = Vector3.MoveTowards(transform.position, targetPosition, maxSpeed * Time.deltaTime);
        if (Time.deltaTime > 0)
        {
            float factor = Mathf.Clamp01(Time.fixedDeltaTime / maxSpeed);
            transform.position = transform.position + error * factor;
        }
    }
}
