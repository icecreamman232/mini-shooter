using System.Collections;
using Shinrai.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Shinrai.VFX
{
    public class LoadingScreenController : MonoBehaviour
    {
        [SerializeField] private Image _backGround;

        private void Awake()
        {
            _backGround.color = Color.clear;
            EventBus.Subscribe<LoadingScreenEvent>(OnReceiveLoadingScreenEvent);
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<LoadingScreenEvent>(OnReceiveLoadingScreenEvent);
        }

        private IEnumerator OnFadeIn(float duration)
        {
            ServiceLocator.GetService<InGameDataManager>().IsGamePaused = true;
            var timeElapsed = 0f;
            while (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;
                _backGround.color = Color.Lerp(_backGround.color, new Color(0.09019608f, 0.07843138f, 0.07843138f), timeElapsed / duration);
                yield return null;
            }
            ServiceLocator.GetService<InGameDataManager>().IsGamePaused = false;
        }

        private IEnumerator OnFadeOut(float duration)
        {
            ServiceLocator.GetService<InGameDataManager>().IsGamePaused = true;
            var timeElapsed = 0f;
            while (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;
                _backGround.color = Color.Lerp(_backGround.color, Color.clear, timeElapsed / duration);
                yield return null;
            }
            ServiceLocator.GetService<InGameDataManager>().IsGamePaused = false;
        }
        
        private void OnReceiveLoadingScreenEvent(LoadingScreenEvent eventArg)
        {
            if (eventArg.IsFadeOut)
            {
                StartCoroutine(OnFadeOut(eventArg.LoadingDuration));
            }
            else
            {
                StartCoroutine(OnFadeIn(eventArg.LoadingDuration));
            }
        }
    }
}
