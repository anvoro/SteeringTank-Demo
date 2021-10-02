
using Tank.Interfaces.Unit;
using UnityEngine;

namespace Tank.WeaponSystem.Reactions
{
    public class SelfDestructReaction : MonoBehaviour
    {
        private IUnit _parent;

        [SerializeField]
        private int _damage = 1;

        [SerializeField]
        private float _explosionForce = 1000f;

        [SerializeField]
        private float _explosionRadius = 5f;

        private void Awake()
        {
            this._parent = this.GetComponent<IUnit>();
            this._parent.OnCollideWithUnit += this.CollideWithUnit;
        }

        private void CollideWithUnit(IUnit unit)
        {
            if (unit.Team != this._parent.Team)
            {
                unit.Hurt(this._damage);
                unit.Body.AddExplosionForce(this._explosionForce, this.transform.position, this._explosionRadius);

                this._parent.Kill();
            }
        }
    }
}
