using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class User : MonoBehaviour
{
    public TMP_Text Text_user_name;
    public TMP_Text Text_user_level;
    public TMP_Text Text_gold;

    public int uid = 1000;              // ���� UID
    public string user_name = "min";    // ������ ������ �̸�
    public static int character = 0;    // ������ ������ ���� ĳ����
    public int user_level = 5;          // ������ ����
    public int exp = 0;                 // ���� ����ġ
    public int gold = 10000;            // ������ ������ ��ȭ
    public int ranking = 10;            // ���� ��ŷ
    public int score = 999;             // ������ Ŭ������ �� ������ ��

    public static int[] song = { 1, 1, 1, 1, 1, 0, 0, 0, 0, 0 }; // ������ ������ �� (1�� ����)
    public int[] clear_song = { 1, 0, 1, 0, 1, 0, 0, 0, 0, 0 }; // �ش� ���� Ŭ���� ���� (1�� Ŭ����)
    public int[] score_song = { 999, 0, 1999, 0, 2999, 0, 0, 0, 0, 0 }; // �ش� ��� ���� �ְ� ����

    // Start is called before the first frame update
    void Start()
    {
        Text_user_level.text = user_level.ToString();
        Text_gold.text = gold.ToString();
        Text_user_name.text = user_name.ToString();
    }

    public void touch_character_select_button(int a)
    {
        character = a;
    }
}
