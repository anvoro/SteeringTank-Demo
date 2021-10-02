
using System;
using Tank.BodySystem.PlayerBody;
using Tank.WeaponSystem;
using UnityEngine;

namespace Tank.InputSystem
{
    internal sealed class PlayerInput : MonoBehaviour, IMoveInput, IWeaponInput
    {
        private Camera _mainCamera;

        [SerializeField]
        private string _horizontal = "Horizontal";

        [SerializeField]
        private string _vertical = "Vertical";

        [SerializeField]
        private KeyCode _mainFireButton = KeyCode.Space;

        [SerializeField]
        private KeyCode _additionalFireButton = KeyCode.X;

        [SerializeField]
        private KeyCode _changeWeaponButtonForward = KeyCode.E;

        [SerializeField]
        private KeyCode _changeWeaponButtonBack = KeyCode.Q;

        public event Action Fire;
        public event Action<bool> ChangeWeapon;

        public float Acceleration => Input.GetAxis(this._vertical);

        public float Rotation => Input.GetAxis(this._horizontal);

        public Vector3 Target
        {
            get
            {
                Ray ray = this._mainCamera.ScreenPointToRay(Input.mousePosition);

                return Physics.Raycast(ray, out RaycastHit hit) ? hit.point : Vector3.zero;
            }
        }

        private void Awake()
        {
            this._mainCamera = Camera.main;
        }

        private void Update()
        {
            if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(this._mainFireButton) || Input.GetKeyDown(this._additionalFireButton))
                this.Fire?.Invoke();

            if (Input.GetKeyDown(this._changeWeaponButtonForward))
            {
                this.ChangeWeapon?.Invoke(true);
            }
            else if (Input.GetKeyDown(this._changeWeaponButtonBack))
            {
                this.ChangeWeapon?.Invoke(false);
            }
        }
    }
}
