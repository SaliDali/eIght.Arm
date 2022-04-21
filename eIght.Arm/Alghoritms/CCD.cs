using eIght.Arm.Entities;
using System;
using System.Linq;
using System.Numerics;

namespace eIght.Arm.Alghoritms
{
    public class CCD : IAlghoritmIK
    {
        #region PUBLIC PROPERTY
        public float PrecissionDistance { get; set; }

        #endregion

        #region CTOR
        public CCD(float precission)
        {
            PrecissionDistance = precission;

        }

        #endregion

        #region IAlghoritmIK
        public void Calculate(IArm arm, Vector3 destinationPoint)
        {
            Vector3 reachedPoint = Vector3.Zero;

            float tempDistance = 0.0f;
            float distance = 0.0f;

            float _prec = 0.1f * PrecissionDistance;

            int n = 0;

            while (true)
            {
                n += 1;

                foreach (var nodePriority in arm.GetArmNodesIKPriorityOrdered())
                {
                    if (nodePriority.node == arm.LastNode)
                    {
                        continue;
                    }
                    // Inverted node transformation
                    Matrix4x4 invertedTransformation;
                    Matrix4x4.Invert(nodePriority.node.Transformation, out invertedTransformation);

                    // LastNode in local node coordinates
                    Vector3 lastNodeInLocalCoord = Vector3.Normalize(Vector3.Transform(arm.LastNode.Transformation.Translation, invertedTransformation));

                    // Destination point in local node coordinates
                    Vector3 destPointInLocalCoord = Vector3.Normalize(Vector3.Transform(destinationPoint, invertedTransformation));

                    Vector3 lastNodeInLocalCoord_XY = new Vector3(lastNodeInLocalCoord.X, lastNodeInLocalCoord.Y, 0.0f);
                    Vector3 lastNodeInLocalCoord_XZ = new Vector3(lastNodeInLocalCoord.X, 0.0f, lastNodeInLocalCoord.Z);
                    Vector3 lastNodeInLocalCoord_YZ = new Vector3(0.0f, lastNodeInLocalCoord.Y, lastNodeInLocalCoord.Z);

                    Vector3 destPointInLocalCoord_XY = new Vector3(destPointInLocalCoord.X, destPointInLocalCoord.Y, 0.0f);
                    Vector3 destPointInLocalCoord_XZ = new Vector3(destPointInLocalCoord.X, 0.0f, destPointInLocalCoord.Z);
                    Vector3 destPointInLocalCoord_YZ = new Vector3(0.0f, destPointInLocalCoord.Y, destPointInLocalCoord.Z);

                    float angXY = AngleBetweenVectors(lastNodeInLocalCoord_XY, destPointInLocalCoord_XY, true);
                    float angXZ = AngleBetweenVectors(lastNodeInLocalCoord_XZ, destPointInLocalCoord_XZ, true);
                    float angYZ = AngleBetweenVectors(lastNodeInLocalCoord_YZ, destPointInLocalCoord_YZ, true);

                    if (float.IsNaN(angXY))
                        angXY = 0.0f;

                    if (float.IsNaN(angXZ))
                        angXZ = 0.0f;

                    if (float.IsNaN(angYZ))
                        angYZ = 0.0f;

                    if (nodePriority.node.GetType() == typeof(ArmNodeX))
                    {
                        float ang = nodePriority.node.Rotation.Angle;
                        nodePriority.node.Rotation.Angle -= angYZ;

                    }

                    if (nodePriority.node.GetType() == typeof(ArmNodeY))
                    {
                        float ang = nodePriority.node.Rotation.Angle;
                        nodePriority.node.Rotation.Angle -= angXZ;

                    }

                    if (nodePriority.node.GetType() == typeof(ArmNodeZ))
                    {
                        float ang = nodePriority.node.Rotation.Angle;
                        nodePriority.node.Rotation.Angle -= angXY;

                    }

                }

                reachedPoint = arm.LastNode.Transformation.Translation;

                distance = Vector3.Distance(reachedPoint, destinationPoint);

                if (n == 1)
                {
                    tempDistance = distance;

                }

                if (n == 10)
                {
                    n = 0;

                    float delta = tempDistance - distance;

                    if (delta < _prec)
                    {
                        return;

                    }

                }

                if (distance <= PrecissionDistance)
                {
                    return;

                }

            }

        }

        #endregion

        #region STATIC METHODS
        public static Vector3 VectorFromTwoPoints(Vector3 startPoint, Vector3 endPoint)
        {
            return endPoint - startPoint;

        }
        public static Vector3 ProjectPointOnPlane(Vector3 point, Plane plane)
        {
            // Point on plane closest to the origin (position of plane)
            float denominator = (float)Math.Sqrt(Math.Pow(plane.Normal.X, 2) + Math.Pow(plane.Normal.Y, 2) + Math.Pow(plane.Normal.Z, 2));
            Vector3 pointOnPlane = new Vector3
                                                (
                                                    (float)(plane.Normal.X * plane.D / denominator),
                                                    (float)(plane.Normal.Y * plane.D / denominator),
                                                    (float)(plane.Normal.Z * plane.D / denominator)
                                                );

            Vector3 normal = plane.Normal;

            float x = point.X;
            float y = point.Y;
            float z = point.Z;

            float a = normal.X;
            float b = normal.Y;
            float c = normal.Z;

            float d = pointOnPlane.X;
            float e = pointOnPlane.Y;
            float f = pointOnPlane.Z;

            float t = (a * d - a * x + b * e - b * y + c * f - c * z) / (a * a + b * b + c * c);

            Vector3 projectedPoint = new Vector3(x + t * a, y + t * b, z + t * c);

            return projectedPoint;

        }
        //returns angle between two vectors
        //input two vectors u and v
        //for 'returndegrees' enter true for an answer in degrees, false for radians
        public static float AngleBetweenVectors(Vector3 vector1, Vector3 vector2, bool returndegrees)
        {
            float dotProduct = Vector3.Dot(Vector3.Normalize(vector1), Vector3.Normalize(vector2));
            Vector3 crossProduct = Vector3.Cross(Vector3.Normalize(vector1), Vector3.Normalize(vector2));

            float min = Math.Min(1.0f, dotProduct);

            float angle = (float)Math.Acos(min);

            if (Vector3.Dot(crossProduct, Vector3.One) > 0)
            {
                angle = -angle;

            }

            if (returndegrees)
            {
                angle *= 360.0f / (float)(2 * Math.PI);

            }


            return angle;
        }

        #endregion

    }
}
