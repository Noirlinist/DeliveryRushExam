using System;
using System.Threading.Tasks;
using DeliveryRushExam.Data;
using DeliveryRushExam.UGS;
using UnityEngine;

namespace DeliveryRushExam.Save
{
    public class SaveManager : MonoBehaviour
    {
        public PlayerProgressData CurrentProgress { get; private set; } = new PlayerProgressData();

        public event Action<PlayerProgressData> ProgressLoaded;

        private ISaveService saveService;
        
        private void Awake()
        {
            saveService =
                ServiceLocator.Get<ISaveService>();
        }
        
        private async void Start()
        {
            await WaitForUgs();

            await LoadProgressAsync();
        }

        public async Task LoadProgressAsync()
        {
            CurrentProgress =
                await saveService.LoadAsync();

            Debug.Log(
                $"Loaded BestScore: {CurrentProgress.bestScore}");

            Debug.Log(
                $"Loaded Coins: {CurrentProgress.totalCoins}");

            ProgressLoaded?.Invoke(CurrentProgress);
        }

        public async Task SaveMatchResultAsync(int score, int coins, int completedOrders)
        {
            CurrentProgress.bestScore = Mathf.Max(CurrentProgress.bestScore, score);
            CurrentProgress.totalCoins += coins;
            CurrentProgress.completedOrders += completedOrders;

            // Nivel simple para tener un dato extra persistido.
            CurrentProgress.unlockedLevel = Mathf.Max(CurrentProgress.unlockedLevel, 1 + CurrentProgress.completedOrders / 10);

            await saveService.SaveAsync(CurrentProgress);
        }
        
        private async Task WaitForUgs()
        {
            UgsInitializer initializer =
                FindFirstObjectByType<UgsInitializer>();

            if (initializer == null)
            {
                return;
            }

            while (!initializer.IsReady)
            {
                await Task.Yield();
            }
        }
    }
}
