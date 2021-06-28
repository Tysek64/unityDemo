using UnityEngine;

public class CollideWithPlayer : MonoBehaviour
{
    public GameObject coin;

    void OnCollisionEnter(UnityEngine.Collision collisionInfo) {
        Debug.Log("Coll");
        if (collisionInfo.gameObject.name == "Player") {
            coin.SetActive(false);
        }
    }
}
