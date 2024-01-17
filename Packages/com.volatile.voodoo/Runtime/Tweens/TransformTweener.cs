using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using VolatileVoodoo.Events;
using VolatileVoodoo.Utils;

namespace VolatileVoodoo.Tweens
{
    public enum TweenTarget
    {
        [LabelText("Position", SdfIconType.ArrowsMove)]
        Position,

        [LabelText("Rotation", SdfIconType.ArrowClockwise)]
        Rotation,

        [LabelText("Scale", SdfIconType.ArrowsAngleExpand)]
        Scale
    }

    public enum TweenControl
    {
        [LabelText("Event (Manual)", SdfIconType.LightningFill)]
        Event,

        [LabelText("YoYo (Auto)", SdfIconType.ArrowDownUp)]
        YoYo
    }

    public enum EasingState
    {
        [LabelText("Eased out", SdfIconType.ChevronBarLeft)]
        Beginning,

        [LabelText("Eased in", SdfIconType.ChevronBarRight)]
        End
    }

    public class TransformTweener : MonoBehaviour
    {
        [Title("Target"), HorizontalGroup]
        [EnumToggleButtons, HideLabel, DisableInPlayMode]
        public TweenTarget tweenTarget;

        [Title("Control"), HorizontalGroup]
        [EnumToggleButtons, HideLabel, DisableInPlayMode]
        public TweenControl tweenControl;

        [OnValueChanged(nameof(CreateTween))]
        public Vector3 amount;

        [OnValueChanged(nameof(CreateTween))]
        public float duration;

        [OnValueChanged(nameof(UpdateTween))]
        public bool unscaledTime;

        [TitleGroup("Easing State"), LabelText("Editor")]
        [EnumToggleButtons, DisableInPlayMode]
        public EasingState editorPosition;

        [TitleGroup("Easing State"), LabelText("PlayMode start")]
        [EnumToggleButtons, DisableInPlayMode]
        public EasingState playModeStartPosition;

        [TitleGroup("Transition")]
        [OnValueChanged(nameof(UpdateTween))]
        [ShowIf("@" + nameof(tweenControl) + " == TweenControl.Event")]
        public Ease easeInFunction;

        [TitleGroup("Transition")]
        [OnValueChanged(nameof(UpdateTween))]
        [ShowIf("@" + nameof(tweenControl) + " == TweenControl.Event")]
        public Ease easeOutFunction;

        [TitleGroup("Transition")]
        [OnValueChanged(nameof(UpdateTween))]
        [ShowIf("@" + nameof(tweenControl) + " == TweenControl.YoYo")]
        public Ease yoYoFunction;

        [TitleGroup("Events"), LabelText("Eased in")]
        [ShowIf("@" + nameof(tweenControl) + " == TweenControl.Event")]
        public GameEventSource easedInEvent;

        [TitleGroup("Events"), LabelText("Eased out")]
        [ShowIf("@" + nameof(tweenControl) + " == TweenControl.Event")]
        public GameEventSource easedOutEvent;

        [TitleGroup("Events"), LabelText("State changed")]
        [ShowIf("@" + nameof(tweenControl) + " == TweenControl.Event")]
        public BoolEventSource stateChangedEvent;

        private Tween transitionTween;
        private Vector3 initialTarget;

        private void Awake()
        {
            var currentTransform = transform;
            initialTarget = tweenTarget switch
            {
                TweenTarget.Position => currentTransform.localPosition,
                TweenTarget.Rotation => currentTransform.localEulerAngles,
                TweenTarget.Scale => currentTransform.localScale,
                _ => throw new ArgumentOutOfRangeException()
            };

            CreateTween();
        }

        public void SetEase(bool active)
        {
            if (tweenControl == TweenControl.YoYo)
                return;

            if (transitionTween.isBackwards == active)
                transitionTween.SetEase(active ? easeInFunction : easeOutFunction);

            if (transitionTween.IsPlaying())
                transitionTween.isBackwards = !active;
            else
            {
                if (active) transitionTween.PlayForward();
                else transitionTween.PlayBackwards();
            }
        }

        [TitleGroup("Debug"), ButtonGroup("Debug/Triggers"), Button]
        [ShowIf(Voodoo.IsPlaying + " && " + nameof(tweenControl) + " == TweenControl.Event")]
        public void EaseIn()
        {
            SetEase(true);
        }

        [ButtonGroup("Debug/Triggers"), Button]
        [ShowIf(Voodoo.IsPlaying + " && " + nameof(tweenControl) + " == TweenControl.Event")]
        public void EaseOut()
        {
            SetEase(false);
        }

        private void OnDestroy()
        {
            transitionTween?.Kill();
        }

        private void CreateTween()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return;

            transitionTween?.Kill();
            transitionTween = null;
#endif
            Vector3 tweenStart;
            Vector3 tweenEnd;

            switch (editorPosition)
            {
                case EasingState.Beginning:
                    tweenStart = initialTarget;
                    tweenEnd = initialTarget + amount;
                    break;
                case EasingState.End:
                    tweenStart = initialTarget - amount;
                    tweenEnd = initialTarget;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var currentTransform = transform;
            switch (tweenTarget)
            {
                case TweenTarget.Position:
                    transform.localPosition = tweenStart;
                    break;
                case TweenTarget.Rotation:
                    transform.localEulerAngles = tweenStart;
                    break;
                case TweenTarget.Scale:
                    transform.localScale = tweenStart;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            transitionTween = tweenTarget switch
            {
                TweenTarget.Position => currentTransform.DOLocalMove(tweenEnd, duration),
                TweenTarget.Rotation => currentTransform.DOLocalRotate(tweenEnd, duration),
                TweenTarget.Scale => currentTransform.DOScale(tweenEnd, duration),
                _ => throw new ArgumentOutOfRangeException()
            };
            transitionTween.SetAutoKill(false).SetUpdate(unscaledTime).Goto(playModeStartPosition switch
            {
                EasingState.Beginning => 0f,
                EasingState.End => duration,
                _ => throw new ArgumentOutOfRangeException()
            });

            switch (tweenControl)
            {
                case TweenControl.Event:
                    transitionTween.SetEase(easeInFunction).OnRewind(OnBeginningReached).OnComplete(OnEndReached);
                    break;
                case TweenControl.YoYo:
                    transitionTween.SetEase(yoYoFunction).SetLoops(-1, LoopType.Yoyo).Play();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnBeginningReached()
        {
            easedOutEvent.Raise();
            stateChangedEvent.Raise(false);
        }

        private void OnEndReached()
        {
            easedInEvent.Raise();
            stateChangedEvent.Raise(true);
        }


        private void UpdateTween()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying || transitionTween == null)
                return;

            transitionTween.SetUpdate(unscaledTime).SetEase(tweenControl switch
            {
                TweenControl.Event => transitionTween.isBackwards ? easeOutFunction : easeInFunction,
                TweenControl.YoYo => yoYoFunction,
                _ => throw new ArgumentOutOfRangeException()
            });
#endif
        }
    }
}