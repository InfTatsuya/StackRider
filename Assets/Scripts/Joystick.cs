using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private Vector2 startPosition;
    private Vector2 moveDirection;
    private bool isDragging;

    [SerializeField] float thershold = 0.2f;

    public Vector2 Direction
    {
        get => isDragging ? moveDirection : Vector2.zero;
        private set => moveDirection = value;
    }

    private void Start()
    {
        Direction = Vector2.zero;
        isDragging = false;
    }

    private void LateUpdate()
    {
        isDragging = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        isDragging = false;

        if(eventData.delta.sqrMagnitude > thershold)
        {
            Direction = (eventData.position - startPosition).normalized;
            startPosition = eventData.position;
            isDragging = true;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startPosition = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Direction = Vector2.zero;
        isDragging = false;
    }
}
