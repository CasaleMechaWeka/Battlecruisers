using System;
using UnityEngine;

namespace BattleCruisers.Cameras
{
	public interface ICameraTarget
	{
		Vector3 Position { get; } 
		float OrthographicSize { get; }
		CameraState State { get; }
	}

	public class CameraTarget : ICameraTarget
	{
		public Vector3 Position { get; private set; } 
		public float OrthographicSize { get; private set; }
		public CameraState State { get; private set; }

		public CameraTarget(Vector3 position, float orthographicSize, CameraState state)
		{
			Position = position;
			OrthographicSize = orthographicSize;
			State = state;
		}
	}
}

