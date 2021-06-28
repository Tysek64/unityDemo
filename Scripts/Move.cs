using System;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float StartX = 0f;
    public float StartY = 0f;
    public float StartZ = 0f;
    public float EndX = 0f;
    public float EndY = 0f;
    public float EndZ = 0f;
    Vector3 offset = new Vector3(0f, 0f, 0f);
    public Vector3 last = new Vector3(0f, 0f, 0f);
    public float speed = 0f;
    public bool DirX = true;
    public bool DirY = true;
    public bool DirZ = true;

    void Update()
    {
        if (StartX != EndX) {
            if (DirX) {
                if (transform.position.x < EndX) {
                    offset.x = speed * Time.deltaTime;
                } else if (transform.position.x >= EndX) {
                    offset.x = -2 * speed * Time.deltaTime;
                    DirX = false;
                }
            } else {
                if (transform.position.x > StartX) {
                    offset.x = -speed * Time.deltaTime;
                } else if (transform.position.x <= StartX) {
                    offset.x = 2 * speed * Time.deltaTime;
                    DirX = true;
                }
            }
        }
        if (StartY != EndY) {
            if (DirY) {
                if (transform.position.y < EndY) {
                    offset.y = speed * Time.deltaTime;
                } else if (transform.position.y >= EndY) {
                    offset.y = -2 * speed * Time.deltaTime;
                    DirY = false;
                }
            } else {
                if (transform.position.y > StartY) {
                    offset.y = -speed * Time.deltaTime;
                } else if (transform.position.y <= StartY) {
                    offset.y = 2 * speed * Time.deltaTime;
                    DirY = true;
                }
            }
        }
        if (StartZ != EndZ) {
            if (DirZ) {
                if (transform.position.z < EndZ) {
                    offset.z = speed * Time.deltaTime;
                } else if (transform.position.z >= EndZ) {
                    offset.z = -2 * speed * Time.deltaTime;
                    DirZ = false;
                }
            } else {
                if (transform.position.z > StartZ) {
                    offset.z = -speed * Time.deltaTime;
                } else if (transform.position.z <= StartZ) {
                    offset.z = 2 * speed * Time.deltaTime;
                    DirZ = true;
                }
            }
        }
        last = offset;
        transform.position += offset;// * Time.deltaTime;
        offset = new Vector3(0f, 0f, 0f);
    }
}
