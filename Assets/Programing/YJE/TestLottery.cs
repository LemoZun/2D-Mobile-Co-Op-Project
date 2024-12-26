using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TestLottery : MonoBehaviour
{
    List<Lottery> lotteries = new List<Lottery>();
    private int total = 0;
    private void Awake()
    {
        Lottery gold = new Lottery();
        gold.Id = 500;
        gold.Probability = 3000;
        lotteries.Add(gold);

        Lottery binoBlood = new Lottery();
        binoBlood.Id = 501;
        binoBlood.Probability = 3500;
        lotteries.Add(binoBlood); 

        Lottery boneCrystal = new Lottery();
        boneCrystal.Id = 502;
        boneCrystal.Probability = 2500;
        lotteries.Add(boneCrystal); 
        
        Lottery dinoStone = new Lottery();
        dinoStone.Id = 503;
        dinoStone.Probability = 500;
        lotteries.Add(dinoStone); 
        
        Lottery stone = new Lottery();
        stone.Id = 504;
        stone.Probability = 500;
        lotteries.Add(stone);

        for(int i = 0; i < lotteries.Count; i++)
        {
            total += lotteries[i].Probability;
        }
    }

    public void SingleLotteryBtn()
    {
        int weight = 0;
        int selectNum = 0;
        selectNum = Mathf.RoundToInt(total * Random.Range(0.0f, 1.0f));
        for(int i = 0;i< lotteries.Count; i++)
        {     
            weight += lotteries[i].Probability;
            if (selectNum <= weight)
            {
                Debug.Log("��ȯ�� ������ : " + lotteries[i].Id);
                break;
            }
        }
    }

    public void TenLotteryBtn()
    {
        int weight = 0;
        int selectNum = 0;
        int count = 0;
        // �׽�Ʈ �����
        Dictionary<int, float> tester = new Dictionary<int, float>();
        tester.Add(500, 0f);
        tester.Add(501, 0f);
        tester.Add(502, 0f);
        tester.Add(503, 0f);
        tester.Add(504, 0f);
        // �׽�Ʈ��
        do
        {
            selectNum = Mathf.RoundToInt(total * Random.Range(0.0f, 1.0f));
            for (int i = 0; i < lotteries.Count; i++)
            {
                weight += lotteries[i].Probability;
                if (selectNum <= weight)
                {
                    Debug.Log(lotteries[i].Id);
                    if (tester.ContainsKey(lotteries[i].Id))
                    {
                        tester[lotteries[i].Id] += 1;
                    }
                    count++;
                    break;
                }
            }
        } while (count < 10000); // �׽�Ʈ�� ī��Ʈ 10000

        Debug.Log($"500�� : {tester[500]} / {count} => {tester[500] / count * 100}");
        Debug.Log($"501�� : {tester[501]} / {count} => {tester[501] / count * 100}");
        Debug.Log($"502�� : {tester[502]} / {count} => {tester[502] / count * 100}");
        Debug.Log($"503�� : {tester[503]} / {count} => {tester[503] / count * 100}");
        Debug.Log($"504�� : {tester[504]} / {count} => {tester[504] / count * 100}");
    }
}
