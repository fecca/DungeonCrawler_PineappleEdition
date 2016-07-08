using UnityEngine;

public class Exit
{
	public int CornerIndex { get; private set; }
	public Vector3 TopLeftExit { get; private set; }
	public Vector3 BottomLeftExit { get; private set; }
	public Vector3 BottomRightExit { get; private set; }
	public Vector3 TopRightExit { get; private set; }
	public Vector3 Position { get; private set; }

	public Exit(int cornerIndex, Vector3 topLeftExit, Vector3 bottomLeftExit, Vector3 bottomRightExit, Vector3 topRightExit)
	{
		CornerIndex = cornerIndex;
		TopLeftExit = topLeftExit;
		BottomLeftExit = bottomLeftExit;
		BottomRightExit = bottomRightExit;
		TopRightExit = topRightExit;
		Position = Vector3.Lerp(bottomLeftExit, bottomRightExit, 0.5f);
	}
}