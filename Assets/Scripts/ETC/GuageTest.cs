using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuageTest : MonoBehaviour {
    public Image guage;
    public Text hitCountText;

    int hitCount = 0;
    Spawner spawner = null;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Red") || other.gameObject.layer == LayerMask.NameToLayer("Blue")) {
#if !DEVELOPMENT_BUILD
            Damaged();
            print("Damaged");
#endif
        }
    }

    public void Damaged() {
        guage.fillAmount -= 0.02f;
        if (guage.fillAmount <= 0) {
            guage.fillAmount = 0;
            if (spawner == null)
                spawner = FindObjectOfType<Spawner>();
            spawner.SongStop();
        }
        hitCount = 0;
        hitCountText.text = "Hit : 0";
    }

    public void Hit() {
        hitCount++;
        hitCountText.text = $"Hit : {hitCount}";
        if (hitCount % 10 == 0) {
            guage.fillAmount += 0.1f;
            if (guage.fillAmount >= 1)
                guage.fillAmount = 1;
        }
    }
}