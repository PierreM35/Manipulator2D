using habo.geometry;
using System;

namespace Manipulator2D
{
    public class Node
    {
        private Point2D _position;

        public string Name { get; private set; }
        public Point2D Position
        {
            get => _position; 
            set
            {
                if (_position.Equals(value))
                    return;

                _position = value;
                OnPositionChanged();
            }
        }

        public Node(string name)
        {
            Name = name;
        }

        public Node(string name, double x, double z)
        {
            Name = name;
            Position = new Point2D(x, z);
        }

        public Node(string name, Node node)
        {
            Name = name;
            Position = node.Position;
        }

        #region Events

        public event EventHandler PositionChanged;
        private void OnPositionChanged()
        {
            PositionChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
