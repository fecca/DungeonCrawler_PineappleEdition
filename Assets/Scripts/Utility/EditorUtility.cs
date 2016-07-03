using UnityEngine;
using UnityEditor;

public class EditorUtility : Editor
{
	[MenuItem("CONTEXT/MeshFilter/Save Mesh...")]
	public static void SaveMeshInPlace(MenuCommand menuCommand)
	{
		MeshFilter mf = menuCommand.context as MeshFilter;
		Mesh m = mf.sharedMesh;
		SaveMesh(m, m.name, false, true);
	}
	[MenuItem("CONTEXT/MeshFilter/Save Mesh As New Instance...")]
	public static void SaveMeshNewInstanceItem(MenuCommand menuCommand)
	{
		MeshFilter mf = menuCommand.context as MeshFilter;
		Mesh m = mf.sharedMesh;
		SaveMesh(m, m.name, true, true);
	}
	[MenuItem("Utility/Meshes/CombineSelectedObjects")]
	static void CombineSelected()
	{
		if (UnityEditor.EditorUtility.DisplayDialog("Do you want to combine these objects?", "This can't be undone! Make sure you have a backup if you don't know what you're doing.", "Heck Yeah!", "No, I'm scared"))
		{
			int amountSelected = Selection.gameObjects.Length;

			MeshFilter[] meshFilters = new MeshFilter[amountSelected];
			CombineInstance[] combineInstances = new CombineInstance[amountSelected];


			for (var i = 0; i < amountSelected; i++)
			{
				meshFilters[i] = Selection.gameObjects[i].GetComponent<MeshFilter>();

				combineInstances[i].mesh = meshFilters[i].sharedMesh;
				combineInstances[i].transform = meshFilters[i].transform.localToWorldMatrix;
			}

			GameObject obj = new GameObject("CombinededMeshes", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
			obj.GetComponent<MeshFilter>().mesh = new Mesh();
			obj.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(combineInstances);
			obj.GetComponent<MeshRenderer>().sharedMaterial = new Material(meshFilters[0].gameObject.GetComponent<MeshRenderer>().sharedMaterial);
			obj.GetComponent<MeshCollider>().sharedMesh = obj.GetComponent<MeshFilter>().sharedMesh;

			foreach (MeshFilter m in meshFilters)
			{
				DestroyImmediate(m.gameObject);
			}
		}
	}

	public static void SaveMesh(Mesh mesh, string name, bool makeNewInstance, bool optimizeMesh)
	{
		string path = UnityEditor.EditorUtility.SaveFilePanel("Save Separate Mesh Asset", "Assets/", name, "asset");
		if (string.IsNullOrEmpty(path)) return;

		path = FileUtil.GetProjectRelativePath(path);

		Mesh meshToSave = (makeNewInstance) ? Object.Instantiate(mesh) as Mesh : mesh;

		if (optimizeMesh)
			meshToSave.Optimize();

		AssetDatabase.CreateAsset(meshToSave, path);
		AssetDatabase.SaveAssets();
	}
}
