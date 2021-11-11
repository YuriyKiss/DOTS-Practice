using UnityEngine;

using Input = InputWrapper.Input;

public class Movement : MonoBehaviour
{
    // Personal components
    private Rigidbody body;

    // Personal values
    private Vector2 playerInput = Vector2.zero;
    private Vector3 velocity = Vector3.zero;

    [SerializeField, Range(0f, 100f)]
    private float maxSpeed = 10f;

    [SerializeField, Range(0f, 100f)]
    private float maxAcceleration = 10f;

    [SerializeField, Min(0.1f)]
    private float ballRadius = 0.5f;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    void Update()
    {
        playerInput = Vector2.zero;

        if (Input.TouchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                playerInput = touch.deltaPosition;
                playerInput = Vector2.ClampMagnitude(playerInput, 1f);
            }
        }
    }

    private void FixedUpdate()
    {
        Vector3 desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;

        float maxSpeedChange = maxAcceleration * Time.deltaTime;

        velocity = body.velocity;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

        Vector3 movement = velocity * Time.deltaTime;

        float distance = movement.magnitude;
        if (distance > 0.001f)
        {
            float angle = distance * (180f / Mathf.PI) / ballRadius;

            Vector3 rotationAxis = Vector3.Cross(Vector3.up, movement).normalized;
            transform.localRotation = Quaternion.Euler(rotationAxis * angle) * transform.localRotation;
        }

        body.velocity = velocity;
    }
}
