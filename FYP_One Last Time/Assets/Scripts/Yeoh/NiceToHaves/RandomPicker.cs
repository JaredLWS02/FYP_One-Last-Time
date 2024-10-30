using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPicker : MonoBehaviour
{
    [System.Serializable]
    public class Option
    {
        public string name;
        public float weight=1;
    }

    public List<Option> options = new();

    public string currentOption;

    // ============================================================================

    [Header("Randomize")]
    public Vector2 randomInterval = new(0.25f, 3);
    float currentInterval;

    void RandomizeInterval()
    {
        currentInterval = Random.Range(randomInterval.x, randomInterval.y);
    }

    // ============================================================================
    
    public bool enableTimerOnAwake=true;

    void OnEnable()
    {
        RandomizeInterval();

        if(enableTimerOnAwake)
        StartCoroutine(Randomizing());
    }

    IEnumerator Randomizing()
    {
        while(true)
        {
            RandomizeOption();

            currentInterval = Random.Range(randomInterval.x, randomInterval.y);

            yield return new WaitForSeconds(currentInterval);
        }
    }

    // ============================================================================

    float manualCheck=0;
    
    public void UpdateManualTimer()
    {
        if(enableTimerOnAwake)
        {
            Debug.LogWarning($"{gameObject.name}: Turn off enableTimerOnAwake if updating manual timer");
            return;
        }

        manualCheck += Time.deltaTime;

        if(manualCheck >= currentInterval)
        {
            manualCheck=0;

            RandomizeInterval();

            RandomizeOption();
        }
    }

    // ============================================================================

    [ContextMenu("Randomize")]
    void RandomizeOption()
    {
        if(options.Count<=0) return;

        if(options.Count<=1)
        {
            currentOption = options[0].name;
            return;
        }

        float total_weight=0;

        foreach(var option in options)
        {
            total_weight += option.weight;
        }

        float random_value = Random.Range(0f, total_weight);
        
        float cumulative_weight=0;

        foreach(var option in options)
        {
            cumulative_weight += option.weight;

            if(random_value <= cumulative_weight)
            {
                currentOption = option.name; // Set the current option
                return; // Exit once the option is found
            }
        }
    }
}
