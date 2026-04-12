using UnityEngine;

namespace Shinrai.VFX
{
    public class SpriteOutline : MonoBehaviour
    {
        [SerializeField] Color _outlineColor = Color.white;
        [SerializeField] int _outlineSize = 1;

        SpriteRenderer _spriteRenderer;
        MaterialPropertyBlock _materialPropertyBlock;

        static readonly int OutlineSizeProp  = Shader.PropertyToID("_OutlineSize");
        static readonly int OutlineColorProp = Shader.PropertyToID("_OutlineColor");

        void Awake()
        {
            _spriteRenderer  = GetComponent<SpriteRenderer>();
            _materialPropertyBlock = new MaterialPropertyBlock();
        }

        public void ShowOutline()
        {
            _spriteRenderer.GetPropertyBlock(_materialPropertyBlock);
            _materialPropertyBlock.SetInt(  OutlineSizeProp,  _outlineSize);
            _materialPropertyBlock.SetColor(OutlineColorProp, _outlineColor);
            _spriteRenderer.SetPropertyBlock(_materialPropertyBlock);
        }

        public void HideOutline()
        {
            _spriteRenderer.GetPropertyBlock(_materialPropertyBlock);
            _materialPropertyBlock.SetInt(OutlineSizeProp, 0);  // 0px = no outline, no sample cost
            _spriteRenderer.SetPropertyBlock(_materialPropertyBlock);
        }
    }
}
