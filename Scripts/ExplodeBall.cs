using UnityEngine;

public class ExplodeBall : MonoBehaviour
{
    int timer = 0;
    public GameObject explosion;
    public bool ready = false;

    void Awake () {
        transform.localScale *= 0;
    }

    void Update()
    {
        if (timer < 20) {
            transform.localScale += new Vector3(0.25f, 0.25f, 0.25f);
        } else if (this.gameObject.transform.position.z <= -22 || GetComponent<Rigidbody>().velocity.sqrMagnitude <= 0.2f || transform.position.y < -10) {
            ready = true;
            this.gameObject.GetComponent<SphereCollider>().radius *= 2f;
            Instantiate(explosion, transform.position, Quaternion.identity);
            FindObjectOfType<SpawnRollingBalls>().Spawn();
            Destroy(this.gameObject);
        }
        timer++;
    }
}
