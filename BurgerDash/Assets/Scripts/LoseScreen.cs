using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseScreen : MonoBehaviour
{
  private const float FALL_LIMIT = -8.0f;
  private Vector3 startingPosition;
  private Rigidbody2D rigidBody;

  void Start()
  {
    rigidBody = GetComponent<Rigidbody2D>();
    startingPosition = transform.position;
  }

  void Update()
  {
    if (transform.position.y <= FALL_LIMIT)
    {
      transform.position = startingPosition;
      rigidBody.velocity = Vector2.zero;
    }

    if (Input.anyKeyDown)
    {
      UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
  }
}
