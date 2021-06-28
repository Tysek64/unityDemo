using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableButton : MonoBehaviour
{
    public bool pressed = false;
    bool activated = false;
    public GameObject targets;
    public bool[] collectedTargets = new bool[5];

    void Start () {
        for (int i = 0;i < 5;i++) {
            collectedTargets[i] = false;
        }
    }

    void Update()
    {
        targets.SetActive(pressed);
        if (pressed && !activated) {
            FindObjectOfType<RegenerateTargets>().regenerate();
            FindObjectOfType<RegenerateTargets>().fade();
            activated = true;
            Invoke("returnToNormal", 7f);
        }
    }

    void returnToNormal () {
        if (!collectedTargets[4]) {
            FindObjectOfType<Collision>().nextTarget = 0;
            pressed = false;
            activated = false;
            this.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            this.gameObject.transform.position += new Vector3(0f, 0.5f, 0f);
        }
    }
}
