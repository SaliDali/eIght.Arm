using eIght.Arm.Entities;
using System.Numerics;

namespace eIght.Arm.Alghoritms
{
    public interface IAlghoritmIK
    {
        public float PrecissionDistance { get; set; }
        public void Calculate(IArm arm, Vector3 destinationPoint);

    }
}