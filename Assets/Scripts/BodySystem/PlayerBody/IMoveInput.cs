
using UnityEngine;

namespace Tank.BodySystem.PlayerBody
{
    public interface IMoveInput
    {
        float Acceleration { get; }
        float Rotation { get; }
    }
}
