
using Tank.Interfaces.Body;
using UnityEngine;

namespace Tank.BodySystem
{
    [DisallowMultipleComponent]
    public class ObstacleBody : MonoBehaviour, IBody
    {
        public string Name => gameObject.name;

        public Vector2 Position2D => new Vector2(this.transform.position.x, this.transform.position.z);
        public Vector3 Position => this.transform.position;

        public BodyType BodyType => BodyType.Static;
    }
}
