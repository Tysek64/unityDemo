using UnityEngine;
using UnityEngine.UI;

public class Air : MonoBehaviour
{
    public Text air;

    void Update()
    {
        air.text = FindObjectOfType<Movement>().air.ToString();
    }
}
