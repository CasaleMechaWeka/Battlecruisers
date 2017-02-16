public interface ICameraController 
{
	void FocusOnFriendlyCruiser();
	void FocusOnEnemyCruiser();
	void ShowFullMapView();
}

public enum CameraPosition
{
	Unitialized, FriendlyCruiser, EnemyCruiser, Center
}