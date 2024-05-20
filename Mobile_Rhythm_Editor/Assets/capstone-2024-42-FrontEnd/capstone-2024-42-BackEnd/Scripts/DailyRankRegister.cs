using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using Unity.VisualScripting;

public class RankRegister : MonoBehaviour
{
    public void Process(int newScore)
    {
        //        UpdateMyRankData(newScore);

        //������ ������Ʈ

        UpdateMyBestRankData(newScore);
    }

    public void UpdateMyBestRankData(int newScore)
    {
        Backend.URank.User.GetMyRank(Constants.DAILY_RANK_UUID, callback => { 
            if(callback.IsSuccess())
            {
                try
                {
                    LitJson.JsonData rankDataJson = callback.FlattenRows();

                    if(rankDataJson.Count <= 0)
                    {
                        Debug.LogWarning("������ ������");
                    }
                    else
                    {
                        int bestScore = int.Parse(rankDataJson[0]["Score"].ToString());

                        if(newScore > bestScore)
                        {
                            UpdateMyRankData(newScore);

                            Debug.Log($"�ְ� ���� ���� {bestScore} -> {newScore}");
                        }
                    }
                }
                catch(System.Exception e)
                {
                    Debug.LogException(e);
                }
            }
            else
            {
                if (callback.GetMessage().Contains("userRank"))
                {
                    UpdateMyRankData(newScore);

                    Debug.Log($"���ο� ��ŷ ������ ���� �� ��� : {callback}");
                }
            }
        });
    }

        private void UpdateMyRankData(int newScore)
        {
            string rowInDate = string.Empty;

            Backend.GameData.GetMyData(Constants.USER_DATA_TABLE, new Where(), callback =>
            {
                if (!callback.IsSuccess())
                {
                    Debug.LogError($"������ ��ȸ �� ���� �߻� : {callback}");
                    return;
                }

                Debug.Log($"������ ��ȸ ���� : {callback}");

                if (callback.FlattenRows().Count > 0)
                {
                    rowInDate = callback.FlattenRows()[0]["inDate"].ToString();
                }
                else
                {
                    Debug.LogError("������ ������");
                    return;
                }
            });

            Param param = new Param()
            {
                { "dailyBestScore", newScore }
            };

            Backend.URank.User.UpdateUserScore(Constants.DAILY_RANK_UUID, Constants.USER_DATA_TABLE, rowInDate, param, callback =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log($"��ŷ ��� ���� : {callback}");
                }
                else
                {
                    Debug.LogError($"��ŷ ��� ���� : {callback}");
                }
            });
        }
}
