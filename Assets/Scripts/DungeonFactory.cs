using System.Collections.Generic;
using UnityEngine;

public class DungeonFactory
{
	private RoomFactory _roomFactory = new RoomFactory();
	private CorridorFactory _corridorFactory = new CorridorFactory();

	public List<Module> CreateDungeon()
	{
		//var moduleList = new List<Module>();
		//for (var i = 0; i < 1; i++)
		//{
		//	moduleList.Add(_roomFactory.CreateCircularRoom(19, 10, 1, 3));
		//}

		//for (var i = 0; i < moduleList.Count; i++)
		//{
		//	var module = moduleList[i];
		//	CreateGameObject(module);
		//}




		var room = _roomFactory.CreateCircularRoom(19, 10, 1, 3);
		var corridor = _corridorFactory.CreateCorridor(room, 25);











		return new List<Module>()
		{
		};
	}

	//private GameObject CreateGameObject(CircularRoom room)
	//{
	//	var newGameObject = new GameObject("Module");

	//	var mesh = new Mesh();
	//	mesh.vertices = room.GetVertices();
	//	mesh.triangles = triangles.ToArray();
	//	mesh.RecalculateBounds();
	//	mesh.RecalculateNormals();

	//	var meshFilter = newGameObject.AddComponent<MeshFilter>();
	//	meshFilter.mesh = mesh;

	//	var meshRenderer = newGameObject.AddComponent<MeshRenderer>();
	//	meshRenderer.material = Resources.Load<Material>("WallMaterial_Unlit");

	//	var meshCollider = newGameObject.AddComponent<MeshCollider>();
	//	meshCollider.sharedMesh = mesh;

	//	//var room = newGameObject.AddComponent<Module>();
	//	//room.AddExitVertices(exitVertices);
	//}
}