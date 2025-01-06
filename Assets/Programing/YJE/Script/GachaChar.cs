using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GachaChar : MonoBehaviour
{
    private int charId;
    public int CharId { get { return charId; } set { charId = value; } }

    private string charName;
    public string CharName { get { return charName; } set { charName = value; } }

    private int rarity;
    public int Rarity { get { return rarity; } set { rarity = value; } }

    private Sprite charImageProfile; // �����տ��� ����� �̹���
    public Sprite CharImageProfile { get { return charImageProfile; } set { charImageProfile = value; } }

    // �̱� �� ����� �̹���
    // Resources ������ �ִ� �̹����� �����Ͽ� �����
    // Resources.Load<Sprite>("���ϰ��/���ϸ�");
    private Sprite charGachaImage;
    public Sprite CharGachaImage { get { return charGachaImage; } set { charGachaImage = value; } }

    private int amount;
    public int Amount { get { return amount; } set { amount = value; } }

    private GameObject video;
    public GameObject Video { get { return video; } set { video = value; } }

    /// <summary>
    /// Gacha���� ����ϴ� CharacterList�� Dictionary�� ����� �� ���
    /// - ĳ���� ������ �߰��Ǵ� ��� Switch���� �б� �����Ͽ� ���
    //  - GachaSceneController.cs�� MakeCharList()���� �����Ͽ� ���
    /// </summary>
    /// <param name="dataBaseList"></param>
    /// <param name="result"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public GachaChar MakeCharList(Dictionary<int, Dictionary<string, string>> dataBaseList, GachaChar result, int index)
    {
        result.charId = index;
        result.charName = dataBaseList[index]["Name"];
        result.rarity = TypeCastManager.Instance.TryParseInt(dataBaseList[index]["Rarity"]);
        switch (index) // �� ĳ���Ϳ� �˸´� �̹��� ����
        {
            case 1:
                result.charImageProfile = Resources.Load<Sprite>("Characters/2_testCelesProfile");
                result.charGachaImage = Resources.Load<Sprite>("Characters/1_testTricia");
                result.video = Resources.Load<GameObject>("Characters/1_Tricia");
                break;
            case 2:
                result.charImageProfile = Resources.Load<Sprite>("Characters/2_testCelesProfile");
                result.charGachaImage = Resources.Load<Sprite>("Characters/2_testCeles");
                result.video = Resources.Load<GameObject>("Characters/2_Celes");
                break;
            case 3:
                result.charImageProfile = Resources.Load<Sprite>("Characters/3_testReginaProfile");
                result.charGachaImage = Resources.Load<Sprite>("Characters/3_testRegina");
                result.video = Resources.Load<GameObject>("Characters/3_Regina");
                break;
            case 4:
                result.charImageProfile = Resources.Load<Sprite>("Characters/4_testSpinneProfile");
                result.charGachaImage = Resources.Load<Sprite>("Characters/4_testSpinne");
                result.video = Resources.Load<GameObject>("Characters/4_Spinne");
                break;
            case 5:
                result.charImageProfile = Resources.Load<Sprite>("Characters/5_testAilaProfile");
                result.charGachaImage = Resources.Load<Sprite>("Characters/5_testAila");
                result.video = Resources.Load<GameObject>("Characters/5_Aila");
                break;
            case 6:
                result.charImageProfile = Resources.Load<Sprite>("Characters/5_testAilaProfile");
                result.charGachaImage = Resources.Load<Sprite>("Characters/6_testQuezna.png");
                result.video = Resources.Load<GameObject>("Characters/6_Quezna");
                break;
            case 7:
                result.charImageProfile = Resources.Load<Sprite>("Characters/5_testAilaProfile");
                result.charGachaImage = Resources.Load<Sprite>("Characters/7_testUloro");
                result.video = Resources.Load<GameObject>("Characters/7_Uloro");
                break;
        }
        return result;
    }

    /// <summary>
    /// GachaChar�� ������ ResultPanel/Panel �Ʒ��� ���� ������� ������UI�� �����ϴ� �Լ�
    // - GachaSceneController.cs���� ���
    /// </summary>
    /// <param name="gachaChar"></param>
    /// <param name="resultCharUI"></param>
    /// <returns></returns>
    public GameObject SetGachaCharUI(GachaChar gachaChar, GameObject resultCharUI)
    {
        // ������ ����
        resultCharUI.gameObject.GetComponent<GachaChar>().charId = gachaChar.charId;
        resultCharUI.gameObject.GetComponent<GachaChar>().charName = gachaChar.CharName;
        resultCharUI.gameObject.GetComponent<GachaChar>().rarity = gachaChar.rarity;
        resultCharUI.GetComponent<GachaChar>().video = gachaChar.video;

        // UI ��� ����
        resultCharUI.transform.GetChild(0).GetComponent<Image>().sprite = gachaChar.charImageProfile;
        resultCharUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = gachaChar.charName;

        GameObject rarities = resultCharUI.transform.GetChild(2).gameObject;
        // �� ���� ����
        for (int i = 0; i< gachaChar.rarity; i++)
        {
            rarities.transform.GetChild(i).gameObject.SetActive(true);
        }
        return resultCharUI;
    }

}
