using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragShadow : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] Transform _transform;

    public void Enable()
    {
        _spriteRenderer.enabled = true;
    }
    public void Disable()
    {
        _spriteRenderer.enabled = false; 
    }
    public void SetPosition(Vector3 position)
    {
        _transform.position = position;
    }
    public void SetSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }
}
