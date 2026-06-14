using Gob3AQ.Libs.Arith;
using Gob3AQ.VARMAP.GraphicsMaster;
using Gob3AQ.VARMAP.Types;
using System;
using UnityEngine;

[Serializable]
public class CameraEffectsClass : MonoBehaviour
{
    private enum ActiveEffectType
    {
        ACTIVE_EFFECT_NONE,
        ACTIVE_EFFECT_DRUNK
    }

    [SerializeField]
    private Material drunkMaterial;

    private ActiveEffectType activeEffectType;
    private int drunkForce;

    public void ActivateDrunkEffect(int force)
    {
        activeEffectType = ActiveEffectType.ACTIVE_EFFECT_DRUNK;
        drunkForce = force;
    }

    public void DeactivateEffects()
    {
        activeEffectType = ActiveEffectType.ACTIVE_EFFECT_NONE;
    }

    private void Start()
    {
        activeEffectType = ActiveEffectType.ACTIVE_EFFECT_NONE;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        switch(activeEffectType)
        {
            case ActiveEffectType.ACTIVE_EFFECT_DRUNK:
                drunkMaterial.SetFloat("_DistortionStrength", 0.01f*drunkForce);
                Color drunkTint = new Color(0.05f, 0.547f, 0.202f, 0.053f*drunkForce);
                drunkMaterial.SetColor("_ColorTint", drunkTint);

                Graphics.Blit(source, destination, drunkMaterial);
                break;

            default:
                Graphics.Blit(source, destination);
                break;
        }
    }
}
