using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {

    public float moveSpeed = 2;
    public bool isDot = false;

    void Update() {
        Debug.DrawRay(transform.position, transform.up);
        transform.position += Time.deltaTime * -transform.forward * moveSpeed;
        if(transform.position.z < -5) {
            Destroy(gameObject);
        }
    }
}
