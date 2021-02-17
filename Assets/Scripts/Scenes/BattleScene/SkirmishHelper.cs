using BattleCruisers.AI;
using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Threading;
using System;

namespace BattleCruisers.Scenes.BattleScene
{
    public class SkirmishHelper : NormalHelper
    {
        public SkirmishHelper(IApplicationModel appModel, IPrefabFactory prefabFactory, IDeferrer deferrer) 
            : base(appModel, prefabFactory, deferrer)
        {
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