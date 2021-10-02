
using System.Collections.Generic;
using Tank.Environment.Interfaces;
using Tank.Helpers;
using Tank.Interfaces.Body;
using UnityEngine;

namespace Tank.SpatialStorage
{
    [DisallowMultipleComponent]
    internal class QuadTree : MonoBehaviour, ISpatialStorage
    {
        public const int MAX_BODIES_PER_NODE = 6;
        public const int MAX_LEVEL = 5;

        private readonly Stack<QuadTreeNode> _checkConsistencyStack = new Stack<QuadTreeNode>();

        private QuadTreeNode _treeParent;

        [SerializeField]
        private BoxCollider _boxCollider;

        private void Awake()
        {
            Rect treeRect = this._boxCollider.GetRectInXZAxis();

            this._treeParent = new QuadTreeNode(treeRect);
        }

        public void Build(IEnumerable<IBody> bodies)
        {
            this._treeParent.Build(bodies);
        }

        public void AddBody(IBody body)
        {
            this._treeParent.AddBody(body);
        }

        public IEnumerable<IBody> GetBodiesInRadius(Vector2 point, float radius, BodyType bodyType)
        {
            return this._treeParent.GetBodiesInRadius(point, radius, bodyType);
        }

        public bool CheckConsistency()
        {
            this._checkConsistencyStack.Clear();
            this._checkConsistencyStack.Push(this._treeParent);

            while (this._checkConsistencyStack.Count > 0)
            {
                QuadTreeNode node = this._checkConsistencyStack.Pop();

                if (node.CheckConsistency() == false)
                {
                    return false;
                }

                foreach (QuadTreeNode child in node.Children)
                {
                    this._checkConsistencyStack.Push(child);
                }
            }

            return true;
        }

        private void OnDrawGizmos()
        {
            this._treeParent?.DrawGizmos();
        }
    }
}
