
using System.Collections.Generic;
using Tank.Interfaces.Body;

namespace Tank.Game.Interfaces
{
    public interface IEnvironment
    {
        void AddBody(IBody body);
        void RemoveBody(IBody body);
        IEnumerable<IBody> GetMovableAround(IBody observer, float radius, bool includeSelf);
    }
}
