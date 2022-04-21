using System.Numerics;

namespace eIght.Arm.Entities
{
    public interface IArmNode : INode
    {
        public string Name { get; set; }
        public IArmNode ParentNode { get; }
        public IArmNode ChildNode { get; }
        /// <summary>
        /// Node translation relative to transformated parent node
        /// </summary>
        Vector3 BaseTranslation { get; set; }
        /// <summary>
        /// Node rotation relative to transformated parent node
        /// </summary>
        Vector3 BaseRotation { get; set; }
        Matrix4x4 Transformation { get; }
        Matrix4x4 RotationMatrix { get;}
    }
}
