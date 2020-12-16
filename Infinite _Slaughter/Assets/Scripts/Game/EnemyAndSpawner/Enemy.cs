using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float maxSpeed = 4.0f;
    public Transform target;
    //public float maxHealth = 100.0f;
    [SerializeField]
    private Slider healthBar;
    private AudioSource _audioSource;

    private NavMeshAgent _agent;
    private Animator _animator;
    private float dividedSpeed = 0.0f;
    private bool isDead = false;
    private WaypointManager.Path _path;
    //private int _currentWaypoint = 0;
    //private float _currentHealth = 0.0f;
    public GameObject dropPrefab = null;
    private void Awake()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        healthBar.maxValue = this.GetComponent<DestructibleObject>().MaxHealth;
        healthBar.value = healthBar.maxValue;
    }

    // Start is called before the first frame update
    void Start()
    {
       // _currentHealth = maxHealth;
        _animator = GetComponent<Animator>();
        _agent = gameObject.AddComponent<NavMeshAgent>();
        
        if(_agent != null)
        {            
            _agent.SetDestination(target.position);
            _agent.speed = maxSpeed;
        }
        dividedSpeed = 1 / maxSpeed;

        AnimationClip[] animations = _animator.runtimeAnimatorController.animationClips;
        if(animations == null || animations.Length <= 0)
        {
            Debug.Log("animations Error");
            return;
        }
        //sound effect
        /* 
        for(int i = 0; i<animations.Length; ++i)
        {
            if(animations[i].name == "Die")
            {
                deathClipLength = animations[i].length;
            }
            if(animations[i].name == "GetHit")
            {
                getHitClipLength = animations[i].length;
            }
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimation();

        if (isDead)
        {

            return;
        }
        //move to target
        float dist = 0;
        if (target != null)
        {
            dist = Vector3.Distance(target.position, transform.position);
        }
        
        if (target == null || dist < 2f)
        {
            _animator.SetBool("IsWalking", false);
            return;
        }        
        else
        {
            _animator.SetBool("IsWalking", true);
        }
        if (_agent.speed < maxSpeed)
        {
            _agent.speed = maxSpeed;
        }
        /*if(_path == null || _path.Waypoints == null || _path.Waypoints.Count <= _currentWaypoint)
        {
            return;
        }
        Transform destination = waypoint;        
        _agent.SetDestination(destination.position);
        if((transform.position - destination.position).sqrMagnitude < 3.0f * 3.0f)
        {
            _currentWaypoint++;
        }*/

        _agent.SetDestination(target.position);
        if (GetComponent<DestructibleObject>().CurrentHealth <= 0)
        {
            isDead = true;
            StartCoroutine("Kill");
        }

    }

    private void UpdateAnimation()
    {
        //_animator.SetFloat("EnemySpeed", _agent.velocity.magnitude * dividedSpeed);
        _animator.SetBool("IsDead", isDead);
    }

    public void Initialize(WaypointManager.Path path)
    {
        _path = path;
    }

    //public float GetHealth()
    //{
    //    return _currentHealth;
    //}

    //public void TakeDamage(float damage)
    //{
    //    _currentHealth -= damage;
    //    if(_currentHealth <= 0.0f && isDead == false)
    //    {
    //        isDead = true;
    //        StartCoroutine("Kill");
    //    }
    //}

    public IEnumerator Kill()
    {
        _agent.isStopped = true;
        AudioManager audio = ServiceLocator.Get<AudioManager>();
        _audioSource.PlayOneShot(audio.Explode1);
        if(dropPrefab != null)
        {
            Vector3 pos = transform.position;
            pos.y += 1;
            GameObject dropItem = Instantiate(dropPrefab, pos, Quaternion.identity);
        }
        yield return new WaitForSeconds(0.1f);
        ResetAndRecycle();
    }

    public void UpdateHealthBar(float health)
    {
        healthBar.value = health;

    }
    public void ResetAndRecycle()
    {
       
        isDead = false;
      
        transform.rotation = Quaternion.identity;
        ServiceLocator.Get<ObjectPoolManager>().RecycleObject(gameObject);

    }
}
