using System;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using UnityEngine;

namespace Drive56.Firebase
{
    public static class FirebaseBootstrap
    {
        public static FirebaseApp App { get; private set; }
        public static FirebaseAuth Auth { get; private set; }
        public static bool IsInitialized { get; private set; }

        public static async Task InitializeAsync()
        {
            if (IsInitialized) return;

            try
            {
                FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.LogError($"Firebase dependency resolution failed: {task.Exception}");
                        return;
                    }

                    App = FirebaseApp.DefaultInstance;
                    Auth = FirebaseAuth.DefaultInstance;
                    Auth.StateChanged += HandleAuthStateChanged;
                    IsInitialized = true;

                    if (Auth.CurrentUser != null)
                    {
                        Debug.Log($"Firebase Auth already signed in as: {Auth.CurrentUser.UserId}");
                    }
                });

                await Task.Yield();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Firebase bootstrap failed: {ex}");
            }
        }

        private static void HandleAuthStateChanged(object sender, EventArgs e)
        {
            var auth = sender as FirebaseAuth;
            if (auth == null) return;

            var user = auth.CurrentUser;
            if (user != null)
            {
                Debug.Log($"Firebase user state changed: {user.UserId}");
            }
            else
            {
                Debug.Log("Firebase user signed out.");
            }
        }
    }
}
