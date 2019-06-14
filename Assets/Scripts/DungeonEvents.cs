using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DungeonEvent
{
    public virtual void Update()
    {
    }

    public void ActivateEvent()
    {
        DungeonTable.instance.AddEvent(this);
    }

    public void RemoveEvent()
    {
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
        if (Input.GetButtonUp("Fire1"))
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
