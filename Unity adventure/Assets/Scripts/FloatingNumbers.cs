using UnityEngine;
using UnityEngine.UI;

public class FloatingNumbers : MonoBehaviour
{
    public float speed;
    public int damage;
    public Text display;

    void Update()
    {
        if (damage > 0)
        {
            display.text = $"{damage}";
            transform.position = new Vector3(transform.position.x, transform.position.y + (speed * Time.deltaTime), transform.position.z);
        }
    }
}