using habo.geometry;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;

namespace Manipulator2D.Transfos
{
    public class QuadTransfo : Transfo
    {
        private readonly Node _node1;
        private readonly Node _node2;
        private readonly Node _node3;
        private readonly double _r1;
        private readonly double _r3;

        /// <summary>
        /// The QuadTransfo transforms an axis being centered on node3 with y axis parallel to node2node3 in an axis
        /// centered on node3 with x axis parallel to node4node3.
        /// Node4 is contantly away from node1 from r1 and from node3 from r3.
        /// </summary>
        public QuadTransfo(
            string name,
            Node node1, 
            Node node2, 
            Node node3, 
            double r1,
            double r3,
            string resultingNodeName = "")
            : base(name, null, resultingNodeName)
        {
            _node1 = node1;
            _node2 = node2;
            _node3 = node3;
            _r1 = r1;
            _r3 = r3;

            _node3.PositionChanged += (s, e) => UpdateTransfoMatrix();
            UpdateTransfoMatrix();
        }

        protected override void UpdateTransfoMatrix(double newSetting = 0)
        {
            var q4 = ComputePt4();
            var q2q3 = new Vector2D(_node2.Position, _node3.Position);
            var q4q3 = new Vector2D(q4, _node3.Position);
            var angle = Math.PI / 2 - Angle.Compute(q4q3, q2q3);

            LocalTransfoMatrix = Matrix<double>.Build.DenseOfColumns(new List<List<double>>
            {
                new List<double> { Math.Cos(angle), Math.Sin(angle), 0 },
                new List<double> { -Math.Sin(angle), Math.Cos(angle), 0 },
                new List<double> { 0, 0, 1 }
            });
        }

        private Point2D ComputePt4()
        {
            var circle1 = new Circle2D(_node1.Position, _r1);
            var circle3 = new Circle2D(_node3.Position, _r3);
            var intersections = Intersection.Compute(circle3, circle1);

            return intersections[0].Y > intersections[1].Y ? intersections[0] : intersections[1];
        }
    }
}
