using UnityEngine;

public class Billboard : MonoBehaviour
{
    public GameObject cam;

	void LateUpdate () {
		transform.LookAt (cam.transform);
	}
}