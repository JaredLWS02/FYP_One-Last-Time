using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelManager : MonoBehaviour
{
    public static ModelManager Current;

    void Awake()
    {
        if(!Current) Current=this;
    }

    // Getters ============================================================================
    
    public List<MeshRenderer> GetMeshRenderers(GameObject target)
    {
        List<MeshRenderer> renderers = new();

        renderers.AddRange(target.GetComponents<MeshRenderer>());
        renderers.AddRange(target.GetComponentsInChildren<MeshRenderer>());

        return renderers;
    }
    
    public List<SkinnedMeshRenderer> GetSkinnedMeshRenderers(GameObject target)
    {
        List<SkinnedMeshRenderer> renderers = new();

        renderers.AddRange(target.GetComponents<SkinnedMeshRenderer>());
        renderers.AddRange(target.GetComponentsInChildren<SkinnedMeshRenderer>());

        return renderers;
    }

    public List<Renderer> GetRenderers(GameObject target)
    {
        List<Renderer> renderers = new();

        renderers.AddRange(GetMeshRenderers(target));
        renderers.AddRange(GetSkinnedMeshRenderers(target));

        return renderers;
    }
    
    public List<MeshFilter> GetMeshFilters(GameObject target)
    {
        List<MeshFilter> meshFilters = new();

        meshFilters.AddRange(target.GetComponents<MeshFilter>());
        meshFilters.AddRange(target.GetComponentsInChildren<MeshFilter>());

        return meshFilters;
    }

    public List<Material> GetMaterials(GameObject target)
    {
        List<Material> materials = new();

        foreach(var renderer in GetRenderers(target))
        {
            materials.AddRange(renderer.materials);
        }
        return materials;
    }

    public List<Material> GetMaterials(GameObject target, Material target_material)
    {
        List<Material> materials = new();

        foreach(var material in GetMaterials(target))
        {
            if(material.shader != target_material.shader) continue;

            materials.Add(material);
        }
        return materials;
    }

    // Material Add/Remove/Change ============================================================================

    public void AddMaterial(GameObject target, Material materialToAdd)
    {
        RecordMaterials(target);

        foreach(var renderer in GetRenderers(target))
        {
            List<Material> copyMaterials = originalMaterials[renderer];

            copyMaterials.Add(materialToAdd);

            renderer.materials = copyMaterials.ToArray();
        }
    }

    public void RemoveMaterial(GameObject target, Material materialToRemove)
    {
        foreach(var renderer in GetRenderers(target))
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

    public void RemoveAllMaterials(GameObject target)
    {
        foreach(var renderer in GetRenderers(target))
        {
            renderer.materials = new Material[0];
        }
    }

    public void ChangeMaterials(GameObject target, Material newMaterial)
    {
        foreach(var renderer in GetRenderers(target))
        {
            renderer.material = newMaterial;
        }
    }

    Dictionary<Renderer, List<Material>> originalMaterials = new();

    public void RecordMaterials(GameObject target)
    {
        foreach(var renderer in GetRenderers(target))
        {
            List<Material> materials = new(renderer.sharedMaterials);

            if(!originalMaterials.ContainsKey(renderer))
            {
                originalMaterials.Add(renderer, materials);
            }
        }
    }

    public void RevertMaterials(GameObject target)
    {
        foreach(var renderer in GetRenderers(target))
        {
            if(originalMaterials.ContainsKey(renderer))
            {
                renderer.materials = originalMaterials[renderer].ToArray();

                originalMaterials.Remove(renderer); // cleanup
            }
        }
    }

    // Material Colours ============================================================================

    Dictionary<Material, Color> originalColors = new();
    Dictionary<Material, Color> originalEmissionColors = new();

    public void RecordColors(GameObject target)
    {
        foreach(var material in GetMaterials(target))
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

    public void OffsetColor(GameObject target, float rOffset, float gOffset, float bOffset)
    {
        RecordColors(target);

        Color colorOffset = new(rOffset, gOffset, bOffset);

        foreach(var material in GetMaterials(target))
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

    public void RevertColor(GameObject target)
    {
        List<Material> materialsToRevert = new(); // new list

        foreach(var material in GetMaterials(target))
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

                originalColors.Remove(material); // clean up
            }
            
            if(material.HasProperty("_EmissionColor") && originalEmissionColors.ContainsKey(material))
            {
                Color color = originalEmissionColors[material];

                material.SetColor("_EmissionColor", color);

                originalEmissionColors.Remove(material); //clean up
            }
        }
    }

    public void FlashColor(GameObject target, float rOffset=0, float gOffset=0, float bOffset=0, float time=.1f)
    {
        if(flashingColorRts.ContainsKey(target))
        {
            if(flashingColorRts[target]!=null) StopCoroutine(flashingColorRts[target]);
        }

        flashingColorRts[target] = StartCoroutine(FlashingColor(target, time, rOffset, gOffset, bOffset));
    }

    Dictionary<GameObject, Coroutine> flashingColorRts = new();

    IEnumerator FlashingColor(GameObject target, float t, float r, float g, float b)
    {
        OffsetColor(target, r, g, b);
        yield return new WaitForSeconds(t);
        if(target) RevertColor(target);
    }

    // Vertex Finder ============================================================================

    public List<Mesh> GetMeshes(GameObject target)
    {
        List<Mesh> meshes = new();

        foreach(var smr in GetSkinnedMeshRenderers(target))
        {
            meshes.Add(smr.sharedMesh);
        }
        foreach(var mf in GetMeshFilters(target))
        {
            meshes.Add(mf.sharedMesh);
        }
        return meshes;
    }

    public List<Vector3> GetVertices(GameObject target)
    {
        List<Vector3> vertices = new();

        foreach(Mesh mesh in GetMeshes(target))
        {
            vertices.AddRange(mesh.vertices);
        }
        return vertices;
    }

    public Vector3 GetTopVertex(GameObject target)
    {
        List<Vector3> vertices = GetVertices(target);

        if(vertices.Count>0)
        {
            Vector3 topMostVertex = target.transform.TransformPoint(vertices[0]);

            foreach(var vertex in vertices)
            {
                Vector3 worldPoint;

                foreach(var mf in GetMeshFilters(target))
                {
                    worldPoint = mf.transform.TransformPoint(vertex);

                    if(worldPoint.y > topMostVertex.y)
                    {
                        topMostVertex = worldPoint;
                    }
                }

                foreach(var smr in GetSkinnedMeshRenderers(target))
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
        Debug.LogError($"GetTopVertex: Can't find vertices on {target.name}");
        return target.transform.position;
    }

    // Mesh Bounding Box ============================================================================

    public Bounds GetCombinedBounds(GameObject target)
    {
        Bounds combinedBounds = new(Vector3.zero, Vector3.zero);

        foreach(var renderer in GetRenderers(target))
        {
            combinedBounds.Encapsulate(renderer.bounds);
        }

        return combinedBounds;
    }

    public Vector3 GetBoundingBoxSize(GameObject target)
    {
        Bounds combinedBounds = GetCombinedBounds(target);

        return combinedBounds.size;
    }

    public Vector3 GetBoundingBoxCenter(GameObject target)
    {
        Bounds combinedBounds = GetCombinedBounds(target);

        Vector3 worldCenter = target.transform.TransformPoint(combinedBounds.center);

        return worldCenter;
    }

    public Vector3 GetBoundingBoxTop(GameObject target)
    {
        float halfHeight = GetBoundingBoxSize(target).y * .5f;

        Vector3 center = GetBoundingBoxCenter(target);

        return new Vector3(target.transform.position.x, center.y+halfHeight, target.transform.position.z);
    }
    
}
