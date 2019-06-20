using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum DungeonState
{
    Waiting, Initializing, Busy
}

[DisallowMultipleComponent]
public class DungeonTable : MonoBehaviour
{
    private bool turn;
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
        RemoveEvents();
        if (events.Count > 0)
        {
            state = DungeonState.Busy;
            events[0].Update();
        } else
        {
            state = DungeonState.Waiting;
        }
        AddEvents();

        var entities = GameObject.FindObjectsOfType<EntityHolder>();
        for (int i = 0; i < entities.Length; i++)
        {
            if (entities[i].isDeath()) { SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                Debug.Log("YAAAA");
            }

        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
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
