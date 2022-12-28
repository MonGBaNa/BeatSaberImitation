using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Newtonsoft.Json;

[System.Serializable]
public class Note {
    public double _time;
    public int _lineIndex;
    public int _lineLayer;
    public int _type;
    public int _cutDirection;
}

[System.Serializable]
public class DatInfo {
    public List<Note> _notes;
}

[System.Serializable]
public class DifficultyBeatmapSet {
    public List<DifficultyBeatmap> _difficultyBeatmaps;

    /*
        Easy
        Normal
        Hard
        Expert
        Expert+
     */
    public int GetDifficultyMoveSpeed(string difficulty) {
        for(int i=0; i<_difficultyBeatmaps.Count; i++) {
            if (this._difficultyBeatmaps[i]._difficulty == difficulty) {
                return _difficultyBeatmaps[i]._noteJumpMovementSpeed;
            }
        }
        return -1;
    }
}

[System.Serializable]
public class DifficultyBeatmap {
    public string _difficulty;
    public int _noteJumpMovementSpeed;
}

[System.Serializable]
public class SongInfo {
    public string _beatsPerMinute;
    public List<DifficultyBeatmapSet> _difficultyBeatmapSets;
}

public class DatParsing : MonoBehaviour {

    public DatInfo GetInfo(string SongName, Diff difficulty) {
        Stream readStream = new FileStream(Application.dataPath + $"/Resources/Data/{SongName}/Map/{difficulty}.json", FileMode.Open);
        byte[] data = new byte[readStream.Length];
        readStream.Read(data, 0, data.Length);
        readStream.Close();

        string JsonData = Encoding.UTF8.GetString(data);
        DatInfo info = JsonConvert.DeserializeObject<DatInfo>(JsonData);
        return info;
    }

    public SongInfo GetSongInfo(string SongName) {
        Stream readStream = new FileStream(Application.dataPath + $"/Resources/Data/{SongName}/Info.json", FileMode.Open);
        byte[] data = new byte[readStream.Length];
        readStream.Read(data, 0, data.Length);
        readStream.Close();

        string JsonData = Encoding.UTF8.GetString(data);
        SongInfo info = JsonConvert.DeserializeObject<SongInfo>(JsonData);
        return info;
    }
}