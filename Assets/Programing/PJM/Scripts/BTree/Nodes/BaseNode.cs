namespace Programing.PJM.Scripts.BTree.Nodes
{
    public abstract class BaseNode
    {
        public enum ENodeState // 노드 평가 결과
        {
            Running, // 수행중
            Success, // 성공
            Failure, // 실패
        }

        public abstract ENodeState Evaluate(); // 위의 평가 결과를 반환하는 평가 메서드

        public virtual void ResetNode() { } // 노드 리셋용 메서드
    }
}
