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
        DungeonTable.instance.AddEvent(this);
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
        if (Input.GetButtonUp("Fire1") && IsActive)
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
    private DungeonState state = DungeonState.Initializing;
    private static DungeonTable m_instance;
    private List<DungeonEvent> events = new List<DungeonEvent>();
    private List<DungeonEvent> deleteEvents = new List<DungeonEvent>();
    private List<DungeonEvent> addEvents = new List<DungeonEvent>();

    public bool isWaiting {
        get {
            return state == DungeonState.Waiting;
        }
    }

    public static DungeonTable instance {
        get { return m_instance; }
    }

    public void RemoveEvent(DungeonEvent dungeonEvent)
    {
        deleteEvents.Add(dungeonEvent);
    }

    public void AddEvent(DungeonEvent dungeonEvent)
    {
        addEvents.Add(dungeonEvent);
    }

    private void Awake()
    {
        m_instance = this;
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
        AddEvents();
    }

    private void AddEvents()
    {
        foreach (var dungeonEvent in addEvents)
        {
            events.Add(dungeonEvent);
        }
        addEvents.Clear();
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
