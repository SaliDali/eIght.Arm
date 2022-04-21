using System.Numerics;

namespace eIght.Arm.Entities
{
    public class ArmNodeX : AbstractArmNode
    {
        #region CTOR
        public ArmNodeX(string name) : base(name)
        {
        }

        #endregion

        #region Node
        public override Matrix4x4 RotationMatrix => NodeRotation.CreateRotationX(Rotation.Angle);

        #endregion

    }
}
