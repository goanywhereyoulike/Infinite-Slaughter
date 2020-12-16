using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float maxSpeed = 2.0f;
    public Transform target;
    //public float maxHealth = 100.0f;
    [SerializeField]
    private Slider healthBar;

    private NavMeshAgent _agent;
    private Animator _animator;
    private float dividedSpeed = 0.0f;
    private bool isDead = false;
    private WaypointManager.Path _path;
    private int _currentWaypoint = 0;
    //private float _currentHealth = 0.0f;
    private float deathClipLength;
    public GameObject dropPrefab = null;
    private void Awake()
    {
        healthBar.maxValue = this.GetComponent<DestructibleObject>().MaxHealth;
        healthBar.value = healthBar.maxValue;
        _animator = gameObject.GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.Log("Animator compoent does not exist.");
        }
    }

    // Start is called before the first frame update
    //void Start()
    //{
    //   //// _currentHealth = maxHealth;
    //   // //_animator = GetComponent<Animator>();
    //   // _agent = GetComponent<NavMeshAgent>();
        
    //   // if(_agent != null)
    //   // {            
    //   //     _agent.SetDestination(target.position);
    //   //     _agent.speed = maxSpeed;
    //   // }
    //   // dividedSpeed = 1 / maxSpeed;

    //   // AnimationClip[] animations = _animator.runtimeAnimatorController.animationClips;
    //   // if(animations == null || animations.Length <= 0)
    //   // {
    //   //     Debug.Log("animations Error");
    //   //     return;
    //   // }
    //    //sound effect
    //    /* 
    //    for(int i = 0; i<animations.Length; ++i)
    //    {
    //        if(animations[i].name == "Die")
    //        {
    //            deathClipLength = animations[i].length;
    //        }
    //        if(animations[i].name == "GetHit")
    //        {
    //            getHitClipLength = animations[i].length;
    //        }
    //    }
    //    */
    //}

    // Update is called once per frame
    void Update()
    {
        UpdateAnimation();

        //if (isDead)
        //{

        //    return;
        //}


        ////move to target
        //float dist = Vector3.Distance(target.position, transform.position);
        //if (target == null || dist < 2f)
        //{
        //    _animator.SetBool("IsWalking", false);
        //    return;
        //}        
        //else
        //{
        //    _animator.SetBool("IsWalking", true);
        //}
        //if (_agent.speed < maxSpeed)
        //{
        //    _agent.speed = maxSpeed;
        //}
        if(_path == null || _path.Waypoints == null || _path.Waypoints.Count <= _currentWaypoint)
        {
            return;
        }
        Transform destination = _path.Waypoints[_currentWaypoint];
        _agent.SetDestination(destination.position);
        if((transform.position - destination.position).sqrMagnitude < 3.0f * 3.0f)
        {
            _currentWaypoint++;
        }

        //_agent.SetDestination(target.position);

        if (GetComponent<DestructibleObject>().CurrentHealth <= 0)
        {
            StartCoroutine("Kill");

        }

    }

    private void UpdateAnimation()
    {
        _animator.SetFloat("Move", _agent.velocity.magnitude * dividedSpeed);
        _animator.SetBool("IsDead", isDead);
    }

    public void Initialize(WaypointManager.Path path)
    {
        _path = path;
        _agent = gameObject.AddComponent<NavMeshAgent>();
        if (_agent != null)
        {
            _agent.SetDestination(target.position);
            _agent.speed = maxSpeed;
        }
        dividedSpeed = 1 / maxSpeed;

        AnimationClip[] animations = _animator.runtimeAnimatorController.animationClips;
        if (animations == null || animations.Length <= 0)
        {
            Debug.Log("animations Error");
            return;
        }

        for (int i = 0; i < animations.Length; ++i)
        {
            if (animations[i].name == "anim_close")
            {
                deathClipLength = animations[i].length;
                break;
            }
        }
    }


    public IEnumerator Kill()
    {
        _agent.isStopped = true;
        isDead = true;
        Vector3 pos = transform.position;
        pos.y += 1;
        //if(dropPrefab != null)
        //{
        //    GameObject dropItem = Instantiate(dropPrefab, pos, Quaternion.identity);
        //}
        yield return new WaitForSeconds(1f);
        ResetAndRecycle();
    }

    public void UpdateHealthBar(float health)
    {
        healthBar.value = health;

    }

    public void ResetAndRecycle()
    {
        _currentWaypoint = 0;
        isDead = false;
        GetComponent<DestructibleObject>().CurrentHealth = 100;
        transform.rotation = Quaternion.identity;
        ServiceLocator.Get<ObjectPoolManager>().RecycleObject(gameObject);

    }
}
