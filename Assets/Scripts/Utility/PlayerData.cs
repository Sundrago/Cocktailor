using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Cocktailor
{
    /// <summary>
    /// Provides access to player-related data and functionalities.
    /// </summary>
    public static class PlayerData
    {
        private static Dictionary<int, UserCocktailData> userData = new();
        public static Action<int, MemorizedState> OnuserMemorizedStatehange;

        static PlayerData()
        {
            LoadData();
        }

        public static List<int> MemorizedRecipes { get; private set; }
        public static List<int> NotMemorizedRecipes { get; private set; }

        private static void UpdateMemorizedLists()
        {
            MemorizedRecipes = new List<int>();
            NotMemorizedRecipes = new List<int>();

            foreach (var data in userData)
                if (data.Value.UserState == MemorizedState.Yes)
                    MemorizedRecipes.Add(data.Key);
                else if (data.Value.UserState == MemorizedState.No) NotMemorizedRecipes.Add(data.Key);
        }

        private static UserCocktailData GetUserDataByIndex(int index)
        {
            if (!userData.ContainsKey(index)) userData[index] = UserCocktailData.CreateNewUser();

            return userData[index];
        }

        public static MemorizedState GetUserState(int index)
        {
            return GetUserDataByIndex(index).UserState;
        }

        public static string GetNote(int index)
        {
            return GetUserDataByIndex(index).UserNote;
        }

        public static void SetUserState(int index, MemorizedState state)
        {
            var userData = GetUserDataByIndex(index);
            userData.UserState = state;
            SaveData();
            UpdateMemorizedLists();
        }

        public static void SetNote(int index, string note)
        {
            userData[index].UserNote = note;
            SaveData();
        }

        private static void LoadData()
        {
            var dataInString = PlayerPrefs.GetString("UserData");
            userData = JsonConvert.DeserializeObject<Dictionary<int, UserCocktailData>>(dataInString);

            userData = userData ?? new Dictionary<int, UserCocktailData>();
            UpdateMemorizedLists();
        }

        private static void SaveData()
        {
            var dataInString = JsonConvert.SerializeObject(userData);
            PlayerPrefs.SetString("UserData", dataInString);
        }

        private static void UpdateAndSaveData(int index, UserCocktailData data)
        {
            userData[index] = data;
            SaveData();
        }

        public static bool HasSubscribed()
        {
            return PlayerPrefs.GetInt("subscribed") == 1;
        }

        public static void UpdateUserState(int index, bool isOMarker)
        {
            var currentState = GetUserState(index);
            var newState = isOMarker ? MemorizedState.Yes : MemorizedState.No;
            if (currentState == newState) newState = MemorizedState.Undefined;

            SetUserState(index, newState);
            OnuserMemorizedStatehange?.Invoke(index, newState);
        }
    }

    public enum MemorizedState
    {
        Undefined,
        Yes,
        No
    }

    [Serializable]
    public class UserCocktailData
    {
        public MemorizedState UserState { get; set; }
        public string UserNote { get; set; }

        public static UserCocktailData CreateNewUser()
        {
            return new UserCocktailData
            {
                UserState = MemorizedState.Undefined,
                UserNote = string.Empty
            };
        }
    }
}