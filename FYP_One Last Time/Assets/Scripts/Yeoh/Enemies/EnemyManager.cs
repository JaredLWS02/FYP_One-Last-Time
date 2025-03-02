using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Current;

    void Awake()
    {
        Current=this;
    }

    // ============================================================================

    public List<GameObject> enemies = new();

    public void RegisterEnemy(GameObject who)
    {
        if(!enemies.Contains(who))
        enemies.Add(who);
    }

    public void UnregisterEnemy(GameObject who)
    {
        if(enemies.Contains(who))
        enemies.Remove(who);
    }

    // ============================================================================
    
    [System.Serializable]
    public class EnemyCombatSlot
    {
        public int slotNum = 0;
        public List<GameObject> enemies;
        public EnemyCombatTypeEvents events;

        // ============================================================================

        public void TryAdd(GameObject who)
        {
            if(!enemies.Contains(who))
                enemies.Add(who);

            UpdateCount();
        }

        public void TryRemove(GameObject who)
        {
            if(enemies.Contains(who))
                enemies.Remove(who);
            
            UpdateCount();
        }
        
        public void RemoveNulls()
        {
            enemies.RemoveAll(item => item == null);
            UpdateCount();
        }

        // ============================================================================

        int lastCount = 0;

        void UpdateCount()
        {
            int count = enemies.Count;

            if(!IsEmpty() && WasEmpty()) TryFirst();
            else if(IsEmpty() && !WasEmpty()) TryLast();

            lastCount = count;
        }

        public bool IsEmpty() => enemies.Count <= 0;
        public bool WasEmpty() => lastCount <= 0;

        // ============================================================================

        void TryFirst()
        {
            var slot = EnemyManager.Current.GetHighestSlotWithEnemies();
            if(slot != this) return;

            events.OnEnemyFirstEnterCombat?.Invoke();
        }

        void TryLast()
        {
            var slot = EnemyManager.Current.GetHighestSlotWithEnemies(true);
            if(slot != this) return;

            events.OnEnemyLastExitCombat?.Invoke();

            EnemyManager.Current.NextSlotTakeover();
        }
    }

    // ============================================================================

    [System.Serializable]
    public struct EnemyCombatTypeEvents
    {
        public UnityEvent OnEnemyFirstEnterCombat;
        public UnityEvent OnEnemyLastExitCombat;
    }

    // ============================================================================

    public List<EnemyCombatSlot> enemyCombatSlots = new();

    EnemyCombatSlot GetEnemyCombatSlot(int slot_num)
    {
        foreach(var slot in enemyCombatSlots)
        {
            if(slot.slotNum == slot_num)
            {
                return slot;
            }
        }
        return null;
    }

    EnemyCombatSlot GetEnemyCombatSlot(GameObject who)
    {
        foreach(var slot in enemyCombatSlots)
        {
            foreach(var enemy in slot.enemies)
            {
                if(enemy == who)
                {
                    return slot;
                }
            }            
        }
        return null;
    }

    // ============================================================================

    public void RegisterEnemyCombat(GameObject who, int slot_num)
    {
        var slot = GetEnemyCombatSlot(slot_num);
        if(slot==null) { Debug.LogError($"No such Enemy Combat Slot: {slot_num}"); return; }

        slot.TryAdd(who);
    }

    public void UnregisterEnemyCombat(GameObject who)
    {
        var slot = GetEnemyCombatSlot(who);
        if(slot==null) return;

        slot.TryRemove(who);
    }

    // ============================================================================

    EnemyCombatSlot GetHighestSlotWithEnemies(bool check_last=false)
    {
        EnemyCombatSlot highest_slot = null;
        int highest_slot_num = int.MinValue;

        foreach(var slot in enemyCombatSlots)
        {
            bool is_empty = check_last ? slot.WasEmpty() : slot.IsEmpty();

            if(!is_empty && slot.slotNum > highest_slot_num)
            {
                highest_slot = slot;
                highest_slot_num = slot.slotNum;
            }
        }
        return highest_slot;
    }

    // ============================================================================

    public void NextSlotTakeover()
    {
        var slot = GetHighestSlotWithEnemies();
        if(slot==null) return;
        slot.events.OnEnemyFirstEnterCombat?.Invoke();
    }

    // ============================================================================

    void LateUpdate()
    {
        RemoveNulls(enemies);

        foreach(var slot in enemyCombatSlots)
        {
            slot.RemoveNulls();
        }
    }

    void RemoveNulls(List<GameObject> list)
    {
        list.RemoveAll(item => item == null);
    }
}
