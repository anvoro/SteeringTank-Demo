
using System.Collections.Generic;
using Tank.Interfaces.Body;
using UnityEngine;

namespace Tank.Interfaces.TargetProvider
{
    public interface IDataProvider
    {
        void Init(IDynamicBody drivenBody);
        IDynamicBody GetDrivenBody();
        IDynamicBody GetTargetBody();
        Vector3 GetFutureTargetPosition(float predictionMultiplier);
        IEnumerable<IDynamicBody> GetNeighborBodies();
    }
}
