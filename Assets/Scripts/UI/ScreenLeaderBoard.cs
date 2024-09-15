using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeaderBoard;
using Utility;
using LootLocker.Requests;
namespace UnityEngine.UI
{

    
    public class ScreenLeaderBoard : ScreenBase
    {
        [SerializeField]
        LeaderBoardData data;
        [SerializeField]
        PlayerData playerDataPrefab;
        [SerializeField]
        List<LevelButtonLeaderboard> buttonLevel;
        [SerializeField]
        GameObject infoDataContainer;
        [SerializeField]
        PlayerData playerInfoLevel;

        private void Start()
        {
            foreach(LevelButtonLeaderboard button in buttonLevel)
            {
                button.OnButtonLoadLevel += PopulatePlayerData;
            }
        }
        public void printInfo()
        {
            Debug.Log(data.playerInfo["Level1"].score);
        }
        public void PopulatePlayerData(SceneName level)
        {
            // Clear existing player data objects in the container
            if (!data.leaderboardInfo.ContainsKey(level.ToString()))
            {
                Debug.LogWarning($"Level {level} does not exist in the leaderboard info.");
                return;
            }
            foreach (Transform child in infoDataContainer.transform)
            {
                Destroy(child.gameObject);
            }

            LootLockerLeaderboardMember[] playersData = data.leaderboardInfo[level.ToString()] ;

            if (playersData == null)
            {
                Debug.Log("no data for:" + level);
                return;
            }
            // Populate new player data
            foreach (var players in playersData)
            {
                // Instantiate a new PlayerData object
                PlayerData newPlayerData = Instantiate(playerDataPrefab, infoDataContainer.transform);

                // Set up the new PlayerData object with the player's information
                newPlayerData.setUpData(players.rank.ToString(), players.player.id.ToString(), players.score.ToString(), players.metadata);
               
            }
            var player = data.playerInfo[level.ToString()];
            playerInfoLevel.setUpData(player.rank.ToString(), player.player.id.ToString(), player.score.ToString(), player.metadata);
        }

    }
}
