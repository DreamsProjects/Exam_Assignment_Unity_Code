using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public GameObject followTarget;
    public float moveSpeed;

    //Private fields
    Camera cameraComponent;
    Vector3 targetPosition;
    static bool cameraExists;
    PlayerController player;

    void Start()
    {
        cameraComponent = GetComponent<Camera>();
        player = followTarget.GetComponent<PlayerController>();

        if (!cameraExists)
        {
            cameraExists = true;
            DontDestroyOnLoad(transform.gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        targetPosition = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (player.currentMap == "mainMenu")
        {
            Destroy(gameObject);
        }
    }
}