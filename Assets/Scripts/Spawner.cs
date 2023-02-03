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
    private Dead_UI DeadUI = null;

    Cube blueCube = null;
    Cube blueDotCube = null;
    Cube redCube = null;
    Cube redDotCube = null;
    Cube Bomb = null;

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

        DeadUI = FindObjectOfType<Dead_UI>(true);
        DeadUI.Init();
        DeadUI.DeActive();

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
        StartCoroutine(IEMakeCube(info._notes[curIndex++]));
    }

    private void SongStart() {
        Invoke(nameof(PlaySong), 0.2f);
        songStart = true;
    }

    public void SongStop() {
        _audio.Stop();
        songEnd = true;
        DeadUI.Active();
        print("Die");
    }

    private void PlaySong() {
        _audio.Play();
    }

    IEnumerator IEMakeCube(Note note) {
        yield return null;
        int type = note._type;
        int lineIndex = note._lineIndex;
        int lineLayer = note._lineLayer;
        int cutDir = note._cutDirection;

        Cube pref = null;
        switch (type) {
            case 0:
                if (cutDir == 8) {
                    if (redDotCube == null) { redDotCube = Resources.Load<Cube>("Prefabs/RedCubeDotFBX"); }
                    pref = redDotCube;
                }
                else {
                    if (redCube == null) { redCube = Resources.Load<Cube>("Prefabs/RedCubeFBX"); }
                    pref = redCube;
                }
                break;
            case 1:
                if (cutDir == 8) {
                    if (blueDotCube == null) { blueDotCube = Resources.Load<Cube>("Prefabs/BlueCubeDotFBX"); }
                    pref = blueDotCube;
                }
                else {
                    if (blueCube == null) { blueCube = Resources.Load<Cube>("Prefabs/BlueCubeFBX"); }
                    pref = blueCube;
                }
                break;
            case 3:
                if (Bomb == null) { Bomb = Resources.Load<Cube>("Prefabs/BombFBX"); }
                pref = Bomb;
                break;
        }
        if (pref == null) {
            print("No Prefab");
            yield break;
        }
        Cube cube = Instantiate(pref, pointDic[lineLayer][lineIndex]);
        //cube.transform.localPosition = Vector3.zero;
        cube.moveSpeed = cubeSpeed;
        cube.transform.Rotate(transform.forward, direction[cutDir]);
        cube.isDot = cutDir == 8;
    }
}
