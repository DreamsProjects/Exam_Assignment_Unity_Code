using UnityEngine;

public class Wand : MonoBehaviour
{
    public float wandSpeed;
    public Rigidbody2D wandRigidBody;
    public PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        wandRigidBody = GetComponent<Rigidbody2D>();

        Vector3 playersLastPosition = new Vector3(player.lastMove.x, player.lastMove.y, 0f);

        wandRigidBody.velocity = playersLastPosition * wandSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }
}