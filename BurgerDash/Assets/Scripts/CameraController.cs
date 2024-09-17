using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  private GameObject player;

  private void Start()
  {
    player = GameObject.FindGameObjectWithTag("Player");
  }

  private void Update()
  {
    var playerPosition = player.transform.position;
    var cameraPosition = transform.position;

    cameraPosition.x = playerPosition.x;
    cameraPosition.y = playerPosition.y;

    transform.position = cameraPosition;
  }
}
