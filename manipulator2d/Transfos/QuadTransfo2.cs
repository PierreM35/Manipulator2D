using habo.geometry;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;

namespace Manipulator2D.Transfos
{
    public class QuadTransfo2 : Transfo
    {
        private readonly Node _node1;
        private readonly Node _node2;
        private readonly Node _node4;
        private readonly double _r4;
        private readonly double _r2;

        /// <summary>
        /// The QuadTransfo2 transforms an axis being centered on node4 with y axis parallel to node1node4 in an axis
        /// centered on node4 with x axis parallel to node4node3.
        /// Node3 is contantly away from node2 from r2 and from node4 from r4.
        /// </summary>
        public QuadTransfo2(
            string name,
            Node node1,
            Node node2,
            Node node4,
            double r4,
            double r2,
            string resultingNodeName = "")
            : base(name, null, resultingNodeName)
        {
            _node1 = node1;
            _node2 = node2;
            _node4 = node4;
            _r4 = r4;
            _r2 = r2;

            _node4.PositionChanged += (s, e) => UpdateTransfoMatrix();
            UpdateTransfoMatrix();
        }

        protected override void UpdateTransfoMatrix(double newSetting = 0)
        {
            var q3 = ComputePt3();
            var q1q4 = new Vector2D(_node1.Position, _node4.Position);
            var q4q3 = new Vector2D(_node4.Position, q3);
            var angle = Math.PI / 2 + Angle.Compute(q1q4, q4q3);

            LocalTransfoMatrix = Matrix<double>.Build.DenseOfColumns(new List<List<double>>
            {
                new List<double> { Math.Cos(angle), Math.Sin(angle), 0 },
                new List<double> { -Math.Sin(angle), Math.Cos(angle), 0 },
                new List<double> { 0, 0, 1 }
            });
        }

        private Point2D ComputePt3()
        {
            var circle2 = new Circle2D(_node2.Position, _r2);
            var circle4 = new Circle2D(_node4.Position, _r4);
            var intersections = Intersection.Compute(circle4, circle2);

            return intersections[0].Y > intersections[1].Y ? intersections[0] : intersections[1];
        }
    }
}
