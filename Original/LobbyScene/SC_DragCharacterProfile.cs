using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SC_DragCharacterProfile : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public static Vector2 DefaultPos;

    [SerializeField] int CharacterIndex;
    Sprite DefaultImage;

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        SC_HeroTabUI._Instance.DragIndex = CharacterIndex;
        DefaultImage = GetComponent<Image>().sprite;
        DefaultPos = transform.position;

        GetComponent<Image>().raycastTarget = false;       
        GetComponent<Image>().sprite = SC_CharacterPool._instance.characterPool[CharacterIndex].GetComponent<SpriteRenderer>().sprite;

    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        Vector2 currentPos = eventData.position;
        transform.position = currentPos;
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {

        GetComponent<Image>().raycastTarget = true;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = DefaultPos;
        GetComponent<Image>().sprite = DefaultImage;

    }
}
