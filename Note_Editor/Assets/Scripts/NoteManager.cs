using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using System.IO;
using SFB;
using TMPro;

public class NoteManager : MonoBehaviour
{
    public AudioManager audioManager;
    public List<NoteData> notes = new List<NoteData>();
    public TMP_InputField timeInputField; // TMP 입력 필드에 대한 참조

    public void SaveNotes()
    {
        var path = StandaloneFileBrowser.SaveFilePanel("Save Notes", "", "notes", "json");
        if (!string.IsNullOrEmpty(path))
        {
            string json = JsonUtility.ToJson(new NoteDataWrapper { Notes = notes }, true);
            File.WriteAllText(path, json);
        }
    }
    public void DeleteNotesButtonPressed()
    {
        if (float.TryParse(timeInputField.text, out float seconds))
        {
            float currentTime = audioManager.GetMusicTime(); // 현재 음악 시간을 가져옵니다.
            float newTime = Mathf.Max(0, currentTime - seconds); // 음악 시간에서 입력된 시간을 빼서 새로운 시간을 계산합니다.

            // 음악의 재생 위치를 새로운 시간으로 설정합니다.
            audioManager.SetMusicTime(newTime);

            // 새로운 시간과 현재 시간 사이에 생성된 노트들을 삭제합니다.
            notes.RemoveAll(note => note.time >= newTime && note.time <= currentTime);
        }
        else
        {
            Debug.LogError("Invalid input. Please enter a valid number.");
        }
    }
    // 시간 입력에 따라 음악을 되감고, 해당 시간 동안 생성된 노트들을 삭제
    public void RewindAndDeleteNotes(float seconds)
    {
        audioManager.RewindMusicBy(seconds); // 음악 되감기

        float currentTime = audioManager.GetMusicTime(); // 현재 음악 시간 가져오기
        notes.RemoveAll(note => note.time >= currentTime); // 현재 시간 이후에 생성된 노트들 삭제
    }
   

    // 방향 정보를 포함하여 노트를 추가하는 메서드
    public void AddNote(float time, string type, Vector3 position, Vector3 direction)
    {
        notes.Add(new NoteData(time, type, position, direction));
    }

    [System.Serializable]
    public class NoteDataWrapper
    {
        public List<NoteData> Notes;
    }
}
