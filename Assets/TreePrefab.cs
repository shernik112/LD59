using UnityEngine;

using UnityEngine;

public class TreePrefab : OnBehaviour
{
    [SerializeField] private Sprite shadowSprite;
    [SerializeField] private PropData treeData;
    [SerializeField] private PropData rockData;
    [SerializeField] private Material cutoutMaterial;

    protected override void OnInitialize()
    {
        bool isRock = Random.value < 0.5f;

        var data = isRock ? rockData : treeData;

        var randomIndex = Random.Range(0, data.sprites.Length);
        var sprite = data.sprites[randomIndex];

        CreateQuad("A", sprite, data.scale, 0f, -0.01f);
        CreateQuad("B", sprite, data.scale, 90f, 0.01f);
        // CreateShadow(shadowSprite, data.scale);
    }

    private void CreateQuad(string name, Sprite sprite, float scale, float yRotation, float offsetZ)
    {
        var quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quad.name = name;

        quad.transform.SetParent(transform);
        quad.transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);
        quad.transform.localScale = Vector3.one * scale;

        var renderer = quad.GetComponent<MeshRenderer>();

        var matInstance = new Material(cutoutMaterial);
        renderer.material = matInstance;
        matInstance.mainTexture = sprite.texture;

        // ✔ правильное выравнивание по земле
        quad.transform.localPosition = new Vector3(0f, scale * 0.5f, offsetZ);
    }
    
    // private void CreateShadow(Sprite sprite, float scale)
    // {
    //     var shadow = GameObject.CreatePrimitive(PrimitiveType.Quad);
    //     shadow.name = "Shadow";
    //
    //     shadow.transform.SetParent(transform);
    //
    //     // ✔ позиция — на земле
    //     shadow.transform.localPosition = new Vector3(0.04f, -0.077f, -0.181f);
    //
    //     // ✔ твой поворот
    //     shadow.transform.localRotation = Quaternion.Euler(120f, -47f, -42f);
    //
    //     // ✔ немного больше, чем объект
    //     shadow.transform.localScale = Vector3.one * 1.8f;
    //
    //     var renderer = shadow.GetComponent<MeshRenderer>();
    //
    //     var matInstance = new Material(cutoutMaterial);
    //     renderer.material = matInstance;
    //
    //     matInstance.mainTexture = sprite.texture;
    // }
}
    
    [System.Serializable]
    public class PropData
    {
        public PropType type;
        public Sprite[] sprites;
        public float scale = 1f;
    }
    
    
    public enum PropType
    {
        Tree,
        Rock
    }
