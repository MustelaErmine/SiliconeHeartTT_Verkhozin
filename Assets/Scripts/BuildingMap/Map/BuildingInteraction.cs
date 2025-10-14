using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class BuildingInteraction : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerMoveHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Map map;
    private BuildingEditData _buildingData;

    public BuildingState State { get; set; } = new None();

    private Vector3 _mouseDragDelta;
    [SerializeField] private bool _isDrag = false;

    [SerializeField] private DragShadow _dragShadow;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public BuildingEditData BuildingEditData { set  {_buildingData = value; guid = _buildingData.data.GUID.ToString(); } }
    public string guid;

    public void StartDrag(Vector3 mousePosition)
    {
        var mouse = Camera.main.ScreenToWorldPoint(mousePosition);
        mouse.z = 0;
        _mouseDragDelta = new Vector3(-_buildingData.display.sizeX / 2f, -_buildingData.display.sizeY / 2f);
        transform.position = mouse + _mouseDragDelta;

        _isDrag = true;

        _dragShadow.Enable();
        _dragShadow.SetPosition(transform.position);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!_isDrag && State is None)
        {
            StartDrag(eventData.position);
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (_isDrag)
        {
            var mouse = Camera.main.ScreenToWorldPoint(eventData.position);
            mouse.z = 0;
            var new_position = _mouseDragDelta + mouse;
            transform.position = new_position;

            var normalized = NormalizePosition(new_position);
            _dragShadow.SetPosition(normalized);
        }
    }

    public void EndDrag(Vector3 mousePosition)
    {
        if (_isDrag)
        {
            var end_vector = NormalizePosition(transform.position);
            var new_data = new BuildingEditData();
            new_data.data = new BuildingData();

            new_data.data.positionX = end_vector.x;
            new_data.data.positionY = end_vector.y;
            new_data.data.GUID = _buildingData.data.GUID;
            new_data.display = _buildingData.display;


            if (map != null)
            {
                map.PlaceBuilding(new_data);
            }
            else
            {
                transform.position = end_vector;
                _isDrag = false;
            }
        }
    }

    public void SetPosition()
    {
        transform.position = new Vector3(_buildingData.data.positionX, _buildingData.data.positionY, 0f);
        _isDrag = false;
        _dragShadow.Disable();
    }

    static Vector3Int NormalizePosition(Vector3 position)
    {
        return new Vector3Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y), 0);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_isDrag)
        {
            EndDrag(eventData.position);
        }
        else if (State is Delete)
        {
            map.TryDelete(_buildingData.data.GUID);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (State is Delete)
        {
            _spriteRenderer.color = Color.red;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (State is Delete)
        {
            _spriteRenderer.color = Color.white;
        }
    }

    public abstract class BuildingState { }
    public class PlaceNew : BuildingState { }
    public class Delete : BuildingState { }
    public class None : BuildingState { }
}
