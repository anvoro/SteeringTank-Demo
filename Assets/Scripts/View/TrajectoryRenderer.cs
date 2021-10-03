
using Tank.Interfaces.Weapon;
using UnityEngine;

namespace Tank.View
{
    [RequireComponent(typeof(LineRenderer))]
    internal class TrajectoryRenderer : MonoBehaviour
    {
        private LineRenderer _lineRenderer;
        private GameObject _cross;

        private Vector3[] _points;

        private IWeaponView _provider;

        [SerializeField]
        private GameObject _crossPrefab;

        [SerializeField]
        private LayerMask _layerMask;

        private void Awake()
        {
            this._points = new Vector3[100];

            this._lineRenderer = GetComponent<LineRenderer>();
            this._lineRenderer.positionCount = this._points.Length;

            this._provider = this.GetComponent<IWeaponView>();

            this._cross = Instantiate(this._crossPrefab, this.transform);
        }

        private void Update()
        {
            this.DrawTrajectory();
        }

        private void DrawTrajectory()
        {
            this._points[0] = this._provider.FireTransform.position;

            for (int i = 1; i < this._points.Length; i++)
            {
                float time = i * 0.1f;

                this._points[i] = this._provider.FireTransform.position + this._provider.VelocityVector * time + time * time * Physics.gravity / 2f;

                if (Physics.Linecast(this._points[i - 1], this._points[i], out RaycastHit hit, this._layerMask))
                {
                    this._lineRenderer.positionCount = i + 1;
                    drawCross(hit.point);
                    break;
                }
            }

            this._lineRenderer.SetPositions(this._points);

            void drawCross(Vector3 position)
            {
                position = new Vector3(position.x, .1f, position.z);
                this._cross.transform.position = position;
                this._cross.transform.localScale = Vector3.one * (this._provider.ExplosionRadius > 0 ? this._provider.ExplosionRadius : 1f);
            }
        }
    }
}
