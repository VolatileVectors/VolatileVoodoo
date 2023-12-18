using Sirenix.OdinInspector;
using UnityEngine;

namespace VolatileVoodoo.Runtime.Setters
{
    public class AnimatorTriggerSetter : MonoBehaviour
    {
        [Tooltip("Animator to set parameter on.")]
        [Required]
        public Animator animator;

        [Tooltip("Name of the parameter to set.")]
        [Required]
        public string triggerName;

        [SerializeField]
        [HideInInspector]
        private int triggerHash;

        private void OnValidate()
        {
            triggerHash = Animator.StringToHash(triggerName);
        }

        public void Trigger()
        {
            animator.SetTrigger(triggerHash);
        }
    }
}