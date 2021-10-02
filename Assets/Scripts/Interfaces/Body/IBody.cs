
using System;
using UnityEngine;

namespace Tank.Interfaces.Body
{
    [Flags]
    public enum BodyType
    {
        Static = 0,
        Movable = 1 << 0,
    }

    public interface IBody
    {
        string Name { get; }
        Vector2 Position2D { get; }
        Vector3 Position { get; }
        BodyType BodyType { get; }
    }
}
