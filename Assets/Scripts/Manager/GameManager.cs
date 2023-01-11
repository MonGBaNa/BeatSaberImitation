using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private static GameManager instance;
    public static GameManager Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<GameManager>(true);
                if (instance == null) {
                    GameObject go = new GameObject(nameof(GameManager), typeof(GameManager));
                    instance = go.GetComponent<GameManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return instance;
        }
    }

    private static SongInfo curSongInfo = null;
    public static SongInfo GetCurSongInfo() {
        return curSongInfo;
    }

    private string choosenSong = null;
    public string ChoosenSong => choosenSong;
    //public void SetChoosenSong(string songName) => choosenSong = songName;
    public void SetChoosenSong(string songName) {
        choosenSong = songName;
        Debug.Log(choosenSong);
    }

    private Diff? choosenDiff = null;
    public Diff? ChoosenDiff => choosenDiff;
    //public void SetChoosenDiff(Diff? diff) => choosenDiff = diff;
    public void SetChoosenDiff(Diff? diff) {
        choosenDiff = diff;
        Debug.Log(choosenDiff);
    }
}
