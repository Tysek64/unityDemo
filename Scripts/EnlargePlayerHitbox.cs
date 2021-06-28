using UnityEngine;
using System;

public class EnlargePlayerHitbox : MonoBehaviour
{
    public Transform player;
    public CapsuleCollider playerHitbox;

    void Update()
    {
        if (((transform.position.x - player.position.x) * (transform.position.x - player.position.x) + (transform.position.z - player.position.z) * (transform.position.z - player.position.z) <= 2.5) && Math.Abs(transform.position.y - player.position.y) <= 2) {
            if (this.gameObject.GetComponent<BoxCollider>() == null) {
                if (this.gameObject.GetComponent<SphereCollider>().enabled) {
                    this.gameObject.GetComponent<MeshRenderer>().enabled = false;
                    transform.position = player.position;
                }
            } else {
                playerHitbox.radius = 1.6f;
            }
        }
    }
}
