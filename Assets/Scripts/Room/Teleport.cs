using System.Collections;
using Shinrai.Core;
using UnityEngine;

namespace Shinrai.Levels
{
    public class Teleport : MonoBehaviour
    {
        private float _teleportDuration = 0.5f;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            var player = other.gameObject.transform;
            ServiceLocator.GetService<InputService>().SetActive(false);
            StartCoroutine(TweenToTeleport(player));
        }

        private IEnumerator TweenToTeleport(Transform playerTransform)
        {
            var startPos = playerTransform.position;
            var startRot = playerTransform.rotation;
            var startScale = playerTransform.localScale;
            var timeElapsed = 0f;
            while (timeElapsed < _teleportDuration)
            {
                timeElapsed += Time.deltaTime;
                var t = timeElapsed / _teleportDuration;
                playerTransform.position = Vector3.Lerp(startPos, transform.position, t);
                playerTransform.rotation = Quaternion.Lerp(startRot, Quaternion.AngleAxis(180, Vector3.forward), t);
                playerTransform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
                yield return null;
            }
            
            EventBus.Emit(new GameEventChanged(GameEvent.LoadNextRoom));
        }
    }
}
