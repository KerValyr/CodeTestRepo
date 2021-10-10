using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Utilities;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    [Serializable]
    public class PlayerSave : ISerializationCallbackReceiver
    {
        [SerializeField] private List<string> scoresKeys;
        [SerializeField] private List<int> scoresValues;

        [SerializeField] private List<int> openLevelKeys;
        [SerializeField] private List<int> openLevelValues;

        [SerializeField] private int currentSetting;
        [SerializeField] private bool isPlayerVote;
        [SerializeField] private string version = Application.version;

        public Dictionary<string, int> Scores { get; private set; }
        public Dictionary<int, int> OpenLevels { get; private set; }

        public string Version
        {
            get { return version; }
        }

        public bool IsPlayerVote
        {
            get { return isPlayerVote; }
        }

        public int Setting
        {
            get { return currentSetting; }
        }

        public void ChangeScore(string levelName, int pointCount)
        {
            if (Scores.ContainsKey(levelName))
                Scores[levelName] = pointCount;
            else
                Scores.Add(levelName, pointCount);
        }

        public void SetSetting(int setting)
        {
            currentSetting = setting;
        }

        public void SetLastLevel(int setting, int level)
        {
            if (!OpenLevels.ContainsKey(setting))
                OpenLevels.Add(setting, level);
            else
                OpenLevels[setting] = level;
        }

        public KeyValuePair<int, int> GetLastLevel()
        {
            var maxSetting = OpenLevels.Max(l => l.Key);
            return new KeyValuePair<int, int>(maxSetting, OpenLevels[maxSetting]);
        }

        public void SetPlayerVote(bool isrVote)
        {
            isPlayerVote = isrVote;
        }

        public PlayerSave()
        {
            Scores = new Dictionary<string, int>();
            OpenLevels = new Dictionary<int, int>();
            OpenLevels.Add(0, 4);
            OpenLevels.Add(1, 1);
        }

        public PlayerSave(Dictionary<string, int> scores, Dictionary<int, int> openLavels)
        {
            this.Scores = scores;
            this.OpenLevels = openLavels;
        }

        public static PlayerSave CreateFromOld(string oldSaveText, string oldVesion)
        {
            return JsonUtility.FromJson<PlayerSave>(oldSaveText);
        }

        public void OnBeforeSerialize()
        {
            scoresKeys = new List<string>(Scores.Keys);
            scoresValues = new List<int>(Scores.Values);
            openLevelKeys = new List<int>(OpenLevels.Keys);
            openLevelValues = new List<int>(OpenLevels.Values);
        }

        public void OnAfterDeserialize()
        {
            var count = Math.Min(scoresKeys.Count, scoresValues.Count);
            Scores = new Dictionary<string, int>(count);
            for (var i = 0; i < count; ++i)
            {
                Scores.Add(scoresKeys[i], scoresValues[i]);
            }

            count = Math.Min(openLevelKeys.Count, openLevelValues.Count);
            OpenLevels = new Dictionary<int, int>(count);
            for (var i = 0; i < count; ++i)
            {
                OpenLevels.Add(openLevelKeys[i], openLevelValues[i]);
            }
        }

        #region OldSaves

        #endregion
    }
}