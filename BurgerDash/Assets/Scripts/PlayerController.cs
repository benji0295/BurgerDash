using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  private const int SPEED = 8;
  private const int JUMP_POWER = 5;
  private const float FALL_LIMIT = -8.0f;
  private UnityEngine.Vector2 movement;
  private UnityEngine.Vector3 startingPosition;
  private Rigidbody2D rigidBody;
  private bool isJumping;

  public LayerMask groundLayer;

  private void Start()
  {
    rigidBody = GetComponent<Rigidbody2D>();
    startingPosition = transform.position;
    isJumping = false;
  }

  private void Update()
  {
    // Reset to beginning if player falls off the map
    if (transform.position.y <= FALL_LIMIT)
    {
      transform.position = startingPosition;
      rigidBody.velocity = UnityEngine.Vector2.zero;
    }

    // Horizontal movement input
    movement.x = Input.GetAxis("Horizontal");

    //UnityEngine.Vector2 bottomOfCharacter = new UnityEngine.Vector2(transform.position.x, transform.position.y - 0.5f);
    //UnityEngine.Vector2 groundHitBoxDimensions = new UnityEngine.Vector2(0.8f, 0.1f);
    //bool isGrounded = Physics2D.OverlapBox(bottomOfCharacter, groundHitBoxDimensions, 0, groundLayer);

    if (Input.GetButtonDown("Jump")) //&& isGrounded)
    {
      isJumping = true;
    }
  }
  private void FixedUpdate()
  {
    rigidBody.AddForce(movement * SPEED);

    if (isJumping)
    {
      isJumping = false;
      rigidBody.AddForce(UnityEngine.Vector2.up * JUMP_POWER, ForceMode2D.Impulse);
    }
  }
}
