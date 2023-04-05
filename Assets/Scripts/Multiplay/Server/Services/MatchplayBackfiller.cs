using System;
using System.Linq;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Matchmaker;
using Unity.Services.Matchmaker.Models;
using System.Threading.Tasks;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;



namespace BattleCruisers.Network.Multiplay.Matchplay.Server
{
    public class MatchplayBackfiller : IDisposable
    {
        public bool Backfilling { get; private set; } = false;

        CreateBackfillTicketOptions m_CreateBackfillOptions;
        BackfillTicket m_LocalBackfillTicket;
        bool m_LocalDataDirty = false;
        const int k_TicketCheckMs = 1000;
        int m_MaxPlayers;
        int MatchPlayerCount => m_LocalBackfillTicket?.Properties.MatchProperties.Players.Count ?? 0;

        public MatchplayBackfiller(string connection, string queueName, MatchProperties matchmakerPayloadProperties, int maxPlayers)
        {
            m_MaxPlayers = maxPlayers;
            var backfillProperties = new BackfillTicketProperties(matchmakerPayloadProperties);
            m_LocalBackfillTicket = new BackfillTicket
            {
                Id = matchmakerPayloadProperties.BackfillTicketId,
                Properties = backfillProperties
            };
            m_CreateBackfillOptions = new CreateBackfillTicketOptions
            {
                Connection = connection,
                QueueName = queueName,
                Properties = backfillProperties
            };

        }

        public async Task BeginBackfillng()
        {
            if (Backfilling)
            {
                Debug.Log("Already backfilling, no need to start another.");
                return;
            }
            Debug.Log($"Starting backfill Server: {MatchPlayerCount}/{m_MaxPlayers}");

            if (string.IsNullOrEmpty(m_LocalBackfillTicket.Id))
            {
                m_LocalBackfillTicket.Id = await MatchmakerService.Instance.CreateBackfillTicketAsync(m_CreateBackfillOptions);
            }
            Backfilling = true;

#pragma warning disable 4014
            BackfillLoop();
#pragma warning restore 4014
        }

        public void AddPlayerToMatch(UserData userData)
        {
            if (!Backfilling)
            {
                Debug.LogWarning("Can't add users to the backfill ticket before it's been created");
                return;
            }

            if (GetPlayerById(userData.userAuthId) != null)
            {
                Debug.LogWarning($"User: {userData.userName} - {userData.userAuthId} already in Match. Igoring add.");
                return;
            }

            var matchmakerPlayer = new Player(userData.userAuthId, userData.userGamePreferences);

            m_LocalBackfillTicket.Properties.MatchProperties.Players.Add(matchmakerPlayer);
            m_LocalBackfillTicket.Properties.MatchProperties.Teams[0].PlayerIds.Add(matchmakerPlayer.Id);
            m_LocalDataDirty = true;

        }

        public int RemovePlayerFormMatch(string userID)
        {
            var playerToRemove = GetPlayerById(userID);
            if (playerToRemove == null)
            {
                Debug.LogWarning($"No user by the ID: {userID} in local backfill Data.");
                return MatchPlayerCount;
            }

            m_LocalBackfillTicket.Properties.MatchProperties.Players.Remove(playerToRemove);

            m_LocalBackfillTicket.Properties.MatchProperties.Teams[0].PlayerIds.Remove(userID);
            m_LocalDataDirty = true;
            return MatchPlayerCount;
        }

        public async Task StopBackfill()
        {
            if (!Backfilling)
            {
                Debug.LogError("Can't stop backfilling before we start.");
                return;
            }
            await MatchmakerService.Instance.DeleteBackfillTicketAsync(m_LocalBackfillTicket.Id);
            Backfilling = false;
            m_LocalBackfillTicket.Id = null;
        }

        public bool NeedsPlayers()
        {
            return MatchPlayerCount < m_MaxPlayers;

        }

        Player GetPlayerById(string userID)
        {
            return m_LocalBackfillTicket.Properties.MatchProperties.Players.FirstOrDefault(p => p.Id.Equals(userID));
        }


        async Task BackfillLoop()
        {
            while (Backfilling)
            {
                if (m_LocalDataDirty)
                {
                    await MatchmakerService.Instance.UpdateBackfillTicketAsync(m_LocalBackfillTicket.Id, m_LocalBackfillTicket);
                    m_LocalDataDirty = false;
                }
                else
                {
                    m_LocalBackfillTicket = await MatchmakerService.Instance.ApproveBackfillTicketAsync(m_LocalBackfillTicket.Id);

                }

                if (!NeedsPlayers())
                {
                    await StopBackfill();
                    break;
                }

                await Task.Delay(k_TicketCheckMs);
            }
        }


        public void Dispose()
        {
#pragma warning disable 4014
            StopBackfill();
#pragma warning restore 4014
        }
    }

}

