using System;
using System.Collections.Generic;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace BattleCruisers.Movement
{
    public class CruiserPhysics
    {
        
        
        // MEMBERS

        private Cruiser _cruiser;
        private GameObject _joint;
        private RelativeJoint2D _relativeJoint2D;
        private Rigidbody2D _rigidbody2D;
        private Rigidbody2D _jointRigidbody2D;
        private CruiserJoint _cruiserJoint;

        // The RelativeJoint2D to work properly needs to be at least 1 unit of distance to its connectedBody;
        private const float CRUISER_Z_OFFSET_MODIFIER = 1.0f;
        
        // PROPERTIES
        [Serializable]
        public struct Settings
        {
            [Tooltip("Enables or disables completely the physics behaviour")]
            public bool enabledPhysics;
            [Tooltip("sets the maximum force that can be applied to the cruiser to maintain its relative position. If the cruiser is pushed or pulled beyond its allowed range, the cruiser will apply a force to move it back into position, but this force will not exceed the MaxForce value.")]
            public float maxForce;
            [Tooltip("sets the maximum torque that can be applied to the cruiser to maintain its relative angle. If the cruiser is twisted or rotated beyond its allowed range, the cruiser will apply a torque to rotate it back into position, but this torque will not exceed the MaxTorque value.")]
            public float maxTorque;
            [Range(0,1)]
            [Tooltip("A larger CorrectionScale value means that more of the error is corrected in each step, resulting in faster correction behavior, but potentially less accurate or smooth movement. A smaller CorrectionScale value means that less of the error is corrected in each step, resulting in slower correction behavior, but potentially more accurate or smooth movement.")]
            public float correctionScale;

            public Settings(bool defaultEnablePhysics,float defaultMaxForce, float defaultMaxTorque, float defaultCorrectionScale)
            {
                enabledPhysics = defaultEnablePhysics;
                maxForce = defaultMaxForce;
                maxTorque = defaultMaxTorque;
                correctionScale = defaultCorrectionScale;
            }
        }
        
        
        public CruiserPhysics(Cruiser cruiser)
        {
            Assert.IsNotNull(cruiser);
            _cruiser = cruiser;
        }

        
        // METHODS
        public void Initialize()
        {
            SetupCruiserRigidbody();
            SetupRelativeJoint();
            //SetupCruiserJoint();
        }

        private void SetupCruiserRigidbody()
        {
            // Setting up all the requisites for the rigidbody of the cruiser
            _rigidbody2D = _cruiser.gameObject.GetComponent<Rigidbody2D>();
            _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            _rigidbody2D.isKinematic = false;
            _rigidbody2D.simulated = true;
            _rigidbody2D.interpolation = RigidbodyInterpolation2D.Interpolate;
            _rigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            //_rigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition;
        }

        private void SetupRelativeJoint()
        {
            // Setting up a relative joint, that will act as the "Pivot" of the cruiser
            _joint = new GameObject($"{_cruiser.name}RelativeJoint")
            {
                transform =
                {
                    position = new Vector3(_cruiser.transform.position.x, _cruiser.gameObject.transform.position.y, _cruiser.gameObject.transform.position.z + CRUISER_Z_OFFSET_MODIFIER),
                    rotation = _cruiser.gameObject.transform.rotation
                }
            };
            // Adding the current cruiser as the RelativeJoint connectedBody
            _jointRigidbody2D = _joint.AddComponent<Rigidbody2D>();
            // Joint needs to be kinematic since it will not react to physics, only act as pivot that the cruiser will try to follow
            _jointRigidbody2D.isKinematic = true;
            
            _relativeJoint2D = _joint.AddComponent<RelativeJoint2D>();
            _relativeJoint2D.connectedBody = _rigidbody2D;
            _relativeJoint2D.autoConfigureOffset = true;
            _relativeJoint2D.breakForce = Mathf.Infinity;
            _relativeJoint2D.breakTorque = Mathf.Infinity;
            _relativeJoint2D.maxForce = _cruiser.physicsSettings.maxForce;
            _relativeJoint2D.maxTorque = _cruiser.physicsSettings.maxTorque;
            _relativeJoint2D.correctionScale = _cruiser.physicsSettings.correctionScale;
        }

        private void SetupCruiserJoint()
        {
            // Custom behaviour not yet implemented, but aims to solve issues with custom behaviour cruiser, like the Huntress Boss
            _cruiserJoint = _joint.AddComponent<CruiserJoint>();
            _cruiserJoint.Cruiser = _cruiser.transform;
        }
    }
}