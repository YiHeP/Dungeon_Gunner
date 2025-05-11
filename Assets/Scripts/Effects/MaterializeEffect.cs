using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterializeEffect : MonoBehaviour
{
    public IEnumerator MaterializeRoutine(Shader materializeShader, Color materializeColor,float materializeTime, SpriteRenderer[] 
        spriteRendererArray, Material normalMaterial)
    {
        Material materializeMaterial = new Material(materializeShader);

        materializeMaterial.SetColor("_EmissionColor",materializeColor);

        foreach(SpriteRenderer spriteRenderer in spriteRendererArray)
        {
            spriteRenderer.material = materializeMaterial;
        }

        float dissolveAmout = 0f;

        while(dissolveAmout < 1f)
        {
            dissolveAmout += Time.deltaTime / materializeTime;

            materializeMaterial.SetFloat("_DissolveAmout",dissolveAmout);

            yield return null;
        }

        foreach(SpriteRenderer spriteRenderer in spriteRendererArray)
        {
            spriteRenderer.material = normalMaterial;
        }
    }
}
