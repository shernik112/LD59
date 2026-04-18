using UnityEngine;

public class TreePrefab : OnBehaviour
{
    [SerializeField] private TreeData data;
    
    protected override void OnInitialize()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        var randomIndex = Random.Range(0, data.sprites.Length);
        spriteRenderer.sprite = data.sprites[randomIndex];
    }

    protected override void OnUpdate()
    {
        if (data.target != null)
        {
            var direction = data.target.position - transform.position;
            
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
