using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        enemiesInCombat.Add(who);
    }

    public void UnregisterEnemyCombat(GameObject who)
    {
        if(enemiesInCombat.Contains(who))
        enemiesInCombat.Remove(who);
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

}
