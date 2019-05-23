using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayer : MonoBehaviour
{
    public int damage;
    public GameObject damageNumber;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" || collision.gameObject.name == "Hand")
        {
            collision.gameObject.GetComponent<PlayerHealthManager>().HurtPlayer(damage);

            var clone = (GameObject)Instantiate(damageNumber, collision.transform.position, Quaternion.Euler(Vector3.zero));

            clone.GetComponent<FloatingNumbers>().damage = damage;
        }
    }
}