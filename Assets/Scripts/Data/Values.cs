using UnityEngine;

public static class Values
{
	public const int MaximumNumberOfRoomsAllowed = 100;
	public const int MaximumNumberOfRetries = 10;

	private const float RoomFloorHeightMin = -0.1f;
	private const float RoomFloorHeightMax = 0.1f;
	public static float RandomRoomFloorHeight
	{
		get
		{
			return Random.Range(RoomFloorHeightMin, RoomFloorHeightMax);
		}
	}

	private const float RoomCornerAngleMin = -0.5f;
	private const float RoomCornerAngleMax = 0.5f;
	public static float RandomRoomCornerAngle
	{
		get
		{
			return Random.Range(RoomCornerAngleMin, RoomCornerAngleMax);
		}
	}

	private const int NumberOfRoomCornersMin = 20;
	private const int NumberOfRoomCornersMax = 40;
	public static int RandomNumberOfRoomCorners
	{
		get
		{
			return Random.Range(NumberOfRoomCornersMin, NumberOfRoomCornersMax);
		}
	}

	private const float RoomRadiusIntervalMin = 0.95f;
	private const float RoomRadiusIntervalMax = 1.05f;
	public static float RandomRoomRadiusInterval
	{
		get
		{
			return Random.Range(RoomRadiusIntervalMin, RoomRadiusIntervalMax);
		}
	}

	private const int RoomRadiusMin = 20;
	public const int RoomRadiusMax = 30;
	public static int RandomRoomRadius
	{
		get
		{
			return Random.Range(RoomRadiusMin, RoomRadiusMax);
		}
	}

	private const int RoomHeightMin = 5;
	private const int RoomHeightMax = 10;
	public static int RandomRoomHeight
	{
		get
		{
			return Random.Range(RoomHeightMin, RoomHeightMax);
		}
	}

	private const int RoomThicknessMin = 2;
	private const int RoomThicknessMax = 4;
	public static int RandomRoomThickness
	{
		get
		{
			return Random.Range(RoomThicknessMin, RoomThicknessMax);
		}
	}

	private const int NumberOfRoomExitsMin = 2;
	private const int NumberOfRoomExitsMax = 5;
	public static int RandomNumberOfRoomExits
	{
		get
		{
			return Random.Range(NumberOfRoomExitsMin, NumberOfRoomExitsMax);
		}
	}

	private const float RoomHeightPositionMin = -0.25f;
	private const float RoomHeightPositionMax = 0.25f;
	public static Vector3 RandomRoomHeightPosition
	{
		get
		{
			return Vector3.up * Random.Range(RoomHeightPositionMin, RoomHeightPositionMax);
		}
	}

	private const int NumberOfCorridorQuadsMin = 2;
	private const int NumberOfCorridorQuadsMax = 5;
	public static int RandomNumberOfCorridorQuads
	{
		get
		{
			return Random.Range(NumberOfCorridorQuadsMin, NumberOfCorridorQuadsMax);
		}
	}
}