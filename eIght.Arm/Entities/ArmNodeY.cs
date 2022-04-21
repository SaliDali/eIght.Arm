using System.Numerics;

namespace eIght.Arm.Entities
{
    public class ArmNodeY : AbstractArmNode
    {
        #region CTOR
        public ArmNodeY(string name) : base(name)
        {
        }

        #endregion

        #region Node
        public override Matrix4x4 RotationMatrix => NodeRotation.CreateRotationY(Rotation.Angle);

        #endregion

    }
}
