using System.Numerics;

namespace eIght.Arm.Entities
{
    public class ArmNodeZ : AbstractArmNode
    {
        #region CTOR
        public ArmNodeZ(string name) : base(name)
        {
        }

        #endregion

        #region Node
        public override Matrix4x4 RotationMatrix => NodeRotation.CreateRotationZ(Rotation.Angle);

        #endregion

    }
}
