using System;
using TMPro;
using UnityEngine;

namespace DeliveryRushExam.UI
{
    public class ScorePopupView : MonoBehaviour
    {
        [SerializeField] private TMP_Text messageText;
        [SerializeField] private float lifetime = 1.1f;
        [SerializeField] private float moveSpeed = 55f;

        private CanvasGroup _canvasGroup;
        private float _age;

        public event Action<ScorePopupView> Expired;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Setup(string message)
        {
            _age = 0f;

            messageText.text = message;

            if (_canvasGroup != null)
            {
                _canvasGroup.alpha = 1f;
            }
        }

        private void Update()
        {
            _age += Time.deltaTime;

            transform.localPosition +=
                Vector3.up * (moveSpeed * Time.deltaTime);

            if (_canvasGroup != null)
            {
                _canvasGroup.alpha =
                    1f - _age / lifetime;
            }

            if (_age >= lifetime)
            {
                Expired?.Invoke(this);
            }
        }
    }
}