using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitControllerNoProjectile : PlayableBaseUnitController
{
    protected override void Awake()
    {
        base.Awake();
        SkillTargets = new List<BaseUnitController>();
        CoolTimeCounter = 0.5f;
    }

    protected override void Start()
    {
        base.Start();
        // 추가로 해줄 동작 설정
    }


    protected override BaseNode SetBTree()
    {
        return new SelectorNode //Behaviour Selector
        (
            new List<BaseNode>
            {
                new SelectorNode
                (
                    new List<BaseNode>
                    {
                        new ActionNode(CheckBattleWin),
                        new ActionNode(CheckUnitDying),
                        new ActionNode(CheckCrowdControl),
                    }
                ),

                new SelectorNode // skillable Dicision Selector
                (
                    new List<BaseNode>
                    {
                        new DecoratorNode
                        (
                            new ConditionNode(IsSkillAlreadyRunning),
                            UniqueSkill.CreatePerformNode(this, SkillTargets)
                        ),
                        new SequenceNode // skillable Dicision Sequence
                        (
                            new List<BaseNode>()
                            {
                                new ConditionNode(CheckSkillCooltimeBack),
                                new SelectorNode // condition Selector
                                (
                                    new List<BaseNode>
                                    {
                                        new ConditionNode(CheckAutoOn),
                                        new ConditionNode(CheckUserInput)
                                    }
                                ),
                                UniqueSkill.CreateSkillBTree(this, SkillTargets)
                            }
                        ),
                    }
                ),

                new SequenceNode // Attack Dicision
                (
                    new List<BaseNode>
                    {
                        new ConditionNode(CheckAttackRange),
                        new ActionNode(SetTargetToAttack),
                        new ActionNode(PerformAttack)
                    }
                ),
                new SequenceNode
                (
                    new List<BaseNode>
                    {
                        new ActionNode(SetDetectedTarget),
                        // CheckMoveable ?
                        new ActionNode(ChaseTarget)
                    }
                ),
                new ActionNode(StayIdle)
            }
        );
    }


// skill

    /*
    public override BaseNode.ENodeState UseSkill()
    {
        // 배틀 매니저에서 컷신 띄우기 및 타임스케일 조정
        // 애니메이션이 끝났을 때 Success 반환?
        
        Debug.Log($"전사 유닛  스킬 사용! "); //{UnitID}
        return BaseNode.ENodeState.Success;
    }*/

    /*protected override BaseNode.ENodeState SetTargetToSkill() // 임시 스킬
    {
        if (SkillTarget != null)
        {
            //Debug.Log($"스킬타겟 있음 : {SkillTarget.gameObject.name}");
            return BaseNode.ENodeState.Success;
        }
        Debug.Log("스킬타겟 없음 찾아야함");
           
        
        Collider2D[] detectedColliders = Physics2D.OverlapAreaAll(_bottomLeft,_topRight, _enemyLayer);

        if (detectedColliders.Length == 0)
        {
            SkillTarget = null;
            return BaseNode.ENodeState.Failure;
        }
        
        float maxDistance = float.MinValue;
        Transform farthestEnemy = null;

        foreach (Collider2D col in detectedColliders)
        {
            float distance = Vector2.Distance(transform.position, col.transform.position);

            if (distance > maxDistance)
            {
                maxDistance = distance;
                farthestEnemy = col.transform;
            }
        }
        
        SkillTarget = farthestEnemy;
        Debug.Log($"스킬타겟 있음 : {SkillTarget.gameObject.name}");
        return BaseNode.ENodeState.Success;
            
    }


    // 스킬의 조건과 타겟대상은 스킬별로 상이함
    // 스킬 종류를 따로 클래스에 정할 필요가 있음
    // 스킬범위와 스킬타겟은?
    // 스킬에서 스킬 대상이 없으면 Failure -> 
    // 임시로 근접 공격범위 내의 대상에게 스킬을 쓰도록 함
    protected override BaseNode.ENodeState PerformSkill(string animationName)
    {
        // 스킬 타겟을 따로 둬야함
        // 비활성화도 인식아 필요할경우 activeInHierarchy나 activeSelf사용
        if (SkillTarget == null)
        {
            SkillTriggered = false;
            return BaseNode.ENodeState.Failure;
        }
            
        
        if (!SkillTriggered)
        {
            SkillTriggered = true;
            UnitViewer.UnitAnimator.SetTrigger("Skill");
            Debug.Log($"******{SkillTarget.gameObject.name}에 밀리 스킬 공격 시작!******");
            StartCoroutine(ResetSkillTrigger(animationName));
            return BaseNode.ENodeState.Running;
        }
        if (UnitViewer.IsAnimationRunning(animationName))
        {
            Debug.Log($"스킬 진행중 어택트리거 상태 : {SkillTriggered}");
            return BaseNode.ENodeState.Running;
        }
        if(!SkillTriggered)
        {
            Debug.Log($"스킬 종료됨 스킬트리거 상태 : {SkillTriggered}");
            //UnitAnimator.ResetTrigger("Skill");
            return BaseNode.ENodeState.Success;
        }

        return BaseNode.ENodeState.Failure;
    }*/
    
    // condition
    /*protected override bool CheckSkillRange()
    {
        if (DetectedEnemy == null || !DetectedEnemy.gameObject.activeSelf)
            return false;
        float sqrDistance = Vector2.SqrMagnitude(DetectedEnemy.position - transform.position);
        return sqrDistance <= SkillRange * SkillRange;
    }*/
}
