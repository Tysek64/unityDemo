using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ActivateSign : MonoBehaviour
{
    public Transform player;
    public Text signUI;
    public GameObject signPanel;
    public GameObject signImg;
    bool continueText = true;

    void Update()
    {
        if (((transform.position.x - player.position.x) * (transform.position.x - player.position.x) + (transform.position.z - player.position.z) * (transform.position.z - player.position.z) <= 2.5) && Math.Abs(transform.position.y - player.position.y) <= 2) {
            player.gameObject.GetComponent<NearSign>().nearSign = true;
            signImg.SetActive(true);
        } else {
            signImg.SetActive(false);
        }
        if (player.gameObject.GetComponent<NearSign>().nearSign) {
            if ((((transform.position.x - player.position.x) * (transform.position.x - player.position.x) + (transform.position.z - player.position.z) * (transform.position.z - player.position.z) <= 2.5) && Math.Abs(transform.position.y - player.position.y) <= 2)) {
                signUI.text = this.gameObject.GetComponent<TextOnSign>().text;
                if (Input.GetKey("space") && continueText) {
                    continueText = false;
                    if (player.gameObject.GetComponent<Movement>().enabled) {
                        player.gameObject.GetComponent<Movement>().enabled = false;
                        player.gameObject.GetComponent<Collision>().enabled = false;
                        signPanel.SetActive(true);
                    } else {
                        player.gameObject.GetComponent<Movement>().afterReading = 10;
                        player.gameObject.GetComponent<Movement>().enabled = true;
                        player.gameObject.GetComponent<Collision>().enabled = true;
                        signPanel.SetActive(false);
                    }
                }
                if (!Input.GetKey("space")) {
                    continueText = true;
                }
            }
        }
    }
}
