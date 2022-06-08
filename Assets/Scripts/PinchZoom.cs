using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PinchZoom : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [SerializeField]
    private float zoomSpeedPinch = 0.001f;
    [SerializeField]
    private float zoomSpeedMouseScrollWheel = 0.05f;
    [SerializeField]
    private float zoomMin = 0.1f;
    [SerializeField]
    private float zoomMax = 1f;
    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private ScrollRect scrollRect;

    void Zoom()
    {
        float scaleChange = 0f;


        Vector2 direction;
        //float speed;

        if (Input.touches[0].phase == TouchPhase.Moved)//Check if Touch has moved.
        {
            direction = Input.touches[0].deltaPosition.normalized;  //Unit Vector of change in position
            //speed = Input.touches[0].deltaPosition.magnitude / Input.touches[0].deltaTime; //distance traveled divided by time elapsed
            //transform.position += i
            Debug.Log($"direction = {direction}");
        }


        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = touchDeltaMag - prevTouchDeltaMag;

            scaleChange = deltaMagnitudeDiff * zoomSpeedPinch;

        }

  

        if (scaleChange != 0)
        {
            var scaleX = transform.localScale.x;
            scaleX += scaleChange;
            scaleX = Mathf.Clamp(scaleX, zoomMin, zoomMax);
            var size = rectTransform.rect.size;
            size.Scale(rectTransform.localScale);
            var parentRect = ((RectTransform)rectTransform.parent);
            var parentSize = parentRect.rect.size;
            parentSize.Scale(parentRect.localScale);

            transform.localScale = new Vector3(scaleX, scaleX, transform.localScale.z);
        }
    }


    // please note that scrollRect is the component on the scroll view game object, not where this script is

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        Zoom();
        if (Input.touchCount <= 1) scrollRect.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");

        

        scrollRect.OnEndDrag(eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        if (Input.touchCount <= 1) scrollRect.OnBeginDrag(eventData);
    }

}