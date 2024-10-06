using System;

namespace Manipulator2D
{
    public interface IActuator
    {
        double LowerLimit { get; }
        double UpperLimit { get; }
        double IsValue { get; set; }
        event EventHandler ValueChanged;
    }
}
