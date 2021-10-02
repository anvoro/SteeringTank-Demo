
using System.Collections.Generic;
using UnityEngine;
using Tank.Helpers;

namespace Tank.Game.SpawnShapes
{
    internal class SpawnRect : MonoBehaviour, ISpawnShape
    {
        private readonly List<Vector3> _resultVariants = new List<Vector3>(4);

        private BoxCollider _boxCollider;

        public Vector3 Center => transform.TransformPoint(this._boxCollider.center);

        private void Awake()
        {
            this._boxCollider = this.GetComponent<BoxCollider>();
        }

        public Vector3 GetRandomPoint(Vector3 playerPosition)
        {
            this._resultVariants.Clear();
            for (int i = 0; i < 4; i++)
            {
                this._resultVariants.Add(this._boxCollider.GetRandomPointInsideCollider());
            }

            Vector3 fartherVariant = this._resultVariants[0];
            for (int i = 1; i < this._resultVariants.Count; i++)
            {
                Vector3 variant = this._resultVariants[i];
                if (Vector3.Distance(variant, playerPosition) > Vector3.Distance(fartherVariant, playerPosition))
                    fartherVariant = variant;
            }

            return fartherVariant;
        }
    }
}
