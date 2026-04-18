using UnityEngine;

public class TreePrefab : OnBehaviour
{
    [SerializeField] private TreeData data;

    private Transform spriteA;
    private Transform spriteB;

    protected override void OnInitialize()
    {
        var randomIndex = Random.Range(0, data.sprites.Length);
        var sprite = data.sprites[randomIndex];

        spriteA = CreateSprite("A", sprite, 0f);
        spriteB = CreateSprite("B", sprite, 90f);
    }

    private Transform CreateSprite(string name, Sprite sprite, float yRotation)
    {
        var go = new GameObject(name);
        go.transform.SetParent(transform);
        go.transform.localPosition = Vector3.zero;
        
        go.transform.localRotation = Quaternion.Euler(0, yRotation, 0);

        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;

        return go.transform;
    }
}