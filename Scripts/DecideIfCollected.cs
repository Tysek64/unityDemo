using UnityEngine;
using System;

public class DecideIfCollected : MonoBehaviour
{
    public Material normal;
    public Material transparent;

    void Update () {
        if (FindObjectOfType<Collision>().starsCollected[Convert.ToInt32(GetComponent<myID>().ID)]) {
            GetComponent<MeshRenderer>().material = transparent;
        } else {
            GetComponent<MeshRenderer>().material = normal;
        }
    }
}
