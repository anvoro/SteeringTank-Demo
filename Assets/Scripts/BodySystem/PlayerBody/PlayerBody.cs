
using UnityEngine;

namespace Tank.BodySystem.PlayerBody
{
    internal class PlayerBody : DynamicBody
    {
        private IMoveInput _moveInput;

        [SerializeField]
        [RequiredInterface(typeof(IMoveInput))]
        private MonoBehaviour _moveProviderObject;

        protected override void Awake()
        {
            this._moveInput = (IMoveInput)this._moveProviderObject;

            base.Awake();
        }

        protected override void Tick(float deltaTime)
        {
            this.Move(this._moveInput.Acceleration);
            this.Turn(this._moveInput.Rotation);
        }

        private void Turn(float value)
        {
            float turn = value * this._turnSpeed;
            this._rigidbody.AddRelativeTorque(0f, turn, 0f);
        }

        private void Move(float value)
        {
            Vector3 move = Vector3.forward * value * this._maxSpeed;

            this._rigidbody.AddRelativeForce(move);
        }
    }
}
