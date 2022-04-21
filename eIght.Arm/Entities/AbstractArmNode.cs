using System;
using System.Numerics;

namespace eIght.Arm.Entities
{
    public abstract class AbstractArmNode : AbstractNode, IArmNode
    {
        #region CTOR
        public AbstractArmNode(string name)
        {
            Name = name;
            RotationChanged += ArmNode_RotationChanged;

        }

        #endregion

        #region IArmNode
        public string Name { get; set; }
        public IArmNode ParentNode { get; internal set; }
        public IArmNode ChildNode { get; internal set; }

        Vector3 _baseTranslation;
        public Vector3 BaseTranslation
        {
            get
            {
                return _baseTranslation;
            }
            set
            {
                _baseTranslation = value;
                UpdateNodesTransformationMatrix();

            }
        }
        Vector3 _baseRotation;
        public Vector3 BaseRotation
        {
            get
            {
                return _baseRotation;
            }
            set
            {
                _baseRotation = value;
                UpdateNodesTransformationMatrix();

            }
        }
        public Matrix4x4 Transformation { get; internal set; } = Matrix4x4.Identity;
        public abstract Matrix4x4 RotationMatrix { get; }

        #endregion

        #region PRIVATE METHODS
        private void ArmNode_RotationChanged(object sender, EventArgs e)
        {
            UpdateNodesTransformationMatrix();

        }
        private Matrix4x4 getRotationMatrix(Vector3 rotationXYZ)
        {
            Matrix4x4 rotationMatrix = Matrix4x4.CreateRotationX(-rotationXYZ.X * ((float)Math.PI / 180.0f)) *
                                        Matrix4x4.CreateRotationY(rotationXYZ.Y * ((float)Math.PI / 180.0f)) *
                                        Matrix4x4.CreateRotationZ(-rotationXYZ.Z * ((float)Math.PI / 180.0f))
                ;

            return rotationMatrix;

        }
        public void UpdateNodesTransformationMatrix()
        {
            RotationChanged -= ArmNode_RotationChanged;

            AbstractArmNode _childNode = this;

            // Root node
            if (ParentNode == null)
            {
                Transformation = getRotationMatrix(BaseRotation) * Matrix4x4.CreateTranslation(BaseTranslation);
                _childNode = (AbstractArmNode)ChildNode;

            }

            // 

            while (_childNode != null)
            {
                Matrix4x4 baseTransformation = getRotationMatrix(_childNode.BaseRotation) * Matrix4x4.CreateTranslation(_childNode.BaseTranslation);
                _childNode.Transformation = baseTransformation *
                                                _childNode.ParentNode.RotationMatrix *
                                                _childNode.ParentNode.Transformation
                                                ;

                _childNode = (AbstractArmNode)_childNode.ChildNode;

            }

            RotationChanged += ArmNode_RotationChanged;

        }

        #endregion

    }
}
