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

    // GETTERS
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    public List<MeshRenderer> GetMeshRenderers(GameObject target)
    {
        List<MeshRenderer> renderers = new List<MeshRenderer>();

        renderers.AddRange(target.GetComponents<MeshRenderer>());
        renderers.AddRange(target.GetComponentsInChildren<MeshRenderer>());

        return renderers;
    }
    
    public List<SkinnedMeshRenderer> GetSkinnedMeshRenderers(GameObject target)
    {
        List<SkinnedMeshRenderer> renderers = new List<SkinnedMeshRenderer>();

        renderers.AddRange(target.GetComponents<SkinnedMeshRenderer>());
        renderers.AddRange(target.GetComponentsInChildren<SkinnedMeshRenderer>());

        return renderers;
    }

    public List<Renderer> GetRenderers(GameObject target)
    {
        List<Renderer> renderers = new List<Renderer>();

        renderers.AddRange(GetMeshRenderers(target));
        renderers.AddRange(GetSkinnedMeshRenderers(target));

        return renderers;
    }
    
    public List<MeshFilter> GetMeshFilters(GameObject target)
    {
        List<MeshFilter> meshFilters = new List<MeshFilter>();

        meshFilters.AddRange(target.GetComponents<MeshFilter>());
        meshFilters.AddRange(target.GetComponentsInChildren<MeshFilter>());

        return meshFilters;
    }

    public List<Material> GetMaterials(GameObject target, Material materialToGet=null)
    {
        List<Material> materials = new List<Material>();

        foreach(Renderer renderer in GetRenderers(target))
        {
            foreach(Material material in renderer.materials)
            {
                if(materialToGet)
                {
                    if(material.shader == materialToGet.shader)
                    {
                        materials.Add(material);
                    }
                }
                else
                {
                    materials.Add(material);
                }
            }
        }

        return materials;
    }

    // MATERIAL ADD/REMOVE/CHANGE
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void AddMaterial(GameObject target, Material materialToAdd)
    {
        RecordMaterials(target);

        foreach(Renderer renderer in GetRenderers(target))
        {
            List<Material> copyMaterials = originalMaterials[renderer];

            copyMaterials.Add(materialToAdd);

            renderer.materials = copyMaterials.ToArray();
        }
    }

    public void RemoveMaterial(GameObject target, Material materialToRemove)
    {
        foreach(Renderer renderer in GetRenderers(target))
        {
            List<Material> materials = new List<Material>(renderer.sharedMaterials);

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
        foreach(Renderer renderer in GetRenderers(target))
        {
            renderer.materials = new Material[0];
        }
    }

    public void ChangeMaterials(GameObject target, Material newMaterial)
    {
        foreach(Renderer renderer in GetRenderers(target))
        {
            renderer.material = newMaterial;
        }
    }

    Dictionary<Renderer, List<Material>> originalMaterials = new Dictionary<Renderer, List<Material>>();

    public void RecordMaterials(GameObject target)
    {
        foreach(Renderer renderer in GetRenderers(target))
        {
            List<Material> materials = new List<Material>(renderer.sharedMaterials);

            if(!originalMaterials.ContainsKey(renderer))
            {
                originalMaterials.Add(renderer, materials);
            }
        }
    }

    public void RevertMaterials(GameObject target)
    {
        foreach(Renderer renderer in GetRenderers(target))
        {
            if(originalMaterials.ContainsKey(renderer))
            {
                renderer.materials = originalMaterials[renderer].ToArray();

                originalMaterials.Remove(renderer); // cleanup
            }
        }
    }

    // OFFSET MESH COLORS
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public List<Color> GetColors(GameObject target)
    {
        List<Color> colors = new List<Color>();

        foreach(Material material in GetMaterials(target))
        {
            colors.Add(material.color);
        }

        return colors;
    }

    public List<Color> GetEmissionColors(GameObject target)
    {
        List<Color> emissionColors = new List<Color>();

        foreach(Material material in GetMaterials(target))
        {
            emissionColors.Add(material.GetColor("_EmissionColor"));
        }

        return emissionColors;
    }

    Dictionary<Material, Color> originalColors = new();
    Dictionary<Material, Color> originalEmissionColors = new();

    public void RecordColors(GameObject target)
    {
        foreach(Material material in GetMaterials(target))
        {
            if(material.HasProperty("_Color") && !originalColors.ContainsKey(material))
            {
                originalColors[material] = material.color;
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

        Color colorOffset = new Color(rOffset, gOffset, bOffset);

        foreach(Material material in GetMaterials(target))
        {
            if(material.HasProperty("_Color"))
            {
                material.color += colorOffset;
            }

            if(material.HasProperty("_EmissionColor"))
            {
                material.SetColor("_EmissionColor", material.GetColor("_EmissionColor") + colorOffset);
            }
        }
    }

    public void RevertColor(GameObject target)
    {
        List<Material> materialsToRevert = new List<Material>(); // new list

        foreach(Material material in GetMaterials(target))
        {
            if(originalColors.ContainsKey(material) || originalEmissionColors.ContainsKey(material))
            {
                materialsToRevert.Add(material); // Add materials to the list for reverting
            }
        }

        foreach(Material material in materialsToRevert)
        {
            if(material.HasProperty("_Color") && originalColors.ContainsKey(material))
            {
                material.color = originalColors[material];

                originalColors.Remove(material); // clean up
            }
            
            if(material.HasProperty("_EmissionColor") && originalEmissionColors.ContainsKey(material))
            {
                material.SetColor("_EmissionColor", originalEmissionColors[material]);

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

    // TOP VERTEX FINDER
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public List<Mesh> GetMeshes(GameObject target)
    {
        List<Mesh> meshes = new List<Mesh>();

        foreach(SkinnedMeshRenderer smr in GetSkinnedMeshRenderers(target))
        {
            meshes.Add(smr.sharedMesh);
        }
        foreach(MeshFilter mf in GetMeshFilters(target))
        {
            meshes.Add(mf.sharedMesh);
        }
        
        return meshes;
    }

    public List<Vector3> GetVertices(GameObject target)
    {
        List<Vector3> vertices = new List<Vector3>();

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

            foreach(Vector3 vertex in vertices)
            {
                Vector3 worldPoint;

                foreach(MeshFilter mf in GetMeshFilters(target))
                {
                    worldPoint = mf.transform.TransformPoint(vertex);

                    if(worldPoint.y > topMostVertex.y)
                    {
                        topMostVertex = worldPoint;
                    }
                }

                foreach(SkinnedMeshRenderer smr in GetSkinnedMeshRenderers(target))
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

    // BOUNDING BOX
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public Bounds GetCombinedBounds(GameObject target)
    {
        Bounds combinedBounds = new Bounds(Vector3.zero, Vector3.zero);

        foreach(Renderer renderer in GetRenderers(target))
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

    // COLLIDER BOUNDING BOX
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public List<Collider> GetColliders(GameObject target)
    {
        List<Collider> colliders = new List<Collider>();

        Collider[] colls = target.GetComponents<Collider>();
        Collider[] childColls = target.GetComponentsInChildren<Collider>();

        foreach(Collider coll in colls)
        {
            if(!coll.isTrigger) colliders.Add(coll);
        }
        foreach(Collider coll in childColls)
        {
            if(!coll.isTrigger) colliders.Add(coll);
        }

        return colliders;
    }

    public Vector3 GetColliderTop(GameObject target)
    {
        List<Collider> colliders = GetColliders(target);
        
        if(colliders.Count==0)
        {
            Debug.LogError($"{name}: Couldn't find any Collider on {target.name}");
            return Vector3.zero;
        }

        float highestPoint = float.MinValue;

        foreach(Collider coll in colliders)
        {
            Vector3 topPoint = coll.bounds.max;

            if(highestPoint < topPoint.y) highestPoint = topPoint.y;
        }

        return new Vector3(target.transform.position.x, highestPoint, target.transform.position.z);
    }

    public Vector3 GetColliderCenter(GameObject target)
    {
        List<Collider> colliders = GetColliders(target);

        Vector3 center = Vector3.zero;

        // Calculate the average position of all colliders' centers
        foreach(Collider col in colliders)
        {
            center += col.bounds.center;
        }

        center /= colliders.Count;

        return center;
    }
}
