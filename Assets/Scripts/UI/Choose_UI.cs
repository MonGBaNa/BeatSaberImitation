using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Choose_UI : MonoBehaviour {
    private Transform songList;
    private Transform diffList;

    private SongListBtn[] songBtnList = new SongListBtn[]{ };
    private DiffListBtn[] diffBtnList = new DiffListBtn[]{ };

    private Text choosenSong;
    private Text choosenDiff;

    private Button playBtn;

    public void Init() {
        songList = transform.Find("SongList");
        songBtnList = songList.GetComponentsInChildren<SongListBtn>(true);
        foreach(var btn in songBtnList) { btn.Init(); }

        diffList = transform.Find("DiffList");
        diffList.gameObject.SetActive(false);
        diffBtnList = diffList.GetComponentsInChildren<DiffListBtn>(true);
        foreach (var btn in diffBtnList) { btn.Init(); }

        choosenSong = transform.Find("ChoosenSong").GetComponent<Text>();
        choosenDiff = transform.Find("ChoosenDiff").GetComponent<Text>();

        playBtn = transform.Find("PlayBtn").GetComponent<Button>();
        playBtn.onClick.AddListener(() => {
            SceneManager.LoadScene("GameScene");
        });
        playBtn.gameObject.SetActive(false);
    }

    private void Start() {
        Init();
    }

    private void Update() {
        SetChoosenSong();
        SetChoosenDiff();
        ShowDiffList();
        ShowPlayBtn();
    }

    void ShowPlayBtn() {
        if(GameManager.Instance.ChoosenDiff != null) { playBtn.gameObject.SetActive(true); }
        else                                         { playBtn.gameObject.SetActive(false); }
    }

    void SetChoosenSong() {
        string str = "선택한 곡 : ";
        if (GameManager.Instance.ChoosenSong != null) {
            str += GameManager.Instance.ChoosenSong;
        }
        choosenSong.text = str;
    }

    void SetChoosenDiff() {
        string str = "선택한 난이도 : ";
        if (GameManager.Instance.ChoosenDiff != null) {
            if(GameManager.Instance.ChoosenDiff == Diff.ExpertPlus) { str += "Expert+"; }
            else                                                    { str += GameManager.Instance.ChoosenDiff.ToString(); }
        }
        choosenDiff.text = str;
    }

    void ShowDiffList() {
        if (GameManager.Instance.ChoosenSong == null) return;
        SongInfo info = DatParsing.GetSongInfo(GameManager.Instance.ChoosenSong);

        foreach (var btn in diffBtnList) { btn.DeActive(); }
        foreach (var btn in diffBtnList) {
            foreach (var set in info._difficultyBeatmapSets[0]._difficultyBeatmaps) {
                if (set._difficulty == btn.name) {
                    btn.Active();
                }
            }
        }

        diffList.gameObject.SetActive(true);
    }
}
