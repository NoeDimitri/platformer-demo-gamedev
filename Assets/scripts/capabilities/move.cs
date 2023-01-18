using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{

    [SerializeField] private inputController input = null;
    [SerializeField, Range(0f, 100f)] private float maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float maxAirAcceleration = 20f;

    [SerializeField, Range(0f, 50f)] private float deceleration = 5f;


    private Vector2 direction;
    private Vector2 desiredVelocity;
    private Vector2 velocity;
    private Rigidbody2D body;
    private ground ground;

    private float maxSpeedChange;
    private float acceleration;
    private bool onGround;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<ground>();
        
    }

    // Update is called once per frame
    void Update()
    {
        direction.x = input.RetrieveMoveInput();
        //desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(maxSpeed - ground.getFriction(), 0f);
        desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(maxSpeed - deceleration, 0f);

    }

    private void FixedUpdate()
    {
        onGround = ground.getOnGround();
        velocity = body.velocity;

        acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        maxSpeedChange = acceleration * Time.deltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);

        body.velocity = velocity;
    }
}
