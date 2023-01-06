using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class End_UI : MonoBehaviour {
    private Button toChooseScene;

    public void Init() {
        toChooseScene = transform.Find("BG/ToChooseScene").GetComponent<Button>();
        toChooseScene.onClick.AddListener(() => {
            GameManager.Instance.SetChoosenDiff(null);
            GameManager.Instance.SetChoosenSong(null);
            SceneManager.LoadScene("ChooseScene");
        });
    }

    public void Active() {
        gameObject.SetActive(true);
    }   

    public void DeActive() {
        gameObject.SetActive(false);
    }
}