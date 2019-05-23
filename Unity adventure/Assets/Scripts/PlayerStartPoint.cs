using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartPoint : MonoBehaviour
{
    public string pointToLoad;
    public Vector2 start;

    //Private fields
    PlayerController player;
    CameraController cameraPosition;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        cameraPosition = FindObjectOfType<CameraController>();

        if (player.startPoint == pointToLoad)
        {
            player.transform.position = transform.position;
            player.lastMove = start;

            cameraPosition.transform.position = new Vector3(transform.position.x, transform.position.y, cameraPosition.transform.position.z);
        }
    }
}