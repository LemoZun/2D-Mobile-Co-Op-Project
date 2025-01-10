using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMaker : MonoBehaviour
{
    [SerializeField] GachaSceneController gachaSceneController;
    private Dictionary<int, ShopChar> charDictionary;
    private RectTransform characterContent; // ���� ĳ���� �������� ���� �� ��ġ
    [SerializeField] GameObject shopCharPrefab;

    public void ShopCharMaker()
    {
        // ĳ���� ����Ʈ�� �޾Ƽ� ��� BuyCharacter ���������� �����ϱ�
        charDictionary = gachaSceneController.CharDictionary; // ĳ���� ��ü ����Ʈ ����
        characterContent = gachaSceneController.GetUI<RectTransform>("CharacterContent"); // ���� ĳ���� �������� ���� �� ��ġ ����
        
        for(int i = 1; i <= charDictionary.Count; i++)
        {
            GameObject shopCharUI = Instantiate(shopCharPrefab, characterContent);
            ShopChar shopChar = shopCharUI.GetComponent<ShopChar>();
            charDictionary.TryGetValue(i, out shopChar);

            shopCharUI = shopChar.SetShopCharUI(shopChar, shopCharUI);
        }
    }


}
