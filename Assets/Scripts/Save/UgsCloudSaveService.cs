using System.Collections.Generic;
using System.Threading.Tasks;
using DeliveryRushExam.Data;
using Unity.Services.CloudSave;
using UnityEngine;

namespace DeliveryRushExam.Save
{
    public class UgsCloudSaveService : ISaveService
    {
        private const string ProgressKey = "delivery_rush_progress";

        public async Task SaveAsync(PlayerProgressData progressData)
        {
            Debug.Log("Saving to Cloud");
            Debug.Log(JsonUtility.ToJson(progressData));
            
            progressData.TouchSaveDate();

            string json =
                JsonUtility.ToJson(progressData);

            var data =
                new Dictionary<string, object>
                {
                    { ProgressKey, json }
                };

            await CloudSaveService.Instance.Data.Player.SaveAsync(data);
        }

        public async Task<PlayerProgressData> LoadAsync()
        {
            Debug.Log("Loading from Cloud");
            var result =
                await CloudSaveService.Instance.Data.Player.LoadAsync(
                    new HashSet<string>
                    {
                        ProgressKey
                    });

            if (!result.TryGetValue(ProgressKey, out var item))
            {
                return new PlayerProgressData();
            }

            string json =
                item.Value.GetAsString();

            return JsonUtility.FromJson<PlayerProgressData>(json)
                   ?? new PlayerProgressData();
        }
    }
}