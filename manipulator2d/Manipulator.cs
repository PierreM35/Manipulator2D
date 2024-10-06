using Manipulator2D.Resources;
using Manipulator2D.Transfos;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manipulator2D
{
    public class Manipulator
    {
        private readonly List<Node> _nodes;
        private Transfo _baseTransfo;

        public Transfo BaseTransfo
        {
            get => _baseTransfo;
            set
            {
                _baseTransfo = value;
                OnUpwardTransfoAdded(null, _baseTransfo);
                BaseTransfo.TransfoAdded += OnUpwardTransfoAdded;
            }
        }

        public Manipulator()
        {
            _nodes = new List<Node>();
        }

        public Manipulator(Transfo baseTransfo)
        {
            _nodes = new List<Node>();
            BaseTransfo = baseTransfo;
        }

        #region Event Handling

        private void OnUpwardTransfoAdded(object sender, Transfo addedTransfo)
        {
            var newNodes = addedTransfo.GetNodes();
            foreach (var node in newNodes)
            {
                if (ExistNode(node))
                    throw new ManipulatorExceptionNodeAlreadyExists(node);
                _nodes.Add(node);
            }
        }

        #endregion

        public void ApplyTransformation(Matrix<double> transfoMatrix)
        {
            BaseTransfo.GlobalTransfoMatrix = transfoMatrix;
        }

        public IEnumerable<Node> GetNodes()
        {
            return _nodes.Where(n => !string.IsNullOrEmpty(n.Name));
        }

        /// <summary>
        /// Returns manipulator node by name or null if no node has the given name
        /// </summary>
        /// <param name="nodeName"></param>
        public Node GetNode(string nodeName)
        {
            return BaseTransfo.GetNode(nodeName);
        }

        /// <summary>
        /// Returns manipulator node by name or null if no node has the given name
        /// </summary>
        /// <param name="nodeName"></param>
        public Transfo GetTransfo(string transfoName)
        {
            return BaseTransfo?.GetTransfo(transfoName);
        }

        private bool ExistNode(Node node)
        {
            return !string.IsNullOrEmpty(node.Name) && _nodes.Any(n => n.Name.Equals(node.Name));
        }
    }
}
