using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiffListBtn : MonoBehaviour {
    private Button btn;

    public void Init() {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() => {
            Diff tDiff;
            System.Enum.TryParse<Diff>(transform.gameObject.name, out tDiff);
            GameManager.Instance.SetChoosenDiff(tDiff);
        });
    }

    public void Active() {
        this.gameObject.SetActive(true);
    }

    public void DeActive() {
        this.gameObject.SetActive(false);
    }
}
