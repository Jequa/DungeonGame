using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10;
    private Vector3 originalScale;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FireGun();
        UpdateGunSize();
    }

    //change gun scale so it stays the same size when crouched
    
    void FireGun()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;
        }
    }

    void UpdateGunSize()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            transform.localScale = new Vector3(2, 3.5f, 2);
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            transform.localScale = new Vector3(2, 2, 2);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            transform.localScale = new Vector3(2, 2, 2);
        }
    }
}
