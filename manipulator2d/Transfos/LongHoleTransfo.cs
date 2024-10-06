using habo.geometry;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;

namespace Manipulator2D.Transfos
{
    public class LongHoleTransfo : Transfo
    {
        private readonly Node _center;
        private readonly Node _nodeInLongHole;
        private readonly Node _baseNode;
        private readonly double _radius;

        /// <summary>
        /// Compute the rotation of the part around the given center node, so that the new x axis is parallel to the long hole after 
        /// application.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="center">Rotation point of the part</param>
        /// <param name="nodeInLongHole">Point representing the part moving in the long hole</param>
        public LongHoleTransfo(
            string name,
            Node baseNode,
            Node center,
            Node nodeInLongHole,
            double radius,
            string resultingNodeName = "")
            : base(name, null, resultingNodeName)
        {
            _baseNode = baseNode;
            _center = center;
            _nodeInLongHole = nodeInLongHole;
            _radius = radius;

            _center.PositionChanged += (s, e) => UpdateTransfoMatrix();
            _nodeInLongHole.PositionChanged += (s, e) => UpdateTransfoMatrix();

            UpdateTransfoMatrix();
        }

        protected override void UpdateTransfoMatrix(double newSetting = 0)
        {
            var rotationAngle = ComputeAngle() - Math.PI / 2;
            LocalTransfoMatrix = Matrix<double>.Build.DenseOfColumns(new List<List<double>>
            {
                new List<double> { Math.Cos(rotationAngle), Math.Sin(rotationAngle), 0 },
                new List<double> { -Math.Sin(rotationAngle), Math.Cos(rotationAngle), 0 },
                new List<double> { 0, 0, 1 }
            });
        }

        private double ComputeAngle()
        {
            var baseToCenter = new Vector2D(_baseNode.Position, _center.Position);
            var centerNodeInLonghole = new Vector2D(_center.Position, _nodeInLongHole.Position);

            return Angle.Compute(baseToCenter, centerNodeInLonghole) + Math.Asin(_radius / centerNodeInLonghole.Length);
        }
    }
}
