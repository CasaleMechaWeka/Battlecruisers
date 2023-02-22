using System;
using System.Collections.Generic;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Movement
{
    public class CruiserPhysics
    {
        // The RelativeJoint2D to work properly needs to be at least 1 unit of distance to its connectedBody;
        private const float CRUISER_Z_OFFSET_MODIFIER = 1.0f;
        public CruiserPhysics(GameObject cruiser)
        {
            Assert.IsNotNull(cruiser);
            _cruiser = cruiser;
        }

        private GameObject _cruiser;
        private GameObject _joint;
        private RelativeJoint2D _relativeJoint2D;
        private Rigidbody2D _rigidbody2D;
        private Rigidbody2D _jointRigidbody2D;
        private CruiserJoint _cruiserJoint;

        public void Initialize()
        {
            SetupCruiserRigidbody();
            SetupRelativeJoint();
            //SetupCruiserJoint();
        }

        private void SetupCruiserRigidbody()
        {
            // Setting up all the requisites for the rigidbody of the cruiser
            _rigidbody2D = _cruiser.GetComponent<Rigidbody2D>();
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
                    position = new Vector3(_cruiser.transform.position.x, _cruiser.transform.position.y, _cruiser.transform.position.z + CRUISER_Z_OFFSET_MODIFIER),
                    rotation = _cruiser.transform.rotation
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
            _relativeJoint2D.maxForce = 40f;
            _relativeJoint2D.maxTorque = 40f;
            _relativeJoint2D.correctionScale = 0.75f;
        }

        private void SetupCruiserJoint()
        {
            // Custom behaviour not yet implemented, but aims to solve issues with custom behaviour cruiser, like the Huntress Boss
            _cruiserJoint = _joint.AddComponent<CruiserJoint>();
            _cruiserJoint.Cruiser = _cruiser.transform;
        }
    }
}