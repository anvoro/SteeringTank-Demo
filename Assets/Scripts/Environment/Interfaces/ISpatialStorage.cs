
using System.Collections.Generic;
using Tank.Interfaces.Body;
using UnityEngine;

namespace Tank.Environment.Interfaces
{
    public interface ISpatialStorage
    {
        void Build(IEnumerable<IBody> bodies);
        void AddBody(IBody body);
        bool CheckConsistency();
        IEnumerable<IBody> GetBodiesInRadius(Vector2 point, float radius, BodyType bodyType);
    }
}
