using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  private const int SPEED = 7;
  private const int JUMP_POWER = 5;
  private const float FALL_LIMIT = -6.0f;
  private const float LEFT_LIMIT = -10.0f;
  private UnityEngine.Vector2 movement;
  private UnityEngine.Vector3 startingPosition;
  private Rigidbody2D rigidBody;
  private bool isJumping;
  private int score;
  private int lives;

  public LayerMask groundLayer;
  public TMP_Text scoreText;
  public TMP_Text livesText;

  private void Start()
  {
    rigidBody = GetComponent<Rigidbody2D>();
    startingPosition = transform.position;
    isJumping = false;
    score = 0;
    lives = 3;
  }

  private void Update()
  {
    // Reset to beginning if player falls off the map
    if (transform.position.y <= FALL_LIMIT)
    {
      transform.position = startingPosition;
      rigidBody.velocity = UnityEngine.Vector2.zero;
    }
    if (transform.position.x <= LEFT_LIMIT)
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
  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.CompareTag("Burger"))
    {
      Destroy(collision.gameObject);
      score++;
      scoreText.text = "Score: " + score;
    }
    if (collision.CompareTag("Enemy"))
    {
      transform.position = startingPosition;
      rigidBody.velocity = UnityEngine.Vector2.zero;
      lives--;
      livesText.text = "Lives: " + lives;

      if (lives == 0)
      {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LoseScreen");
      }
    }
    if (collision.CompareTag("Door") && score >= 4)
    {
      UnityEngine.SceneManagement.SceneManager.LoadScene("LevelTwo");
    }
  }
}
