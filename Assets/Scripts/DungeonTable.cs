using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DungeonEvent
{
    private bool active = false;
    public bool IsActive {
        get { return active; }
    }

    public virtual void Update()
    {
    }
    
    public void ActivateEvent()
    {
        active = true;
        DungeonTable.instance.events.Add(this);
    }

    public void RemoveEvent()
    {
        active = false;
        DungeonTable.instance.RemoveEvent(this);
    }
}

public class SelectEntity : DungeonEvent
{
    public UnityEngine.Events.UnityAction<EntityHolder> OnClick;
    public EntityHolder holder;

    public override void Update()
    {
        base.Update();
        if (Input.GetButtonDown("Fire1") && IsActive)
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hits = Physics.RaycastAll(mouseRay);

            foreach (var hit in hits)
            {
                if (hit.transform.gameObject.GetComponentInParent<EntityHolder>())
                {
                    holder = hit.transform.gameObject.GetComponentInParent<EntityHolder>();
                    OnClick.Invoke(holder);
                    RemoveEvent();
                    break;
                }
            }

            OnClick.Invoke(null);
            RemoveEvent();
        }
    }
}

public enum DungeonState
{
    Waiting, Initializing, Busy
}

[DisallowMultipleComponent]
public class DungeonTable : MonoBehaviour
{
    public DungeonState state = DungeonState.Initializing;
    public List<DungeonEvent> events = new List<DungeonEvent>();
    public static DungeonTable instance;
    public List<DungeonEvent> deleteEvents = new List<DungeonEvent>();

    public void RemoveEvent(DungeonEvent dungeonEvent)
    {
        deleteEvents.Add(dungeonEvent);
    }

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (events.Count > 0)
        {
            state = DungeonState.Busy;
        } else
        {
            state = DungeonState.Waiting;
        }
        RemoveEvents();
        foreach (var dungeonEvent in events)
        {
            dungeonEvent.Update();
        }
    }

    private void RemoveEvents()
    {
        foreach (var dungeonEvent in deleteEvents)
        {
            events.Remove(dungeonEvent);
        }
        deleteEvents.Clear();
    }
}
