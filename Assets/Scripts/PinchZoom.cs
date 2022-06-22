using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PinchZoom : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField]
    private float _zoomSpeedPinch = 0.001f;
    [SerializeField]
    private float _moveSpeed = 0.05f;
    [SerializeField]
    private float _zoomMin = 0.1f;
    [SerializeField]
    private float _zoomMax = 1f;
    private RectTransform _rectTransform;
    private Vector3 _originalPosition;
    [SerializeField]
    private ScrollRect _scrollRect;



    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _originalPosition = _rectTransform.localPosition;
    }

   // private void Update()
   // {
   //     Debug.Log(transform.localPosition.x);
   // }
    private void Zoom()
    {
        float scaleChange = 0f;

        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = touchDeltaMag - prevTouchDeltaMag;

            scaleChange = deltaMagnitudeDiff * _zoomSpeedPinch;

        }

  

        if (scaleChange != 0)
        {
            var scaleX = transform.localScale.x;
            scaleX += scaleChange;
            scaleX = Mathf.Clamp(scaleX, _zoomMin, _zoomMax);
            var size = _rectTransform.rect.size;
            size.Scale(_rectTransform.localScale);
            var parentRect = ((RectTransform)_rectTransform.parent);
            var parentSize = parentRect.rect.size;
            parentSize.Scale(parentRect.localScale);

            transform.localScale = new Vector3(scaleX, scaleX, transform.localScale.z);
        }
    }


    private void MoveWhileZoomed()
    {
        if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Moved && transform.localScale.x != 1)
        {
            Vector3 direction;
            direction = Input.touches[0].deltaPosition.normalized;
            transform.position += direction * _moveSpeed;
            Debug.Log($"direction = {direction}");
        }


        if (transform.localScale.x == 1) transform.localPosition = new Vector3(_originalPosition.x, transform.localPosition.y, transform.localPosition.z);

        // If not in bounds, return on bouund pos  NEED TESTING!!!!!!!!!
        //  else if (transform.localScale.x <= 2)  leftboundx=1091.5 rightboundx = -1108.5
        // else if (transform.localScale.x <= 3)  leftboundx=2291.5 rightboundx = -2108.5
        // else if (transform.localScale.x <= 4)  leftboundx=3491.5 rightboundx = -3308.5
        // else if (transform.localScale.x <= 5)  leftboundx=4691.5 rightboundx = -4508.5  localpos.x

        else if (transform.localScale.x <= 2)
        {
            if (transform.localPosition.x >= 400) transform.localPosition = new Vector3(400f, transform.localPosition.y, transform.localPosition.z);
            if (transform.localPosition.x <= -400) transform.localPosition = new Vector3(-400f, transform.localPosition.y, transform.localPosition.z);
        }

        else if (transform.localScale.x <= 3)
        {
            if (transform.localPosition.x >= 1400) transform.localPosition = new Vector3(1400f, transform.localPosition.y, transform.localPosition.z);
            if (transform.localPosition.x <= -1400) transform.localPosition = new Vector3(-1400f, transform.localPosition.y, transform.localPosition.z);
        }

        else if (transform.localScale.x <= 4)
        {
            if (transform.localPosition.x >= 2400) transform.localPosition = new Vector3(2400f, transform.localPosition.y, transform.localPosition.z);
            if (transform.localPosition.x <= -2400) transform.localPosition = new Vector3(-2400f, transform.localPosition.y, transform.localPosition.z);
        }

        else if (transform.localScale.x <= 5)
        {
            if (transform.localPosition.x >= 3400) transform.localPosition = new Vector3(3400f, transform.localPosition.y, transform.localPosition.z);
            if (transform.localPosition.x <= -3400) transform.localPosition = new Vector3(-3400f, transform.localPosition.y, transform.localPosition.z);
        }
    }



    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        Zoom();
        MoveWhileZoomed();
        if (Input.touchCount <= 1) _scrollRect.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        _scrollRect.OnEndDrag(eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        if (Input.touchCount <= 1) _scrollRect.OnBeginDrag(eventData);
    }

}