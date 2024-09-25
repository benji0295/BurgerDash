using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor.Build.Content;
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

    livesText.text = "Lives: " + GameManager.gameManager.lives;
    scoreText.text = "Score: " + GameManager.gameManager.score;
  }

  private void Update()
  {
    if (transform.position.y <= FALL_LIMIT)
    {
      transform.position = startingPosition;
      rigidBody.velocity = UnityEngine.Vector2.zero;
      GameManager.gameManager.lives--;
      livesText.text = "Lives: " + GameManager.gameManager.lives;
      scoreText.text = "Score: " + GameManager.gameManager.score;

      if (GameManager.gameManager.lives == 0)
      {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LoseScreen");
      }
    }
    movement.x = Input.GetAxis("Horizontal");

    if (movement.x < 0)
    {
      spriteRenderer.flipX = true;
    }
    else if (movement.x > 0)
    {
      spriteRenderer.flipX = false;
    }

    UnityEngine.Vector2 bottomOfCharacter = new UnityEngine.Vector2(transform.position.x, transform.position.y - 1f);
    UnityEngine.Vector2 groundHitBoxDimensions = new UnityEngine.Vector2(0.8f, 0.1f);
    bool isGrounded = Physics2D.OverlapBox(bottomOfCharacter, groundHitBoxDimensions, 0, groundLayer);

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
      GameManager.gameManager.score++;
      scoreText.text = "Score: " + GameManager.gameManager.score;
    }
    if (collision.CompareTag("Enemy"))
    {
      audioSource.PlayOneShot(hitSound);
      transform.position = startingPosition;
      rigidBody.velocity = UnityEngine.Vector2.zero;
      GameManager.gameManager.lives--;
      livesText.text = "Lives: " + GameManager.gameManager.lives;

      if (GameManager.gameManager.lives == 0)
      {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LoseScreen");
      }
    }
    if (collision.CompareTag("Door"))
    {
      if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "LevelOne" && GameManager.gameManager.score == 4)
      {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelTwo");
      }
      else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "LevelTwo" && GameManager.gameManager.score == 8)
      {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelThree");
      }
      else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "LevelThree" && GameManager.gameManager.score == 14)
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
