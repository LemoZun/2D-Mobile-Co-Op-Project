using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UI Binding ����
// �ٸ� Ŭ�������� BaseUI ����ϴ� ��� UI�� ������ ���� ������ �� �ֵ��� ����
// ex) public class TestUI : BaseUI
public class BaseUI : MonoBehaviour
{
    // �ڽ����� ���� GameObject�� Dictionary 
    private Dictionary<string, GameObject> gameObjectDic;
    // �ڽ����� ���� GameObject�� Component�� �̸� �޾� �����ϴ� Dictionary
    private Dictionary<string, Component> componentDic;


    // private void Awake()�� ����ϴ� ���
    // ����ϰ� �ִ� �ڽĿ��� Bind()�� �Ͼ�� ���� �� ����
    protected virtual void Awake() 
    {
        // UI�� ������ڸ��� Binding������ ����
        Bind();
    }

    /// <summary>
    /// UI�� �ִ� ��� �ڽĵ��� Ȯ���Ͽ� ����
    /// </summary>
    private void Bind()
    {
        // Transform Componenet�� ���� GameObject�� �����Ƿ� Transform�������� �ҷ�����
        //                                                          (true) => ��Ȱ��ȭ �� Component���� �ҷ�����
        Transform[] transforms = GetComponentsInChildren<Transform>(true);
        // transforms.Length << 1 : Dictionary�� ũ��� transforms�迭�� 2�� << 2 = 4��
        // �ʹ� ���ϰ� �����ϴ� ��� �޸������� ���� ����
        gameObjectDic = new Dictionary<string, GameObject>(transforms.Length << 2);
        componentDic = new Dictionary<string, Component>();
        // transforms�� �迭�� GameObject�� ���� gameObjectDic�� ����
        foreach(Transform t in transforms)
        {
            // ������ �̸��� ���� �� ������ TryAdd() ���
            gameObjectDic.TryAdd(t.gameObject.name, t.gameObject);
        }
    }

    /// <summary>
    /// ���ϴ� UI�� ã�� ����� ����
    /// 1. GetUI(name) : name�̸��� ���ӿ�����Ʈ ��������
    /// 2. GetUI<T>(name) : �̸��� name�� UI���� ������Ʈ T�� �������� 2222222 
    ///     ex) GetUI<Image>("name") : �̸��� name�� ������Ʈ���� ������Ʈ T�� ��������
    /// </summary>
    public GameObject GetUI(in string name)
    {
        // �̸����� ������ Dictionary���� �̸����� GameObject�� ã�Ƽ� ��ȯ
        gameObjectDic.TryGetValue(name, out GameObject obj);
        // name�� ���� ��� null�� ��ȯ
        return obj;
    }
    
    public T GetUI<T>(in string name) where T : Component
    {
        string key = $"{name}_{typeof(T).Name}";

        // 1. componentDic�� ���� ��� : ã�� �� ��ųʸ��� �߰� �� ��ȯ
        componentDic.TryGetValue(key, out Component component);
        if(component != null) return component as T;

        gameObjectDic.TryGetValue(name, out GameObject gameObject);
        if (gameObject = null) return null;

        // 2. componentDic�� �̹� �ִ� ��� (= �̹� ã�ƺ� ���� �ִ� ���) : ã�Ҵ� ���� ��ȯ
        component = gameObject.GetComponent<T>();
        if (component == null) return null;
        componentDic.TryAdd(key, component);
        return component as T;



        // 1. componentDic�� ���� ��� : ã�� �� ��ųʸ��� �߰� �� ��ȯ
        /*if (componentDic.ContainsKey(key))
        {
            return componentDic[key] as T;
        }*/
        // 2. componentDic�� �̹� �ִ� ��� (= �̹� ã�ƺ� ���� �ִ� ���) : ã�Ҵ� ���� ��ȯ
        /*else
        {
            T component = gameObjectDic[name].GetComponent<T>();
            if(component != null)
            {
                componentDic.TryAdd(key, component);
                return component;
            }
            else
            {
                return null;
            }
        }*/



    }
}
