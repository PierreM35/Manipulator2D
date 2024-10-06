using MathNet.Numerics.LinearAlgebra;

namespace Manipulator2D.Transfos
{
    public class MatrixBasedTransfo : Transfo
    {
        public MatrixBasedTransfo(string name, Matrix<double> matrix, IActuator actuator = null, string resultingNodeName = "") 
            : base(name, actuator, resultingNodeName)
        {
            LocalTransfoMatrix = matrix;
        }

        protected override void UpdateTransfoMatrix(double newSetting)
        {
        }
    }
}
