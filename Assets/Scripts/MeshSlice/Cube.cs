using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {

    public float moveSpeed = 2;
    public bool isDot = false;

    private bool isMove = false;
    private Vector3 originPos;
    private float targetTime = 0.2f;

    private void Start() {
        originPos = transform.localPosition;
        StartCoroutine(MoveToStartPos());
    }

    void Update() {
        Debug.DrawRay(transform.position, transform.up);
        if (!isMove) return;
        transform.position += Time.smoothDeltaTime * -transform.forward * moveSpeed;
        if(transform.position.z < -5) {
            Destroy(gameObject);
        }
    }

    IEnumerator MoveToStartPos() {
        float elapsed = 0;
        while (true) {
            elapsed += Time.deltaTime;
            float t = elapsed / targetTime;
            transform.localPosition = Vector3.Lerp(originPos, originPos - transform.forward * 18, t);
            if (t >= 1) break;
            yield return null;
        }
        isMove = true;
    }

    public void Boom() {
        GetComponent<ParticleSystem>().Play();
    }
}
