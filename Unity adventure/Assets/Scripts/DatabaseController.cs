using Assets.Repository;
using UnityEngine;

public class DatabaseController : MonoBehaviour
{
    public GameObject connectionLost;

    //private fields
    PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (AccountDatabase.PlayerID <= 0)
        {
            connectionLost.SetActive(true);
            player.canMove = false;
        }
    }
}