using UnityEngine;

public interface IArtillery
{
	GameObject Target { set; }
	ITurretStats TurretStats { set; }
	Rigidbody2D ShellPrefab { set; }
	Vector2 ShellOrigin { set; }
}