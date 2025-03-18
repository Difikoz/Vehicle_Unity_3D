using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WinterUniverse
{
    public class VitalityBarUI : MonoBehaviour
    {
        [SerializeField] private string _vitalityName = "Vitality";
        [SerializeField] private TMP_Text _valueText;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Image _backgroundFillImage;
        [SerializeField] private Image _foregroundFillImage;
        [SerializeField] private Image _frameImage;
        [SerializeField] private Color _backgroundColor = Color.gray;
        [SerializeField] private Color _fillColor = Color.blue;
        [SerializeField] private Color _frameColor = Color.black;
        [SerializeField] private Color _reduceColor = Color.red;
        [SerializeField] private Color _restoreColor = Color.green;

        private float _floatValue;
        private float _currentValue;
        private float _maxValue;
        private float _differenceValue;
        private Coroutine _coroutine;

        public void Initialize()
        {
            _backgroundImage.color = _backgroundColor;
            _foregroundFillImage.color = _fillColor;
            _frameImage.color = _frameColor;
        }

        public void SetValues(float current, float max)
        {
            _currentValue = current;
            _maxValue = max;
            _valueText.text = $"{_vitalityName}: {_currentValue:0}/{_maxValue:0}";
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            _coroutine = StartCoroutine(ChangeValueTimer());
        }

        private IEnumerator ChangeValueTimer()
        {
            UpdateUI();
            yield return null;
            while (_differenceValue != 0f)
            {
                _floatValue = Mathf.MoveTowards(_floatValue, _currentValue, (_differenceValue + 1f) * Time.deltaTime);
                UpdateUI();
                yield return null;
            }
            _coroutine = null;
        }

        private void UpdateUI()
        {
            _differenceValue = 0f;
            if (_currentValue > _floatValue)// restore
            {
                _differenceValue = _currentValue - _floatValue;
                _backgroundFillImage.color = _restoreColor;
                _backgroundFillImage.fillAmount = _currentValue / _maxValue;
                _foregroundFillImage.fillAmount = _floatValue / _maxValue;
            }
            else if (_floatValue > _currentValue)// reduce
            {
                _differenceValue = _floatValue - _currentValue;
                _backgroundFillImage.color = _reduceColor;
                _backgroundFillImage.fillAmount = _floatValue / _maxValue;
                _foregroundFillImage.fillAmount = _currentValue / _maxValue;
            }
            else
            {
                _backgroundFillImage.color = Color.black;
                _backgroundFillImage.fillAmount = _floatValue / _maxValue;
                _foregroundFillImage.fillAmount = _floatValue / _maxValue;
            }
        }
    }
}