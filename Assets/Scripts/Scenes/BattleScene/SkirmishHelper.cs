using BattleCruisers.AI;
using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Threading;
using System;
using System.Threading.Tasks;

namespace BattleCruisers.Scenes.BattleScene
{
    public class SkirmishHelper : NormalHelper
    {
        private readonly IRandomGenerator _random;

        public SkirmishHelper(
            IApplicationModel appModel,
            IPrefabFetcher prefabFetcher,
            IPrefabFactory prefabFactory, 
            IDeferrer deferrer) 
            : base(appModel, prefabFetcher, prefabFactory, deferrer)
        {
            _random = RandomGenerator.Instance;
        }

        public override ILevel GetLevel()
        {
            // FELIX
            throw new NotImplementedException();
        }

        public override IArtificialIntelligence CreateAI(ICruiserController aiCruiser, ICruiserController playerCruiser, int currentLevelNum)
        {
            // FELIX
            throw new NotImplementedException();
        }

        public override Task<string> GetEnemyNameAsync(int levelNum)
        {
            return Task.FromResult("SKIRMISH");
        }
    }
}