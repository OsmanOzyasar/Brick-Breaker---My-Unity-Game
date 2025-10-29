using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball_ : MonoBehaviour
{
    [SerializeField] float baseSpeed;
    [SerializeField] float minSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] private float minBounceAngle = 15f;
    [SerializeField] private float maxBounceAngle = 75f;
    private Rigidbody2D rb;
    private Vector2 lastVelocity;
    private List<ContactPoint2D> contacts = new List<ContactPoint2D>();
    private GameManager gameManager;
    public bool isTouchedThePaddle = false;

    private bool justHitPaddle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();

        Vector3 leftLimit = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, 0));
        Vector3 rightLimit = Camera.main.ViewportToWorldPoint(new Vector3(1, 0.5f, 0));

        float leftX = leftLimit.x;
        float rightX = rightLimit.x;
        float centerX = (leftX + rightX) / 2;

        transform.position = new Vector3(centerX, transform.position.y, transform.position.z);

        rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = new Vector2(0, baseSpeed);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lastVelocity = rb.linearVelocity;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        HandleCollision(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isTouchedThePaddle = false;
        }
    }

    private Vector2 ApplyBounceAngleLimits(Vector2 velocity)
    {
        float angle = Vector2.Angle(Vector2.up, velocity);
        float sign = Mathf.Sign(velocity.x);

        if (angle < minBounceAngle)
        {
            float radians = minBounceAngle * Mathf.Deg2Rad;
            velocity = new Vector2(Mathf.Sin(radians) * sign, Mathf.Cos(radians));
        }
        else if (angle > maxBounceAngle)
        {
            float radians = maxBounceAngle * Mathf.Deg2Rad;
            velocity = new Vector2(Mathf.Sin(radians) * sign, Mathf.Cos(radians));
        }

        return velocity.normalized;
    }

    private void HandleCollision(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Respawn"))
        {
            FindAnyObjectByType<GameManager>().GameOver();
        }

        var contactCount = collision.GetContacts(contacts);
        var normal = Vector2.zero;

        var lastSpeed = lastVelocity.magnitude;

        for (int i = 0; i < contactCount; i++)
        {
            var contactNormal = contacts[i].normal;

            if (Vector2.Dot(lastVelocity, contactNormal) > 0)
                continue;

            normal += contactNormal;
        }

        if (normal == Vector2.zero)
            return;

        Vector2 reflected = Vector2.Reflect(lastVelocity, normal).normalized;

        if (collision.gameObject.CompareTag("Player"))
        {
            isTouchedThePaddle = true;
            justHitPaddle = true;

            var paddleRb = collision.gameObject.GetComponent<Rigidbody2D>();

            var paddleVelocity = paddleRb.linearVelocity;

            reflected += paddleVelocity.normalized;

            reflected = ApplyBounceAngleLimits(reflected);

        }

        float clampedSpeed = Mathf.Clamp(lastSpeed, minSpeed, maxSpeed);

        rb.linearVelocity = clampedSpeed * reflected.normalized;

        if (collision.gameObject.CompareTag("Bricks"))
        {
            isTouchedThePaddle = false;
            collision.gameObject.GetComponentInParent<Brick>().brickHealth -= 50;
        }
    }

    public bool ConsumePaddleHit()
    {
        if (justHitPaddle)
        {
            justHitPaddle = false;
            return true;
        }
        return false;
    }
}
