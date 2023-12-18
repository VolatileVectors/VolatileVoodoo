using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using VolatileVoodoo.Runtime.Values;

namespace VolatileVoodoo.Runtime.Setters
{
    public class ImageFillSetter : MonoBehaviour
    {
        [Tooltip("Min value to have no fill on Image.")]
        [Required]
        public FloatReference min;

        [Tooltip("Max value to fill Image.")]
        public FloatReference max;

        [Tooltip("Image to set the fill amount on.")]
        [Required]
        public Image image;

        public void SetFill(float fill)
        {
            image.fillAmount = Mathf.Clamp01(Mathf.InverseLerp(min, max, fill));
        }
    }
}