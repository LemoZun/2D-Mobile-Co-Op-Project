using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ �׽�Ʈ�� ���ؼ� ������ ��ũ��Ʈ
/// Behavior Tree���� �̿� �����ϰ� ��� ����
/// </summary>
public class UnitManager : MonoBehaviour
{
    [SerializeField] private AtkSingleSkillSO skill;
    [SerializeField] private UnitSO unit;

    [SerializeField] EnemyList enemyList;
    [SerializeField] public List<GameObject> enemies = new List<GameObject>();

    private void Awake()
    {
        enemyList = GameObject.FindGameObjectWithTag("EditorOnly").GetComponent<EnemyList>(); // ������ �±� ����Ͽ� ���� ����Ʈ ����
    }

    private void Start()
    {
        enemies = enemyList.enemyList;
    }

    private void Update()
    {
        if (enemyList.isChanged)
        {
            enemies = enemyList.enemyList;
        }
        else return;
    }

    /// <summary>
    /// ���Ƿ� ��ư�� ������ ��ų�� �߻�Ǵ� �Լ��� ������
    /// - Behavior Tree���� ��ų�� ����ϴ� ��� �� �Լ��� ����Ͽ� ��� ����
    /// </summary>
    public void OnSkillBtn()
    {
        Debug.Log("��ư�Է�");
        skill.DoSkill(unit.damage, enemies, gameObject);
        skill.DoAnimationSkill();
        skill.DoSoundSkill();
        //skill.OnSkill(unit.damage, enemies, gameObject);
        //skill.OnMotion();
    }
}
