using System;
using System.Collections.Generic;

namespace Manipulator2D.Transfos
{
    public class PrismaticMappingTransfo : PrismaticTransfo
    {
        private KeyValuePair<double, double> _origin;
        private KeyValuePair<double, double> _target;

        public PrismaticMappingTransfo(
            string name,
            KeyValuePair<double, double> origin,
            KeyValuePair<double, double> target,
            double[] direction,
            IActuator actuator,
            string resultingNodeName = "")
            : base(name, direction, actuator, resultingNodeName)
        {
            _origin = origin;
            _target = target;
            UpdateTransfoMatrix(MapValue(actuator.IsValue));
        }

        protected override void OnActuatorValueChanged(object sender, EventArgs e)
        {
            var actuator = (IActuator)sender;
            UpdateTransfoMatrix(MapValue(actuator.IsValue));
        }

        private double MapValue(double isValue)
        {
            return _target.Key + (_target.Value - _target.Key) * 
                Math.Abs(isValue - _origin.Key) / Math.Abs(_origin.Value - _origin.Key);
        }
    }
}
