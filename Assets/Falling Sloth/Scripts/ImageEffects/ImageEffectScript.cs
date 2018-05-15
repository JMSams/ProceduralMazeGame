using UnityEngine;

namespace FallingSloth.ImageEffects
{
    [ExecuteInEditMode]
    [AddComponentMenu("Falling Sloth/Image Effects/Image Effect Script")]
    public class ImageEffectScript : MonoBehaviour
    {
        public Material mat;

        protected virtual void OnRenderImage(RenderTexture src, RenderTexture dst)
        {
            Graphics.Blit(src, dst, mat);
        }
    }
}