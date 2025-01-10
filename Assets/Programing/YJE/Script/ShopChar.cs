using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopChar : MonoBehaviour
{
    private int charId;
    public int CharId { get { return charId; } set { charId = value; } }

    private string charName;
    public string CharName { get { return charName; } set { charName = value; } }

    private int rarity;
    public int Rarity { get { return rarity; } set { rarity = value; } }

    private Sprite charImageProfile; // �����տ��� ����� �̹���
    public Sprite CharImageProfile { get { return charImageProfile; } set { charImageProfile = value; } }

    private int amount;
    public int Amount { get { return amount; } set { amount = value; } }

    // ���� ���� ���� - ��� ���� �ٸ� / �ڵ忡�� ����
    [SerializeField] private int price;

    // �̱� �� ����� �ƾ�    
    // Resources ������ �ִ� �̹����� �����Ͽ� �����
    // Resources.Load<Sprite>("���ϰ��/���ϸ�");
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
    public ShopChar MakeCharList(Dictionary<int, Dictionary<string, string>> dataBaseList, ShopChar result, int index)
    {
        result.charId = index;
        result.charName = dataBaseList[index]["Name"];
        result.rarity = TypeCastManager.Instance.TryParseInt(dataBaseList[index]["Rarity"]);
        switch (index) // �� ĳ���Ϳ� �˸´� �̹��� ����
        {
            case 1:
                result.charImageProfile = Resources.Load<Sprite>("Portrait/portrait_1");
                result.video = Resources.Load<GameObject>("CutScenePrefabs/1_Tricia");
                result.price = SetPrice(result.rarity);
                break;
            case 2:
                result.charImageProfile = Resources.Load<Sprite>("Portrait/portrait_2");
                result.video = Resources.Load<GameObject>("CutScenePrefabs/2_Celes");
                result.price = SetPrice(result.rarity);
                break;
            case 3:
                result.charImageProfile = Resources.Load<Sprite>("Portrait/portrait_3");
                result.video = Resources.Load<GameObject>("CutScenePrefabs/3_Regina");
                result.price = SetPrice(result.rarity);
                break;
            case 4:
                result.charImageProfile = Resources.Load<Sprite>("Portrait/portrait_1");
                result.video = Resources.Load<GameObject>("CutScenePrefabs/4_Spinne");
                result.price = SetPrice(result.rarity);
                break;
            case 5:
                result.charImageProfile = Resources.Load<Sprite>("Portrait/portrait_5");
                result.video = Resources.Load<GameObject>("CutScenePrefabs/5_Aila");
                result.price = SetPrice(result.rarity);
                break;
            case 6:
                result.charImageProfile = Resources.Load<Sprite>("Portrait/portrait_1");
                result.video = Resources.Load<GameObject>("CutScenePrefabs/6_Quezna");
                result.price = SetPrice(result.rarity);
                break;
            case 7:
                result.charImageProfile = Resources.Load<Sprite>("Portrait/portrait_1");
                result.video = Resources.Load<GameObject>("CutScenePrefabs/7_Uloro");
                result.price = SetPrice(result.rarity);
                break;
            case 8:
                result.charImageProfile = Resources.Load<Sprite>("Portrait/portrait_1");
                result.video = Resources.Load<GameObject>("CutScenePrefabs/8_Eost");
                result.price = SetPrice(result.rarity);
                break;
            case 9:
                result.charImageProfile = Resources.Load<Sprite>("Portrait/portrait_1");
                result.video = Resources.Load<GameObject>("CutScenePrefabs/9_Melorin"); 
                result.price = SetPrice(result.rarity); 
                break;
        }
        return result;
    }

    private int SetPrice(int rarity)
    {
        switch (rarity)
        {
            case 2:
                price = 50;
                break;
            case 3:
                price = 100;
                break;
            case 4:
                price = 300;
                break;
        }
        return price;
    }


    /// <summary>
    /// ShopChar�� ������ ResultPanel/Panel �Ʒ��� ���� ������� ������UI�� �����ϴ� �Լ�
    // - GachaSceneController.cs���� ���
    /// </summary>
    /// <param name="gachaChar"></param>
    /// <param name="resultCharUI"></param>
    /// <returns></returns>
    public GameObject SetGachaCharUI(ShopChar gachaChar, GameObject resultCharUI)
    {
        // ������ ����
        resultCharUI.GetComponent<ShopChar>().charId = gachaChar.charId;
        resultCharUI.GetComponent<ShopChar>().charName = gachaChar.CharName;
        resultCharUI.GetComponent<ShopChar>().rarity = gachaChar.rarity;
        resultCharUI.GetComponent<ShopChar>().video = gachaChar.video;

        // UI ��� ����
        resultCharUI.transform.GetChild(0).GetComponent<Image>().sprite = gachaChar.charImageProfile;
        resultCharUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = gachaChar.charName;

        GameObject rarities = resultCharUI.transform.GetChild(2).gameObject;
        // �� ���� ����
        for (int i = 0; i < gachaChar.rarity; i++)
        {
            rarities.transform.GetChild(i).gameObject.SetActive(true);
        }
        return resultCharUI;
    }

    /// <summary>
    /// shopChar�� ������ ShopScrollView-Viewport-CharacterContent �Ʒ��� ���� ������� ������UI�� �����ϴ� �Լ�
    // - ShopMaker.cs���� ���
    /// </summary>
    /// <param name="gachaChar"></param>
    /// <param name="resultCharUI"></param>
    /// <returns></returns>
    public GameObject SetShopCharUI(ShopChar shopChar, GameObject resultCharUI)
    {
        // ������ ����
        resultCharUI.GetComponent<ShopChar>().charId = shopChar.charId;
        resultCharUI.GetComponent<ShopChar>().charName = shopChar.CharName;
        resultCharUI.GetComponent<ShopChar>().rarity = shopChar.rarity;
        resultCharUI.GetComponent<ShopChar>().price = shopChar.price;
        resultCharUI.GetComponent<ShopChar>().video = shopChar.video;

        // UI ��� ����
        resultCharUI.transform.GetChild(0).GetComponent<Image>().sprite = shopChar.charImageProfile;
        resultCharUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = shopChar.charName;

        GameObject rarities = resultCharUI.transform.GetChild(2).gameObject;
        // �� ���� ����
        for (int i = 0; i < shopChar.rarity; i++)
        {
            rarities.transform.GetChild(i).gameObject.SetActive(true);
        }
        return resultCharUI;
    }

    public void OnBuyCharacter()
    {
        Debug.Log("��ưŬ��");
        // �����ϱ� ��ư Ŭ�� �� ����� �Լ�
        // �� ������(BuyCharacter.gameobject.GachaChar.CharId)�� ���̵�� �÷��̾��� ���� ���� ���̵� �ߺ����� Ȯ��
        bool isChecked = false;
        if (PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.Stone] >= price) // ���ļ� ���� Ȯ�� �ʿ�
        {
            // �ߺ� ĳ���� ���� Ȯ��
            for (int i = 0; i < PlayerDataManager.Instance.PlayerData.UnitDatas.Count; i++)
            {
                if (gameObject.GetComponent<ShopChar>().CharId == PlayerDataManager.Instance.PlayerData.UnitDatas[i].UnitId)
                {
                    isChecked = true;
                    break;
                }
                else
                    continue;
            }

            if (isChecked)
            {
                // �ؽ�Ʈ ���� �ڷ�ƾ ����
                Debug.Log("�ߺ�");
            }

            else if (!isChecked)
            {
                DatabaseReference root = BackendManager.Database.RootReference.Child("UserData");

                PlayerUnitData newUnit = new PlayerUnitData();
                newUnit.UnitId = gameObject.GetComponent<ShopChar>().CharId;
                newUnit.UnitLevel = 1;
                PlayerDataManager.Instance.PlayerData.UnitDatas.Add(newUnit);
                DatabaseReference unitRoot = root.Child(BackendManager.Auth.CurrentUser.UserId).Child("_unitDatas");

                // ��� playerData.UnitDatas�� ������ DB������ ����
                for (int num = 0; num < PlayerDataManager.Instance.PlayerData.UnitDatas.Count; num++)
                {
                    // nowData�� PlayerUnitData ����
                    PlayerUnitData nowData = new PlayerUnitData();
                    nowData.UnitId = PlayerDataManager.Instance.PlayerData.UnitDatas[num].UnitId;
                    nowData.UnitLevel = PlayerDataManager.Instance.PlayerData.UnitDatas[num].UnitLevel;

                    // ������ ��ġ�� ������� ������ ����
                    unitRoot.Child($"{num}/_unitId").SetValueAsync(nowData.UnitId);
                    unitRoot.Child($"{num}/_unitLevel").SetValueAsync(nowData.UnitLevel);
                }

                // ���� �Ϸ� �ؽ�Ʈ�� ���� �ڷ�ƾ ���
                Debug.Log($"���ſϷ� : {gameObject.GetComponent<ShopChar>().price}");
                int result = PlayerDataManager.Instance.PlayerData.Items[(int)E_Item.Stone] - gameObject.GetComponent<ShopChar>().price;
                PlayerDataManager.Instance.PlayerData.SetItem((int)E_Item.Stone, result);
                // ���� ���� �� ��� - UserId�ҷ�����
                DatabaseReference setItemRoot;
                setItemRoot = root.Child(BackendManager.Auth.CurrentUser.UserId).Child("_items/4");
                setItemRoot.SetValueAsync(result); // firebase �� ����
                gameObject.GetComponentInParent<GachaSceneController>().ShowSingleResultPanel();
                StartCoroutine(CharacterVideoR(gameObject));
            }
            StopCoroutine(CharacterVideoR(gameObject));
        }
    }

    /// <summary>
    /// ��í�� ĳ���� �̱� �� ������ ���� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator CharacterVideoR(GameObject gameObj)
    {
        if (gameObj.GetComponent<ShopChar>())
        {
            GameObject obj = Instantiate(gameObj.GetComponent<ShopChar>().Video, gameObject.GetComponentInParent<GachaBtn>().SingleVideoContent);
            obj.SetActive(true);
            yield return new WaitUntil(() => obj.gameObject == false);
        }
        gameObject.GetComponentInParent<GachaSceneController>().DisableSingleImage();
        gameObject.GetComponentInParent<GachaSceneController>().DisabledGachaResultPanel();
    }

}
