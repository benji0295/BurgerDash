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
  private const float FALL_LIMIT = -10.0f;
  private UnityEngine.Vector2 movement;
  private UnityEngine.Vector3 startingPosition;
  private Rigidbody2D rigidBody;
  private bool isJumping;
  private int totalScore;
  private int levelScore;
  private int lives;
  private SpriteRenderer spriteRenderer;
  private AudioSource audioSource;

  public LayerMask groundLayer;
  public TMP_Text scoreText;
  public TMP_Text livesText;
  public AudioClip jumpSound;
  public AudioClip collectSound;
  public AudioClip hitSound;

  private void Start()
  {
    rigidBody = GetComponent<Rigidbody2D>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    audioSource = GetComponent<AudioSource>();

    startingPosition = transform.position;
    isJumping = false;

    totalScore = 0;
    levelScore = 0;
    lives = 3;
  }

  private void Update()
  {
    // Reset to beginning if player falls off the map
    if (transform.position.y <= FALL_LIMIT)
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

    // Horizontal movement input
    movement.x = Input.GetAxis("Horizontal");

    // Flip sprite based on movement direction
    if (movement.x < 0)
    {
      spriteRenderer.flipX = true;
    }
    else if (movement.x > 0)
    {
      spriteRenderer.flipX = false;
    }

    // Establish grounded state
    UnityEngine.Vector2 bottomOfCharacter = new UnityEngine.Vector2(transform.position.x, transform.position.y - 1f);
    UnityEngine.Vector2 groundHitBoxDimensions = new UnityEngine.Vector2(0.8f, 0.1f);
    bool isGrounded = Physics2D.OverlapBox(bottomOfCharacter, groundHitBoxDimensions, 0, groundLayer);

    // Jump input
    if (Input.GetButtonDown("Jump") && isGrounded)
    {
      isJumping = true;
      audioSource.PlayOneShot(jumpSound);
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
      audioSource.PlayOneShot(collectSound);
      Destroy(collision.gameObject);
      totalScore++;
      levelScore++;
      scoreText.text = "Score: " + totalScore;
    }
    if (collision.CompareTag("Enemy"))
    {
      audioSource.PlayOneShot(hitSound);
      transform.position = startingPosition;
      rigidBody.velocity = UnityEngine.Vector2.zero;
      lives--;
      livesText.text = "Lives: " + lives;

      if (lives == 0)
      {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LoseScreen");
      }
    }
    if (collision.CompareTag("Door"))
    {
      if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "LevelOne" && levelScore == 4)
      {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelTwo");
      }
      else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "LevelTwo" && levelScore == 4)
      {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelThree");
      }
      else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "LevelThree" && levelScore == 6)
      {
        UnityEngine.SceneManagement.SceneManager.LoadScene("WinScreen");
      }
      else
      {
        UnityEngine.SceneManagement.SceneManager.LoadScene("WinScreen");
      }
    }
  }
}
