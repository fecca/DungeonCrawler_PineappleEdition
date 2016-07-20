using UnityEngine;

public class Exit
{
	public int CornerIndex { get; private set; }
	public Vector3 LeftOuterFloor { get; private set; }
	public Vector3 LeftOuterWall { get; private set; }
	public Vector3 LeftWall { get; private set; }
	public Vector3 LeftFloor { get; private set; }
	public Vector3 RightFloor { get; private set; }
	public Vector3 RightWall { get; private set; }
	public Vector3 RightOuterWall { get; private set; }
	public Vector3 RightOuterFloor { get; private set; }
	public Vector3 Position { get; private set; }

	public Exit(int cornerIndex,
		Vector3 leftOuterFloor,
		Vector3 leftOuterWall,
		Vector3 leftWall,
		Vector3 leftFloor,
		Vector3 rightFloor,
		Vector3 rightWall,
		Vector3 rightOuterWall,
		Vector3 rightOuterFloor)
	{
		CornerIndex = cornerIndex;

		LeftOuterFloor = leftOuterFloor;
		LeftOuterWall = leftOuterWall;
		LeftWall = leftWall;
		LeftFloor = leftFloor;
		RightFloor = rightFloor;
		RightWall = rightWall;
		RightOuterWall = rightOuterWall;
		RightOuterFloor = rightOuterFloor;

		Position = Vector3.Lerp(leftFloor, rightFloor, 0.5f);
	}
}