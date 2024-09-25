using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
  public static GameManager gameManager;
  public int score;
  public int lives = 3;

  void Awake()
  {
    if (gameManager == null)
    {
      gameManager = this;
      DontDestroyOnLoad(gameObject);
    }
    else if (gameManager != this)
    {
      Destroy(gameObject);
    }

  }
}
