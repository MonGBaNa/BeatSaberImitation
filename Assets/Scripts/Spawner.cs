using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum Mode {
    Beat,
    Second
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
    public DatParsing parse;
    public DatInfo info;
    public AudioSource _audio;

    public string songName = "";
    //public Mode mode = Mode.Beat;
    public double bpm;
    public int cubeSpeed = 0;

    [Header("테스트 스폰")]
    public int layer = 0;
    public int index = 0;
    public int dir = 0;

    private Dictionary<int, List<Transform>> pointDic = new Dictionary<int, List<Transform>>();
    private int[] direction = new int[]{180, 0, -90, 90, -135, 135, -45, 45,0};

    private double timer = 0f;
    private int curIndex = 0;

    bool songStart = false;

    public void Start() {
        PointDicInit();
        info = parse.GetInfo(songName);
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
        if (!songStart) return;
        if (curIndex > info._notes.Count - 1) {
            enabled = false;
            print("출력 끝");
            return;
        }
        if (timer >= info._notes[curIndex]._time * 60d / bpm) { CreateCube(); }
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
                if (cutDir == 8) { pref = Resources.Load<Cube>("Prefabs/RedCubeDot"); }
                else { pref = Resources.Load<Cube>("Prefabs/RedCube"); }
                break;
            case 1:
                if (cutDir == 8) { pref = Resources.Load<Cube>("Prefabs/BlueCubeDot"); }
                else { pref = Resources.Load<Cube>("Prefabs/BlueCube"); }
                break;
            case 3:
                pref = Resources.Load<Cube>("Prefabs/Bomb");
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
        _audio.Play();
        songStart = true;
    }
}
