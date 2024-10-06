using System;

namespace Manipulator2D
{
    public class ActuatorBase : IActuator
    {
        private double _isValue;

        public double LowerLimit { get; set; }
        public double UpperLimit { get; set; }
        public double IsValue
        {
            get { return _isValue; }
            set
            {
                _isValue = value;
                OnValueChanged();
            }
        }

        public ActuatorBase(double lowerLimit, double upperLimit)
        {
            LowerLimit = lowerLimit;
            UpperLimit = upperLimit;
            IsValue = LowerLimit;
        }

        #region Events

        public event EventHandler ValueChanged;
        private void OnValueChanged()
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        public void MoveBy(double diff)
        {
            var shouldValue = IsValue + diff;
            if (shouldValue < LowerLimit)
            {
                IsValue = LowerLimit;
                return;
            }

            if (shouldValue > UpperLimit)
            {
                IsValue = UpperLimit;
                return;
            }

            IsValue = shouldValue;
        }
    }
}
