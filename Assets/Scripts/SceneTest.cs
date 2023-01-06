using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTest : MonoBehaviour {
    public void GoToA() {
        SceneManager.LoadScene("SceneA");
    }

    public void GoToB() {
        SceneManager.LoadScene("SceneB");
    }

    public void GoToMain() {
        SceneManager.LoadScene("SceneMain");
    }
}