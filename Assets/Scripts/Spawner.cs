using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum Diff {
    Easy,
    Normal,
    Hard,
    Expert,
    ExpertPlus
}

[System.Serializable]
public class NoteInfo {
    public double time;
    public int index;
    public int layer;
    public int type;
    public int direction;
}

public class Spawner : MonoBehaviour {
    public DatInfo info;

    public AudioSource _audio;

    public string songName = "";
    public Diff difficulty = Diff.Easy;

    public double bpm;
    public int cubeSpeed = 0;

    [Header("테스트 스폰")]
    public int layer = 0;
    public int index = 0;
    public int dir = 0;

    public SongInfo songInfo;

    private Dictionary<int, List<Transform>> pointDic = new Dictionary<int, List<Transform>>();
    private int[] direction = new int[]{180, 0, -90, 90, -135, 135, -45, 45,0};

    private double timer = 0f;
    private int curIndex = 0;

    bool songStart = false;
    bool songEnd = false;

    private End_UI EndUI = null;

    private void Start() {
        Invoke(nameof(Init), 0.1f);
    }

    public void Init() {
        PointDicInit();
        songName = GameManager.Instance.ChoosenSong;
        difficulty = (Diff)GameManager.Instance.ChoosenDiff;

        songInfo = DatParsing.GetSongInfo(songName);
        double.TryParse(songInfo._beatsPerMinute, out bpm);
        if(bpm == -1) {
            throw new MissingReferenceException("Cant Find BPM");
        }

        info = DatParsing.GetInfo(songName, difficulty);
        cubeSpeed = songInfo._difficultyBeatmapSets[0].GetDifficultyMoveSpeed(difficulty.ToString());
        AudioClip clip = Resources.Load<AudioClip>($"Musics/{songName}");
        transform.Find("Music").GetComponent<AudioSource>().clip = clip;

        EndUI = FindObjectOfType<End_UI>(true);
        EndUI.Init();
        EndUI.DeActive();

        Invoke(nameof(SongStart), 0.2f);
    }

    [ContextMenu("SpawnTest")]
    public void SpawnTest() {
        Cube pref = null;
        if (Random.Range(0, 2) == 1) {
            pref = Resources.Load<Cube>("Prefabs/BlueCube");
        }
        else {
            pref = Resources.Load<Cube>("Prefabs/RedCube");
        }
        if (pref == null) return;
        Cube cube = Instantiate(pref, pointDic[layer][index]);
        cube.transform.localPosition = Vector3.zero;
        cube.transform.Rotate(transform.forward, direction[dir]);
    }

    void Update() {
        if (!songStart || songEnd) return;
        if (curIndex == info._notes.Count) {
            if (!_audio.isPlaying) {
                enabled = false;
                songEnd = true;
                print("출력 끝");
                EndUI.Active();
                return;
            }
        }
        if (curIndex != info._notes.Count && timer >= info._notes[curIndex]._time * 60d / bpm) { CreateCube(); }
        //switch (mode) {
        //    case Mode.Beat:
        //        if(timer >= info._notes[curIndex]._time * 60d / bpm) { CreateCube(); }
        //        break;
        //    case Mode.Second:
        //        if (timer >= info._notes[curIndex]._time) { CreateCube(); }
        //        break;
        //}
        timer += Time.deltaTime;
    }

    private void PointDicInit() {
        pointDic.Clear();
        for (int i = 0; i < 3; i++) {
            Transform curLayer = transform.Find("Layer " + i);
            if (curLayer == null) {
                print("Layer " + i + "가 없습니다.");
                return;
            }
            List<Transform> tList = new List<Transform>();
            for (int j = 0; j < 4; j++) {
                Transform curIndex = curLayer.Find("Index " + j);
                if (curIndex == null) {
                    print("Index " + j + "가 없습니다.");
                    return;
                }
                tList.Add(curIndex);
            }
            pointDic.Add(i, tList);
        }
    }

    private void CreateCube() {
        int type = info._notes[curIndex]._type;
        int lineIndex = info._notes[curIndex]._lineIndex;
        int lineLayer = info._notes[curIndex]._lineLayer;
        int cutDir = info._notes[curIndex]._cutDirection;
        Cube pref = null;
        switch (type) {
            case 0:
                if (cutDir == 8) { pref = Resources.Load<Cube>("Prefabs/RedCubeDotFBX"); }
                else { pref = Resources.Load<Cube>("Prefabs/RedCubeFBX"); }
                break;
            case 1:
                if (cutDir == 8) { pref = Resources.Load<Cube>("Prefabs/BlueCubeDotFBX"); }
                else { pref = Resources.Load<Cube>("Prefabs/BlueCubeFBX"); }
                break;
            case 3:
                pref = Resources.Load<Cube>("Prefabs/BombFBX");
                break;
        }
        if (pref == null) {
            print("No Prefab");
            return;
        }

        Cube cube = Instantiate(pref, pointDic[lineLayer][lineIndex]);
        cube.moveSpeed = cubeSpeed;
        cube.transform.localPosition = Vector3.zero;
        cube.transform.Rotate(transform.forward, direction[cutDir]);
        cube.isDot = cutDir == 8;
        curIndex++;
    }

    private void SongStart() {
        Invoke(nameof(PlaySong), 0.2f);
        songStart = true;
    }

    private void PlaySong() {
        _audio.Play();
    }
}
