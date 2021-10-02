
using Tank.Interfaces.Body;

namespace Tank.SteeringBehaviors.SteeringControl
{
    public interface ISteeringRule
    {
        bool ProcessRule(IDynamicBody drivenBody);
    }
}
