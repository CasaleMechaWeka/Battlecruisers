using System;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.Buildables.Units
{
    public abstract class Unit : Buildable, IUnit, IPointerClickHandler
	{
		public UnitCategory category;

		public float maxVelocityInMPerS;

		public Rigidbody2D rigidBody;

		#region Properties
		public IDroneConsumerProvider DroneConsumerProvider	{ set { _droneConsumerProvider = value;	} }
		public UnitCategory Category { get { return category; } }
		public override Vector2 Velocity { get { return rigidBody.velocity; } }

		private Direction _facingDirection;
		protected Direction FacingDirection
		{
			get { return _facingDirection; }
			set
			{
				_facingDirection = value;
				OnDirectionChange();
			}
		}

        protected override HealthBarController HealthBarController
        {
            get
            {
                UnitWrapper buildableWrapper = gameObject.GetComponentInInactiveParent<UnitWrapper>();
				return buildableWrapper.GetComponentInChildren<HealthBarController>(includeInactive: true);
            }
        }
		#endregion Properties

		void IUnit.Initialise(ICruiser parentCruiser, ICruiser enemyCruiser, IUIManager uiManager, IFactoryProvider factoryProvider)
		{
            base.Initialise(parentCruiser, enemyCruiser, uiManager, factoryProvider);
			
            Assert.IsTrue(maxVelocityInMPerS > 0);
			FacingDirection = _parentCruiser.Direction;

            OnInitialised();
        }

		void FixedUpdate()
		{
			OnFixedUpdate();
		}

		protected virtual void OnFixedUpdate() { }

		public void OnPointerClick(PointerEventData eventData)
		{
			_uiManager.ShowUnitDetails(this);
		}

		protected virtual void OnDirectionChange()
		{
			int yRotation = FindYRotation(FacingDirection);
			Quaternion rotation = gameObject.transform.rotation;
			rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, yRotation, rotation.eulerAngles.z);
			gameObject.transform.rotation = rotation;
		}

		private int FindYRotation(Direction facingDirection)
		{
			switch (facingDirection)
			{
				case Direction.Right:
					// Sprites by default are facing right, so DO NOT mirror
					return 0;
				case Direction.Left:
					// Sprites by default are facing right, so DO mirror
					return 180;
				default:
					throw new ArgumentException();
			}
		}

		protected override bool CanRepairCommandExecute()
		{
			// Cannot repair units :)
			return false;
		}
    }
}
