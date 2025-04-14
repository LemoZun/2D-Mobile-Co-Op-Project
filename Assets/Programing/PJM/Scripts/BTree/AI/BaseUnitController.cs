using System;
using System.Collections;
using System.Collections.Generic;
using Programing.PJM.Scripts.BTree.Nodes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class BaseUnitController : MonoBehaviour
{
    [SerializeField] private Transform _centerPosition;
    public Transform CenterPosition { get => _centerPosition; private set => _centerPosition = value; }

    [SerializeField] private Transform _muzzlePoint;
    public Transform MuzzlePoint { get => _muzzlePoint; protected set => _muzzlePoint = value; }
    private bool _inAttackDelay;
    public bool isDying { get; set; } = false;

    private UnitView _unitViewer;
    public UnitView UnitViewer { get => _unitViewer; private set => _unitViewer = value; }
    private UnitModel _unitModel;
    public UnitModel UnitModel { get => _unitModel; private set => _unitModel = value; }

    private BehaviourTreeRunner _BTRunner;
    private BaseUnitController _detectedEnemy;
    public BaseUnitController DetectedEnemy { get => _detectedEnemy; set => _detectedEnemy = value; }

    private BaseUnitController _currentTarget;

    protected BaseUnitController CurrentTarget { get => _currentTarget; set => _currentTarget = value; }

    private BaseUnitController _tauntSource;
    public BaseUnitController TauntSource { get => _tauntSource; set => _tauntSource = value; }

    private Skill _curSkill;
    public Skill CurSkill { get => _curSkill; set => _curSkill = value; }
    
    protected int unitID;
    public int UnitID => unitID;

    private LayerMask _allianceLayer;
    public LayerMask AllianceLayer { get => _allianceLayer; private set => _allianceLayer = value; }
    private LayerMask _enemyLayer;
    public LayerMask EnemyLayer { get => _enemyLayer; private set => _enemyLayer = value; }

    private bool _isAttacking = false;

    protected bool IsAttacking { get => _isAttacking; set => _isAttacking = value;}
    
    public float CoolTimeCounter { get; set; }
    public bool IsSkillRunning { get; set; }

    private Skill.SkillState _curSkillState;
    public Skill.SkillState CurSkillState { get => _curSkillState; set => _curSkillState = value; }

    protected virtual void Awake()
    {
        if(UnitViewer == null)
            UnitViewer = GetComponent<UnitView>();
        if(UnitModel == null)
            UnitModel = GetComponent<UnitModel>();
        if(CenterPosition == null)
            CenterPosition = transform.Find("CenterPosition");
    }

    protected virtual void OnEnable()
    {
        UnitModel.OnDeath += HandleDeath;
    }

    protected virtual void Start()
    {
        SetLayer();
        BaseNode rootNode = SetBTree();
        _BTRunner = new BehaviourTreeRunner(rootNode);
    }

    protected virtual void Update()
    {
        if (Time.timeScale == 0)
            return;
        
        // ��Ÿ�� 
        if (CoolTimeCounter > 0)
        {
            CoolTimeCounter -= UnitModel.CoolDownAcc * Time.deltaTime;
        }

        _BTRunner.Operate();
    }

    protected void OnDisable()
    {
        UnitModel.OnDeath -= HandleDeath;
    }


    protected abstract BaseNode SetBTree(); // �� ������ ������ �ൿ Ʈ�� �޼���

    protected virtual void SetLayer()
    {
        string myLayerName = LayerMask.LayerToName(gameObject.layer);
        EnemyLayer = myLayerName == "UserCharacter" ? LayerMask.GetMask("Enemy") : LayerMask.GetMask("UserCharacter");
        AllianceLayer = LayerMask.GetMask(myLayerName);
    }
    
    protected BaseNode.ENodeState CheckUnitDying()
    {
        if (!isDying)
            return BaseNode.ENodeState.Failure;
        
        var stateInfo = UnitViewer.UnitAnimator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Dead"))
        {
            if (stateInfo.normalizedTime < 1.0f)
            {
                // �״���
                return BaseNode.ENodeState.Running;
            }
            else if (stateInfo.normalizedTime >= 1.0f)
            {
                isDying = false;
                gameObject.SetActive(false);
                return BaseNode.ENodeState.Success;
            }
        }

        return BaseNode.ENodeState.Failure;
    }

    protected BaseNode.ENodeState CheckBattleWin()
    {
        if (BattleSceneManager.Instance.curBattleState != BattleSceneManager.BattleState.Win)
            return BaseNode.ENodeState.Failure;
        UnitViewer.UnitAnimator.SetTrigger(UnitViewer.ParameterHash[(int)Parameter.Win]);
        return BaseNode.ENodeState.Success;
    }

    protected BaseNode.ENodeState CheckCrowdControl()
    {
        // �� �߰� ����?, ���� �����̻���� ���ÿ� �ɷ����� ���� �ʿ�
        switch (UnitModel.CurCc)
        {
            case CrowdControls.Stun:
                return BaseNode.ENodeState.Running;
            default:
                return BaseNode.ENodeState.Failure;
        }
    }

    protected BaseNode.ENodeState SetTargetToAttack()
    {
        if(DetectedEnemy != null && DetectedEnemy.gameObject.activeSelf)
        {
            CurrentTarget = DetectedEnemy;
            //UnitViewer.CheckNeedFlip(transform, CurrentTarget.transform);
            return BaseNode.ENodeState.Success;
        }
        return BaseNode.ENodeState.Failure;
    }
    
    protected virtual BaseNode.ENodeState PerformAttack()
    {
        if(CurrentTarget == null || !CurrentTarget.gameObject.activeSelf && CurrentTarget.isDying)
        {
            UnitViewer.UnitAnimator.SetBool(UnitViewer.ParameterHash[(int)Parameter.Attack], false);
            IsAttacking = false;
            Debug.Log(" Ÿ���� ��ȿ���� �ʾ� ���� ����."); // ���� �ִϸ��̼� ������ ����� ������� ���?
            return BaseNode.ENodeState.Failure;
        }
        
        if ((UnitModel.CurCc & CrowdControls.Taunt) != 0) // �ɸ� �����̻� �� ������ �������
        {
            if (UnitModel.CcCaster != null && UnitModel.CcCaster.gameObject.activeSelf) // ������ �� ����� ��ȿ�� ����� ��
            {
                CurrentTarget = UnitModel.CcCaster;
            }
        }
        
        UnitViewer.CheckNeedFlip(transform, CurrentTarget.transform);
        // ������ ����
        // ���� �Ķ���Ͱ� False���� ��쿡�� True�� �ٲ��ָ� ���� ����
        UnitViewer.UnitAnimator.SetBool(UnitViewer.ParameterHash[(int)Parameter.Run], false);
        
        if(!UnitViewer.UnitAnimator.GetBool(UnitViewer.ParameterHash[(int)Parameter.Attack]))
        {
            UnitViewer.UnitAnimator.SetBool(UnitViewer.ParameterHash[(int)Parameter.Attack], true);
            IsAttacking = true; 
            return BaseNode.ENodeState.Running;
        }
        
        // ���� ������, Attack �Ķ���� True ����
        var stateInfo = UnitViewer.UnitAnimator.GetCurrentAnimatorStateInfo(0);
        switch (stateInfo.normalizedTime)
        {
            case < 1.0f:
                //Debug.Log($"{gameObject.name}�� {CurrentTarget.gameObject.name}�� ���� ��");
                return BaseNode.ENodeState.Running;
            case >= 1.0f:
                // ���� �ִϸ��̼��� ������ ���
                //Debug.Log($"{gameObject.name}�� {CurrentTarget.gameObject.name}�� ���� ������ �Ϸ�");
                UnitViewer.UnitAnimator.SetBool(UnitViewer.ParameterHash[(int)Parameter.Attack], false);
                IsAttacking = false;
                // ���ݼ��� ������ ���� ��Ŵ
                CurrentTarget.UnitModel.TakeDamage(UnitModel.AttackPoint);
                return BaseNode.ENodeState.Success;
            default:
                Debug.LogWarning("����ġ ���� ���¿��� ���� ����.");
                return BaseNode.ENodeState.Failure;
        }
    }

    protected virtual bool IsSkillAlreadyRunning()
    {
        return IsSkillRunning;
    }

    protected bool CheckMoveable()
    {
        return !(IsAttacking || IsSkillRunning);
    }
    
    protected IEnumerator AttackRoutine(string animationName)
    {
        /*while (UnitViewer.IsAnimationRunning(animationName))
        {
            yield return null;
        }*/
        
        //UnitAnimator.SetTrigger("Attack");
        
        // �ִϸ��̼��� ���̸�ŭ ��� �� ���� + �ĵ�����?
        // ���� �ִϸ������� info�� �������� ������ ���� ���� �ִϸ��̼��� �������� Ȯ�� �� �� ���� �ٸ���� �ʿ�
        //Debug.Log($"{UnitViewer.UnitAnimator.GetCurrentAnimatorStateInfo(0).length + _tempDelay}�� �� ����");
        yield return new WaitForSeconds(1.0f); // �ִϸ��̼� ����
        //UnitViewer.UnitAnimator.SetBool(UnitViewer.parameterHash[(int)UnitView.AniState.Attack], false);
        IsAttacking = false;
        Debug.Log($"{animationName} �ִϸ��̼� �Ϸ�: ���� ���µ�.");
    }
    protected BaseNode.ENodeState ChaseTarget()
    {
        if(DetectedEnemy != null && DetectedEnemy.gameObject.activeSelf)
        {
            UnitViewer.CheckNeedFlip(transform, DetectedEnemy.transform);
            float sqrDistance = Vector2.SqrMagnitude(DetectedEnemy.gameObject.transform.position - transform.position);
            if (sqrDistance > UnitModel.AttackRange * UnitModel.AttackRange) // Ÿ���� ���� �������� �ֶ�
            {
                UnitViewer.UnitAnimator.SetBool(UnitViewer.ParameterHash[(int)Parameter.Attack], false); // �ӽ�
                UnitViewer.UnitAnimator.SetBool(UnitViewer.ParameterHash[(int)Parameter.Run], true);
                transform.position = Vector2.MoveTowards(transform.position, DetectedEnemy.gameObject.transform.position, UnitModel.Movespeed * Time.deltaTime);
                //Debug.Log($"Ÿ�� {DetectedEnemy.gameObject.name}�� ���� ��");
                return BaseNode.ENodeState.Running;
            }
            else // Ÿ���� ���� ���� ���� ������ , �ൿƮ�� �Ĺݿ� �־ �������� �ٷ� �Ѿ�� ���� ����
            {
                UnitViewer.UnitAnimator.SetBool(UnitViewer.ParameterHash[(int)Parameter.Run], false);
                //Debug.Log($"Ÿ�� {DetectedEnemy.gameObject.name} �����Ϸ�");
                return BaseNode.ENodeState.Success;
            }
        }
        // Ÿ���� ������
        Debug.Log("Ÿ�� ����");
        UnitViewer.UnitAnimator.SetBool(UnitViewer.ParameterHash[(int)Parameter.Run], false);
        return BaseNode.ENodeState.Failure;
    }

    protected BaseNode.ENodeState StayIdle()
    {
        Debug.Log("Idle ����");
        UnitViewer.UnitAnimator.SetBool(UnitViewer.ParameterHash[(int)Parameter.Attack], false);
        UnitViewer.UnitAnimator.SetBool(UnitViewer.ParameterHash[(int)Parameter.Run], false);
        return BaseNode.ENodeState.Success;
    }

    protected bool CheckAttackRange()
    {
        if(DetectedEnemy != null && DetectedEnemy.gameObject.activeSelf)
        {
            if (CurrentTarget != null && CurrentTarget.gameObject.activeSelf)
            {
                float sqrDistanceToCur = Vector2.SqrMagnitude(CurrentTarget.gameObject.transform.position - transform.position);
                return sqrDistanceToCur <= UnitModel.AttackRange * UnitModel.AttackRange;
            }
            float sqrDistance = Vector2.SqrMagnitude(DetectedEnemy.gameObject.transform.position - transform.position);
            return sqrDistance <= UnitModel.AttackRange * UnitModel.AttackRange;
        }
        
        return false;
    }
    
    protected void HandleDeath()
    {
        isDying = true;
        UnitViewer.UnitAnimator.SetTrigger(UnitViewer.ParameterHash[(int)Parameter.Die]);
    }
    
    protected bool CheckSkillCooltimeBack()
    {
        if (CoolTimeCounter <= 0)
        {
            CoolTimeCounter = 0;
            return true;
        }
        return false;
    }

    protected abstract BaseNode.ENodeState SetDetectedTarget();
    /*{
        //Debug.LogWarning("ī�޶� �������� �� üũ�� ���Ż���Դϴ�.");
        throw new System.NotImplementedException();
        /*Debug.LogWarning("�⺻ Ÿ�� ���� �޼��� ������");
        if ((UnitModel.CurCc & CrowdControls.Taunt) != 0) // �ɸ� �����̻� �� ������ �������
        {
            if (UnitModel.CcCaster != null && UnitModel.CcCaster.gameObject.activeSelf) // ������ �� ����� ��ȿ�� ����� ��
            {
                DetectedEnemy = UnitModel.CcCaster;
            }
        }
        // �̹� ������ ���� �־�����쿣 ������ �ʿ� ����,  �ٷ� chase�� ��ȯ
        if(DetectedEnemy != null && DetectedEnemy.gameObject.activeSelf)
            return BaseNode.ENodeState.Success;
        
        Collider2D[] detectedColliders = Physics2D.OverlapAreaAll(_bottomLeft,_topRight, _enemyLayer);

        if (detectedColliders.Length == 0)
        {
            DetectedEnemy = null;
            return BaseNode.ENodeState.Failure;
        }
        
        float minDistance = float.MaxValue;
        float maxDistance = float.MinValue;
        BaseUnitController closetEnemy = null;
        BaseUnitController farthestEnemy = null;

        foreach (var col in detectedColliders)
        {
            BaseUnitController unit = col.gameObject.GetComponent<BaseUnitController>();
            if (unit == null)
            {
                Debug.LogWarning($"{col.gameObject.name}�� BaseUnitController�� ����.");
                continue;
            }
            
            float distance = Vector2.Distance(transform.position, col.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                closetEnemy = unit;
            }

            if (distance > maxDistance)
            {
                maxDistance = distance;
                farthestEnemy = unit;
            }
        }
        if (UnitModel.IsPriorityTargetFar)
        {
            // ���� �� Ÿ���� DetectedEnemy �� ����
            DetectedEnemy = farthestEnemy;
        }
        else
        {
            // ���� ����� Ÿ���� DetectedEnemy�� ����
            DetectedEnemy = closetEnemy;
        }
        
        UnitViewer.CheckNeedFlip(transform, DetectedEnemy.transform);

        return BaseNode.ENodeState.Success;#1#
        
    }*/

    /*protected void ExtractDetectedTargetFromList()
    {
        // ������ �Ϸ�� �̺�Ʈ�� ���缭 ����� �޼���? �ƴϸ� �׳� ����Ʈ���� ���⸸?
        if(AllianceLayer == )
        
    }*/
    
    // others
    /*protected void SetDetectingArea()
    {
        if (Camera.main != null)
        {
            _bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
            _topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        }
    }*/

    /*protected void OnDrawGizmos()
    {
        string layerName = LayerMask.LayerToName(gameObject.layer);
        Gizmos.color = (layerName == "UserCharacter") ? Color.green : Color.red;
        
        //Gizmos.color = Color.yellow;
        if(DetectedEnemy != null && DetectedEnemy.gameObject.activeSelf)
            Gizmos.DrawLine(transform.position, _detectedEnemy.gameObject.transform.position);

        Gizmos.color = Color.cyan;
        Vector2 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector2 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        
        Gizmos.DrawLine(new Vector3(bottomLeft.x, bottomLeft.y, 0), new Vector3(topRight.x, bottomLeft.y, 0)); // �Ʒ���
        Gizmos.DrawLine(new Vector3(bottomLeft.x, topRight.y, 0), new Vector3(topRight.x, topRight.y, 0));    // ����
        Gizmos.DrawLine(new Vector3(bottomLeft.x, bottomLeft.y, 0), new Vector3(bottomLeft.x, topRight.y, 0)); // ����
        Gizmos.DrawLine(new Vector3(topRight.x, bottomLeft.y, 0), new Vector3(topRight.x, topRight.y, 0));    // ������

        /*Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _detectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);#1#
    }*/
}

/*private BaseNode.ENodeState CheckAutoOn()
{
    if (BattleManager.Instance.IsAutoBattle)
    {
        return BaseNode.ENodeState.Success;
    }

    return BaseNode.ENodeState.Failure;
}

private BaseNode.ENodeState DetectEnemys()
{
    var overlapColliders = Physics2D.OverlapCircleAll(transform.position, _detectRange, LayerMask.GetMask("Player"));
    if (overlapColliders != null && overlapColliders.Length > 0)
    {
        _detectedEnemy = overlapColliders[0].transform;
        return BaseNode.ENodeState.Success;
    }

    _detectedEnemy = null;
    return BaseNode.ENodeState.Failure;
}


private BaseNode.ENodeState MoveToEnemy()
{
    if (_detectedEnemy != null)
    {
        float sqrDistance = Vector2.SqrMagnitude(_detectedEnemy.position - transform.position);
        if (sqrDistance < _attackRange * _attackRange)
        {
            return BaseNode.ENodeState.Success;
        }

        if (sqrDistance > Mathf.Epsilon)
        {
            transform.position = Vector2.MoveTowards(transform.position, _detectedEnemy.position, _moveSpeed * Time.deltaTime);
            return BaseNode.ENodeState.Running;
        }

    }
    return BaseNode.ENodeState.Failure;
}

private BaseNode.ENodeState DoAttack()
{
    if (IsAnimationRunning("attackStateNameTemp"))
    {
        return BaseNode.ENodeState.Running;
    }

    return BaseNode.ENodeState.Success;
}

private BaseNode.ENodeState TempMethod()
{
    return BaseNode.ENodeState.Success;
}*/

/*if (_detectedEnemy != null)
            return true;

        // ���� ī�޶� ���̴� ��ü ���� Ž��

        //Rect screenRect = new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
        Collider2D[] detectedEnemys = Physics2D.OverlapAreaAll(_bottomLeft,_topRight, _enemyLayer);

        if (detectedEnemys.Length > 0)
        {
            _detectedEnemy = detectedEnemys[0].transform;
            return true;
        }

        return false;*/
        
/*// ���� ���� Ž���� ���� �켱������ ������ ���
Collider2D[] detectedColliders = Physics2D.OverlapCircleAll(transform.position, _detectRange, _enemyLayer);
if (detectedColliders.Length > 0)
{
    _detectedEnemy = detectedColliders[0].transform;
    return true;
}
_detectedEnemy = null;
return false;*/