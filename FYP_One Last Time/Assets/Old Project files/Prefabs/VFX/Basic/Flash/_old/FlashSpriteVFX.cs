// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class FlashSpriteVFX : MonoBehaviour
// {
//     public GameObject flashPrefab;

//     public void SpawnFlash(Vector3 pos, Color color, float scaleMult=.5f)
//     {
//         SpriteRenderer sr = Instantiate(flashPrefab, pos, Quaternion.identity).GetComponent<SpriteRenderer>();
//         sr.gameObject.hideFlags = HideFlags.HideInHierarchy;

//         sr.color = color;

//         sr.transform.localScale *= scaleMult;
//     }
// }
