using habo.geometry;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;

namespace Manipulator2D.Transfos
{
    public class PointOnCircleTransfo : Transfo
    {
        private readonly Arc2D _arc;
        private readonly Matrix<double> _translToRotationPt;
        private readonly Matrix<double> _translToNewPosition;

        public PointOnCircleTransfo(string name, Arc2D arc, IActuator actuator, string resultingNodeName = "") 
            : base(name, actuator, resultingNodeName)
        {
            _arc = arc;
            _translToRotationPt = Matrix<double>.Build.DenseOfColumns(new List<List<double>>
            {
                new List<double> { 1, 0, 0 },
                new List<double> { 0, 1, 0 },
                new List<double> { 0, -_arc.Radius, 1 }
            });
            _translToNewPosition = Matrix<double>.Build.DenseOfColumns(new List<List<double>>
            {
                new List<double> { 1, 0, 0 },
                new List<double> { 0, 1, 0 },
                new List<double> { 0, _arc.Radius, 1 }
            });

            UpdateTransfoMatrix(actuator.IsValue);
        }

        protected override void UpdateTransfoMatrix(double newSetting)
        {
            var angle = ComputeAngle(newSetting);
            var rotMatrix = Matrix<double>.Build.DenseOfColumns(new List<List<double>>
            {
                new List<double> { Math.Cos(angle), Math.Sin(angle), 0 },
                new List<double> { -Math.Sin(angle), Math.Cos(angle), 0 },
                new List<double> { 0, 0, 1 }
            });

            LocalTransfoMatrix = _translToRotationPt * rotMatrix * _translToNewPosition;
        }

        private double ComputeAngle(double newSetting)
        {
            var newPosition = _arc.PointAt(newSetting);
            var os = new Vector2D(_arc.Center, _arc.Start);
            var oi = new Vector2D(_arc.Center, newPosition);

            return Angle.Compute(os, oi);
        }
    }
}