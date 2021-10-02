
using UnityEngine;

namespace Tank.View.VFX
{
    internal class ParticleSystemWrapper : MonoBehaviour
    {
        private ParticleSystem _vfx;

        public ParticleSystemWrapper Origin { get; set; }

        private void Awake()
        {
            this._vfx = this.GetComponent<ParticleSystem>();
        }

        private void OnEnable()
        {
            this._vfx.Play(true);

            this.Invoke("OnVFXStop", this._vfx.main.duration);
        }

        private void OnDisable()
        {
            this._vfx.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        private void OnVFXStop()
        {
            this.gameObject.SetActive(false);

            VFXSpawner.ReturnVFX(this);
        }
    }
}
