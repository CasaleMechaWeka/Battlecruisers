using BattleCruisers.AI;
using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Threading;
using System;

namespace BattleCruisers.Scenes.BattleScene
{
    public class SkirmishHelper : NormalHelper
    {
        private readonly IRandomGenerator _random;

        public SkirmishHelper(IApplicationModel appModel, IPrefabFactory prefabFactory, IDeferrer deferrer) 
            : base(appModel, prefabFactory, deferrer)
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
    }
}