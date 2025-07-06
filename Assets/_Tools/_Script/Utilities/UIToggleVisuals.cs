using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using NaughtyAttributes;

namespace Anuj.Utility.Ui
{
    [RequireComponent(typeof(Toggle))]
    public class UIToggleVisuals : MonoBehaviour
    {
        [SerializeField] private Toggle toggle;
        [SerializeField] private ToggleType toggleType;

        [ShowIf(nameof(toggleType), ToggleType.SeparateOnOffVisuals)]
        [SerializeField] private Image visualsToggleOn;

        [ShowIf(nameof(toggleType), ToggleType.SeparateOnOffVisuals)]
        [SerializeField] private Image visualsToggleOff;

        [ShowIf(nameof(toggleType), ToggleType.AnimatedSingleHandle)]
        [SerializeField] private RectTransform handle;

        [ShowIf(nameof(toggleType), ToggleType.AnimatedSingleHandle)]
        [SerializeField] private Vector3 handlePositionToggleOn;

        [ShowIf(nameof(toggleType), ToggleType.AnimatedSingleHandle)]
        [SerializeField] private Vector3 handlePositionToggleOff;

        [SerializeField] private bool animateToggleVisuals;

        [ShowIf(nameof(animateToggleVisuals))]
        [SerializeField] private float animationDuration = .2f;

        [ShowIf(nameof(animateToggleVisuals))]
        [SerializeField] private float backgroundAnimationDuration = .2f;

        [ShowIf(nameof(animateToggleVisuals))]
        [SerializeField] private Image toggleBackground;

        [ShowIf(nameof(animateToggleVisuals))]
        [SerializeField] private Color colorToggleOn;

        [ShowIf(nameof(animateToggleVisuals))]
        [SerializeField] private Color colorToggleOff = Color.white;

        private Action<bool> toggleAction;
        private enum ToggleType
        {
            AnimatedSingleHandle,
            SeparateOnOffVisuals,
        }

        private void Awake()
        {
            switch (toggleType)
            {
                case ToggleType.AnimatedSingleHandle:

                    if (animateToggleVisuals)
                    {
                        toggleAction = (value) =>
                        {
                            if (handle != null)
                            {
                                Vector3 targetPosition = value ? handlePositionToggleOn : handlePositionToggleOff;
                                handle.DOLocalMove(targetPosition, animationDuration).SetEase(Ease.OutQuad);
                            }

                            if (toggleBackground != null)
                            {
                                Color targetColor = value ? colorToggleOn : colorToggleOff;
                                toggleBackground.DOColor(targetColor, backgroundAnimationDuration);
                            }
                        };
                    }
                    else
                    {
                        toggleAction = (value) =>
                        {
                            if (handle != null)
                            {
                                Vector3 targetPosition = value ? handlePositionToggleOn : handlePositionToggleOff;
                                handle.localPosition = targetPosition;
                            }

                            if (toggleBackground != null)
                            {
                                Color targetColor = value ? colorToggleOn : colorToggleOff;
                                toggleBackground.color = targetColor;
                            }
                        };
                    }

                    break;

                case ToggleType.SeparateOnOffVisuals:

                    if (animateToggleVisuals)
                    {
                        toggleAction = (value) =>
                        {
                            if (value)
                            {
                                visualsToggleOn.DOFade(1f, animationDuration).SetEase(Ease.Linear);
                                visualsToggleOff.DOFade(0f, animationDuration).SetEase(Ease.Linear);
                            }
                            else
                            {
                                visualsToggleOn.DOFade(0f, animationDuration).SetEase(Ease.Linear);
                                visualsToggleOff.DOFade(1f, animationDuration).SetEase(Ease.Linear);
                            }
                        };
                    }
                    else
                    {
                        toggleAction = (value) =>
                        {
                            if (value)
                            {
                                visualsToggleOn.color = new Color(visualsToggleOn.color.r, visualsToggleOn.color.g, visualsToggleOn.color.b, 1f);
                                visualsToggleOff.color = new Color(visualsToggleOn.color.r, visualsToggleOn.color.g, visualsToggleOn.color.b, 0);
                            }
                            else
                            {
                                visualsToggleOn.color = new Color(visualsToggleOn.color.r, visualsToggleOn.color.g, visualsToggleOn.color.b, 0);
                                visualsToggleOff.color = new Color(visualsToggleOn.color.r, visualsToggleOn.color.g, visualsToggleOn.color.b, 1);
                            }
                        };
                    }

                    break;

            }

            toggle.onValueChanged.AddListener(OnValueChanged);
            OnValueChanged(toggle.isOn);
        }

        private void OnValueChanged(bool value)
        {
            toggleAction(value);
        }
    }

}