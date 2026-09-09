using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadScreen : MonoBehaviour
{
    [SerializeField] Slider loadBar;

    private void OnEnable()
    {
        loadBar.value = 0f;
    }
    void UpdateSlider(float value)
    {
        value = Mathf.Clamp01(value);
        loadBar.value = value;
    }

    public IEnumerator FakeLoad(float dur = 1f)
    {
        float timer = 0f;

        while (timer < dur)
        {
            timer += Time.deltaTime;
            UpdateSlider(timer/dur);
            yield return null;
        }

        UpdateSlider(1f);
    }
}
