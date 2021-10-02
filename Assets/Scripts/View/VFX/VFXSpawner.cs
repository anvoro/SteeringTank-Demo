
using Tank.Helpers.ObjectPool;
using UnityEngine;

namespace Tank.View.VFX
{
    internal class VFXSpawner : MonoBehaviour
    {
        private static Transform vfxParent;

        private static Transform VFXParent => vfxParent ??= GameObject.FindGameObjectWithTag("VFXParent").transform;

        private static readonly MultiKeyObjectPool<ParticleSystemWrapper, ParticleSystemWrapper> pool = new MultiKeyObjectPool<ParticleSystemWrapper, ParticleSystemWrapper>(CreateVFX);

        [SerializeField]
        private ParticleSystemWrapper _vfx;

        private static ParticleSystemWrapper CreateVFX(ParticleSystemWrapper system)
        {
            ParticleSystemWrapper prefab = Instantiate(system, VFXParent);
            prefab.Origin = system;

            return prefab;
        }

        public static void ReturnVFX(ParticleSystemWrapper vfx)
        {
            pool.Return(vfx.Origin, vfx);
        }

        public void SpawnVFX()
        {
            ParticleSystemWrapper prefab = pool.GetOrCreate(this._vfx);

            Vector3 position = this.transform.position;
            if (position.y <= 0)
                position = new Vector3(position.x, .1f, position.z);

            prefab.transform.SetPositionAndRotation(position, this.transform.rotation);
            prefab.gameObject.SetActive(true);
        }
    }
}
