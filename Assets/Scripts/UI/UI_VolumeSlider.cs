using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_VolumeSlider : MonoBehaviour
{
    public Slider slider;
    public string parametr;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float multiplier;

    private void Start()
    {
        SliderValue(slider.value);
    }

    private void SliderValue(float _value)
    {
        if (_value == 0)
        {
            audioMixer.SetFloat(parametr, -80f);
            return;
        }

        audioMixer.SetFloat(parametr, Mathf.Log10(_value) * multiplier);
    }

    public void LoadSlider(float _value)
    {
        slider.value = _value;
        SliderValue(_value);
    }
}