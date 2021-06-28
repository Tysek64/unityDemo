using UnityEngine;
using UnityEngine.UI;

public class CoinCount : MonoBehaviour
{
    public Text coinCount;

    void Update()
    {
        coinCount.text = FindObjectOfType<Collision>().coinCounter.ToString();
    }
}
