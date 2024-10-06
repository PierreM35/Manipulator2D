using System;

namespace Manipulator2D.Resources
{
    public class ManipulatorExceptionNodeAlreadyExists : Exception
    {
        public ManipulatorExceptionNodeAlreadyExists(Node node) : base(node.Name)
        {
        }
    }
}
