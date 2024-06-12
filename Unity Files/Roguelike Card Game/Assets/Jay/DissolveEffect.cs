using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class DissolveEffect : MonoBehaviour
{
    [Range(0, 1)]
    public float dissolveAmount = 0f; // Controls the dissolve effect (0: no dissolve, 1: fully dissolved)
    public float dissolveSpeed = 0.5f; // Speed of the dissolve effect
    public Texture2D dissolveTexture; // Texture to control the dissolve pattern
    public Color edgeColor = Color.white; // Color of the edge during dissolve

    private Renderer[] renderers;
    private List<MaterialPropertyBlock> propertyBlocks;
    private static readonly int DissolveAmountID = Shader.PropertyToID("_DissolveAmount");
    private static readonly int DissolveTextureID = Shader.PropertyToID("_DissolveTex");
    private static readonly int EdgeColorID = Shader.PropertyToID("_EdgeColor");
    private static readonly int ObjectYPosID = Shader.PropertyToID("_ObjectYPos");
    private static readonly int DissolveHeightID = Shader.PropertyToID("_DissolveHeight");

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        propertyBlocks = new List<MaterialPropertyBlock>();

        foreach (var renderer in renderers)
        {
            var propertyBlock = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(propertyBlock);
            propertyBlocks.Add(propertyBlock);
        }

        if (dissolveTexture == null)
        {
            Debug.LogWarning("Dissolve texture not set. Please assign a dissolve texture.");
        }
    }

    void Update()
    {
        // // Update the dissolve amount over time
        // dissolveAmount = Mathf.Clamp01(dissolveAmount + Time.deltaTime * dissolveSpeed);
        //
        // foreach (var renderer in renderers)
        // {
        //     var propertyBlock = propertyBlocks[renderers.IndexOf(renderer)];
        //
        //     propertyBlock.SetFloat(DissolveAmountID, dissolveAmount);
        //     propertyBlock.SetTexture(DissolveTextureID, dissolveTexture);
        //     propertyBlock.SetColor(EdgeColorID, edgeColor);
        //     propertyBlock.SetFloat(ObjectYPosID, transform.position.y);
        //     propertyBlock.SetFloat(DissolveHeightID, 1.0f); // Adjust this value as needed
        //
        //     renderer.SetPropertyBlock(propertyBlock);
        // }
        //
        // // Optionally, destroy the object when fully dissolved
        // if (dissolveAmount >= 1f)
        // {
        //     Destroy(gameObject);
        // }
    }
}
