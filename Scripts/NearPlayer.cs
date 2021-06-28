using UnityEngine;

public class NearPlayer : MonoBehaviour
{
    public GameObject player;

    void Update()
    {
        if (((player.transform.position.x - transform.position.x) * (player.transform.position.x - transform.position.x)) + ((player.transform.position.z - transform.position.z) * (player.transform.position.z - transform.position.z)) <= 1 && player.transform.position.y <= transform.position.y + this.gameObject.GetComponent<CapsuleCollider>().height / 2 && player.GetComponent<Movement>().jumpedOff == 0) {
            player.GetComponent<Movement>().nearPole = true;
            player.GetComponent<Movement>().pole = this.gameObject;
        } else {
            player.GetComponent<Movement>().nearPole = false;
        }
    }
}
