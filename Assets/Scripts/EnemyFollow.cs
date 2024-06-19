using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 2f;
    public float detectionRange = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        if (target == null)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, target.position);
        if (distanceToPlayer <= detectionRange)
        {
            Vector3 direction = target.position - transform.position;
            direction.Normalize();
            transform.position += direction * moveSpeed * Time.deltaTime; transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
