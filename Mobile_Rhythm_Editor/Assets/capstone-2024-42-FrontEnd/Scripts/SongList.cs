using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongList : MonoBehaviour
{
    public int Song_Count = 10;
    public GameObject [] Song = new GameObject [10];
   
    void Start() // �� ���� â���� �ڽ��� ������ ��鸸 �����ִ� ���
    {
        for (int i = 0; i < Song_Count ; i++)
        {
            if (User.song[i] == 0)
            {
                Song[i].gameObject.SetActive(false);
            }
        }
    }
}
