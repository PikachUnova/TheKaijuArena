using UnityEngine;
using UnityEditor;

public class TerrainTreesToGameObjects : EditorWindow
{
    [MenuItem("Tools/Convert Terrain Trees")]
    static void Convert()
    {
        Terrain terrain = Terrain.activeTerrain;
        if (!terrain) { Debug.LogError("No active terrain found."); return; }

        TerrainData data = terrain.terrainData;
        GameObject parent = new GameObject("Converted Trees");

        foreach (TreeInstance tree in data.treeInstances)
        {
            GameObject prefab = data.treePrototypes[tree.prototypeIndex].prefab;
            Vector3 position = Vector3.Scale(tree.position, data.size) + terrain.transform.position;
            Vector3 scale = new Vector3(tree.widthScale, tree.heightScale, tree.widthScale);
            Quaternion rotation = Quaternion.Euler(0, tree.rotation * Mathf.Rad2Deg, 0);

            GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            instance.transform.localScale = scale;
            instance.transform.parent = parent.transform;
        }

        // Optional: Clear trees from terrain so they aren't duplicated
        data.treeInstances = new TreeInstance[0];
    }
}
