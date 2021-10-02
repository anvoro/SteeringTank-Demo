
using Assets.Scripts.View.UI;
using UnityEngine;

namespace Assets.Scripts.View
{
    internal class CameraController : MonoBehaviour
    {
        public float _dampTime = 0.2f;

        private Vector3 _velocity;

        private void LateUpdate()
        {
            this.Move();
        }
        
        private void Move()
        {
            Vector3 position = Vector3.SmoothDamp(transform.position, BattleUI.PlayerPosition, ref _velocity, _dampTime); ;
            position.y = this.transform.position.y;
            transform.position = position;
        }

        public void SetStartPositionAndSize()
        {
            transform.position = BattleUI.PlayerPosition;
        }
    }
}
