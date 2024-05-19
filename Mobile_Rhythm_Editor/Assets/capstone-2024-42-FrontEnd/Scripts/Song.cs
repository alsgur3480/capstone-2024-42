using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Song : MonoBehaviour
{
    public static string[] s_name = {"AAA", "BBB", "CCC", "DDD", "EEE", "FFF", "GGG", "HHH", "III", "JJJ"};
    public static string[] artist = { "kang", "kim", "lee", "choi", "soe", "han", "jung", "yang", "lim", "ko" };
    public static string[] difficulty = { "Easy", "Normal", "Hard", "Easy", "Normal", "Hard", "Easy", "Normal", "Hard", "Easy", };

    public static int user_song_count = 5;
    public static int[] user_song = {0, 1, 2, 3, 4, 0, 0, 0, 0, 0}; // ������ ������ �� ����Ʈ
    public int[] clear; // �� Ŭ���� ����Ʈ (1�̸� Ŭ����)
    public static int[] score = { 100000, 0, 99999, 12345, 0, 0, 0, 0, 0, 0 }; // Ŭ���� �� ���� �ִ� ����
    public int[] combo; // Ŭ���� �� ���� �ִ� �޺� ��

    public void user_song_sort()
    {
        Array.Sort(user_song, 0, user_song_count - 1);
    }
}
