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
    
    public List<GameObject> enemiesInCombat = new();

    public void RegisterEnemyCombat(GameObject who)
    {
        if(!enemiesInCombat.Contains(who))
        {
            if(enemiesInCombat.Count<=0)
            enemyMEvents.OnEnemyFirstEnterCombat?.Invoke();

            enemiesInCombat.Add(who);
        }
    }

    public void UnregisterEnemyCombat(GameObject who)
    {
        if(enemiesInCombat.Contains(who))
        {
            enemiesInCombat.Remove(who);

            if(enemiesInCombat.Count<=0)
            enemyMEvents.OnEnemyLastExitCombat?.Invoke();
        }
    }

    // ============================================================================

    void LateUpdate()
    {
        RemoveNulls(enemies);
        RemoveNulls(enemiesInCombat);
    }

    void RemoveNulls(List<GameObject> list)
    {
        list.RemoveAll(item => item == null);
    }

    // ============================================================================

    [System.Serializable]
    public struct EnemyMEvents
    {
        public UnityEvent OnEnemyFirstEnterCombat;
        public UnityEvent OnEnemyLastExitCombat;
    }
    [Space]
    public EnemyMEvents enemyMEvents;
}
