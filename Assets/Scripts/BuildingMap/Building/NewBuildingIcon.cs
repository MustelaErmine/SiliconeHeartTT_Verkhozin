using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class NewBuildingIcon : MonoBehaviour, IPointerClickHandler
{
    public BuildingIconData Data { get; set; }

    public PlaceNewBuilding Controller { get; set; }

    [SerializeField] private Image _image;

    public void Initialize()
    {
        _image.sprite = Data.sprite;
    }
    public void Place()
    {
        Controller.Place(Data.guid);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Place();
    }
}
