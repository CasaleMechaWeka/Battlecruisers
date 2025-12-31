using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.DeploymentApi.Editor;
using Unity.Services.Leaderboards.Authoring.Core.Model;

namespace Unity.Services.Leaderboards.Editor.Authoring.Model
{
    class EditorLeaderboardConfig : LeaderboardConfig
    {

        [JsonProperty("$schema")]
        public string Value = "https://ugs-config-schemas.unity3d.com/v1/leaderboards.schema.json";

        public EditorLeaderboardConfig(string name) : base(name) {}

        internal void ClearOwnedStates()
        {
            var i = 0;
            while (i < States.Count)
            {
                if (LeaderboardsSerializer.IsDeserializationError(States[i].Description))
                {
                    States.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }
    }
}
