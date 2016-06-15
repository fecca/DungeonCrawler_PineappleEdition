using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonHandler : MonoBehaviour
{
	public List<GameObject> RoomPrefabs = new List<GameObject>();
	public int NumberOfRooms = 5;

	private List<Room> _rooms;

	private void Awake()
	{
		_rooms = new List<Room>();
	}

	public void CreateDungeon()
	{
		StartCoroutine(CreateRooms());
	}

	private IEnumerator CreateRooms()
	{
		// Create first room
		var gameObject = Instantiate(GetRandomRoomPrefabOfType(ModuleType.Room));
		var firstRoom = gameObject.GetComponent<Room>();
		var _roomList = new List<Room>() { firstRoom };

		for (var i = 0; i < NumberOfRooms; i++)
		{
			var room = _roomList.GetRandomElement();

			var exits = room.GetExits();
			for (var j = 0; j < exits.Count; j++)
			{
				var fromRoomExit = exits[j];
				if (!fromRoomExit.Open)
				{
					continue;
				}

				var newType = room.Type == ModuleType.Room ? ModuleType.Corridor : ModuleType.Room;
				var newGameObject = Instantiate(GetRandomRoomPrefabOfType(newType));
				var newRoom = newGameObject.GetComponent<Room>();
				var newRoomExits = newRoom.GetExits();
				var exitToMatch = newRoomExits.GetRandomElement();
				exitToMatch.Transform.gameObject.SnapTo(fromRoomExit.Transform.gameObject);

				yield return new WaitForFixedUpdate();

				if (newRoom.IsColliding)
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

		yield return null;
	}
	private GameObject GetRandomRoomPrefabOfType(ModuleType type)
	{
		var prefabsOfType = RoomPrefabs.Where(p => p.GetComponent<Room>().Type == type).ToList();

		return prefabsOfType.GetRandomElement();
	}
}