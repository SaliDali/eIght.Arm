using eIght.Arm.Alghoritms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace eIght.Arm.Tests
{
    public class CCDTests
    {
        Vector3 vector1 = new Vector3(0.0f, 0.0f, 1.0f);
        Vector3 vector2 = new Vector3(1.0f, 0.0f, 0.0f);
        Vector3 vector3 = new Vector3(0.0f, 1.0f, 1.0f);
        [Fact]
        public void AngleBetweenEqual90deg()
        {
            float ang = CCD.AngleBetweenVectors(vector1, vector2, true);
            Assert.Equal(90.0f, ang);

        }

        [Fact]
        public void AngleBetweenEqual45deg()
        {
            float ang = CCD.AngleBetweenVectors(vector1, vector3, true);
            Assert.Equal(45.0f, ang);

        }

        [Fact]
        public void Vector()
        {
            Vector3 point = new Vector3(0.0f, 1.0f, 1.0f);
            Plane plane =  Plane.CreateFromVertices(new Vector3(0.0f,0.0f,0.0f), 
                                                    new Vector3(0.0f, 0.0f, 1.0f), 
                                                    new Vector3(1.0f, 1.0f, 0.0f));

            Vector3 vector = CCD.ProjectPointOnPlane(point, plane);
            Assert.Equal(new Vector3(0.5f,0.5f,1.0f), vector);

        }

    }
}
