using Sirenix.OdinInspector;
using UnityEngine;
using VolatileVoodoo.Runtime.Values;

namespace VolatileVoodoo.Runtime.Setters
{
    public class PositionRotationSetter : MonoBehaviour
    {
        [Tooltip("degrees per second, 0 = unlimited")]
        public FloatReference angularSpeedLimit;

        [Title("Bindings")]
        public Vector3Reference bindPosition;

        public QuaternionReference bindRotation;

        private Quaternion targetRotation;

        private void Update()
        {
            if (angularSpeedLimit <= 0 || transform.rotation.Equals(targetRotation))
                return;

            var step = angularSpeedLimit * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, step);
        }

        private void OnEnable()
        {
            targetRotation = transform.rotation;
            bindPosition.OnValueChanged += UpdatePosition;
            bindRotation.OnValueChanged += UpdateRotation;
        }

        private void OnDisable()
        {
            bindPosition.OnValueChanged -= UpdatePosition;
            bindRotation.OnValueChanged -= UpdateRotation;
        }

        private void UpdatePosition(Vector3 position)
        {
            transform.position = position;
        }

        private void UpdateRotation(Quaternion rotation)
        {
            if (angularSpeedLimit > 0)
                targetRotation = rotation;
            else
                transform.rotation = rotation;
        }
    }
}