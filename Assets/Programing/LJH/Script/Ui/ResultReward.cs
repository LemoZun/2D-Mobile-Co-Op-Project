using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultReward : MonoBehaviour
{
    [SerializeField] RewardSlot[] rewardSlots;

    private void OnEnable()
    {
        for (int i = 0; i < rewardSlots.Length; i++)
        {
            rewardSlots[i].gameObject.SetActive(false);
        }
        if (PlayerDataManager.Instance.PlayerData.IsStageClear[BattleSceneManager.Instance.curStageNum] == false) // ���� Ŭ����ø� ���� ����
        {
            int x = 0;
            foreach (int i in BattleSceneManager.Instance.curItemValues.Keys)
            {
                rewardSlots[x].gameObject.SetActive(true);
                rewardSlots[x].setRewardData(i,BattleSceneManager.Instance.curItemValues[i].ToString());
                x++;

            }
            x = 0;
        }

    }
}
