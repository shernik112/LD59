using UnityEngine;

[CreateAssetMenu(fileName = "TreeData", menuName = "Scriptable Objects/TreeData")]
public class TreeData : ScriptableObject
{
    public Sprite[] sprites;
    public Transform target;
}
