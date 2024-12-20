using System.Collections.Generic;
using UnityEngine;

public class UnitCharacter : MonoBehaviour
{
    //[SerializeField] private  skill;
    [SerializeField] private UnitSO stats;
    private QueznaSkill queznaSkill;
    [SerializeField] EnemyList enemyList;
    [SerializeField] List<GameObject> enemys = new List<GameObject>();
    private int charId;
    private int skillId;
    private string name;
    private float hp;
    private float ATK;
    private float DEF;
    private float coolTime;

    private void Awake()
    {
        charId = stats.charId;
        skillId = stats.skillId;
        name = stats.name;
        hp = stats.hp;
        ATK = stats.ATK;
        DEF = stats.DEF;
        coolTime = stats.coolTime;
        queznaSkill = GetComponent<QueznaSkill>();
    }
    private void Start()
    {
        enemys = enemyList.enemyList;
    }

    private void Update()
    {
        Debug.Log("������Ʈ�� ����");
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Debug.Log("Space ����");
                queznaSkill.DoSkill(enemys, ATK);

            }
        }
    }


}
