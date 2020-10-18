using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class MouseEnterUIEvent : MonoBehaviour, IPointerEnterHandler
{
    public UnityEvent Event;

    public void OnPointerEnter(PointerEventData ped)
    {
        Event.Invoke();
    }
}
