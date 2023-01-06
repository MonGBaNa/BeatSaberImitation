using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongListBtn : MonoBehaviour {

    private Button btn;
    public void Init() {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() => {
            GameManager.Instance.SetChoosenSong(this.gameObject.name);
            GameManager.Instance.SetChoosenDiff(null);
        });
    }
}