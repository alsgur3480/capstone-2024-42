using BackEnd;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerCharacterGameData : MonoBehaviour
{
    public string ownerIndate = Backend.UserInDate;
    public int characterID;
    public int characterLevel;
    public int characterExp;
    public int count;

    public PlayerCharacterGameData()
    {

    }

    private void Reset()
    {
    }

    public Param ToParam()
    {
        Param param = new Param();
        param.Add("CharacterLevel", characterLevel);
        param.Add("CharacterExp", characterExp);
        param.Add("CharacterID", characterID);
        param.Add("Count", count);

        return param;
    }

    public void Json_write(LitJson.JsonData gameDataJson)
    {
        ownerIndate = gameDataJson[0]["InDate"].ToString();
        characterLevel = int.Parse(gameDataJson[0]["CharacterLevel"].ToString());
        characterExp = int.Parse(gameDataJson[0]["CharacterExp"].ToString());
        characterID = int.Parse(gameDataJson[0]["CharacterID"].ToString());
        count = int.Parse(gameDataJson[0]["Count"].ToString());
    }
}