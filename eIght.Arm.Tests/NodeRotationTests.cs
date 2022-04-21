using eIght.Arm.Alghoritms;
using eIght.Arm.Entities;
using System.Numerics;
using Xunit;

namespace eIght.Arm.Tests
{
    public class NodeRotationTests
    {
        private NodeRotation _nodeRotation = new NodeRotation() { IsLocked = false };

        [Fact]
        public void NormalizeAngle_360_Equal_360_true()
        {
            _nodeRotation.Angle = 360;
            Assert.Equal(360, _nodeRotation.Angle);

        }

        [Fact]
        public void NormalizeAngle_720_Equal_360_true()
        {
            _nodeRotation.Angle = 720;
            Assert.Equal(360, _nodeRotation.Angle);

        }

        [Fact]
        public void NormalizeAngle_700_Equal_340_true()
        {
            _nodeRotation.Angle = 700;
            Assert.Equal(340, _nodeRotation.Angle);

        }

        [Fact]
        public void NormalizeAngle_minus45_Equal_315_true()
        {
            _nodeRotation.Angle = -45;
            Assert.Equal(315, _nodeRotation.Angle);

        }

        [Fact]
        public void NormalizeAngle_minus740_Equal_340_true()
        {
            _nodeRotation.Angle = -740;
            Assert.Equal(340, _nodeRotation.Angle);

        }

        [Fact]
        public void SCCD()
        {
            Vector3 vector1 = new Vector3(0.0f, 1.0f, 0.0f);
            Vector3 vector2 = new Vector3(0.0f, 0.0f, -1.0f);

            float angle = CCD.AngleBetweenVectors(vector1, vector2,true);
            Assert.Equal(-90, angle);

        }

    }
}
