
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
            this.Turn(this._moveInput.Rotation);
            this.Move(this._moveInput.Acceleration);
        }

        private void Turn(float value)
        {
            float turn = value * this._turnSpeed;
            this._rigidbody.AddTorque(0f, turn, 0f);
        }

        private void Move(float value)
        {
            Vector3 move = this._maxSpeed * value * Vector3.forward;
            this._rigidbody.AddRelativeForce(move);
        }
    }
}
