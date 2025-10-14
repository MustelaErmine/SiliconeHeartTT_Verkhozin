using UnityEngine;
using Zenject;
public class BuildingDisplay : MonoBehaviour
{
    private BuildingDisplayData _buildingDisplayData;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private BoxCollider2D _boxCollider;
    [SerializeField] private DragShadow _dragShadow;

    public void Display(BuildingDisplayData buildingDisplayData)
    {
        _buildingDisplayData = buildingDisplayData;

        AdaptCollider();
        SetSprite();
    }
    void AdaptCollider()
    {
        _boxCollider.offset = new Vector2(_buildingDisplayData.sizeX / 2f, _buildingDisplayData.sizeY / 2f);
        _boxCollider.size = new Vector2(_buildingDisplayData.sizeX, _buildingDisplayData.sizeY);
    }
    void SetSprite()
    {
        _spriteRenderer.sprite = _buildingDisplayData.sprite;
        _dragShadow.SetSprite(_buildingDisplayData.sprite);
    }
}

