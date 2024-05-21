using UnityEngine;
using BackEnd;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms;
using JetBrains.Annotations;
using UnityEngine.TextCore.Text;
using Unity.VisualScripting;

public class BackendGameData
{
    [System.Serializable]
    public class GameDataLoadEvent : UnityEvent { }
    public GameDataLoadEvent onGameDataLoadEvent = new GameDataLoadEvent();

    private static BackendGameData instance = null;
    public static BackendGameData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BackendGameData();
            }

            return instance;
        }
    }

    private UserGameData userGameData = new UserGameData();
    private List<PlayerSongGameData> playerSongGameData = new List<PlayerSongGameData>();
    private List<PlayerCharacterGameData> playerCharacterGameData = new List<PlayerCharacterGameData>();

    public UserGameData UserGameData => userGameData;
    public List<PlayerSongGameData> PlayerSongGameData => playerSongGameData;
    public List<PlayerCharacterGameData> PlayerCharacterGameData => playerCharacterGameData;

    private string UserDataRowInDate = string.Empty;
    private string SongDataRowInDate = string.Empty;
    private string CharacterDataRowInDate = string.Empty;

    public void UserDataInsert()
    {
        UserGameData gameData = new UserGameData();

        Param param = gameData.ToParam();

        Backend.GameData.Insert("User", param, callback =>
        {
            if (callback.IsSuccess())
            {
                UserDataRowInDate = callback.GetInDate();
                Debug.Log("�� User Info�� indate : " + UserDataRowInDate);
            }
            else
            {
                Debug.LogError("���� ���� ���� ���� ���� : " + callback.ToString());
                if (callback.IsClientRequestFailError())
                {
                    Debug.Log("��Ʈ��ũ�� �Ͻ������� �������� ���");
//                    UserDataInsert();
                }
            }
        });
    }

    public void UserDataLoad()
    {
        Backend.GameData.GetMyData("User", new Where(), call_back =>
        {
            if (call_back.IsSuccess())
            {
                Debug.Log(call_back.ToString());

                try
                {
                    LitJson.JsonData gameDataJson = call_back.FlattenRows();

                    if (gameDataJson.Count <= 0)
                    {
                        Debug.Log("�����Ͱ� �������� �ʽ��ϴ�");
                        return;
                    }
                    else
                    {
                        UserDataRowInDate = gameDataJson[0]["inDate"].ToString();
                        userGameData.Json_write(gameDataJson);

                        onGameDataLoadEvent?.Invoke();

                        Debug.Log($"���� ���� ������ �ε� ����");
                    }
                }
                catch (System.Exception e)
                {
                    userGameData.Reset();
                    Debug.LogError(e);
                }
            }
            else
            {
                Debug.Log($"���� ���� ������ �ε� ����");
            }
        });
    }

    public void UserDataUpdate(UnityAction action=null)
    {
        if(userGameData == null)
        {
            Debug.Log("Insert or Load ���� �ʿ���");
            return;
        }

        Param param = userGameData.ToParam();

        if(string.IsNullOrEmpty(UserDataRowInDate))
        {
            Debug.LogError("������ inDate ������ X");
        }
        else
        {
            Debug.Log($"{UserDataRowInDate}�� ���� ���� ������ ���� ��û");

            Backend.GameData.UpdateV2("User", UserDataRowInDate, Backend.UserInDate, param, callback =>
            {
                if(callback.IsSuccess())
                {
                    Debug.Log($"���� ���� ������ ���� ���� : {callback}");

                    action?.Invoke();

//                    BackendGameData.Instance.UserDataUpdate();
                }
                else
                {
                    Debug.Log($"���� ���� ������ ���� ���� : {callback}");
                    if (callback.IsClientRequestFailError())
                    {
                        Debug.Log("��Ʈ��ũ�� �Ͻ������� �������� ���");
                        UserDataUpdate(action);
                    }
                }
            });
        }
    }

    public void PlayerSongDataInsert(int songID)
    {
        PlayerSongGameData gameData = new PlayerSongGameData();
        gameData.songID = songID;
        Param param = gameData.ToParam();

        Backend.GameData.Insert("PlayerSong", param, callback =>
        {
            if (callback.IsSuccess())
            {
                SongDataRowInDate = callback.GetInDate();
                Debug.Log("�� Player Song Info�� indate : " + SongDataRowInDate);
            }
            else
            {
                Debug.LogError("�� ���� ���� ���� ���� : " + callback.ToString());
                if (callback.IsClientRequestFailError())
                {
                    Debug.Log("��Ʈ��ũ�� �Ͻ������� �������� ���");
                    //                    UserDataInsert();
                }
            }
        });
    }

    public void PlayerSongDataLoad(int songID)
    {
        Where where = new Where();
        where.Equal("SongID", songID);

        Backend.GameData.GetMyData("PlayerSong", where, call_back =>
        {
            if (call_back.IsSuccess())
            {
                Debug.Log(call_back.ToString());

                try
                {
                    LitJson.JsonData gameDataJson = call_back.FlattenRows();

                    if (gameDataJson.Count <= 0)
                    {
                        Debug.Log("�����Ͱ� �������� �ʽ��ϴ�");
                        return;
                    }
                    else
                    {
                        SongDataRowInDate = gameDataJson[0]["inDate"].ToString();
                        PlayerSongGameData SongDataRow = new PlayerSongGameData();
                        SongDataRow.Json_write(gameDataJson);
                        playerSongGameData.Add(SongDataRow);

                        onGameDataLoadEvent?.Invoke();

                        Debug.Log($"{songID}��° ���� �� ���� ������ �ε� ����");
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e);
                }
            }
            else
            {
                Debug.Log($"���� �� ���� ������ �ε� ����");
            }
        });
    }

    public void PlayerSongDataUpdate(int songID, UnityAction action = null)
    {
        if (playerSongGameData == null)
        {
            Debug.Log("Insert or Load ���� �ʿ���");
            return;
        }

        Param param = new Param();

        for (int i = 0; i < playerSongGameData.Count; i++)
        {
            if (playerSongGameData[i].songID == songID)
            {
                param = playerSongGameData[i].ToParam();
            }
        }

        if (string.IsNullOrEmpty(SongDataRowInDate))
        {
            Debug.LogError("���� ���� inDate ������ X");
        }
        else
        {
            Debug.Log($"{SongDataRowInDate}�� ���� �� ���� ������ ���� ��û");

            Backend.GameData.UpdateV2("PlayerSong", SongDataRowInDate, Backend.UserInDate, param, callback =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log($"���� �� ���� ������ ���� ���� : {callback}");

                    action?.Invoke();
                }
                else
                {
                    Debug.Log($"���� �� ���� ������ ���� ���� : {callback}");
                    if (callback.IsClientRequestFailError())
                    {
                        Debug.Log("��Ʈ��ũ�� �Ͻ������� �������� ���");
                        PlayerSongDataUpdate(songID, action);
                    }
                }
            });
        }
    }

    public void PlayerCharacterDataInsert(int CharacterID)
    {
        PlayerCharacterGameData gameData = new PlayerCharacterGameData();
        gameData.characterID = CharacterID;
        Param param = gameData.ToParam();

        Backend.GameData.Insert("PlayerCharacter", param, callback =>
        {
            if (callback.IsSuccess())
            {
                CharacterDataRowInDate = callback.GetInDate();
                Debug.Log("�� Player Character Info�� indate : " + CharacterDataRowInDate);
            }
            else
            {
                Debug.LogError("���� ĳ���� ���� ���� ���� ���� : " + callback.ToString());
                if (callback.IsClientRequestFailError())
                {
                    Debug.Log("��Ʈ��ũ�� �Ͻ������� �������� ���");
                    //                    UserDataInsert();
                }
            }
        });
    }
 
    public void PlayerCharacterDataLoad(int characterID)
    {
        Where where = new Where();
        where.Equal("CharacterID", characterID);

        Backend.GameData.GetMyData("PlayerCharacter", where, call_back =>
        {
            if (call_back.IsSuccess())
            {
                Debug.Log(call_back.ToString());

                try
                {
                    LitJson.JsonData gameDataJson = call_back.FlattenRows();

                    if (gameDataJson.Count <= 0)
                    {
                        Debug.Log("�����Ͱ� �������� �ʽ��ϴ�");
                        return;
                    }
                    else
                    {
                        CharacterDataRowInDate = gameDataJson[0]["inDate"].ToString();
                        PlayerCharacterGameData CharacterDataRow = new PlayerCharacterGameData();
                        CharacterDataRow.Json_write(gameDataJson);
                        playerCharacterGameData.Add(CharacterDataRow);

                        onGameDataLoadEvent?.Invoke();

                        Debug.Log($"{characterID}��° ���� ĳ���� ���� ������ �ε� ����");
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e);
                }
            }
            else
            {
                Debug.Log($"���� ĳ���� ���� ������ �ε� ����");
            }
        });
    }

    public void PlayerCharacterDataUpdate(int characterID, UnityAction action = null)
    {
        if (playerCharacterGameData == null)
        {
            Debug.Log("Insert or Load ���� �ʿ���");
            return;
        }

        Param param = new Param();

        for (int i = 0; i < playerCharacterGameData.Count; i++)
        {
            if (playerCharacterGameData[i].characterID == characterID)
            {
                param = playerCharacterGameData[i].ToParam();
            }
        }

        if (string.IsNullOrEmpty(CharacterDataRowInDate))
        {
            Debug.LogError("���� ĳ������ inDate ������ X");
        }
        else
        {
            Debug.Log($"{CharacterDataRowInDate}�� ���� ĳ���� ���� ������ ���� ��û");

            Backend.GameData.UpdateV2("PlayerCharacter", CharacterDataRowInDate, Backend.UserInDate, param, callback =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log($"���� ĳ���� ���� ������ ���� ���� : {callback}");

                    action?.Invoke();
                }
                else
                {
                    Debug.Log($"���� ĳ���� ���� ������ ���� ���� : {callback}");
                    if (callback.IsClientRequestFailError())
                    {
                        Debug.Log("��Ʈ��ũ�� �Ͻ������� �������� ���");
                        PlayerCharacterDataUpdate(characterID, action);
                    }
                }
            });
        }
    }

}
