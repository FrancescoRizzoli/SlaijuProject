using UnityEngine;
using Cinemachine;

namespace Utility
{
    public static class CameraShake
    {
        private static CameraShakeHelper cameraShakeHelper;
        private static Camera targetCamera;
        private static float remainingDuration;
        private static float magnitude;
        private static Vector3 originalPos;
        private static bool isShaking;
        private static CinemachineVirtualCamera virtualCamera;
        private static CinemachineBasicMultiChannelPerlin noise;
        private static Vector3 originalVirtualCameraPos;

        // Create helper object for the camera shake
        private static void EnsureHelperExists()
        {
            if (cameraShakeHelper == null)
            {
                GameObject helperObject = new GameObject("CameraShakeHelper");
                cameraShakeHelper = helperObject.AddComponent<CameraShakeHelper>();
                Object.DontDestroyOnLoad(helperObject);
            }
        }

        /// <summary>
        /// Set the data in the MonoBehaviour to handle the shake
        /// The value are set to case of death
        /// </summary>
        /// <param name="cam"></param>
        /// <param name="duration"></param>
        /// <param name="mag"></param>
        public static void Shake(Camera cam, float duration = 0.4f, float mag = 0.2f)
        {
            if (cam == null) return;
            EnsureHelperExists();

            // Check if a shake is already in progress
            if (isShaking && targetCamera == cam)
            {
                // Combine the duration and magnitude of the shakes
                remainingDuration += duration;
                magnitude = Mathf.Max(magnitude, mag);
            }
            else
            {
                targetCamera = cam;
                remainingDuration = duration;
                magnitude = mag;
                originalPos = targetCamera.transform.localPosition;
                isShaking = true;
            }
        }

        /// <summary>
        /// Set the data in the MonoBehaviour to handle the shake using Cinemachine noise
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="mag"></param>
        public static void ShakeCinemachine(float duration = 0.4f, float mag = 2f)
        {
            EnsureHelperExists();

            // Find the active Cinemachine Virtual Camera
            virtualCamera = CinemachineCore.Instance.GetActiveBrain(0)?.ActiveVirtualCamera as CinemachineVirtualCamera;
            if (virtualCamera == null) return;

            noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            if (noise == null)
            {
                noise = virtualCamera.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            }

            // Check if a shake is already in progress
            if (isShaking)
            {
                // Combine the duration and magnitude of the shakes
                remainingDuration += duration;
                magnitude = Mathf.Max(magnitude, mag);
            }
            else
            {
                remainingDuration = duration;
                magnitude = mag;
                noise.m_AmplitudeGain = magnitude;
                originalVirtualCameraPos = virtualCamera.transform.localPosition;
                isShaking = true;
            }
        }

        /// <summary>
        /// Set the data in the MonoBehaviour to handle the shake using Cinemachine transform only
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="mag"></param>
        public static void ShakeCinemachineTransform(float duration = 0.4f, float mag = 0.2f)
        {
            EnsureHelperExists();

            // Find the active Cinemachine Virtual Camera
            virtualCamera = CinemachineCore.Instance.GetActiveBrain(0)?.ActiveVirtualCamera as CinemachineVirtualCamera;
            if (virtualCamera == null) return;

            // Check if a shake is already in progress
            if (isShaking)
            {
                // Combine the duration and magnitude of the shakes
                remainingDuration += duration;
                magnitude = Mathf.Max(magnitude, mag);
            }
            else
            {
                remainingDuration = duration;
                magnitude = mag;
                originalVirtualCameraPos = virtualCamera.transform.localPosition;
                isShaking = true;
            }
        }

        /// <summary>
        /// MonoBehaviour that handles the shake of the camera in the LateUpdate
        /// </summary>
        private class CameraShakeHelper : MonoBehaviour
        {
            void LateUpdate()
            {
                if (!isShaking) return;

                // Check target camera exists and not destroyed
                if (targetCamera == null && virtualCamera == null)
                {
                    isShaking = false;
                    return;
                }

                if (remainingDuration > 0)
                {
                    ApplyCameraShake();
                    ApplyCinemachineShake();
                    remainingDuration -= Time.deltaTime;
                }
                else
                {
                    ResetCameraPosition();
                    ResetCinemachinePosition();
                    isShaking = false;
                }
            }

            private void ApplyCameraShake()
            {
                if (targetCamera != null)
                {
                    float xOffset = Random.Range(-0.5f, 0.5f) * magnitude;
                    float yOffset = Random.Range(-0.5f, 0.5f) * magnitude;
                    targetCamera.transform.localPosition = new Vector3(originalPos.x + xOffset, originalPos.y + yOffset, originalPos.z);
                }
            }

            private void ApplyCinemachineShake()
            {
                if (virtualCamera != null)
                {
                    float xOffset = Random.Range(-0.5f, 0.5f) * magnitude;
                    float yOffset = Random.Range(-0.5f, 0.5f) * magnitude;
                    virtualCamera.transform.localPosition = new Vector3(originalVirtualCameraPos.x + xOffset, originalVirtualCameraPos.y + yOffset, originalVirtualCameraPos.z);

                    if (noise != null)
                    {
                        noise.m_AmplitudeGain = magnitude;
                    }
                }
            }

            private void ResetCameraPosition()
            {
                if (targetCamera != null)
                {
                    targetCamera.transform.localPosition = originalPos;
                }
            }

            private void ResetCinemachinePosition()
            {
                if (virtualCamera != null)
                {
                    virtualCamera.transform.localPosition = originalVirtualCameraPos;

                    if (noise != null)
                    {
                        noise.m_AmplitudeGain = 0f;
                    }
                }
            }
        }
    }
}
