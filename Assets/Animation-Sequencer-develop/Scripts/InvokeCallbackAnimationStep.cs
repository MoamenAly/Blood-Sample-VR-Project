#if DOTWEEN_ENABLED
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace BrunoMikoski.AnimationSequencer
{
    [Serializable]
    public sealed class InvokeCallbackAnimationStep : AnimationStepBase
    {
        [SerializeField]
        private UnityEvent callback = new UnityEvent();

        [SerializeField]
        private UnityEvent2 callbackEvent2 = new UnityEvent2();

        public UnityEvent2 CallbackEvent2
        {
            get => callbackEvent2;
            set => callbackEvent2 = value;
        }

        public UnityEvent Callback
        {
            get => callback;
            set => callback = value;
        }

        public override string DisplayName => "Invoke Callback";


        public override void AddTweenToSequence(Sequence animationSequence)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.SetDelay(Delay);
            sequence.AppendCallback(() => {
                callback.Invoke();
                callbackEvent2.Invoke();
            });

            if (FlowType == FlowType.Append)
                animationSequence.Append(sequence);
            else
                animationSequence.Join(sequence);
        }

        public override void ResetToInitialState()
        {
        }

        public override string GetDisplayNameForEditor(int index)
        {
            string[] persistentTargetNamesArray = new string[callback.GetPersistentEventCount()];
            for (int i = 0; i < callback.GetPersistentEventCount(); i++)
            {
                if (callback.GetPersistentTarget(i) == null)
                    continue;

                if (string.IsNullOrWhiteSpace(callback.GetPersistentMethodName(i)))
                    continue;

                persistentTargetNamesArray[i] = $"{callback.GetPersistentTarget(i).name}.{callback.GetPersistentMethodName(i)}()";
            }

            var persistentTargetNames = $"{string.Join(", ", persistentTargetNamesArray).Truncate(45)}";

            return $"{index}. {DisplayName}: {persistentTargetNames}";
        }
    }
}
#endif