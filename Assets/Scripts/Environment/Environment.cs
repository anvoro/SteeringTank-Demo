
using System.Collections.Generic;
using Tank.Environment.Interfaces;
using Tank.Game.Interfaces;
using Tank.Interfaces.Body;
using UnityEngine;

namespace Tank.Environment
{
    [DisallowMultipleComponent]
    public sealed class Environment : MonoBehaviour, IEnvironment
    {
        private readonly HashSet<IBody> _obstacles = new HashSet<IBody>();
        private readonly HashSet<IBody> _bodies = new HashSet<IBody>();

        private bool _isConsistent;

        private ISpatialStorage _bodyStorage;

        [SerializeField]
        [RequiredInterface(typeof(ISpatialStorage))]
        private Object _bodyStorageObject;

        [SerializeField]
        private Transform _obstaclesParent;

        private void Awake()
        {
            this._bodyStorage = (ISpatialStorage)this._bodyStorageObject;

            foreach (IBody obstacle in this._obstaclesParent.GetComponentsInChildren<IBody>())
            {
                this._obstacles.Add(obstacle);
            }
        }

        private void FixedUpdate()
        {
            if (this._isConsistent == false || this._bodyStorage.CheckConsistency() == false)
            {
                this._bodyStorage.Build(getBodiesInWorld());
                this._isConsistent = true;
            }

            IEnumerable<IBody> getBodiesInWorld()
            {
                foreach (IBody body in this._obstacles)
                {
                    yield return body;
                }

                foreach (IBody body in this._bodies)
                {
                    yield return body;
                }
            }
        }

        public void AddBody(IBody body)
        {
            this._bodies.Add(body);

            this._isConsistent = false;
        }

        public void RemoveBody(IBody body)
        {
            this._bodies.Remove(body);

            this._isConsistent = false;
        }

        public IEnumerable<IBody> GetMovableAround(IBody observer, float radius, bool includeSelf)
        {
            foreach(IBody body in this._bodyStorage.GetBodiesInRadius(observer.Position, radius, BodyType.Movable))
            {
                if (includeSelf == false && body == observer)
                    continue;

                yield return body;
            }
        }
    }
}
