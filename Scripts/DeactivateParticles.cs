using UnityEngine;

public class DeactivateParticles : MonoBehaviour
{
    void Update()
    {
        if (!this.gameObject.GetComponent<ParticleSystem>().IsAlive()) {
            Destroy(this.gameObject);
        }
    }
}
