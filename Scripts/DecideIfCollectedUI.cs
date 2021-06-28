using System;
using UnityEngine;
using UnityEngine.UI;

public class DecideIfCollectedUI : MonoBehaviour
{
    public Texture collected;
    public Texture normal;

    void Update () {
        if (FindObjectOfType<Collision>().starsCollected[7 * (FindObjectOfType<Collision>().newStageNr - 1) + Convert.ToInt32(this.gameObject.name)]) {
            GetComponent<RawImage>().texture = collected;
        } else {
            GetComponent<RawImage>().texture = normal;
        }
    }
}
