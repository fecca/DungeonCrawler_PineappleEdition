using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonHandler : MonoBehaviour
{
	public List<GameObject> RoomPrefabs = new List<GameObject>();
	public List<GameObject> CorridorPrefabs = new List<GameObject>();
	public int NumberOfRooms = 5;

	private List<Room> _rooms;
	private List<Bounds> _bounds;

	private void Awake()
	{
		_rooms = new List<Room>();
		_bounds = new List<Bounds>();
	}

	public void CreateDungeon()
	{
		// Create first room
		var gameObject = Instantiate(GetRandomRoomPrefabOfType(ModuleType.Room));
		var firstRoom = gameObject.GetComponent<Room>();
		var _roomList = new List<Room>() { firstRoom };

		for (var roomIndex = 0; roomIndex < NumberOfRooms; roomIndex++)
		{
			var room = _roomList.GetRandomElement();
			if (room == null)
			{
				break;
			}
			_rooms.Add(room);

			var exits = room.GetExits();
			for (var exitIndex = 0; exitIndex < exits.Count; exitIndex++)
			{
				var fromRoomExit = exits[exitIndex];
				if (!fromRoomExit.Open)
				{
					continue;
				}

				var newType = GetRandomModuleType(room.Type);
				var newGameObject = Instantiate(GetRandomRoomPrefabOfType(newType));
				var newRoom = newGameObject.GetComponent<Room>();
				var newRoomExits = newRoom.GetExits();
				var exitToMatch = newRoomExits.GetRandomElement();
				exitToMatch.Transform.gameObject.SnapTo(fromRoomExit.Transform.gameObject);

				if (CheckIntersection(newGameObject))
				{
					Destroy(newGameObject);
					continue;
				}

				_roomList.Add(newRoom);
				exitToMatch.Open = false;
				fromRoomExit.Open = false;
			}
			_roomList.Remove(room);
		}
	}
	public Vector3 GetStartingPosition()
	{
		if (_rooms.Count > 0)
		{
			return _rooms[0].transform.position;
		}

		return Vector3.zero;
	}

	private bool CheckIntersection(GameObject newGameObject)
	{
		var bounds = new Bounds(newGameObject.transform.position, Vector3.one);
		foreach (var collider in newGameObject.GetComponentsInChildren<Collider>())
		{
			bounds.Encapsulate(collider.bounds);
		}
		bounds.size *= 0.95f;

		for (var k = 0; k < _bounds.Count; k++)
		{
			if (bounds.Intersects(_bounds[k]))
			{
				return true;
			}
		}
		_bounds.Add(bounds);

		return false;
	}
	private GameObject GetRandomRoomPrefabOfType(ModuleType type)
	{
		switch (type)
		{
			case ModuleType.Room:
				return RoomPrefabs.GetRandomElement();
			case ModuleType.Corridor:
				return CorridorPrefabs.GetRandomElement();
			default:
				throw new System.NotSupportedException("Module type not supported");
		}
	}
	private ModuleType GetRandomModuleType(ModuleType type)
	{
		if (type == ModuleType.Room)
		{
			return ModuleType.Corridor;
		}

		if (type == ModuleType.Corridor)
		{
			var fiftyFifty = Random.Range(0, 2) % 2;
			return fiftyFifty == 0 ? ModuleType.Corridor : ModuleType.Room;
		}

		return ModuleType.Corridor;
	}
}