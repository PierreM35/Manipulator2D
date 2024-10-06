using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;

namespace Manipulator2D.Transfos
{
    public class RevoluteTransfo : Transfo
    {
        public RevoluteTransfo(string name, double angleRad, string resultingNodeName = "") :
              base(name, null, resultingNodeName)
        {
            UpdateTransfoMatrix(angleRad);
        }
        
        public RevoluteTransfo(string name, IActuator command, string resultingNodeName = "") : 
            base(name, command, resultingNodeName)
        {
            UpdateTransfoMatrix(command.IsValue);
        }

        protected override void UpdateTransfoMatrix(double newSetting)
        {
            LocalTransfoMatrix = Matrix<double>.Build.DenseOfColumns(new List<List<double>>
            {
                new List<double> { Math.Cos(newSetting), Math.Sin(newSetting), 0 },
                new List<double> { -Math.Sin(newSetting), Math.Cos(newSetting), 0 },
                new List<double> { 0, 0, 1 }
            });
        }
    }
}
