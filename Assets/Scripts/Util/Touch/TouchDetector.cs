using System.Collections.Generic;
using UnityEngine;
using Util.Touch.Impl;

namespace Util.Touch
{
    public class TouchDetector : MonoBehaviour
    {
        private Camera mainCamera;
        private bool touchSupported;

        private readonly List<ITouchListener> listeners = new List<ITouchListener>();
        private void Awake()
        {
            touchSupported = Input.touchSupported;
            mainCamera = Camera.main;
        }

        private void Update()
        {
            int countTouch = 1;
            if (touchSupported)
            {
                countTouch = Input.touchCount;
            }
            
            for (int i = 0; i < countTouch; ++i)
            {
                var posTouch = touchSupported ? Input.GetTouch(i).position : (Vector2)Input.mousePosition;

                var hit = Physics2D.OverlapPoint(mainCamera.ScreenToWorldPoint(posTouch));
                if (hit != null)
                {
                    var touchListener = hit.transform.GetComponent<ITouchListener>();
                    
                    if (touchSupported && touchListener != null)
                    {
                        if (Input.GetTouch(i).phase == TouchPhase.Began)
                        {
                            if (!listeners.Contains(touchListener))
                            {
                                listeners.Add(touchListener);
                            }
                            touchListener.OnTouchDown(i);
                        }
                    }
                    else if(touchListener != null)
                    {
                        if (Input.GetMouseButtonDown(i))
                        {
                            if (!listeners.Contains(touchListener))
                            {
                                listeners.Add(touchListener);
                            }
                            touchListener.OnTouchDown(i);
                        }
                    }
                }

                if (touchSupported)
                {
                    HandledTouches(Input.GetTouch(i).phase == TouchPhase.Ended,Input.GetTouch(i).phase == TouchPhase.Moved,i);
                }
                else
                {
                    HandledTouches(Input.GetMouseButtonUp(i),Input.GetMouseButton(i),i);
                }

            }
            
            print($"count listeners {listeners.Count}");
        }

        private void HandledTouches(bool isEnd,bool isMove,int index)
        {
            if (isEnd)
            {
                if (index < listeners.Count)
                {
                    var touchListener = listeners[index];
                    if (touchListener != null)
                    {
                        touchListener.OnTouchUp(index);
                    }
                    listeners.RemoveAt(index);
                }
                        
            }
            if (isMove)
            {
                if (index < listeners.Count)
                {
                    var touchListener = listeners[index];
                    if (touchListener != null)
                    {
                        touchListener.OnTouchDrag(index);
                    }
                    else
                    {
                        listeners.RemoveAt(index);
                    }
                }
            }
        }
    }
}