using UnityEngine;
using System;

public class GoToPlayer : MonoBehaviour
{
    public Transform player;
    public Transform myPos;
    public Rigidbody me;
    public float speed = 0.1f;
    int mode = 0;
    int time = 0;
    Vector3 home = new Vector3(0f, 0f, 0f);
    Vector3 dest = new Vector3(0f, 0f, 0f);
    Vector3 offset = new Vector3(0f, 0f, 0f);
    bool setDest = false;
    bool posSet = false;
    public float smoothTime = 0.1f;
    float smoothVel;
    System.Random rnd = new System.Random();

    void FixedUpdate()
    {
        if (!posSet) {
            home = new Vector3(myPos.position.x, 0f, myPos.position.z);
            posSet = true;
        }
        if (mode == 0) {
            if (!setDest) {
                dest[0] = home[0] + rnd.Next(-20, 20);
                dest[2] = home[2] + rnd.Next(-20, 20);
                setDest = true;
            } else {
                if (myPos.position.x - dest.x < -0.2) {
                    offset.x = speed * Time.deltaTime;
                } else if (myPos.position.x - dest.x > 0.2) {
                    offset.x = -speed * Time.deltaTime;
                }
                if (myPos.position.z - dest.z < -0.2) {
                    offset.z = speed * Time.deltaTime;
                } else if (myPos.position.z - dest.z > 0.2) {
                    offset.z = -speed * Time.deltaTime;
                }
                transform.position += offset;
                Vector3 direction = new Vector3(offset.x / 2, 0f, offset.z / 2).normalized;
                if (direction.magnitude >= 0.1f) {
                    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothVel, smoothTime);
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);
                }
                offset = new Vector3(0f, 0f, 0f);
                if ((Math.Abs(myPos.position.x - dest.x) <= 5 && Math.Abs(myPos.position.z - dest.z) <= 5) || time > 1500) {
                    setDest = false;
                    offset = new Vector3(0f, 0f, 0f);
                    time = 0;
                }
                if (Math.Abs(myPos.position.x - player.position.x) <= 5 && Math.Abs(myPos.position.z - player.position.z) <= 5) {
                    mode = 1;
                    setDest = false;
                    offset = new Vector3(0f, 0f, 0f);
                }
                time++;
            }
        } else {
            if (myPos.position.x - player.position.x < -0.2) {
                offset.x = 2 * speed * Time.deltaTime;
            } else if (myPos.position.x - player.position.x > 0.2) {
                offset.x = -2 * speed * Time.deltaTime;
            }
            if (myPos.position.z - player.position.z < -0.2) {
                offset.z = 2 * speed * Time.deltaTime;
            } else if (myPos.position.z - player.position.z > 0.2) {
                offset.z = -2 * speed * Time.deltaTime;
            }
            transform.position += offset;
            Vector3 direction = new Vector3(offset.x / 2, 0f, offset.z / 2).normalized;
            if (direction.magnitude >= 0.1f) {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothVel, smoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }
            offset = new Vector3(0f, 0f, 0f);

            if ((Math.Abs(myPos.position.x - player.position.x) > 5 && Math.Abs(myPos.position.z - player.position.z) > 5) || (Math.Abs(myPos.position.x - home.x) > 20 || Math.Abs(myPos.position.z - home.z) > 20)) {
                mode = 0;
                setDest = false;
                offset = new Vector3(0f, 0f, 0f);
                time = 0;
            }
        }
    }
}
