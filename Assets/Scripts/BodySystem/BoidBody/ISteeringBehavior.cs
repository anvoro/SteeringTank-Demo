
using Tank.Interfaces.DataProvider;
using UnityEngine;

namespace Tank.BodySystem.BoidBody
{
    public interface ISteeringBehavior
    {
        void Init(IDataProvider dataProvider);
        Vector3 GetVelocity(float deltaTime);
    }
}
