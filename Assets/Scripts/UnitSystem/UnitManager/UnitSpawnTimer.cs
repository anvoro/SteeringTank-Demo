
namespace Tank.UnitSystem.UnitManager
{
    internal sealed class UnitSpawnTimer
    {
        private readonly float _spawnDelay;

        private float _spawnTimer;

        public UnitSpawnTimer(float spawnDelay)
        {
            this._spawnDelay = spawnDelay;
            this.Reset();
        }

        public void Tick(float deltaTime, out bool isZero)
        {
            this._spawnTimer -= deltaTime;

            if (this._spawnTimer <= 0)
            {
                isZero = true;
            }
            else
            {
                isZero = false;
            }
        }

        public void Reset()
        {
            this._spawnTimer = this._spawnDelay;
        }
    }
}
