using UnityEngine;

public class HealPlayer : MonoBehaviour
{
    public int healing;
    public GameObject damageNumber;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealthManager>().HealPlayer(healing);

            var clone = (GameObject)Instantiate(damageNumber, collision.transform.position, Quaternion.Euler(Vector3.zero));

            clone.GetComponent<FloatingNumbers>().damage = healing;
        }
    }

    public void HealPlayerWithFood(int healing, GameObject player)
    {
        var gameObject = new HealPlayer();
        var floatNumber = new FloatingNumbers();

        gameObject.healing = healing;

        var clone = (GameObject)Instantiate(damageNumber, player.transform.position, Quaternion.Euler(Vector3.zero));
        clone.GetComponent<FloatingNumbers>().damage = healing;
        var playerManager = FindObjectOfType<PlayerHealthManager>();
        playerManager.HealPlayer(healing);
    }
}