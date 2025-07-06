using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System;

namespace Anuj.Utility.Ui
{
    public enum FadeType
    {
        FadeIn,
        FadeOut
    }
    public static class UIAnimator
    {
        private static readonly Dictionary<GameObject, Vector3> OriginalPositions = new Dictionary<GameObject, Vector3>();

        private static CanvasGroup EnsureCanvasGroup(GameObject targetPanel)
        {
            if (targetPanel.TryGetComponent(out CanvasGroup cg))
            {
                return cg;
            }

            return targetPanel.AddComponent<CanvasGroup>();
        }

        public static Action PanelAppear(GameObject targetPanel, float fadeDuration = 0.3f, float moveDistanceMultiplier = 1, float moveDuration = 0.4f)
        {
            CanvasGroup canvasGroup = EnsureCanvasGroup(targetPanel);

            if (!OriginalPositions.ContainsKey(targetPanel))
                OriginalPositions[targetPanel] = targetPanel.transform.localPosition;

            //float moveDistance = 50 * moveDistanceMultiplier;
            float moveDistance = Screen.height * 0.1f * moveDistanceMultiplier;
            Vector3 originalPosition = OriginalPositions[targetPanel];

            targetPanel.SetActive(true);
            targetPanel.transform.localPosition = originalPosition - new Vector3(0f, moveDistance, 0f);
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;

            Sequence sequence = DOTween.Sequence();
            sequence.Join(canvasGroup.DOFade(1f, fadeDuration));
            sequence.Join(targetPanel.transform.DOLocalMoveY(originalPosition.y, moveDuration).SetEase(Ease.OutCubic));
            sequence.OnComplete(() => canvasGroup.blocksRaycasts = true);

            return () =>
            {
                sequence?.Kill();
            };
        }

        public static Action Fade(GameObject targetPanel, FadeType fadeType, float duration = 0.3f)
        {
            CanvasGroup canvasGroup = EnsureCanvasGroup(targetPanel);

            bool isFadeIn = fadeType == FadeType.FadeIn;

            if (isFadeIn)
            {
                targetPanel.SetActive(true);
                canvasGroup.alpha = 0f;
            }

            canvasGroup.blocksRaycasts = false;

            Tween tween = canvasGroup.DOFade(isFadeIn ? 1f : 0f, duration).OnComplete(() =>
            {
                if (!isFadeIn)
                {
                    targetPanel.SetActive(false);
                }

                canvasGroup.blocksRaycasts = true;
            });

            return () =>
            {
                tween.Kill();
            };
        }

        public static Action MoveXAndFade(GameObject targetPanel, float fadeDuration = 0.3f, float moveDistance = 100f, float moveDuration = 0.4f)
        {
            CanvasGroup canvasGroup = EnsureCanvasGroup(targetPanel);

            if (!OriginalPositions.ContainsKey(targetPanel))
            {
                OriginalPositions[targetPanel] = targetPanel.transform.localPosition;
            }

            Vector3 originalPosition = OriginalPositions[targetPanel];

            targetPanel.SetActive(true);
            targetPanel.transform.localPosition = originalPosition - new Vector3(moveDistance, 0f, 0f);
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;

            Sequence sequence = DOTween.Sequence();
            sequence.Join(canvasGroup.DOFade(1f, fadeDuration));
            sequence.Join(targetPanel.transform.DOLocalMoveX(originalPosition.x, moveDuration).SetEase(Ease.OutCubic));
            sequence.OnComplete(() => canvasGroup.blocksRaycasts = true);

            return () =>
            {
                sequence?.Kill();
            };
        }
    }
}