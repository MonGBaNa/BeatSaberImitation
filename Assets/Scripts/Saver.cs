using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saver : MonoBehaviour {

    public LayerMask targetLayer;
    private Vector3 prevPos;

    private void Start() {
        prevPos = transform.position;
    }

    void Update() {
        if(Physics.Raycast(transform.position, transform.forward,out RaycastHit hit, 2, targetLayer)) {
            if(Vector3.Angle(transform.position - prevPos,hit.transform.up) > 130 && Vector3.Magnitude(transform.position - prevPos) > 0.05f) {
                Destroy(hit.transform.gameObject);
            }
            else {
                if(hit.collider.gameObject.GetComponent<Cube>().isDot) {
                    Destroy(hit.transform.gameObject);
                }
            }
        }
        prevPos = transform.position;
    }
}