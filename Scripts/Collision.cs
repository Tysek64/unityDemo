using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class Collision : MonoBehaviour
{
    public MeshRenderer visible;
    public Movement playerMovement;
    public GameObject star;
    public GameObject LevelLoadScreen;
    public GameObject RedCoinStar;
    public GameManager gm;
    public GameObject cameraTarget;
    GameObject button;
    public int coinCounter = 0;
    public int starCounter = 0;
    public int health = 8;
    public bool hitCoin = false;
    public bool[] starsCollected = new bool[120];
    public int[] highCoinScores = new int[15];
    int invTimer = 100;
    public int newStageNr;
    public int nextTarget = 0;

    void Awake () {
        for (int i = 0;i < 120;i++) {
            starsCollected[i] = false;
        }
        for (int i = 0;i < 15;i++) {
            highCoinScores[i] = 0;
        }
        PlayerData data = SaveData.LoadPlayer();
        if (data != null) {
            highCoinScores = data.highCoinCount;
            starCounter = data.starCount;
            starsCollected = data.whichStars;
        }
    }
    
    void OnTriggerEnter(Collider obj) {
        if (obj.tag == "Coin") {
            coinCounter++;
            if (health < 8) {
                health++;
            }
            GameObject.Find(obj.name).SetActive(false);
            Destroy(GameObject.Find(obj.name));
            hitCoin = true;
            if (coinCounter == 25) {
                GameObject hcs = Instantiate(star, transform.position + new Vector3(0f, 2f, 0f), Quaternion.Euler(90, 0, 0));
                hcs.GetComponent<myID>().ID = 6;
                hcs.GetComponent<EnlargePlayerHitbox>().player = this.transform;
                hcs.GetComponent<EnlargePlayerHitbox>().playerHitbox = this.gameObject.GetComponent<CapsuleCollider>();
            }
            if (coinCounter > highCoinScores[SceneManager.GetActiveScene().buildIndex]) {
                highCoinScores[SceneManager.GetActiveScene().buildIndex] = coinCounter;
            }
        }
        if (obj.tag == "Star") {
            if (!starsCollected[Convert.ToInt32(obj.GetComponent<myID>().ID)]) {
                starCounter++;
            }
            starsCollected[Convert.ToInt32(obj.GetComponent<myID>().ID)] = true;
            Destroy(GameObject.Find(obj.name));
            hitCoin = true;
            SaveData.SavePlayer(this);
            if (Convert.ToInt32(obj.GetComponent<myID>().ID) % 7 != 6) {
                FindObjectOfType<GameManager>().ChangeStage(0);
            }
        }
        if (obj.tag == "Enemy") {
            if (playerMovement.onGround || obj.GetComponent<IsIndestructible>().isIn) {
                if (invTimer >= 100) {
                    health--;
                    invTimer = 0;
                    if (health == 0) {
                        FindObjectOfType<GameManager>().GameOver();
                    }
                }
            } else {
                GameObject.Find(obj.name).GetComponent<Stomp>().StompEnemy();
                playerMovement.verticalVelocity.y = 3f;
            }
        }
        if (obj.tag == "StageLoader") {
            LevelLoadScreen.SetActive(true);
            newStageNr = Convert.ToInt32(obj.name);
        }
        if (obj.tag == "Button" && !obj.GetComponent<EnableButton>().pressed && GetComponent<Movement>().verticalVelocity.y != 0) {
            button = obj.gameObject;
            obj.gameObject.transform.localScale += new Vector3(0f, -0.5f, 0f);
            obj.gameObject.transform.position += new Vector3(0f, -0.5f, 0f);
            obj.GetComponent<EnableButton>().pressed = true;
        }
        if (obj.tag == "Red Coin") {
            obj.gameObject.SetActive(false);
            button.GetComponent<EnableButton>().collectedTargets[nextTarget] = true;
            nextTarget++;
            if (nextTarget == 5) {
                RedCoinStar.SetActive(true);
                StartCoroutine("FocusOnStar");
            }
        }
    }

    void Update () {
        invTimer++;
        if (invTimer >= 100) {
            visible.enabled = true;
        } else {
            if (invTimer % 2 < 1) {
                visible.enabled = false;
            } else {
                visible.enabled = true;
            }
        }
        if (this.GetComponent<Movement>().inWater) {
            cameraTarget.GetComponent<Cinemachine.CinemachineFreeLook>().m_RecenterToTargetHeading.m_enabled = false;
        } else {
            cameraTarget.GetComponent<Cinemachine.CinemachineFreeLook>().m_RecenterToTargetHeading.m_enabled = true;
        }
    }

    IEnumerator FocusOnStar () {
        cameraTarget.GetComponent<Cinemachine.CinemachineFreeLook>().m_LookAt = RedCoinStar.transform;
        this.gameObject.GetComponent<Movement>().enabled = false;
        yield return new WaitForSeconds(2f);
        this.gameObject.GetComponent<Movement>().enabled = true;
        cameraTarget.GetComponent<Cinemachine.CinemachineFreeLook>().m_LookAt = this.transform;
    }
}
