
using Tank.Interfaces.View;
using Tank.View.VFX;

namespace Tank.View.VFX
{
    internal class StandaloneVFXSpawner : VFXSpawner
    {
        protected virtual void Awake()
        {
            this.GetComponent<IStandaloneVFXEvent>().OnStandaloneVFXEvent += this.SpawnVFX;
        }
    }
}
