using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SongList : MonoBehaviour
{
    public int Song_Count = 10;
    public GameObject[] song_object = new GameObject[10];

    public TMP_Text[] list_name;
    public TMP_Text[] list_difficulty;
    public Image list_image;

    void Start() // �� ���� â���� �ڽ��� ������ ��鸸 �����ִ� ���
    {
        int a;
        for (int i = 0; i < Song.user_song.Length ; i++)
        {
            a = Song.user_song[i];

            list_name[a].text = Song.s_name[a];
            list_difficulty[a].text = Song.difficulty[a];
            song_object[a].gameObject.SetActive(true);

        }
    }
}
