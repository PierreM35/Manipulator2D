using System;
using System.Collections.Generic;

namespace Manipulator2D.Transfos
{
    public class RevoluteMappingTransfo : RevoluteTransfo
    {
        private KeyValuePair<double, double> _origin;
        private KeyValuePair<double, double> _target;

        public RevoluteMappingTransfo(
            string name,
            KeyValuePair<double, double> origin,
            KeyValuePair<double, double> target,
            IActuator actuator,
            string resultingNodeName = "") : 
            base(name, actuator, resultingNodeName)
        {
            _origin = origin;
            _target = target;
            UpdateTransfoMatrix(MapValue(actuator.IsValue));
        }

        protected override void OnActuatorValueChanged(object actuator, EventArgs e)
        {
            UpdateTransfoMatrix(MapValue(((IActuator)actuator).IsValue));
        }

        private double MapValue(double isValue)
        {
            return _target.Key + (_target.Value - _target.Key) * 
                Math.Abs(isValue - _origin.Key) / Math.Abs(_origin.Value - _origin.Key);
        }
    }
}
