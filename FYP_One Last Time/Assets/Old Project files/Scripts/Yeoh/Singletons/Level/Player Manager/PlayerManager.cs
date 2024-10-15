using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Current;

    void Awake()
    {
        Current=this;
    }

    // ============================================================================

    public List<GameObject> characters = new();

    public void Register(GameObject obj)
    {
        if(!characters.Contains(obj))
        characters.Add(obj);
    }

    public void Unregister(GameObject obj)
    {
        if(characters.Contains(obj))
        characters.Remove(obj);
    }

    // ============================================================================

    [HideInInspector]
    public GameObject player;

    void Update()
    {
        if(characters.Count>0)
        {
            player = characters[0];
        }
        else player = null;
    }

    // ============================================================================
    
    bool canSwitch=true;
    public float switchCooldown=.5f;

    public void TrySwitch(GameObject switcher)
    {
        //if(MultiplayerManager.Current.players!=1) return;
        if(characters.Count<=1) return;

        if(!canSwitch) return;
        StartCoroutine(SwitchCoolingDown());

        int from_index = characters.IndexOf(switcher);
        if(from_index<0) return;

        int to_index = from_index+1;

        if(to_index >= characters.Count)
        {
            to_index=0;
        }

        if(to_index == from_index) return;

        EventManager.Current.OnSwitchPilot(switcher, Pilot.Type.AI);
        EventManager.Current.OnSwitchPilot(characters[to_index], Pilot.Type.Player);

        Debug.Log($"Switched Player from {switcher.name} to {characters[to_index].name}");
    }

    IEnumerator SwitchCoolingDown()
    {
        canSwitch=false;
        yield return new WaitForSeconds(switchCooldown);
        canSwitch=true;
    }
}
