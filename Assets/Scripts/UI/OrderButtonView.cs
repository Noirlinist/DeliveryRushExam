using System;
using DeliveryRushExam.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DeliveryRushExam.UI
{
    public class OrderButtonView : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text rewardText;
        [SerializeField] private TMP_Text timerText;
        [SerializeField] private Button completeButton;

        private OrderData orderData;
        private Action<string> onCompleteClicked;
        private int _lastDisplayedTime = -1;

        public void Setup(OrderData order, Action<string> completeCallback)
        {
            orderData = order;
            onCompleteClicked = completeCallback;

            if (completeButton == null)
            {
                completeButton = GetComponent<Button>();
            }

            completeButton.onClick.RemoveAllListeners();
            completeButton.onClick.AddListener(HandleClick);

            titleText.text = $"Deliver to {orderData.customerName}";
            rewardText.text = $"+{orderData.rewardPoints} pts / +{orderData.rewardCoins} coins";

            Refresh();
        }

        public void Refresh()
        {
            if (orderData == null)
            {
                return;
            }

            int currentTime =
                Mathf.CeilToInt(orderData.remainingTime);

            if (currentTime == _lastDisplayedTime)
            {
                return;
            }

            _lastDisplayedTime = currentTime;

            timerText.text = $"Time {currentTime}";
        }

        private void HandleClick()
        {
            if (orderData != null)
            {
                onCompleteClicked?.Invoke(orderData.id);
            }
        }
    }
}
