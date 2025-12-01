using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu]
public class EntityType_ScriptableObject : ScriptableObject
{
    public string typeName = "default";
    public GameObject[] entityPrefabs;

    public GameObject RandomGameobject()
    {
        int randomEntity = UnityEngine.Random.Range(0,entityPrefabs.Length);
        GameObject randomGameobject = entityPrefabs[randomEntity];
        return randomGameobject;
    }

}
