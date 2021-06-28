using UnityEngine;

public class CheckCollisionWithPlayer : MonoBehaviour
{
    public GameObject player;
    public bool inWater;

    void Update()
    {
        inWater = ((this.gameObject.transform.position.x) <= (player.transform.position.x - player.GetComponent<CapsuleCollider>().radius / 2) && 
        (this.gameObject.transform.position.x + this.gameObject.GetComponent<BoxCollider>().size.x) >= (player.transform.position.x + player.GetComponent<CapsuleCollider>().radius / 2) && 
        (this.gameObject.transform.position.z) >= (player.transform.position.z - player.GetComponent<CapsuleCollider>().radius / 2) && 
        (this.gameObject.transform.position.z - this.gameObject.GetComponent<BoxCollider>().size.z) <= (player.transform.position.z + player.GetComponent<CapsuleCollider>().radius / 2) && 
        (this.gameObject.transform.position.y + this.gameObject.GetComponent<BoxCollider>().size.y) >= (player.transform.position.y) && 
        (this.gameObject.transform.position.y) <= (player.transform.position.y + player.GetComponent<CapsuleCollider>().height / 2));
    }
}
