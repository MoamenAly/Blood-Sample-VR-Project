using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(menuName = "audio data",fileName = "audio_locliztion_file",order = 1)]
public class AudioLocoliztion : ScriptableObject
{
    [TableList(AlwaysExpanded = true), Searchable]
    public AudioData [] audioNarrations;

    Dictionary<Narrations, AudioData> values = new();

    [Button]
    private void Fetch()
    {

        UpdateBackup();

        var enumType = typeof(Narrations);
        var enumMembers = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);

        int numOfIds = enumMembers.Length;

        AudioData[] temp = new AudioData[numOfIds];

        for (int i = 0; i < temp.Length; i++)
        {
          
                temp[i] = new AudioData
                {
                    narrationsId = (Narrations)Enum.Parse(typeof(Narrations), enumMembers[i].Name)
                };


            if (values.ContainsKey(temp[i].narrationsId))
            {
                temp[i].audioClips = values[temp[i].narrationsId].audioClips;
                //temp[i].AudioName = values[temp[i].narrationsId].a;
            }

        }
        audioNarrations = temp;
    }

    void UpdateBackup()
    {

        values = new Dictionary<Narrations, AudioData>();
        for (int i = 0; i < audioNarrations.Length; i++)
        {
            values.Add(audioNarrations[i].narrationsId, audioNarrations[i]);
        }
    }

}

[System.Serializable]
public class AudioData {
    [TableColumnWidth(150)]
    [ReadOnly] public Narrations narrationsId;
    [TableColumnWidth(500, Resizable = true)]
    [Space] public List<AudioClip> audioClips;

    private string AudioName => narrationsId.ToString();

    public bool Compare(int id)
    {
        if ((int)narrationsId == id) return true;
        else return false;
    }
}
