
using System;
using System.Collections.Generic;
using Tank.Environment.Interfaces;
using Tank.Interfaces.Body;
using UnityEngine;

namespace Tank.SpatialStorage
{
    internal sealed class QuadTreeNode : ISpatialStorage
    {
        private static class QuadNodePool
        {
            private const int INIT_POOL_COUNT = 16;

            private static readonly Stack<QuadTreeNode> nodePool = new Stack<QuadTreeNode>();

            static QuadNodePool()
            {
                for (int i = 0; i < INIT_POOL_COUNT; i++)
                {
                    nodePool.Push(new QuadTreeNode(Rect.zero));
                }
            }

            public static QuadTreeNode GetNode(Rect bounds, int parentLevel)
            {
                QuadTreeNode node;

                if (nodePool.Count > 0)
                {
                    node = nodePool.Pop();

                    node._bounds = bounds;
                    node._curLevel = parentLevel + 1;
                }
                else
                {
                    node = new QuadTreeNode(bounds, parentLevel);
                }

                return node;
            }

            public static void ReturnNode(QuadTreeNode node)
            {
                if (node == null)
                    return;

                node.Clear();
                nodePool.Push(node);
            }
        }

        public QuadTreeNode(Rect bounds)
        {
            this._bounds = bounds;

            this._bodies = new List<IBody>(QuadTree.MAX_BODIES_PER_NODE);
            this._children = new List<QuadTreeNode>(4);
        }

        private QuadTreeNode(Rect bounds, int parentLevel) : this(bounds)
        {
            this._curLevel = parentLevel + 1;
        }

        private readonly List<IBody> _bodies;
        private readonly List<QuadTreeNode> _children;

        private Rect _bounds;
        private int _curLevel;

        public IEnumerable<QuadTreeNode> Children => this._children;

        public bool CheckConsistency()
        {
            foreach (IBody body in this._bodies)
            {
                if (this._bounds.Contains(body.Position2D) == false)
                    return false;
            }

            return true;
        }

        public void Build(IEnumerable<IBody> bodies)
        {
            this.Clear();

            foreach (IBody body in bodies)
            {
                this.AddBody(body);
            }
        }

        public void AddBody(IBody body)
        {
            if (this._children.Count > 0)
            {
                QuadTreeNode child = this.GetQuadrant(body.Position2D);
                child.AddBody(body);
            }
            else
            {
                this._bodies.Add(body);
                if (this._bodies.Count > QuadTree.MAX_BODIES_PER_NODE && this._curLevel < QuadTree.MAX_LEVEL)
                {
                    this.Split();
                }
            }
        }

        public IEnumerable<IBody> GetBodiesInRadius(Vector2 point, float radius, BodyType bodyType)
        {
            if (this._children.Count > 0)
            {
                foreach (QuadTreeNode child in this._children)
                {
                    if (child.ContainsCircle(point, radius))
                    {
                        foreach (IBody body in child.GetBodiesInRadius(point, radius, bodyType))
                            if (checkType(body, bodyType))
                                yield return body;
                    }
                }
            }
            else
            {
                foreach (IBody body in this._bodies)
                {
                    if (checkType(body, bodyType))
                        yield return body;
                }
            }

            bool checkType(IBody body, BodyType bodyType)
            {
                return body.BodyType.HasFlag(bodyType);
            }
        }

        private void Clear()
        {
            foreach (QuadTreeNode child in this._children)
            {
                QuadNodePool.ReturnNode(child);
            }

            this._children.Clear();
            this._bodies.Clear();
        }

        private bool ContainsCircle(Vector2 circleCenter, float radius)
        {
            Vector2 center = this._bounds.center;

            float dx = Math.Abs(circleCenter.x - center.x);
            float dy = Math.Abs(circleCenter.y - center.y);

            if (dx > (this._bounds.width / 2 + radius))
                return false;

            if (dy > (this._bounds.height / 2 + radius))
                return false;

            if (dx <= (this._bounds.width / 2))
                return true;

            if (dy <= (this._bounds.height / 2))
                return true;

            double cornerDist = Math.Pow((dx - this._bounds.width / 2), 2) + Math.Pow((dy - this._bounds.height / 2), 2);

            return cornerDist <= (radius * radius);
        }

        private void Split()
        {
            float hx = this._bounds.width / 2;
            float hz = this._bounds.height / 2;
            Vector2 sz = new Vector2(hx, hz);

            Vector2 aLoc = this._bounds.position;
            Rect aRect = new Rect(aLoc, sz);

            Vector2 bLoc = new Vector2(this._bounds.position.x + hx, this._bounds.position.y);
            Rect bRect = new Rect(bLoc, sz);

            Vector2 cLoc = new Vector2(this._bounds.position.x + hx, this._bounds.position.y + hz);
            Rect cRect = new Rect(cLoc, sz);

            Vector2 dLoc = new Vector2(this._bounds.position.x, this._bounds.position.y + hz);
            Rect dRect = new Rect(dLoc, sz);

            this._children.Add(QuadNodePool.GetNode(aRect, this._curLevel));
            this._children.Add(QuadNodePool.GetNode(bRect, this._curLevel));
            this._children.Add(QuadNodePool.GetNode(cRect, this._curLevel));
            this._children.Add(QuadNodePool.GetNode(dRect, this._curLevel));

            for (int i = this._bodies.Count - 1; i >= 0; i--)
            {
                QuadTreeNode child = this.GetQuadrant(this._bodies[i].Position2D);
                child.AddBody(this._bodies[i]);
                this._bodies.RemoveAt(i);
            }
        }

        private QuadTreeNode GetQuadrant(Vector2 point)
        {
            if (this._children.Count == 0)
                return null;

            if (point.x > this._bounds.x + this._bounds.width / 2)
            {
                return point.y > this._bounds.y + this._bounds.height / 2 ? this._children[2] : this._children[1];
            }

            return point.y > this._bounds.y + this._bounds.height / 2 ? this._children[3] : this._children[0];
        }

        public void DrawGizmos()
        {
            Gizmos.color = Color.cyan;

            foreach (QuadTreeNode child in this._children)
            {
                child.DrawGizmos();
            }

            drawRect();

            void drawRect()
            {
                Vector3 p1 = new Vector3(this._bounds.position.x, 0.1f, this._bounds.position.y);
                Vector3 p2 = new Vector3(p1.x + this._bounds.width, 0.1f, p1.z);
                Vector3 p3 = new Vector3(p1.x + this._bounds.width, 0.1f, p1.z + this._bounds.height);
                Vector3 p4 = new Vector3(p1.x, 0.1f, p1.z + this._bounds.height);

                Gizmos.DrawLine(p1, p2);
                Gizmos.DrawLine(p2, p3);
                Gizmos.DrawLine(p3, p4);
                Gizmos.DrawLine(p4, p1);
            }
        }
    }
}
