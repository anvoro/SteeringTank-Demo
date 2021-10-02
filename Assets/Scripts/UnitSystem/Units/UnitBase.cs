
using System;
using Assets.Scripts.UnitSystem.DamageControlSystem;
using Tank.Configs;
using Tank.Game;
using Tank.Interfaces.Body;
using Tank.Interfaces.Unit;
using UnityEngine;

namespace Tank.UnitSystem.Units
{
    [DisallowMultipleComponent]
    internal abstract class UnitBase : MonoBehaviour, IUnit
    {
        private UnitConfig _config;

        private int _currentHealth;
        private bool _isDead;

        private IDynamicBody _body;

        [SerializeField]
        [RequiredInterface(typeof(IDynamicBody))]
        private MonoBehaviour _bodyObject;

        [SerializeField]
        private DamageControlStrategyBase _damageControlStrategy;

        public event Action<bool> OnUnitActiveStateChange;
        public event Action<IUnit> OnUnitDeath;
        public event Action<IUnit> OnCollideWithUnit;
        public event Action OnHealthChange;

        public UnitConfig Config => this._config;

        public string Name => this._body.Name;
        public UnitTeam Team => this._config.Team;

        public bool IsPlayer => ReferenceEquals(World.Player, this);

        public int MaxHealth { get; private set; }

        public int CurrentHealth
        {
            get => this._currentHealth;
            private set
            {
                this._currentHealth = value;

                if (this._currentHealth <= 0)
                {
                    this._currentHealth = 0;
                    this.IsDead = true;
                }

                this.OnHealthChange?.Invoke();
            }
        }

        public bool IsDead
        {
            get => this._isDead;
            private set
            {
                this._isDead = value;

                if(this._isDead == true)
                {
                    this.OnUnitDeath?.Invoke(this);
                }
            }
        }

        public IDynamicBody Body => this._body;

        private void OnCollisionEnter(Collision collision)
        {
            IUnit unit = collision.gameObject.GetComponent<IUnit>();
            if (unit != null)
            {
                this.OnCollideWithUnit?.Invoke(unit);
            }
        }

        public void Init(UnitConfig config)
        {
            this._config = config;
            this._body = (IDynamicBody)this._bodyObject;
        }

        public virtual void ResetUnit()
        {
            this.MaxHealth = this._config.MaxHealth;
            this._currentHealth = this.MaxHealth;

            this.IsDead = false;
        }

        public void Hurt(int value)
        {
            int reducedValue = this._damageControlStrategy.CalculateDamage(value, this._config.Armor);
            this.CurrentHealth -= reducedValue;
        }

        public void Kill()
        {
            this.CurrentHealth = 0;
        }

        public void SetActive(bool active)
        {
            this.gameObject.SetActive(active);
            this.OnUnitActiveStateChange?.Invoke(active);
        }
    }
}
