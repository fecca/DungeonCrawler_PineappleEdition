using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class Module : MonoBehaviour
{
	public RoomModel Model { get; set; }

	private List<Vector3> _exits = new List<Vector3>();

	public void AddExitVertices(List<Vector3> exits)
	{
		_exits = exits;
	}

	public List<Vector3> GetExitVertices()
	{
		return new List<Vector3>(_exits);
	}

	public abstract void GenerateTriangles();
	public abstract void CreateMesh();
}

public class Room : Module
{
	public override void GenerateTriangles()
	{
		var vertices = Model.SharedVertices;
		var numberOfCorners = Model.NumberOfCorners;
		var numberOfVerticesPerGroup = vertices.Count / numberOfCorners;

		var newVertices = new List<Vector3>();
		var newTriangles = new List<int>();
		for (var i = 0; i < numberOfCorners; i++)
		{
			var thisGroup = (numberOfVerticesPerGroup * i);
			var nextGroup = thisGroup + numberOfVerticesPerGroup >= vertices.Count ? 0 : thisGroup + numberOfVerticesPerGroup;

			var thisOrigoVertex = thisGroup;
			var thisFloorVertex1 = thisGroup + 1;
			var thisFloorVertex2 = thisGroup + 2;
			var thisCornerVertex = thisGroup + 3;
			var thisWallVertex = thisGroup + 4;
			var thisOutsideWallVertex = thisGroup + 5;
			var thisOutsideCornerVertex = thisGroup + 6;

			var nextFloorVertex1 = nextGroup + 1;
			var nextFloorVertex2 = nextGroup + 2;
			var nextCornerVertex = nextGroup + 3;
			var nextWallVertex = nextGroup + 4;
			var nextOutsideWallVertex = nextGroup + 5;
			var nextOutsideCornerVertex = nextGroup + 6;

			// Floor
			{
				newVertices.Add(vertices[thisOrigoVertex]);
				newVertices.Add(vertices[thisFloorVertex1]);
				newVertices.Add(vertices[nextFloorVertex1]);
				newTriangles.Add(newVertices.Count - 3);
				newTriangles.Add(newVertices.Count - 2);
				newTriangles.Add(newVertices.Count - 1);

				newVertices.Add(vertices[thisFloorVertex1]);
				newVertices.Add(vertices[thisFloorVertex2]);
				newVertices.Add(vertices[nextFloorVertex2]);
				newTriangles.Add(newVertices.Count - 3);
				newTriangles.Add(newVertices.Count - 2);
				newTriangles.Add(newVertices.Count - 1);

				newVertices.Add(vertices[nextFloorVertex2]);
				newVertices.Add(vertices[nextFloorVertex1]);
				newVertices.Add(vertices[thisFloorVertex1]);
				newTriangles.Add(newVertices.Count - 3);
				newTriangles.Add(newVertices.Count - 2);
				newTriangles.Add(newVertices.Count - 1);

				newVertices.Add(vertices[thisCornerVertex]);
				newVertices.Add(vertices[nextCornerVertex]);
				newVertices.Add(vertices[nextFloorVertex2]);
				newTriangles.Add(newVertices.Count - 3);
				newTriangles.Add(newVertices.Count - 2);
				newTriangles.Add(newVertices.Count - 1);

				newVertices.Add(vertices[nextFloorVertex2]);
				newVertices.Add(vertices[thisFloorVertex2]);
				newVertices.Add(vertices[thisCornerVertex]);
				newTriangles.Add(newVertices.Count - 3);
				newTriangles.Add(newVertices.Count - 2);
				newTriangles.Add(newVertices.Count - 1);
			}

			if (Model.ExitCorner == i)
			{
				//// Save exit vertices
				//exitVertices.Add(vertices[thisOutsideWallVertex]);
				//exitVertices.Add(vertices[thisOutsideCornerVertex]);
				//exitVertices.Add(vertices[nextOutsideCornerVertex]);
				//exitVertices.Add(vertices[nextOutsideWallVertex]);

				// Left wall
				{
					newVertices.Add(vertices[thisCornerVertex]);
					newVertices.Add(vertices[thisWallVertex]);
					newVertices.Add(vertices[thisOutsideWallVertex]);
					newTriangles.Add(newVertices.Count - 3);
					newTriangles.Add(newVertices.Count - 2);
					newTriangles.Add(newVertices.Count - 1);

					newVertices.Add(vertices[thisOutsideWallVertex]);
					newVertices.Add(vertices[thisOutsideCornerVertex]);
					newVertices.Add(vertices[thisCornerVertex]);
					newTriangles.Add(newVertices.Count - 3);
					newTriangles.Add(newVertices.Count - 2);
					newTriangles.Add(newVertices.Count - 1);
				}

				// Floor
				{
					newVertices.Add(vertices[thisCornerVertex]);
					newVertices.Add(vertices[thisOutsideCornerVertex]);
					newVertices.Add(vertices[nextOutsideCornerVertex]);
					newTriangles.Add(newVertices.Count - 3);
					newTriangles.Add(newVertices.Count - 2);
					newTriangles.Add(newVertices.Count - 1);

					newVertices.Add(vertices[nextOutsideCornerVertex]);
					newVertices.Add(vertices[nextCornerVertex]);
					newVertices.Add(vertices[thisCornerVertex]);
					newTriangles.Add(newVertices.Count - 3);
					newTriangles.Add(newVertices.Count - 2);
					newTriangles.Add(newVertices.Count - 1);
				}

				// Right wall
				{
					newVertices.Add(vertices[nextCornerVertex]);
					newVertices.Add(vertices[nextOutsideCornerVertex]);
					newVertices.Add(vertices[nextOutsideWallVertex]);
					newTriangles.Add(newVertices.Count - 3);
					newTriangles.Add(newVertices.Count - 2);
					newTriangles.Add(newVertices.Count - 1);

					newVertices.Add(vertices[nextOutsideWallVertex]);
					newVertices.Add(vertices[nextWallVertex]);
					newVertices.Add(vertices[nextCornerVertex]);
					newTriangles.Add(newVertices.Count - 3);
					newTriangles.Add(newVertices.Count - 2);
					newTriangles.Add(newVertices.Count - 1);
				}

				continue;
			}

			// Inside wall
			{
				newVertices.Add(vertices[thisCornerVertex]);
				newVertices.Add(vertices[thisWallVertex]);
				newVertices.Add(vertices[nextWallVertex]);
				newTriangles.Add(newVertices.Count - 3);
				newTriangles.Add(newVertices.Count - 2);
				newTriangles.Add(newVertices.Count - 1);

				newVertices.Add(vertices[nextWallVertex]);
				newVertices.Add(vertices[nextCornerVertex]);
				newVertices.Add(vertices[thisCornerVertex]);
				newTriangles.Add(newVertices.Count - 3);
				newTriangles.Add(newVertices.Count - 2);
				newTriangles.Add(newVertices.Count - 1);
			}

			// Roof
			{
				newVertices.Add(vertices[thisWallVertex]);
				newVertices.Add(vertices[thisOutsideWallVertex]);
				newVertices.Add(vertices[nextOutsideWallVertex]);
				newTriangles.Add(newVertices.Count - 3);
				newTriangles.Add(newVertices.Count - 2);
				newTriangles.Add(newVertices.Count - 1);

				newVertices.Add(vertices[nextOutsideWallVertex]);
				newVertices.Add(vertices[nextWallVertex]);
				newVertices.Add(vertices[thisWallVertex]);
				newTriangles.Add(newVertices.Count - 3);
				newTriangles.Add(newVertices.Count - 2);
				newTriangles.Add(newVertices.Count - 1);
			}

			// Outside wall
			{
				newVertices.Add(vertices[thisOutsideWallVertex]);
				newVertices.Add(vertices[thisOutsideCornerVertex]);
				newVertices.Add(vertices[nextOutsideCornerVertex]);
				newTriangles.Add(newVertices.Count - 3);
				newTriangles.Add(newVertices.Count - 2);
				newTriangles.Add(newVertices.Count - 1);

				newVertices.Add(vertices[nextOutsideCornerVertex]);
				newVertices.Add(vertices[nextOutsideWallVertex]);
				newVertices.Add(vertices[thisOutsideWallVertex]);
				newTriangles.Add(newVertices.Count - 3);
				newTriangles.Add(newVertices.Count - 2);
				newTriangles.Add(newVertices.Count - 1);
			}
		}

		Model.UniqueVertices = newVertices;
		Model.Triangles = newTriangles;
	}
	public override void CreateMesh()
	{
		var roomGameObject = gameObject;
		GenerateTriangles();

		var mesh = new Mesh();
		mesh.vertices = Model.UniqueVertices.ToArray();
		mesh.triangles = Model.Triangles.ToArray();
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();

		var meshFilter = roomGameObject.AddComponent<MeshFilter>();
		meshFilter.mesh = mesh;

		var meshRenderer = roomGameObject.AddComponent<MeshRenderer>();
		meshRenderer.material = Resources.Load<Material>("WallMaterial_Unlit");

		var meshCollider = roomGameObject.AddComponent<MeshCollider>();
		meshCollider.sharedMesh = mesh;
	}
}

public class Corridor : Module
{
	public override void CreateMesh()
	{
		throw new NotImplementedException();
	}
	public override void GenerateTriangles()
	{
		throw new NotImplementedException();
	}
	//public override void GenerateTriangles()//, int numberOfCorners, ref List<Vector3> vertices, ref List<int> triangles, ref List<Vector3> exitVertices)
	//{
	//	var vertices = Model.SharedVertices;
	//	var numberOfCorners = Model.NumberOfCorners;
	//	var newVertices = new List<Vector3>();
	//	var newTriangles = new List<int>();
	//	var numberOfVerticesPerGroup = vertices.Count / numberOfCorners;

	//	for (var i = 0; i < numberOfCorners; i++)
	//	{
	//		var thisGroup = (numberOfVerticesPerGroup * i);
	//		var nextGroup = thisGroup + numberOfVerticesPerGroup >= vertices.Count ? 0 : thisGroup + numberOfVerticesPerGroup;

	//		var thisOrigoVertex = thisGroup;
	//		var thisFloorVertex1 = thisGroup + 1;
	//		var thisFloorVertex2 = thisGroup + 2;
	//		var thisCornerVertex = thisGroup + 3;
	//		var thisWallVertex = thisGroup + 4;
	//		var thisOutsideWallVertex = thisGroup + 5;
	//		var thisOutsideCornerVertex = thisGroup + 6;

	//		var nextFloorVertex1 = nextGroup + 1;
	//		var nextFloorVertex2 = nextGroup + 2;
	//		var nextCornerVertex = nextGroup + 3;
	//		var nextWallVertex = nextGroup + 4;
	//		var nextOutsideWallVertex = nextGroup + 5;
	//		var nextOutsideCornerVertex = nextGroup + 6;

	//		// Floor
	//		{
	//			newVertices.Add(vertices[thisOrigoVertex]);
	//			newVertices.Add(vertices[thisFloorVertex1]);
	//			newVertices.Add(vertices[nextFloorVertex1]);
	//			newTriangles.Add(newVertices.Count - 3);
	//			newTriangles.Add(newVertices.Count - 2);
	//			newTriangles.Add(newVertices.Count - 1);

	//			newVertices.Add(vertices[thisFloorVertex1]);
	//			newVertices.Add(vertices[thisFloorVertex2]);
	//			newVertices.Add(vertices[nextFloorVertex2]);
	//			newTriangles.Add(newVertices.Count - 3);
	//			newTriangles.Add(newVertices.Count - 2);
	//			newTriangles.Add(newVertices.Count - 1);

	//			newVertices.Add(vertices[nextFloorVertex2]);
	//			newVertices.Add(vertices[nextFloorVertex1]);
	//			newVertices.Add(vertices[thisFloorVertex1]);
	//			newTriangles.Add(newVertices.Count - 3);
	//			newTriangles.Add(newVertices.Count - 2);
	//			newTriangles.Add(newVertices.Count - 1);

	//			newVertices.Add(vertices[thisCornerVertex]);
	//			newVertices.Add(vertices[nextCornerVertex]);
	//			newVertices.Add(vertices[nextFloorVertex2]);
	//			newTriangles.Add(newVertices.Count - 3);
	//			newTriangles.Add(newVertices.Count - 2);
	//			newTriangles.Add(newVertices.Count - 1);

	//			newVertices.Add(vertices[nextFloorVertex2]);
	//			newVertices.Add(vertices[thisFloorVertex2]);
	//			newVertices.Add(vertices[thisCornerVertex]);
	//			newTriangles.Add(newVertices.Count - 3);
	//			newTriangles.Add(newVertices.Count - 2);
	//			newTriangles.Add(newVertices.Count - 1);
	//		}

	//		if (Model.ExitCorners.Contains(i))
	//		{
	//			//// Save exit vertices
	//			//exitVertices.Add(vertices[thisOutsideWallVertex]);
	//			//exitVertices.Add(vertices[thisOutsideCornerVertex]);
	//			//exitVertices.Add(vertices[nextOutsideCornerVertex]);
	//			//exitVertices.Add(vertices[nextOutsideWallVertex]);

	//			// Left wall
	//			{
	//				newVertices.Add(vertices[thisCornerVertex]);
	//				newVertices.Add(vertices[thisWallVertex]);
	//				newVertices.Add(vertices[thisOutsideWallVertex]);
	//				newTriangles.Add(newVertices.Count - 3);
	//				newTriangles.Add(newVertices.Count - 2);
	//				newTriangles.Add(newVertices.Count - 1);

	//				newVertices.Add(vertices[thisOutsideWallVertex]);
	//				newVertices.Add(vertices[thisOutsideCornerVertex]);
	//				newVertices.Add(vertices[thisCornerVertex]);
	//				newTriangles.Add(newVertices.Count - 3);
	//				newTriangles.Add(newVertices.Count - 2);
	//				newTriangles.Add(newVertices.Count - 1);
	//			}

	//			// Floor
	//			{
	//				newVertices.Add(vertices[thisCornerVertex]);
	//				newVertices.Add(vertices[thisOutsideCornerVertex]);
	//				newVertices.Add(vertices[nextOutsideCornerVertex]);
	//				newTriangles.Add(newVertices.Count - 3);
	//				newTriangles.Add(newVertices.Count - 2);
	//				newTriangles.Add(newVertices.Count - 1);

	//				newVertices.Add(vertices[nextOutsideCornerVertex]);
	//				newVertices.Add(vertices[nextCornerVertex]);
	//				newVertices.Add(vertices[thisCornerVertex]);
	//				newTriangles.Add(newVertices.Count - 3);
	//				newTriangles.Add(newVertices.Count - 2);
	//				newTriangles.Add(newVertices.Count - 1);
	//			}

	//			// Right wall
	//			{
	//				newVertices.Add(vertices[nextCornerVertex]);
	//				newVertices.Add(vertices[nextOutsideCornerVertex]);
	//				newVertices.Add(vertices[nextOutsideWallVertex]);
	//				newTriangles.Add(newVertices.Count - 3);
	//				newTriangles.Add(newVertices.Count - 2);
	//				newTriangles.Add(newVertices.Count - 1);

	//				newVertices.Add(vertices[nextOutsideWallVertex]);
	//				newVertices.Add(vertices[nextWallVertex]);
	//				newVertices.Add(vertices[nextCornerVertex]);
	//				newTriangles.Add(newVertices.Count - 3);
	//				newTriangles.Add(newVertices.Count - 2);
	//				newTriangles.Add(newVertices.Count - 1);
	//			}

	//			continue;
	//		}

	//		// Inside wall
	//		{
	//			newVertices.Add(vertices[thisCornerVertex]);
	//			newVertices.Add(vertices[thisWallVertex]);
	//			newVertices.Add(vertices[nextWallVertex]);
	//			newTriangles.Add(newVertices.Count - 3);
	//			newTriangles.Add(newVertices.Count - 2);
	//			newTriangles.Add(newVertices.Count - 1);

	//			newVertices.Add(vertices[nextWallVertex]);
	//			newVertices.Add(vertices[nextCornerVertex]);
	//			newVertices.Add(vertices[thisCornerVertex]);
	//			newTriangles.Add(newVertices.Count - 3);
	//			newTriangles.Add(newVertices.Count - 2);
	//			newTriangles.Add(newVertices.Count - 1);
	//		}

	//		// Roof
	//		{
	//			newVertices.Add(vertices[thisWallVertex]);
	//			newVertices.Add(vertices[thisOutsideWallVertex]);
	//			newVertices.Add(vertices[nextOutsideWallVertex]);
	//			newTriangles.Add(newVertices.Count - 3);
	//			newTriangles.Add(newVertices.Count - 2);
	//			newTriangles.Add(newVertices.Count - 1);

	//			newVertices.Add(vertices[nextOutsideWallVertex]);
	//			newVertices.Add(vertices[nextWallVertex]);
	//			newVertices.Add(vertices[thisWallVertex]);
	//			newTriangles.Add(newVertices.Count - 3);
	//			newTriangles.Add(newVertices.Count - 2);
	//			newTriangles.Add(newVertices.Count - 1);
	//		}

	//		// Outside wall
	//		{
	//			newVertices.Add(vertices[thisOutsideWallVertex]);
	//			newVertices.Add(vertices[thisOutsideCornerVertex]);
	//			newVertices.Add(vertices[nextOutsideCornerVertex]);
	//			newTriangles.Add(newVertices.Count - 3);
	//			newTriangles.Add(newVertices.Count - 2);
	//			newTriangles.Add(newVertices.Count - 1);

	//			newVertices.Add(vertices[nextOutsideCornerVertex]);
	//			newVertices.Add(vertices[nextOutsideWallVertex]);
	//			newVertices.Add(vertices[thisOutsideWallVertex]);
	//			newTriangles.Add(newVertices.Count - 3);
	//			newTriangles.Add(newVertices.Count - 2);
	//			newTriangles.Add(newVertices.Count - 1);
	//		}
	//	}

	//	Model.UniqueVertices = newVertices;
	//	Model.Triangles = newTriangles;
	//}
}

public class RoomModel
{
	public int NumberOfCorners { get; private set; }
	public int Radius { get; private set; }
	public int Height { get; private set; }
	public int Thickness { get; private set; }
	public List<Vector3> UniqueVertices { get; set; }
	public List<Vector3> SharedVertices { get; set; }
	public List<int> Triangles { get; set; }
	public List<Vector3> ExitVertices { get; set; }
	public int ExitCorner { get; set; }

	public RoomModel(int numberOfCorners, int radius, int height, int thickness)
	{
		NumberOfCorners = numberOfCorners;
		Radius = radius;
		Height = height;
		Thickness = thickness;
	}
}