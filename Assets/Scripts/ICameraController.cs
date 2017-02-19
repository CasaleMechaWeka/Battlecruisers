using System.ComponentModel;

public interface ICameraController 
{
	void FocusOnFriendlyCruiser();
	void FocusOnEnemyCruiser();
	void ShowFullMapView();
}

[DefaultValue(Unitialized)]
public enum CameraState
{
	Unitialized, FriendlyCruiser, EnemyCruiser, Center, InTransition
}