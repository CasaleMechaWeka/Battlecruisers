using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Tasks
{
    public interface IPvPTask
    {
        event EventHandler Completed;

        /// <returns>
        /// True if the task was started successfully, false if the task can
        /// no longer run (ie, is insta-completed).  For example, for a 
        /// ConstructBuildingTask this may be because there are no slots available
        /// to constructo the given building.
        /// </returns>
        bool Start();

        // Currently Stop() and Resume() are not implemented anywhere, but could
        // come in handy if an AI task needs to perform some action to stop :)
        void Stop();
        void Resume();
    }
}
