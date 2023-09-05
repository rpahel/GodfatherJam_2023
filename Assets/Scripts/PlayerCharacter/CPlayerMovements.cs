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

    private float MovementInput { get; set; }
    private Rigidbody2D rb2D;
    private CapsuleCollider2D capsuleCollider;

    private void Awake()
    {
        if (!TryGetComponent(out rb2D))
            Debug.LogError("No RigidBody2D found in " + name + " !");

        if (!TryGetComponent(out capsuleCollider))
            Debug.LogError("No CapsuleCollider2D found in " + name + " !");
    }

    private void Update()
    {
        if (!rb2D)
            return;

        if(!capsuleCollider)
            return;

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
        // Milieu de la partie arrondie a la base de la capsule
        Vector2 origin = (Vector2)capsuleCollider.bounds.center
                            + capsuleCollider.size.y * 0.5f * Vector2.down
                            + capsuleCollider.size.x * 0.5f * Vector2.up;

        // Petit raycast en forme de cercle vers le sol pcq notre perso est une capsule, si ca touche return true
        if (Physics2D.CircleCast(
                    origin: origin,
                    radius: capsuleCollider.size.x * 0.5f,
                    direction: Vector2.down,
                    distance:  capsuleCollider.size.x * 0.5f + 0.1f,
                    layerMask: LayerMask.GetMask("Carpet")))
            return true;

        return false;
    }
}
