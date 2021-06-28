using UnityEngine;
using UnityEngine.UI;

public class health : MonoBehaviour
{
    public Text healthText;

    void Update()
    {
        healthText.text = FindObjectOfType<Collision>().health.ToString();
    }
}
