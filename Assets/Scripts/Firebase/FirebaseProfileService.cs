using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Firestore;
using UnityEngine;

namespace Drive56.Firebase
{
    [Serializable]
    public class PlayerProfile
    {
        public string uid;
        public string displayName;
        public long money;
        public List<string> ownedVehicles = new List<string>();
        public List<string> unlockedCars = new List<string>();
        public int selectedVehicleIndex;
        public int xp;
        public int level;

        public PlayerProfile() { }

        public PlayerProfile(string playerUid, string playerName)
        {
            uid = playerUid;
            displayName = playerName;
            money = 0;
            selectedVehicleIndex = 0;
            xp = 0;
            level = 1;
        }
    }

    public class FirebaseProfileService : MonoBehaviour
    {
        private FirebaseFirestore _db;

        public async Task InitializeAsync()
        {
            await FirebaseBootstrap.InitializeAsync();
            _db = FirebaseFirestore.DefaultInstance;
        }

        public async Task<PlayerProfile> LoadProfileAsync()
        {
            var user = FirebaseBootstrap.Auth?.CurrentUser;
            if (user == null)
            {
                Debug.LogWarning("No Firebase Auth user available. Cannot load profile.");
                return null;
            }

            try
            {
                var snapshot = await _db.Collection("players").Document(user.UserId).GetSnapshotAsync();
                if (!snapshot.Exists)
                {
                    var profile = new PlayerProfile(user.UserId, user.DisplayName ?? "Player");
                    await SaveProfileAsync(profile);
                    return profile;
                }

                var data = snapshot.ToDictionary();
                var profile = new PlayerProfile
                {
                    uid = user.UserId,
                    displayName = data.ContainsKey("displayName") ? data["displayName"].ToString() : user.DisplayName ?? "Player",
                    money = data.ContainsKey("money") ? Convert.ToInt64(data["money"]) : 0,
                    selectedVehicleIndex = data.ContainsKey("selectedVehicleIndex") ? Convert.ToInt32(data["selectedVehicleIndex"]) : 0,
                    xp = data.ContainsKey("xp") ? Convert.ToInt32(data["xp"]) : 0,
                    level = data.ContainsKey("level") ? Convert.ToInt32(data["level"]) : 1,
                    ownedVehicles = data.ContainsKey("ownedVehicles") ? new List<string>((IEnumerable<string>)data["ownedVehicles"]) : new List<string>(),
                    unlockedCars = data.ContainsKey("unlockedCars") ? new List<string>((IEnumerable<string>)data["unlockedCars"]) : new List<string>()
                };

                return profile;
            }
            catch (Exception ex)
            {
                Debug.LogError($"LoadProfileAsync failed: {ex}");
                return null;
            }
        }

        public async Task SaveProfileAsync(PlayerProfile profile)
        {
            var user = FirebaseBootstrap.Auth?.CurrentUser;
            if (user == null || profile == null)
            {
                Debug.LogWarning("Cannot save profile. Firebase user is missing or profile is null.");
                return;
            }

            profile.uid = user.UserId;
            profile.displayName ??= user.DisplayName ?? "Player";

            var data = new Dictionary<string, object>
            {
                { "uid", profile.uid },
                { "displayName", profile.displayName },
                { "money", profile.money },
                { "ownedVehicles", profile.ownedVehicles ?? new List<string>() },
                { "unlockedCars", profile.unlockedCars ?? new List<string>() },
                { "selectedVehicleIndex", profile.selectedVehicleIndex },
                { "xp", profile.xp },
                { "level", profile.level }
            };

            try
            {
                await _db.Collection("players").Document(user.UserId).SetAsync(data);
                Debug.Log("Player profile saved to Firestore.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"SaveProfileAsync failed: {ex}");
            }
        }

        public async Task AddMoneyAsync(long amount)
        {
            var profile = await LoadProfileAsync();
            if (profile == null) return;

            profile.money += amount;
            await SaveProfileAsync(profile);
        }

        public async Task BuyVehicleAsync(string vehicleId, long cost)
        {
            var profile = await LoadProfileAsync();
            if (profile == null) return;

            if (profile.money < cost)
            {
                Debug.LogWarning("Not enough money to buy vehicle.");
                return;
            }

            if (!profile.ownedVehicles.Contains(vehicleId))
            {
                profile.ownedVehicles.Add(vehicleId);
            }

            profile.money -= cost;
            await SaveProfileAsync(profile);
        }
    }
}
