
using Tank.Interfaces.Unit;
using UnityEngine;

namespace Assets.Scripts.View.UI
{
    internal class MovableHealthBar : HealthBar
    {
        private RectTransform _parent;

        public Transform UnitHudPoint { get; set; }

        public override void Init(IUnit unit)
        {
            unit.OnUnitActiveStateChange += OnUnitActiveStateChange;

            base.Init(unit);
        }

        protected override void Awake()
        {
            this._parent = transform.parent.GetComponent<RectTransform>();

            base.Awake();
        }

        private void OnDisable()
        {
            this._bar.value = 1f;

            this.Unsubscribe();
        }

        private void LateUpdate()
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(this.UnitHudPoint.position);

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(this._parent, screenPoint, null, out Vector2 localPoint))
            {
                this.transform.localPosition = localPoint;
            }
        }

        private void Unsubscribe()
        {
            this._unit.OnHealthChange -= OnHealthChange;
            this._unit.OnUnitActiveStateChange -= OnUnitActiveStateChange;
        }

        private void OnUnitActiveStateChange(bool active)
        {
            if (active == true)
                return;

            this.Unsubscribe();
            this.gameObject.SetActive(false);
        }
    }
}
