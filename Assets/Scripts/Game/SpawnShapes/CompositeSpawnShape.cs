
using UnityEngine;

namespace Tank.Game.SpawnShapes
{
    internal class CompositeSpawnShape : MonoBehaviour, ISpawnShape
    {
        [SerializeField]
        private SpawnRect[] _childSpawners;

        public Vector3 GetRandomPoint(Vector3 playerPosition)
        {
            SpawnRect fartherSpawner = this._childSpawners[0];
            for (int i = 1; i < this._childSpawners.Length; i++)
            {
                SpawnRect spawnRect = this._childSpawners[i];
                if (Vector3.Distance(playerPosition, spawnRect.Center) > Vector3.Distance(playerPosition, fartherSpawner.Center))
                    fartherSpawner = spawnRect;
            }

            return fartherSpawner.GetRandomPoint(playerPosition);
        }
    }
}
