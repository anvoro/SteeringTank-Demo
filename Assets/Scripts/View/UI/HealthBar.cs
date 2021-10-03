
using Tank.Interfaces.Unit;
using UnityEngine;
using UnityEngine.UI;

namespace Tank.View.UI
{
    internal class HealthBar : MonoBehaviour
    {
        protected IUnit _unit;

        [SerializeField]
        protected Slider _bar;

        [SerializeField]
        private Image _fill;

        [SerializeField]
        private Gradient _healthGradient;

        protected virtual void Awake() { }

        public virtual void Init(IUnit unit)
        {
            this._bar.value = 1f;

            this._unit = unit;
            this._unit.OnHealthChange += OnHealthChange;

            this.OnHealthChange();

            this.gameObject.SetActive(true);
        }

        protected void OnHealthChange()
        {
            float hpNormalized = this._unit.CurrentHealth / (float)this._unit.MaxHealth;

            this._bar.value = hpNormalized;
            this._fill.color = this._healthGradient.Evaluate(hpNormalized);
        }
    }
}
