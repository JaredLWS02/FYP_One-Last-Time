using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelManager : MonoBehaviour
{
    public GameObject owner;
    public List<Renderer> renderers = new();
    public List<MeshFilter> meshFilters = new();
    public List<Material> materials = new();
    public List<Mesh> meshes = new();
    public List<Vector3> vertices = new();
    Bounds combinedBounds;

    // ============================================================================
    
    [ContextMenu("Get Components")]
    public void GetComponents()
    {
        if(!owner) return;

        renderers.Clear();

        renderers.AddRange(owner.GetComponents<MeshRenderer>());
        renderers.AddRange(owner.GetComponentsInChildren<MeshRenderer>());

        renderers.AddRange(owner.GetComponents<SkinnedMeshRenderer>());
        renderers.AddRange(owner.GetComponentsInChildren<SkinnedMeshRenderer>());

        meshFilters.AddRange(owner.GetComponents<MeshFilter>());
        meshFilters.AddRange(owner.GetComponentsInChildren<MeshFilter>());

        foreach(var renderer in renderers)
        {
            materials.AddRange(renderer.materials);
        }

        foreach(SkinnedMeshRenderer renderer in renderers)
        {
            meshes.Add(renderer.sharedMesh);
        }
        foreach(var meshFilter in meshFilters)
        {
            meshes.Add(meshFilter.sharedMesh);
        }

        foreach(var mesh in meshes)
        {
            vertices.AddRange(mesh.vertices);
        }

        foreach(var renderer in renderers)
        {
            combinedBounds.Encapsulate(renderer.bounds);
        }
    }

    // ============================================================================
    
    void Awake()
    {
        RecordMaterials();
        RecordColors();
    }

    Dictionary<Renderer, List<Material>> originalMaterials = new();

    public void RecordMaterials()
    {
        foreach(var renderer in renderers)
        {
            List<Material> materials = new(renderer.sharedMaterials);

            if(!originalMaterials.ContainsKey(renderer))
            {
                originalMaterials.Add(renderer, materials);
            }
        }
    }

    // ============================================================================
    
    public List<Material> GetMaterials(Material target_material)
    {
        List<Material> materials = new();

        foreach(var material in materials)
        {
            if(material.shader != target_material.shader) continue;

            materials.Add(material);
        }
        return materials;
    }

    // ============================================================================
    
    public void AddMaterial(Material materialToAdd)
    {
        foreach(var renderer in renderers)
        {
            List<Material> materials_copy = originalMaterials[renderer];

            materials_copy.Add(materialToAdd);

            renderer.materials = materials_copy.ToArray();
        }
    }

    // ============================================================================

    public void RemoveMaterial(Material materialToRemove)
    {
        foreach(var renderer in renderers)
        {
            List<Material> materials = new(renderer.sharedMaterials);

            for(int i=0; i<materials.Count; i++)
            {
                if(materials[i].shader == materialToRemove.shader)
                {
                    materials.RemoveAt(i);
                    i--;
                }
            }
            renderer.materials = materials.ToArray();
        }
    }

    public void RemoveAllMaterials()
    {
        foreach(var renderer in renderers)
        {
            renderer.materials = new Material[0];
        }
    }

    // ============================================================================

    public void ChangeMaterials(Material newMaterial)
    {
        foreach(var renderer in renderers)
        {
            renderer.material = newMaterial;
        }
    }

    // ============================================================================

    public void RevertMaterials()
    {
        foreach(var renderer in renderers)
        {
            if(originalMaterials.ContainsKey(renderer))
            {
                renderer.materials = originalMaterials[renderer].ToArray();
            }
        }
    }

    // ============================================================================

    Dictionary<Material, Color> originalColors = new();
    Dictionary<Material, Color> originalEmissionColors = new();

    public void RecordColors()
    {
        foreach(var material in materials)
        {
            if(material.HasProperty("_Color") && !originalColors.ContainsKey(material))
            {
                //originalColors[material] = material.color;
                originalColors[material] = material.GetColor("_Color");
            }

            if(material.HasProperty("_EmissionColor") && !originalEmissionColors.ContainsKey(material))
            {
                originalEmissionColors[material] = material.GetColor("_EmissionColor");
            }
        }
    }

    // ============================================================================
    
    [Header("Offset")]
    public Vector3 rgbOffset = new(.75f, -.75f, -.75f); // red

    public void OffsetColors(Vector3 rgb_offset)
    {
        RevertColors();

        Color colorOffset = new(rgb_offset.x, rgb_offset.y, rgb_offset.z);

        foreach(var material in materials)
        {
            //material.color += colorOffset;
            if(material.HasProperty("_Color"))
            {
                Color color = material.GetColor("_Color");
                color += colorOffset;
                material.SetColor("_Color", color);
            }

            if(material.HasProperty("_EmissionColor"))
            {
                Color color = material.GetColor("_EmissionColor");
                color += colorOffset;
                material.SetColor("_EmissionColor", color);
            }
        }
    }

    public void OffsetColors() => OffsetColors(rgbOffset);

    // ============================================================================

    public void RevertColors()
    {
        List<Material> materialsToRevert = new(); // new list

        foreach(var material in materials)
        {
            if(originalColors.ContainsKey(material) || originalEmissionColors.ContainsKey(material))
            {
                materialsToRevert.Add(material); // Add materials to the list for reverting
            }
        }

        foreach(var material in materialsToRevert)
        {
            if(material.HasProperty("_Color") && originalColors.ContainsKey(material))
            {
                //material.color = originalColors[material];
                Color color = originalColors[material];

                material.SetColor("_Color", color);
            }
            
            if(material.HasProperty("_EmissionColor") && originalEmissionColors.ContainsKey(material))
            {
                Color color = originalEmissionColors[material];

                material.SetColor("_EmissionColor", color);
            }
        }
    }

    // ============================================================================

    public void FlashColors(Vector3 rgb_offset, float seconds)
    {
        if(flashing_crt!=null) StopCoroutine(flashing_crt);
        flashing_crt = StartCoroutine(FlashingColors(rgb_offset, seconds));
    }

    public void FlashColors(float seconds) => FlashColors(rgbOffset, seconds);

    Coroutine flashing_crt;

    IEnumerator FlashingColors(Vector3 rgb_offset, float seconds)
    {
        OffsetColors(rgb_offset);
        yield return new WaitForSeconds(seconds);
        RevertColors();
    }

    // ============================================================================

    [Header("Flicker")]
    public float colorFlickerInterval=.05f;

    public void ToggleColorFlicker(bool toggle, float interval)
    {
        if(colorFlickering_crt!=null) StopCoroutine(colorFlickering_crt);

        if(toggle) colorFlickering_crt = StartCoroutine(ColorFlickering(interval));
        else RevertColors();
    }

    public void ToggleColorFlicker(bool toggle) => ToggleColorFlicker(toggle, colorFlickerInterval);

    Coroutine colorFlickering_crt;

    IEnumerator ColorFlickering(float interval)
    {
        while(true)
        {
            OffsetColors();
            yield return new WaitForSeconds(interval);
            RevertColors();
            yield return new WaitForSeconds(interval);
        }
    }

    // ============================================================================

    [Header("Random Offset")]
    public bool randomStartColor;
    public Vector3 randomRgbOffset = new(.15f, .15f, .15f);

    public void RandomOffsetColor(Vector3 rgb_offset)
    {
        rgb_offset = new(
            Random.Range(-rgb_offset.x, rgb_offset.x),
            Random.Range(-rgb_offset.y, rgb_offset.y),
            Random.Range(-rgb_offset.z, rgb_offset.z)
        );

        OffsetColors(rgb_offset);
    }

    public void RandomOffsetColor() => RandomOffsetColor(randomRgbOffset);

    // ============================================================================

    public Vector3 GetTopVertex()
    {
        if(vertices.Count>0)
        {
            Vector3 topMostVertex = owner.transform.TransformPoint(vertices[0]);

            foreach(var vertex in vertices)
            {
                Vector3 worldPoint;

                foreach(var mf in meshFilters)
                {
                    worldPoint = mf.transform.TransformPoint(vertex);

                    if(worldPoint.y > topMostVertex.y)
                    {
                        topMostVertex = worldPoint;
                    }
                }

                foreach(SkinnedMeshRenderer smr in renderers)
                {
                    worldPoint = smr.transform.TransformPoint(vertex);

                    if(worldPoint.y > topMostVertex.y)
                    {
                        topMostVertex = worldPoint;
                    }
                }
            }
            return topMostVertex;
        }
        Debug.LogError($"GetTopVertex: Can't find vertices on {owner.name}");
        return owner.transform.position;
    }

    // ============================================================================

    public Vector3 GetBoundsSize() => combinedBounds.size;

    public Vector3 GetBoundsCenter() => owner.transform.TransformPoint(combinedBounds.center);

    public Vector3 GetBoundsTop()
    {
        float halfHeight = GetBoundsSize().y * .5f;

        Vector3 center = GetBoundsCenter();

        return new(
            owner.transform.position.x,
            center.y + halfHeight,
            owner.transform.position.z
        );
    }
    
}
