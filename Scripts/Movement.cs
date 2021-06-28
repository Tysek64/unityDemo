using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;

public class Movement : MonoBehaviour
{
    public GameObject airUI;
    public GameObject BubbleEmitter;
    public GameObject pole;
    public CapsuleCollider playerColl;
    public CharacterController controller;
    public Collision getCoinHit;
    public Transform cam;
    public Vector3 verticalVelocity = new Vector3(0f, 0f, 0f);
    public Vector3 offset = new Vector3(0f, -2f, 0f);
    public bool inWater = false;
    public bool nearPole = false;
    public bool nearSign = false;
    public bool onGround = false;
    public bool pound = false;
    public bool swim = false;
    public int afterReading = 0;
    public int air = 8;
    public int isOnMoving = 0;
    public int jumpedOff = 0;
    public float Gravity = -10f;
    public float jumpHeight = 1f;
    public float smoothTime = 0.1f;
    public float speed = 6f;
    GameObject bubbles;
    Collider currentGround;
    Coroutine countdown;
    Vector3 forwardAtJump;
    Vector3 movingDir;
    bool crouching = false;
    bool firstFrameInWater = false;
    bool nonGP = false;
    bool onPole = false;
    bool releaseSpace = false;
    int longJump = 0;
    int timeInWater = 0;
    float initialGravity;
    float initialSpeed;
    float movingAngle;
    float smoothVel;

    void Start () {
        initialGravity = Gravity;
        initialSpeed = speed;
    }

    void FixedUpdate()
    {
        if (FindObjectOfType<CheckCollisionWithPlayer>() == null) {
            if (!onPole) {
                if (countdown != null) {
                    StopCoroutine(countdown);
                }
                air = 8;
                timeInWater = 0;
                firstFrameInWater = false;
                swim = false;
                if (!crouching || onGround) {
                    crouching = Input.GetKey("left shift");
                } else {
                    crouching = true;
                }
                float horizontal = Input.GetAxisRaw("Horizontal");
                float vertical = Input.GetAxisRaw("Vertical");
                Vector3 direction = new Vector3 (horizontal, 0f, vertical).normalized;
                if (Input.GetKey("space") && onGround && afterReading == 0 && !this.gameObject.GetComponent<NearSign>().nearSign && !releaseSpace) {
                    releaseSpace = true;
                    if (crouching) {
                        if (direction.magnitude >= 0.5f) {
                            verticalVelocity = new Vector3(0f, 0.75f * jumpHeight, 0f);
                            speed = 1.5f * initialSpeed;
                            longJump = 1;
                        } else {
                            verticalVelocity = new Vector3(0f, 1.2f * jumpHeight, 0f);
                        }
                        nonGP = true;
                    } else {
                        verticalVelocity = new Vector3(0f, jumpHeight, 0f);
                    }
                }
                if (!Input.GetKey("space") && (onGround || Physics.Raycast(transform.position, -Vector3.up, playerColl.height / 2 + 0.5f))) {
                    releaseSpace = false;
                }
                if (crouching) {
                    if (!onGround && !nonGP) {
                        if (Gravity == initialGravity) {
                            verticalVelocity.y = 0f;
                            pound = true;
                        }
                        Gravity = 1.5f * initialGravity;
                        horizontal = 0f;
                        vertical = 0f;
                        direction = new Vector3(0f, 0f, 0f);
                    }
                    if (speed == initialSpeed) {
                        speed = 0.6f * initialSpeed;
                    }
                    transform.localScale = new Vector3(1f, 0.5f, 1f);
                    this.gameObject.GetComponent<CapsuleCollider>().height = 1.1f;
                    this.gameObject.GetComponent<CapsuleCollider>().center = new Vector3(0f, -1.1f, 0f);
                } else {
                    speed = initialSpeed;
                    transform.localScale = new Vector3(1f, 1f, 1f);
                    this.gameObject.GetComponent<CapsuleCollider>().height = 2.1f;
                    this.gameObject.GetComponent<CapsuleCollider>().center = new Vector3(0f, -0.1f, 0f);
                }
                controller.Move(verticalVelocity * Time.deltaTime);
                if (direction.magnitude >= 0.1f || longJump != 0) {
                    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothVel, smoothTime);
                    Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                    if (longJump == 1) {
                        movingDir = moveDir;
                        movingAngle = angle;
                        longJump = 2;
                    } else if (longJump == 2) {
                        moveDir = movingDir;
                        angle = movingAngle;
                    }
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);
                    controller.Move(moveDir.normalized * speed * Time.deltaTime);
                }
                if (verticalVelocity.y >= -100) {
                    verticalVelocity[1] += Gravity * Time.deltaTime;
                }
                if (getCoinHit.hitCoin) {
                    playerColl.radius = 0.5f;
                    getCoinHit.hitCoin = false;
                }
                if (transform.position.y < -10) {
                    FindObjectOfType<GameManager>().GameOver();
                }
                if (isOnMoving == 1) {
                    transform.RotateAround(currentGround.gameObject.transform.position, Vector3.up, currentGround.gameObject.GetComponent<Rotate>().angle * Time.deltaTime);
                }
                if (isOnMoving == 2) {
                    transform.position += currentGround.gameObject.GetComponent<Move>().last;
                }
                if (nearPole && jumpedOff == 0 && verticalVelocity.y != 0) {
                    onPole = true;
                }
                if (jumpedOff != 0) {
                    controller.Move(forwardAtJump * (-jumpedOff / 75f) * speed * Time.deltaTime);
                }
            } else {
                crouching = false;
                transform.localScale = new Vector3(1f, 1f, 1f);
                this.gameObject.GetComponent<CapsuleCollider>().height = 2.1f;
                this.gameObject.GetComponent<CapsuleCollider>().center = new Vector3(0f, -0.1f, 0f);
                if (Input.GetKey("space")) {
                    onPole = false;
                    jumpedOff = 100;
                    verticalVelocity = new Vector3(0f, jumpHeight, 0f);
                    controller.Move(transform.forward * (-jumpedOff / 75f) * speed * Time.deltaTime);
                    forwardAtJump = transform.forward;
                    controller.Move(verticalVelocity * Time.deltaTime);
                } else {
                    float horizontal = Input.GetAxisRaw("Horizontal");
                    float vertical = Input.GetAxisRaw("Vertical");
                    if (vertical < 0) {
                        if (horizontal <= 0) {
                            horizontal = -1;
                        } else {
                            horizontal = 1;
                        }
                    }
                    if (!nearPole && vertical > 0) {
                        vertical = 0;
                    }
                    verticalVelocity = new Vector3(0f, vertical, 0f);
                    controller.Move(verticalVelocity * speed * Time.deltaTime);
                    transform.RotateAround(pole.transform.position, Vector3.up, horizontal * 300 * Time.deltaTime);
                    if (getCoinHit.hitCoin) {
                        playerColl.radius = 0.5f;
                        getCoinHit.hitCoin = false;
                    }
                    if (transform.position.y < -10) {
                        FindObjectOfType<GameManager>().GameOver();
                    }
                }
            }
        } else {
            inWater = FindObjectOfType<CheckCollisionWithPlayer>().inWater;
            if (!inWater) {
                if (!onPole) {
                    if (countdown != null) {
                        StopCoroutine(countdown);
                    }
                    air = 8;
                    timeInWater = 0;
                    airUI.SetActive(false);
                    firstFrameInWater = false;
                    swim = false;
                    if (!crouching || onGround) {
                        crouching = Input.GetKey("left shift");
                    } else {
                        crouching = true;
                    }
                    float horizontal = Input.GetAxisRaw("Horizontal");
                    float vertical = Input.GetAxisRaw("Vertical");
                    Vector3 direction = new Vector3 (horizontal, 0f, vertical).normalized;
                    if (Input.GetKey("space") && onGround && afterReading == 0 && !this.gameObject.GetComponent<NearSign>().nearSign && !releaseSpace) {
                        releaseSpace = true;
                        if (crouching) {
                            if (direction.magnitude >= 0.5f) {
                                verticalVelocity = new Vector3(0f, 0.75f * jumpHeight, 0f);
                                speed = 1.5f * initialSpeed;
                                longJump = 1;
                            } else {
                                verticalVelocity = new Vector3(0f, 1.2f * jumpHeight, 0f);
                            }
                            nonGP = true;
                        } else {
                            verticalVelocity = new Vector3(0f, jumpHeight, 0f);
                        }
                    }
                    if (!Input.GetKey("space") && (onGround || Physics.Raycast(transform.position, -Vector3.up, playerColl.height / 2 + 0.5f))) {
                        releaseSpace = false;
                    }
                    if (crouching) {
                        if (!onGround && !nonGP) {
                            if (Gravity == initialGravity) {
                                verticalVelocity.y = 0f;
                                pound = true;
                            }
                            Gravity = 1.5f * initialGravity;
                            horizontal = 0f;
                            vertical = 0f;
                            direction = new Vector3(0f, 0f, 0f);
                        }
                        if (speed == initialSpeed) {
                            speed = 0.6f * initialSpeed;
                        }
                        transform.localScale = new Vector3(1f, 0.5f, 1f);
                        this.gameObject.GetComponent<CapsuleCollider>().height = 1.1f;
                        this.gameObject.GetComponent<CapsuleCollider>().center = new Vector3(0f, -1.1f, 0f);
                    } else {
                        speed = initialSpeed;
                        transform.localScale = new Vector3(1f, 1f, 1f);
                        this.gameObject.GetComponent<CapsuleCollider>().height = 2.1f;
                        this.gameObject.GetComponent<CapsuleCollider>().center = new Vector3(0f, -0.1f, 0f);
                    }
                    controller.Move(verticalVelocity * Time.deltaTime);
                    if (direction.magnitude >= 0.1f || longJump != 0) {
                        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothVel, smoothTime);
                        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                        if (longJump == 1) {
                            movingDir = moveDir;
                            movingAngle = angle;
                            longJump = 2;
                        } else if (longJump == 2) {
                            moveDir = movingDir;
                            angle = movingAngle;
                        }
                        transform.rotation = Quaternion.Euler(0f, angle, 0f);
                        controller.Move(moveDir.normalized * speed * Time.deltaTime);
                    }
                    if (verticalVelocity.y >= -100) {
                        verticalVelocity[1] += Gravity * Time.deltaTime;
                    }
                    if (getCoinHit.hitCoin) {
                        playerColl.radius = 0.5f;
                        getCoinHit.hitCoin = false;
                    }
                    if (transform.position.y < -10) {
                        FindObjectOfType<GameManager>().GameOver();
                    }
                    if (isOnMoving == 1) {
                        transform.RotateAround(currentGround.gameObject.transform.position, Vector3.up, currentGround.gameObject.GetComponent<Rotate>().angle * Time.deltaTime);
                    }
                    if (isOnMoving == 2) {
                        transform.position += currentGround.gameObject.GetComponent<Move>().last;
                    }
                    if (nearPole && jumpedOff == 0 && verticalVelocity.y != 0) {
                        onPole = true;
                    }
                    if (jumpedOff != 0) {
                        controller.Move(forwardAtJump * (-jumpedOff / 75f) * speed * Time.deltaTime);
                    }
                } else {
                    crouching = false;
                    transform.localScale = new Vector3(1f, 1f, 1f);
                    this.gameObject.GetComponent<CapsuleCollider>().height = 2.1f;
                    this.gameObject.GetComponent<CapsuleCollider>().center = new Vector3(0f, -0.1f, 0f);
                    if (Input.GetKey("space")) {
                        onPole = false;
                        jumpedOff = 100;
                        verticalVelocity = new Vector3(0f, jumpHeight, 0f);
                        controller.Move(transform.forward * (-jumpedOff / 75f) * speed * Time.deltaTime);
                        forwardAtJump = transform.forward;
                        controller.Move(verticalVelocity * Time.deltaTime);
                    } else {
                        float horizontal = Input.GetAxisRaw("Horizontal");
                        float vertical = Input.GetAxisRaw("Vertical");
                        if (vertical < 0) {
                            if (horizontal <= 0) {
                                horizontal = -1;
                            } else {
                                horizontal = 1;
                            }
                        }
                        if (!nearPole && vertical > 0) {
                            vertical = 0;
                        }
                        verticalVelocity = new Vector3(0f, vertical, 0f);
                        controller.Move(verticalVelocity * speed * Time.deltaTime);
                        transform.RotateAround(pole.transform.position, Vector3.up, horizontal * 300 * Time.deltaTime);
                        if (getCoinHit.hitCoin) {
                            playerColl.radius = 0.5f;
                            getCoinHit.hitCoin = false;
                        }
                        if (transform.position.y < -10) {
                            FindObjectOfType<GameManager>().GameOver();
                        }
                    }
                }
            } else {
                crouching = false;
                transform.localScale = new Vector3(1f, 1f, 1f);
                this.gameObject.GetComponent<CapsuleCollider>().height = 2.1f;
                this.gameObject.GetComponent<CapsuleCollider>().center = new Vector3(0f, -0.1f, 0f);
                airUI.SetActive(true);
                if (!firstFrameInWater) {
                    countdown = StartCoroutine("decreaseAir");
                    verticalVelocity *= 0.01f;
                    firstFrameInWater = true;
                    bubbles = Instantiate(BubbleEmitter, transform.position, Quaternion.Euler(-90f, 0f, 0f));
                }
                float horizontal = Input.GetAxisRaw("Horizontal");
                float vertical = Input.GetAxisRaw("Vertical");
                Vector3 direction = new Vector3 (0f, vertical, horizontal).normalized;
                if ((vertical < 0 && transform.rotation.eulerAngles.x < 300 && transform.rotation.eulerAngles.x > 180) || (vertical > 0 && transform.rotation.eulerAngles.x < 180 && transform.rotation.eulerAngles.x > 60)) {
                    vertical = 0;
                }
                transform.Rotate(vertical * Time.deltaTime * 100, 0f, 0f);
                transform.Rotate(0f, horizontal * Time.deltaTime * 300, 0f, Space.World);
                if (Input.GetKey("space") && !swim) {
                    verticalVelocity = transform.forward;
                    swim = true;
                }
                if (!Input.GetKey("space")) {
                    swim = false;
                }
                if (transform.rotation.eulerAngles.x >= 300 && vertical < 0 && Math.Abs(FindObjectOfType<CheckCollisionWithPlayer>().gameObject.transform.position.y + FindObjectOfType<CheckCollisionWithPlayer>().gameObject.GetComponent<BoxCollider>().size.y - transform.position.y) <= 0.2) {
                    verticalVelocity = new Vector3(0f, jumpHeight, 0f);
                }
                controller.Move(verticalVelocity * 1.5f * speed * Time.deltaTime);
                if (getCoinHit.hitCoin) {
                    playerColl.radius = 0.5f;
                    getCoinHit.hitCoin = false;
                }
                if (transform.position.y < -10 || air == 0) {
                    FindObjectOfType<GameManager>().GameOver();
                }
                verticalVelocity *= 0.95f;
                timeInWater++;
            }
        }
        if (onGround || inWater) {
            controller.stepOffset = 0.3f;
        } else {
            controller.stepOffset = 0.001f;
        }
        if (afterReading > 0) {
            afterReading--;
        }
        if (jumpedOff > 0) {
            jumpedOff--;
        }
        nearSign = false;
    }

    IEnumerator decreaseAir () {
        for (int i = 0;i < 8;i++) {
            yield return new WaitForSeconds(5f);
            air--;
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Stage" || other.tag == "Glass" || other.tag == "Button" && !inWater) {
            pound = false;
            speed = initialSpeed;
            Gravity = initialGravity;
            nonGP = false;
            currentGround = other;
            longJump = 0;
            verticalVelocity = new Vector3(0f, 0f, 0f);
            onGround = true;
            if (other.GetComponent<Rotate>() != null) {
                isOnMoving = 1;
            } else if (other.GetComponent<Move>() != null) {
                isOnMoving = 2;
            } else {
                isOnMoving = 0;
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.tag == "Stage" || other.tag == "Glass" || other.tag == "Button") {
            if (verticalVelocity.y < 0) {
                verticalVelocity = new Vector3(0f, 0f, 0f);
            }
            onGround = false;
            isOnMoving = 0;
        }
    }
}
