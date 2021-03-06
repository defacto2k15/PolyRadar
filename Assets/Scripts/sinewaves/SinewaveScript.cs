﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.RadarBattleground;
using UnityEngine;

public class SinewaveScript : MonoBehaviour
{
    public GameObject DotPrefab;
    public int DotsCount;

    public float increment = 0.01f;
    public float amplitude = 1f;
    public float frequency = 1f;
    public float phase = 0f;
    public float scale = 5f;

    float _t = 0;
    private List<GameObject> _dots;

    void Start()
    {
        _dots = Enumerable.Range(0, DotsCount).Select(i =>
        {
            var dot = Instantiate(DotPrefab, transform);
            dot.name = $"Dot {i}";

            dot.transform.localPosition = new Vector3(i / ((float)DotsCount - 1), 0, 0);
            AssureDotHasRegularShape(dot);

            return dot;
        }).ToList();
    }

    private void AssureDotHasRegularShape(GameObject dot)
    {
        var smallestLossyScale = Mathf.Min(new[] {dot.transform.lossyScale.x, dot.transform.lossyScale.y, dot.transform.lossyScale.z});
        var parentScale = transform.lossyScale;
        var newLocalScale = new Vector3(1 / parentScale.x, 1 / parentScale.y, 1 / parentScale.z) * smallestLossyScale;
        dot.transform.localScale = newLocalScale;
    }

    void Update()
    {
        _t += increment;
        foreach (var dot in _dots.Select(c => c.transform))
        {
            Vector3 newPosition = dot.localPosition;
            newPosition.y = amplitude * Mathf.Sin(frequency * _t + phase + dot.localPosition.x / scale);
            dot.localPosition = newPosition;
        }
    }

    public void setDotColor(int i, Color c)
    {
        _dots[i].GetComponent<MaterialPropertyBlockColorSetterOC>().Color = c;
        _dots[i].GetComponent<MaterialPropertyBlockColorSetterOC>().UpdateColor();
    }

    public Vector3 getDotPosition(int i)
    {
        return _dots[i].transform.position;
    }
        
    public Vector3 getDotLocalPosition(int i)
    {
        return _dots[i].transform.localPosition;
    }

    public void sliderHandlerAmplitude(float f)
    {
        amplitude = f;
    }

    public void sliderHandlerScale(float f)
    {
        scale = 0.1f * f;
    }
    public void sliderHandlerPhase (float f)
    {
        phase = 1 - f;
    }
}
