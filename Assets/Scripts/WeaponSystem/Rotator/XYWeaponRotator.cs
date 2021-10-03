
using UnityEngine;

namespace Tank.WeaponSystem.Rotator
{
    internal enum TrajectoryType
    {
        Low = 0,
        High = 1,
    }

    internal class XYWeaponRotator : WeaponRotator
    {
        [SerializeField]
        private TrajectoryType _trajectoryType = TrajectoryType.Low;

        [SerializeField]
        private float _minXAngle = -360f;

        [SerializeField]
        private float _maxXAngle = 360f;

        public override void RotateToTarget(Vector3 target, float deltaTime)
        {
            this.SetFireAngle(target);

            base.RotateToTarget(target, deltaTime);
        }

        //todo: replace to Quaternion
        private void SetFireAngle(Vector3 target)
        {
            float fireAngle = this.CalculateFireAngle(target);

            Quaternion rotation = Quaternion.Euler(fireAngle, 0f, 0f);

            if (wrapAngle(rotation.eulerAngles.x) > this._maxXAngle)
            {
                this._weapon.FireTransform.localRotation = Quaternion.Euler(this._maxXAngle, 0f, 0f);
            }
            else if (wrapAngle(rotation.eulerAngles.x) < this._minXAngle)
            {
                this._weapon.FireTransform.localRotation = Quaternion.Euler(this._minXAngle, 0f, 0f);
            }
            else
            {
                this._weapon.FireTransform.localRotation = rotation;
            }

            float wrapAngle(float angle)
            {
                if (angle > 180)
                    return angle - 360;

                return angle;
            }
        }

        private float CalculateFireAngle(Vector3 target)
        {
            Vector3 fire = this._weapon.FireTransform.position - target;
            Vector3 fireXZ = new Vector3(fire.x, 0f, fire.z);

            float g = Physics.gravity.y;

            float v2 = this._weapon.Velocity * this._weapon.Velocity;

            float x = fireXZ.magnitude;
            float y = fire.y;

            float a0 = v2 * v2 - g * (g * x * x + 2 * y * v2);
            if (a0 < 0)
            {
                a0 = 0;
                //Debug.LogError($"For {this._drivenBody.Name} launch Velocity too small for current fire distance");
            }

            float a = Mathf.Sqrt(a0);

            if (this._trajectoryType == TrajectoryType.High)
                a = v2 + a;
            else
                a = v2 - a;

            float b = g * x;

            return Mathf.Atan2(a, b) * Mathf.Rad2Deg - 180f;
        }
    }
}
