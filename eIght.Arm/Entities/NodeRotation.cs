using System;
using System.Numerics;

namespace eIght.Arm.Entities
{
    public class NodeRotation
    {
        public delegate void NodeRotationEventHandler(object sender, NodeRotationEventArgs args);
        public event NodeRotationEventHandler AngleChanged;

        #region PUBLIC PROPERTIES
        public bool IsLocked { get; set; } = false;

        private float _angle = 0.0f;
        public float Angle
        {
            get
            {
                return _angle;

            }
            set
            {
                if (!IsLocked)
                {
                    float _ang = normalizedAngle(value);

                    if (_ang <= _minAngle)
                    {
                        _angle = _minAngle;
                        onAngleChanged(this, new NodeRotationEventArgs(_ang));

                        return;

                    }

                    if (_ang >= _maxAngle)
                    {
                        _angle = _maxAngle;
                        onAngleChanged(this, new NodeRotationEventArgs(_ang));

                        return;

                    }

                    _angle = _ang;

                    onAngleChanged(this, new NodeRotationEventArgs(_ang));

                }

            }

        }

        private float _minAngle = 0.0f;
        public float MinAngle
        {
            get
            {
                return _minAngle;

            }
            set
            {
                float _ang = normalizedAngle(value);

                if (_ang < _maxAngle)
                {
                    _minAngle = _ang;

                }

            }

        }

        private float _maxAngle = 360.0f;
        public float MaxAngle
        {
            get
            {
                return _maxAngle;

            }
            set
            {
                float _ang = normalizedAngle(value);

                if (_ang > _minAngle)
                {
                    _maxAngle = _ang;

                }

            }

        }

        #endregion

        #region  PUBLIC STATIC
        public static float AngleToRadians(float angle)
        {
            return ((float)Math.PI / 180.0f) * angle;

        }
        public static Matrix4x4 CreateRotationX(float angle)
        {
            return Matrix4x4.CreateRotationX(AngleToRadians(angle+180.0f));

        }
        public static Matrix4x4 CreateRotationY(float angle)
        {
            return Matrix4x4.CreateRotationY(AngleToRadians(angle+180.0f));

        }
        public static Matrix4x4 CreateRotationZ(float angle)
        {
            return Matrix4x4.CreateRotationZ(AngleToRadians(angle+180.0f));

        }

        #endregion

        #region PRIVATE METHODS
        /// <summary>
        /// Get angle in degree in range 0 - 360 deg
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        private float normalizedAngle(float angle)
        {
            // 0.0 degree
            if (angle == 0.0f)
            {
                return angle;

            }

            // modulo 360.0f
            float _modAngle = angle % 360.0f;

            // _modAngle = 0
            if (_modAngle == 0.0f)
            {
                return 360.0f;

            }

            //
            if (_modAngle < 0.0f)
            {
                return 360.0f + _modAngle;

            }

            return _modAngle;

        }

        protected void onAngleChanged(object sender, NodeRotationEventArgs e)
        {
            if (AngleChanged != null)
            {
                AngleChanged(sender, e);

            }

        }

        #endregion

    }
}
