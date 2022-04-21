using System.Numerics;

namespace eIght.Arm.Entities
{
    public interface INode
    {
        delegate void NodeRotationEventHandler(object sender, NodeRotationEventArgs args);
        event NodeRotationEventHandler RotationChanged;

        NodeRotation Rotation { get; set; }

    }
}