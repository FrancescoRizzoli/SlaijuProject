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
            Debug.Log("leaderboard button" + level);
            foreach (Transform child in infoDataContainer.transform)
            {
                Destroy(child.gameObject);
            }
            // Clear existing player data objects in the container
            if (!data.leaderboardInfo.ContainsKey(level.ToString()))
            {
                Debug.LogWarning($"Level {level} does not exist in the leaderboard info.");
                return;
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
                newPlayerData.setUpData(players.rank.ToString(), players.player.id.ToString(), Commons.LeaderboardValueOut(players.score).ToString(), FormatStringToThreeDecimals(players.metadata));
               
            }
            var player = data.playerInfo[level.ToString()];
            Debug.Log(player.rank);
            if(player.metadata != null)
                playerInfoLevel.setUpData(player.rank.ToString(), player.player.id.ToString(), Commons.LeaderboardValueOut(player.score).ToString(), FormatStringToThreeDecimals(player.metadata));
            else
                playerInfoLevel.setUpData("0","0","0","0");
        }
        public string FormatStringToThreeDecimals(string input)
        {
           
            if (float.TryParse(input, out float number))
            {

                Debug.Log(number.ToString("F2"));
                return number.ToString("F2");
            }
            else
            {
                
                return "Invalid input: not a valid number";
            }
        }

    }
}
