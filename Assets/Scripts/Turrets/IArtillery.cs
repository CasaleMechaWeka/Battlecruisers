using UnityEngine;

public interface IArtillery
{
	GameObject Target { set; }
	ITurretStats TurretStats { set; }
	Rigidbody2D ShellPrefab { set; }
	// FELIX:  Hmmm  should artillery determine this itself?
	Vector2 ShellOrigin { set; }
}