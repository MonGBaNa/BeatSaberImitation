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

public class DatParsing : MonoBehaviour {

    public DatInfo GetInfo(string SongName) {
        Stream readStream = new FileStream(Application.dataPath + $"/Resources/Data/{SongName}.json", FileMode.Open);
        byte[] data = new byte[readStream.Length];
        readStream.Read(data, 0, data.Length);
        readStream.Close();

        string JsonData = Encoding.UTF8.GetString(data);
        DatInfo info = JsonConvert.DeserializeObject<DatInfo>(JsonData);
        return info;
    }
}