using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saver : MonoBehaviour {
    [Tooltip("잘린 단면용 매터리얼")]
    public Material mat;
    public LayerMask targetLayer;

    private Vector3 prevPos;

    private Transform _tip;
    private Transform _base;

    private Vector3 _triggerEnterTipPosition;
    private Vector3 _triggerEnterBasePosition;
    private Vector3 _triggerExitTipPosition;

    private GameObject[] slices;

    private bool canCut = false;

    private void Start() {
        prevPos = transform.position;
        _tip = transform.Find("Tip");
        _base = transform.Find("Base");
    }

    void Update() {
        if(Physics.Raycast(transform.position, transform.forward,out RaycastHit hit, 2, targetLayer)) {
            if (Vector3.Angle(transform.position - prevPos,hit.transform.up) > 130 && Vector3.Magnitude(transform.position - prevPos) > 0.05f) {
                //Destroy(hit.transform.gameObject);
                canCut = true;
            }
            else {
                if(hit.collider.gameObject.GetComponent<Cube>().isDot) {
                    //Destroy(hit.transform.gameObject);
                    canCut = true;
                }
            }
        }
        prevPos = transform.position;
    }

    private void OnTriggerEnter(Collider other) {
        _triggerEnterTipPosition = _tip.position;
        _triggerEnterBasePosition = _base.position;
    }

    private void OnTriggerExit(Collider other) {
        _triggerExitTipPosition = _tip.position;

        Vector3 side1 = _triggerExitTipPosition - _triggerEnterTipPosition;
        Vector3 side2 = _triggerExitTipPosition - _triggerEnterBasePosition;

        Vector3 normal = Vector3.Cross(side1, side2).normalized;

        Vector3 transformedNormal = ((Vector3)(other.gameObject.transform.localToWorldMatrix.transpose * normal)).normalized;

        Vector3 transformedStartingPoint = other.gameObject.transform.InverseTransformPoint(_triggerEnterTipPosition);

        if (canCut) {
            //0번 a, 1번 b
            slices = MeshSlice.Slicer(other.gameObject, transformedNormal, transformedStartingPoint, mat);
            slices[0].GetComponent<Rigidbody>().AddForce(-normal * 3f, ForceMode.Impulse);
            slices[1].GetComponent<Rigidbody>().AddForce(normal * 3f, ForceMode.Impulse);
            Destroy(slices[0], 1.5f);
            Destroy(slices[1], 1.5f);
        }
        canCut = false;
    }
}