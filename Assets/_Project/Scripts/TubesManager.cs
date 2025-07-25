using System;
using System.Collections;
using System.Collections.Generic;
using BNG;
using DG.Tweening;
using LiquidVolumeFX;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class TubesManager : MonoBehaviour
{
    [SerializeField] List<Tube> tubes = new List<Tube>();
    [SerializeField] float fillDuration = 1.5f;
    private SnapZone _tubeSnapZone;
    private SnapZoneCheck _snapZoneCheck;
    private GameObject _highlightTube;
    [SerializeField] int currentIndex;
    private int _maxTubes;

    private void Start()
    {
        _maxTubes = tubes.Count;
    }

    public void OnPutTube(SnapZone zone)
    {
        _tubeSnapZone = zone;
        _snapZoneCheck = _tubeSnapZone.GetComponent<SnapZoneCheck>();
        _highlightTube = _tubeSnapZone.transform.GetChild(0).gameObject;
        if (currentIndex < _maxTubes)
        {
            Tube tube = tubes[currentIndex];
            LiquidVolume liquid = tube.liquid.GetComponent<LiquidVolume>();

            tube.onStartFillLiquid?.Invoke();

            float currentLevel = liquid.level;

            DOTween.To(() => currentLevel, x =>
                {
                    currentLevel = x;
                    liquid.level = currentLevel;
                }, 1f, fillDuration)
                .OnComplete(() =>
                {
                    tube.onEndFillLiquid?.Invoke();
                    _tubeSnapZone.CanRemoveItem = true;
                    currentIndex++;
                   _snapZoneCheck.ChangeAllowNames(tubes[currentIndex].nameTube);
                });
        }
    }

    public void OpenHighlightTube()
    {
        _highlightTube.SetActive(true);
    }

    [Serializable]
    public class Tube
    {
        [FoldoutGroup("Tube")] 
        public string nameTube;
        [FoldoutGroup("Tube")]
        public LiquidVolume liquid;
        [FoldoutGroup("Tube")]
        public UnityEvent2 onStartFillLiquid;
        [FoldoutGroup("Tube")]
        public UnityEvent2 onEndFillLiquid;
    }
}
