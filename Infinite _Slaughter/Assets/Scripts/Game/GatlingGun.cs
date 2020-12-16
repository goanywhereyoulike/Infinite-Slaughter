using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatlingGun : MonoBehaviour
{
    // target the gun will aim at
    Transform go_target;
    // Gameobjects need to control rotation and aiming
    public Transform go_baseRotation;
    public Transform go_GunBody;
    public Transform go_barrel;
    public Transform muzzle;


    // Gun barrel rotation
    public float barrelRotationSpeed;
    float currentRotationSpeed;

    // Distance the turret can aim and fire from
    public float firingRange;

    // Particle system for the muzzel flash
    public ParticleSystem muzzelFlash;

    // Used to start and stop the turret firing
    bool canFire = false;

    private float nextTimeToFire = 0.5f;
    public float firerate = 10.0f;
    private float dividedFireRate = 0.0f;

    public float damage;
    void Start()
    {
        // Set the firing range distance
        this.GetComponent<SphereCollider>().radius = firingRange;
        dividedFireRate = 1 / firerate;
    
    }

    void Update()
    {
        if (Time.time > nextTimeToFire)
        {
            nextTimeToFire = Time.time + dividedFireRate;
            AimAndFire();
        }

    }

    void OnDrawGizmosSelected()
    {
        // Draw a red sphere at the transform's position to show the firing range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, firingRange);
    }

    // Detect an Enemy, aim and fire
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            go_target = other.transform;
            canFire = true;
        }

    }
    // Stop firing
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            canFire = false;
        }
    }

    void AimAndFire()
    {
        // Gun barrel rotation
        go_barrel.transform.Rotate(0, 0, currentRotationSpeed * Time.deltaTime);

        // if can fire turret activates
        if (canFire)
        {
            // start rotation
            currentRotationSpeed = barrelRotationSpeed;

            // aim at enemy
            Vector3 baseTargetPostition = new Vector3(go_target.position.x, this.transform.position.y, go_target.position.z);
            Vector3 gunBodyTargetPostition = new Vector3(go_target.position.x, go_target.position.y, go_target.position.z);

            go_baseRotation.transform.LookAt(baseTargetPostition);
            go_GunBody.transform.LookAt(gunBodyTargetPostition);

            // start particle system 
            if (!muzzelFlash.isPlaying)
            {
                muzzelFlash.Play();
            }
            //go_target.GetComponentInParent<IDamageable>().TakeDamage(damage);
            //go_target.GetComponentInParent<Enemy>().UpdateHealthBar(go_target.transform.GetComponentInParent<DestructibleObject>().CurrentHealth);
            RaycastHit hit;
            if (Physics.Raycast(go_barrel.transform.position, go_barrel.transform.forward, out hit, firingRange))
            {
                Debug.Log(hit.transform.name);


                IDamageable target = hit.transform.GetComponentInParent<IDamageable>();
                DestructibleObject enemy = hit.transform.GetComponentInParent<DestructibleObject>();
                Enemy Enemy = hit.transform.GetComponentInParent<Enemy>();
                // Rigidbody rb= hit.transform.GetComponentInParent<Rigidbody>();
                if (target != null && Enemy != null)
                {
                    target.TakeDamage(damage);
                    Enemy.UpdateHealthBar(enemy.CurrentHealth);
                }
                if (enemy != null && enemy.CurrentHealth == 0)
                { 
                    ServiceLocator.Get<GameManager>().UpdateScore(10);
                }
            }
        }
        else
        {
            // slow down barrel rotation and stop
            currentRotationSpeed = Mathf.Lerp(currentRotationSpeed, 0, 10 * Time.deltaTime);

            // stop the particle system
            if (muzzelFlash.isPlaying)
            {
                muzzelFlash.Stop();
            }
        }
    }
}