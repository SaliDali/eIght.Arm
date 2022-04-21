namespace eIght.Arm.Entities
{
    public abstract class AbstractNode : INode
    {
        public event INode.NodeRotationEventHandler RotationChanged;

        #region PUBLIC PROPERTIES
        private NodeRotation _rotation;
        public NodeRotation Rotation
        {
            get
            {
                return _rotation;

            }
            set
            {
                if (_rotation != null)
                {
                    _rotation.AngleChanged -= onRotationChanged;

                }

                _rotation = value;
                _rotation.AngleChanged += onRotationChanged;

                onRotationChanged(Rotation, new NodeRotationEventArgs(_rotation.Angle));

            }

        }

        #endregion

        #region CTOR
        public AbstractNode()
        {
            Rotation = new NodeRotation();

        }
        #endregion

        #region PRIVATE METHODES
        protected virtual void onRotationChanged(object sender, NodeRotationEventArgs e)
        {
            if (RotationChanged != null)
            {
                RotationChanged(sender, new NodeRotationEventArgs(e.Angle));


            }

        }

        #endregion

    }
}
