using System;
using MathNet.Numerics.LinearAlgebra;
using System.Collections.Generic;
using habo.geometry;

namespace Manipulator2D.Transfos
{
    public abstract class Transfo
    {
        private readonly List<Transfo> _nextTransfos;
        private Matrix<double> _localTransfoMatrix;
        private Matrix<double> _globalTransfoMatrix;

        public string Name { get; }
        public Node ResultingNode { get; }
        public Matrix<double> LocalTransfoMatrix
        {
            get => _localTransfoMatrix;
            set
            {
                _localTransfoMatrix = value;
                UpdateNextTransfos();
            }
        }
        public Matrix<double> GlobalTransfoMatrix
        {
            get => _globalTransfoMatrix;
            set
            {
                _globalTransfoMatrix = value;
                UpdateNextTransfos();
            }
        }

        protected Transfo(string name, IActuator actuator, string resultingNodeName)
        {
            Name = name;
            _nextTransfos = new List<Transfo>();
            ResultingNode = new Node(resultingNodeName);
            GlobalTransfoMatrix = Matrix<double>.Build.DenseIdentity(3, 3);
            if (actuator != null)
                actuator.ValueChanged += OnActuatorValueChanged;
        }

        #region Events

        public event EventHandler<Transfo> TransfoAdded;
        private void OnTransfoAdded(Transfo newTransfo)
        {
            TransfoAdded?.Invoke(this, newTransfo);
        }

        #endregion

        #region Event Handling

        protected virtual void OnActuatorValueChanged(object actuator, EventArgs e)
        {
            UpdateTransfoMatrix((actuator as IActuator).IsValue);
        }

        protected void OnUpwardTransfoAdded(object sender, Transfo addedTransfo)
        {
            OnTransfoAdded(addedTransfo);
        }

        #endregion

        protected abstract void UpdateTransfoMatrix(double newSetting);

        internal Node GetNode(string nodeName)
        {
            if (ResultingNode.Name.Equals(nodeName))
                return ResultingNode;

            foreach (var j in _nextTransfos)
            {
                var pt = j.GetNode(nodeName);
                if (pt != null)
                    return pt;
            }

            return null;
        }

        internal Transfo GetTransfo(string transfoName)
        {
            if (Name.Equals(transfoName))
                return this;

            foreach (var transfo in _nextTransfos)
            {
                var searchedTransfo = transfo.GetTransfo(transfoName);
                if (searchedTransfo != null)
                    return searchedTransfo;
            }

            return null;
        }

        internal IEnumerable<Node> GetNodes()
        {
            var points = new List<Node> { ResultingNode };
            foreach (var j in _nextTransfos)
                points.AddRange(j.GetNodes());

            return points;
        }

        public void AddTransfo(Transfo transfo, bool atBeginning = false)
        {
            if (atBeginning)
                _nextTransfos.Insert(0, transfo);
            else
                _nextTransfos.Add(transfo);

            transfo.TransfoAdded += OnUpwardTransfoAdded;
            OnTransfoAdded(transfo);
            transfo.GlobalTransfoMatrix = _globalTransfoMatrix * _localTransfoMatrix;
        }

        public override string ToString()
        {
            return Name;
        }

        #region Private helpers

        private void UpdateNextTransfos()
        {
            if (_localTransfoMatrix == null)
                return;

            var resultingTransfo = _globalTransfoMatrix * _localTransfoMatrix;
            var newResultingPosition = resultingTransfo * Vector<double>.Build.DenseOfArray(new double[] { 0, 0, 1 });
            ResultingNode.Position = new Point2D(newResultingPosition[0], newResultingPosition[1]);

            foreach (var transfo in _nextTransfos)
                transfo.GlobalTransfoMatrix = resultingTransfo;
        }

        #endregion
    }
}
