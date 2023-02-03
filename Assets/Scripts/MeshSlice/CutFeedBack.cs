using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutFeedBack : MonoBehaviour {

    private ParticleSystem blue, red;

    private void Start() {
        blue = transform.Find("BlueParticle").GetComponent<ParticleSystem>();
        red = transform.Find("RedParticle").GetComponent<ParticleSystem>();
    }

    public void FeedBack(bool isRed) {
        if (isRed) {
            red.Play();
            print("red");
        }
        else {
            blue.Play();
            print("blue");
        }
    }
}
