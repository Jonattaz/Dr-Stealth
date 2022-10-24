// Based on ProceduralImage

using System;
using Mz.Numerics;
using UnityEngine;
using UnityEngine.UI;

namespace Mz.App.UI
{
    public class ImageRounded : Image
    {
        private float _borderWidth;

        public float BorderWidth
        {
            get => _borderWidth;
            set
            {
                _borderWidth = value;
                SetAllDirty();
            }
        }

        private float _falloffDistance = 0.2f;

        public float FalloffDistance
        {
            get => _falloffDistance;
            set
            {
                _falloffDistance = value;
                SetAllDirty();
            }
        }

        private static Material _material;

        private ImageRoundedModifier _modifier;

        public ImageRoundedModifier Modifier
        {
            get { return _modifier ?? (_modifier = new RoundModifier()); }
            set
            {
                if (_modifier == value) return;
                _modifier = value;
                SetAllDirty();
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _Initialize();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            m_OnDirtyVertsCallback -= OnVerticesDirty;
        }

        private void _Initialize()
        {
            type = Type.Simple;

            m_OnDirtyVertsCallback += OnVerticesDirty;
            preserveAspect = false;
            if (sprite == null) sprite = EmptySprite.Get();

            if (_material == null)
            {
                // _material = new Material(Shader.Find("UI/Default"));
                _material = Resources.Load<Material>("Materials/ImageRoundedMaterial");
            }

            material = _material;

            SetAllDirty();
        }

        protected void OnVerticesDirty()
        {
            if (sprite == null) sprite = EmptySprite.Get();
        }

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            base.OnPopulateMesh(toFill);
            _EncodeAllOptionsIntoVertices(toFill, _CalculateOptions());
        }

        private ImageRoundedOptions _CalculateOptions()
        {
            var r = GetPixelAdjustedRect();

            var corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            var pixelSize = Vector3.Distance(corners[1], corners[2]) / r.width;
            pixelSize /= Mathf.Max(0, FalloffDistance);

            var radius = _FixRadius(Modifier.CalculateRadius(r));
            var minSide = Mathf.Min(r.width, r.height);
            
            var options = new ImageRoundedOptions(
                r.width + FalloffDistance,
                r.height + FalloffDistance,
                FalloffDistance, pixelSize,
                radius / minSide,
                BorderWidth / minSide * 2
            );

            return options;
        }

        private static void _EncodeAllOptionsIntoVertices(VertexHelper vh, ImageRoundedOptions options)
        {
            var vert = new UIVertex();

            var uv1 = new Vector2(options.Width, options.Height);
            var uv2 = new Vector2(_EncodeFloats_0_1_16_16(options.Radius.x, options.Radius.y),
                _EncodeFloats_0_1_16_16(options.Radius.z, options.Radius.w));
            var uv3 = new Vector2(Math.Abs(options.BorderWidth) < Numbers.Epsilon ? 1 : Mathf.Clamp01(options.BorderWidth), options.PixelSize);

            for (var i = 0; i < vh.currentVertCount; i++)
            {
                vh.PopulateUIVertex(ref vert, i);

                vert.position += ((Vector3) vert.uv0 - new Vector3(0.5f, 0.5f)) * options.FallOffDistance;

                vert.uv1 = uv1;
                vert.uv2 = uv2;
                vert.uv3 = uv3;

                vh.SetUIVertex(vert, i);
            }
        }

        /// <summary>
        /// Encode two values between [0,1] into a single float. Each using 16 bits.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static float _EncodeFloats_0_1_16_16(float a, float b)
        {
            var kDecodeDot = new Vector2(1.0f, 1f / 65535.0f);
            return Vector2.Dot(new Vector2(Mathf.Floor(a * 65534) / 65535f, Mathf.Floor(b * 65534) / 65535f),kDecodeDot);
        }

        /// <summary>
        /// Prevents radius from getting bigger than rect size
        /// </summary>
        /// <returns>The fixed radius.</returns>
        /// <param name="vec">border-radius as Vector4 (starting upper-left, clockwise)</param>
        private Vector4 _FixRadius(Vector4 vec)
        {
            var r = rectTransform.rect;
            vec = new Vector4(Mathf.Max(vec.x, 0), Mathf.Max(vec.y, 0), Mathf.Max(vec.z, 0), Mathf.Max(vec.w, 0));
            var scaleFactor = Mathf.Min(r.width / (vec.x + vec.y), r.width / (vec.z + vec.w),
                r.height / (vec.x + vec.w), r.height / (vec.z + vec.y), 1);
            return vec * scaleFactor;
        }

#if UNITY_EDITOR
        public void Update()
        {
            if (!Application.isPlaying)
            {
                UpdateGeometry();
            }
        }

        protected override void Reset()
        {
            base.Reset();
            OnEnable();
        }

        /// <summary>
        /// Called when the script is loaded or a value is changed in the
        /// inspector (Called in the editor only).
        /// </summary>
        protected override void OnValidate()
        {
            base.OnValidate();

            // Don't allow negative numbers for fall off distance
            FalloffDistance = Mathf.Max(0, FalloffDistance);

            // Don't allow negative numbers for fall off distance
            BorderWidth = Mathf.Max(0, BorderWidth);
        }
#endif
    }
}