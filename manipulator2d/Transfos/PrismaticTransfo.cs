using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;

namespace Manipulator2D.Transfos
{
    public class PrismaticTransfo : Transfo
    {
        private readonly Vector<double> _direction;

        public PrismaticTransfo(string name, double[] translation, string resultingNodeName = "") :
            base(name, null, resultingNodeName)
        {
            if (translation.Length != 2)
                throw new Exception("Double array for translation must have a length of 2!");

            _direction = Vector<double>.Build.DenseOfArray(new double[] { translation[0], translation[1] });

            UpdateTransfoMatrix();
        }

        public PrismaticTransfo(string name, double[] direction, IActuator actuator, string resultingNodeName = "") :
            base(name, actuator, resultingNodeName)
        {
            if (direction.Length != 2)
                throw new Exception("Double array for direction must have a length of 2!");

            var directionVector = Vector<double>.Build.DenseOfArray(new double[] { direction[0], direction[1] });
            _direction = directionVector.Normalize(2);

            UpdateTransfoMatrix(actuator.IsValue);
        }

        protected override void UpdateTransfoMatrix(double newSetting = 1)
        {
            var newTranslation = _direction * newSetting;
            LocalTransfoMatrix = Matrix<double>.Build.DenseOfColumns(new List<List<double>>
            {
                new List<double> { 1, 0, 0 },
                new List<double> { 0, 1, 0 },
                new List<double> { newTranslation[0], newTranslation[1], 1 }
            });
        }
    }
}
