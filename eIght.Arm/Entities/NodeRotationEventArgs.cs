using System;

namespace eIght.Arm.Entities
{
    public class NodeRotationEventArgs : EventArgs
    {
        public float Angle { get; }
        public NodeRotationEventArgs(float angle)
        {
            Angle = angle;

        }

    }
}
