using System.Collections.Generic;
using UnityEngine;

public class MeshFactory : MonoBehaviour
{
	public void CreateDungeonMeshes(List<DungeonObject> dungeonData)
	{
		for (var i = 0; i < dungeonData.Count; i++)
		{
			var dungeonObject = dungeonData[i];
			switch (dungeonObject.Type)
			{
				case ModuleType.Room:
					var roomTriangles = CreateTriangles(dungeonObject as Room);
					CreateGameObject(dungeonObject.Vertices, roomTriangles);
					break;
				case ModuleType.Corridor:
					var corridorTriangles = CreateTriangles(dungeonObject as Corridor);
					CreateGameObject(dungeonObject.Vertices, corridorTriangles);
					break;
				default:
					break;
			}
		}
	}

	private List<int> CreateTriangles(Room room)
	{
		var vertices = room.Vertices;
		var numberOfVerticesPerGroup = vertices.Count / room.NumberOfCorners;

		var newVertices = new List<Vector3>();
		var newTriangles = new List<int>();
		for (var i = 0; i < room.NumberOfCorners; i++)
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

			if (room.ExitCorners.Contains(i))
			{
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

		room.SetVertices(newVertices);

		return newTriangles;
	}
	private List<int> CreateTriangles(Corridor corridor)
	{
		var vertices = corridor.Vertices;
		var newVertices = new List<Vector3>();
		var triangles = new List<int>();
		var numberOfVerticesPerGroup = vertices.Count / (corridor.NumberOfQuads + 1);

		for (var i = 0; i < corridor.NumberOfQuads; i++)
		{
			var thisGroup = numberOfVerticesPerGroup * i;
			var nextGroup = thisGroup + numberOfVerticesPerGroup;

			var thisLeftOutsideFloorVertex = thisGroup;
			var thisLeftOutsideWallVertex = thisGroup + 1;
			var thisLeftWallVertex = thisGroup + 2;
			var thisLeftFloorVertex = thisGroup + 3;
			var thisRightFloorVertex = thisGroup + 4;
			var thisRightWallVertex = thisGroup + 5;
			var thisRightOutsideWallVertex = thisGroup + 6;
			var thisRightOutsideFloorVertex = thisGroup + 7;

			var nextLeftOutsideFloorVertex = nextGroup;
			var nextLeftOutsideWallVertex = nextGroup + 1;
			var nextLeftWallVertex = nextGroup + 2;
			var nextLeftFloorVertex = nextGroup + 3;
			var nextRightFloorVertex = nextGroup + 4;
			var nextRightWallVertex = nextGroup + 5;
			var nextRightOutsideWallVertex = nextGroup + 6;
			var nextRightOutsideFloorVertex = nextGroup + 7;

			// Left outside wall
			{
				newVertices.Add(vertices[thisLeftOutsideFloorVertex]);
				newVertices.Add(vertices[nextLeftOutsideFloorVertex]);
				newVertices.Add(vertices[nextLeftOutsideWallVertex]);
				triangles.Add(newVertices.Count - 3);
				triangles.Add(newVertices.Count - 2);
				triangles.Add(newVertices.Count - 1);

				newVertices.Add(vertices[nextLeftOutsideWallVertex]);
				newVertices.Add(vertices[thisLeftOutsideWallVertex]);
				newVertices.Add(vertices[thisLeftOutsideFloorVertex]);
				triangles.Add(newVertices.Count - 3);
				triangles.Add(newVertices.Count - 2);
				triangles.Add(newVertices.Count - 1);
			}

			// Left roof
			{
				newVertices.Add(vertices[thisLeftOutsideWallVertex]);
				newVertices.Add(vertices[nextLeftOutsideWallVertex]);
				newVertices.Add(vertices[nextLeftWallVertex]);
				triangles.Add(newVertices.Count - 3);
				triangles.Add(newVertices.Count - 2);
				triangles.Add(newVertices.Count - 1);

				newVertices.Add(vertices[nextLeftWallVertex]);
				newVertices.Add(vertices[thisLeftWallVertex]);
				newVertices.Add(vertices[thisLeftOutsideWallVertex]);
				triangles.Add(newVertices.Count - 3);
				triangles.Add(newVertices.Count - 2);
				triangles.Add(newVertices.Count - 1);
			}

			// Left wall
			{
				newVertices.Add(vertices[thisLeftWallVertex]);
				newVertices.Add(vertices[nextLeftWallVertex]);
				newVertices.Add(vertices[nextLeftFloorVertex]);
				triangles.Add(newVertices.Count - 3);
				triangles.Add(newVertices.Count - 2);
				triangles.Add(newVertices.Count - 1);

				newVertices.Add(vertices[nextLeftFloorVertex]);
				newVertices.Add(vertices[thisLeftFloorVertex]);
				newVertices.Add(vertices[thisLeftWallVertex]);
				triangles.Add(newVertices.Count - 3);
				triangles.Add(newVertices.Count - 2);
				triangles.Add(newVertices.Count - 1);
			}

			// Floor
			{
				newVertices.Add(vertices[thisLeftFloorVertex]);
				newVertices.Add(vertices[nextLeftFloorVertex]);
				newVertices.Add(vertices[nextRightFloorVertex]);
				triangles.Add(newVertices.Count - 3);
				triangles.Add(newVertices.Count - 2);
				triangles.Add(newVertices.Count - 1);

				newVertices.Add(vertices[nextRightFloorVertex]);
				newVertices.Add(vertices[thisRightFloorVertex]);
				newVertices.Add(vertices[thisLeftFloorVertex]);
				triangles.Add(newVertices.Count - 3);
				triangles.Add(newVertices.Count - 2);
				triangles.Add(newVertices.Count - 1);
			}

			// Right wall
			{
				newVertices.Add(vertices[thisRightFloorVertex]);
				newVertices.Add(vertices[nextRightFloorVertex]);
				newVertices.Add(vertices[nextRightWallVertex]);
				triangles.Add(newVertices.Count - 3);
				triangles.Add(newVertices.Count - 2);
				triangles.Add(newVertices.Count - 1);

				newVertices.Add(vertices[nextRightWallVertex]);
				newVertices.Add(vertices[thisRightWallVertex]);
				newVertices.Add(vertices[thisRightFloorVertex]);
				triangles.Add(newVertices.Count - 3);
				triangles.Add(newVertices.Count - 2);
				triangles.Add(newVertices.Count - 1);
			}

			// Right roof
			{
				newVertices.Add(vertices[thisRightOutsideWallVertex]);
				newVertices.Add(vertices[thisRightWallVertex]);
				newVertices.Add(vertices[nextRightWallVertex]);
				triangles.Add(newVertices.Count - 3);
				triangles.Add(newVertices.Count - 2);
				triangles.Add(newVertices.Count - 1);

				newVertices.Add(vertices[nextRightWallVertex]);
				newVertices.Add(vertices[nextRightOutsideWallVertex]);
				newVertices.Add(vertices[thisRightOutsideWallVertex]);
				triangles.Add(newVertices.Count - 3);
				triangles.Add(newVertices.Count - 2);
				triangles.Add(newVertices.Count - 1);
			}

			// Right outside wall
			{
				newVertices.Add(vertices[thisRightOutsideFloorVertex]);
				newVertices.Add(vertices[thisRightOutsideWallVertex]);
				newVertices.Add(vertices[nextRightOutsideWallVertex]);
				triangles.Add(newVertices.Count - 3);
				triangles.Add(newVertices.Count - 2);
				triangles.Add(newVertices.Count - 1);

				newVertices.Add(vertices[nextRightOutsideWallVertex]);
				newVertices.Add(vertices[nextRightOutsideFloorVertex]);
				newVertices.Add(vertices[thisRightOutsideFloorVertex]);
				triangles.Add(newVertices.Count - 3);
				triangles.Add(newVertices.Count - 2);
				triangles.Add(newVertices.Count - 1);
			}
		}

		corridor.SetVertices(newVertices);

		return triangles;
	}
	private void CreateGameObject(List<Vector3> vertices, List<int> triangles)
	{
		var newGameObject = new GameObject();
		newGameObject.transform.position = Vector3.zero;

		var mesh = new Mesh
		{
			vertices = vertices.ToArray(),
			triangles = triangles.ToArray()
		};
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();

		var meshFilter = newGameObject.AddComponent<MeshFilter>();
		meshFilter.mesh = mesh;

		var meshRenderer = newGameObject.AddComponent<MeshRenderer>();
		meshRenderer.material = Resources.Load<Material>("WallMaterial_Unlit");

		var meshCollider = newGameObject.AddComponent<MeshCollider>();
		meshCollider.sharedMesh = mesh;
	}
}