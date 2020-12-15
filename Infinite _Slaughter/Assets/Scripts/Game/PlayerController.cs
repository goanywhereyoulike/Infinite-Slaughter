using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12.0f;
    public float gravity = -9.81f;
    public float jumpHeight = 3.0f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public Vector3 velocity;
    bool IsGrounded;
    DestructibleObject player;
    private void Start()
    {
        player = GetComponent<DestructibleObject>();
        ServiceLocator.Get<GameManager>().SetHealthBar(player.MaxHealth);
        ServiceLocator.Get<GameManager>().UpdateHealthBar(player.CurrentHealth);
        DontDestroyOnLoad(gameObject);
        
    }
    // Update is called once per frame
    void Update()
    {
        
        Move();
        if (player.CurrentHealth <= 0)
        {
            
            ServiceLocator.Get<GameManager>().UpdateMessageText("YOU LOSE");
            Time.timeScale = 0;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ServiceLocator.Get<GameManager>().Pause();

        }


    }

    private void Move()
    {
        IsGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (IsGrounded && velocity.y < 0.0f)
        {
            velocity.y = -2.0f;

        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && IsGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);

        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);


    }
   

}
