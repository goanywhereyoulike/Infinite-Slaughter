using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Gun : MonoBehaviour
{
  
    public Transform muzzleTransform;
    private float nextTimeToFire = 0.5f;
    public float firerate = 10.0f;
    private float dividedFireRate = 0.0f;
    public float damage = 10.0f;
    public float range = 100.0f;
    //public float ImpactForce = 1000.0f;
    public Camera fpsCamera;

    private LineRenderer laser;
    public VisualEffect muzzleFlash;
    public GameObject muzzleExplosion;
    //public GameObject impacteffect;
    // Update is called once per frame

    private void Start()
    {
        muzzleFlash.Stop();

        dividedFireRate = 1 / firerate;
        laser = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        Fire();
    }


    private void Fire()
    {
        if (Input.GetButton("Fire1") && Time.time > nextTimeToFire)
        {

            nextTimeToFire = Time.time + dividedFireRate;
            muzzleFlash.Play();
            PlayerShoot();

        }


    }

    public void PlayerShoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            if (!laser.enabled)
            {
                laser.enabled = true;
            }
            laser.SetPosition(0, muzzleTransform.transform.position);
            Vector3 endposition = hit.point;
        
            laser.SetPosition(1, endposition);

            IDamageable target = hit.transform.GetComponentInParent<IDamageable>();
            DestructibleObject enemy = hit.transform.GetComponentInParent<DestructibleObject>();
            Enemy Enemy = hit.transform.GetComponentInParent<Enemy>();
            // Rigidbody rb= hit.transform.GetComponentInParent<Rigidbody>();
            if (target != null && Enemy != null)
            {
                target.TakeDamage(damage);
                Enemy.UpdateHealthBar(enemy.CurrentHealth);





            }

            //if (rb != null)
            //{
            //    rb.AddForce(-hit.normal * ImpactForce);
            //}

            //GameObject impactGO = Instantiate(impacteffect, hit.point, Quaternion.LookRotation(hit.normal));
            //Destroy(impactGO, 2.0f);
            
            if (enemy != null && enemy.CurrentHealth == 0)
            {
                
                ServiceLocator.Get<GameManager>().UpdateScore(10);
                Instantiate(muzzleExplosion, hit.point, Quaternion.LookRotation(hit.normal));
                Invoke("DestroyLaser", 0.1f);


            }

        }
        //if (laser.enabled)
        //{
        //    laser.enabled = false;
        //}
        Invoke("DestroyLaser", 0.1f);
    }

    void DestroyLaser()
    {
        if (laser.enabled)
        {
            laser.enabled = false;
        }
    }

}
