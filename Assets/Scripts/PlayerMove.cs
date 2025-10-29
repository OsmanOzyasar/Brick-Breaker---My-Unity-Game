using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rb;
    public float playerSpeed = 0f;
    InputAction moveAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 leftLimit = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, 0));
        Vector3 rightLimit = Camera.main.ViewportToWorldPoint(new Vector3(1, 0.5f, 0));

        float leftX = leftLimit.x;
        float rightX = rightLimit.x;
        float centerX = (leftX + rightX) / 2;

        transform.position = new Vector3(centerX, transform.position.y, transform.position.z);

        moveAction = InputSystem.actions.FindAction("Move");
        rb = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        
        rb.linearVelocity = new Vector2(playerSpeed * moveValue.x, rb.linearVelocity.y);

        //Debug.Log(rb.linearVelocity);

    }
}
