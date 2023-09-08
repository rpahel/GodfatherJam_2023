using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerMovements : MonoBehaviour
{
    [SerializeField, Tooltip("La vitesse de mouvement max du personnage.")]
    private float maxMovementSpeed;
    [SerializeField, Tooltip("L'acceleration du personnage.")]
    private float acceleration;
    [SerializeField, Tooltip("La force de saut du personnage.")]
    private float jumpForce;

    public bool isPlayerDead = false;
    private float MovementInput { get; set; }
    private Rigidbody2D rb2D;
    public Rigidbody2D Rbobj => rb2D;
    private CircleCollider2D circleCollider;

    private void Awake()
    {
        if (!TryGetComponent(out rb2D))
            Debug.LogError("No RigidBody2D found in " + name + " !");

        if (!TryGetComponent(out circleCollider))
            Debug.LogError("No CircleCollider found in " + name + " !");
    }

    private void Update()
    {
        if (!rb2D)
            return;

        if (!circleCollider)
            return;

        if (transform.position.y <= -7)
            isPlayerDead = true;
        if (MovementInput != 0 && Mathf.Abs(rb2D.velocity.x) < maxMovementSpeed)
            rb2D.velocity += new Vector2(MovementInput * acceleration * Time.deltaTime, 0);
    }

    public void HorizontalMovement(float horizontalInput)
    {
        MovementInput = horizontalInput;
    }

    public void Jump()
    {
        if (IsGrounded())
            rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private bool IsGrounded()
    {
        Vector2 origin = (Vector2)circleCollider.bounds.center;

        if (Physics2D.CircleCast(
                    origin: origin,
                    radius: circleCollider.radius * 0.5f,
                    direction: Vector2.down,
                    distance: circleCollider.radius * 0.5f + 0.1f,
                    layerMask: LayerMask.GetMask("Belt", "Obstacle")))
            return true;

        return false;
    }
}
