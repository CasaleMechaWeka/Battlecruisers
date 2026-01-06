using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Pools;
using System.IO;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene
{
    /// <summary>
    /// Monitors performance metrics during battle scene to help identify memory leaks,
    /// garbage collection issues, and object pooling problems.
    /// </summary>
    public class PvPPerformanceMonitor : MonoBehaviour
    {
        [SerializeField] private bool enableMonitoring = true;
        [SerializeField] private float logIntervalSeconds = 10f;
        [SerializeField] private bool logToFile = true;
        [SerializeField] private bool logToConsole = true;

        private float _lastLogTime;
        private float _battleStartTime;
        private int _frameCount;
        private float _totalFrameTime;
        private float _lastGCCollectionTime;
        private int _gcCollectionCount;
        private long _lastGCAllocatedMemory;
        private long _lastGCTotalMemory;

        // Unit tracking
        private int _totalUnitsCreated;
        private int _totalUnitsDestroyed;
        private int _peakActiveUnits;
        private Dictionary<string, int> _unitTypeCounts = new Dictionary<string, int>();

        // Memory tracking
        private long _peakMemoryMB;
        private long _lastMemoryMB;
        private int _gcCollectionsSinceStart;

        // Pool tracking (if accessible)
        private PvPUnitPoolProvider _unitPoolProvider;

        // Cruiser references for unit monitoring
        private PvPCruiser _playerCruiser;
        private PvPCruiser _enemyCruiser;

        private string _logFilePath;
        private StreamWriter _logWriter;

        private void Start()
        {
            if (!enableMonitoring)
                return;

            _battleStartTime = Time.time;
            _lastLogTime = Time.time;
            _lastGCCollectionTime = Time.time;
            _lastGCAllocatedMemory = GC.GetTotalMemory(false);
            _lastGCTotalMemory = GC.GetTotalMemory(true);

            // Setup log file
            if (logToFile)
            {
                SetupLogFile();
            }

            LogInitialState();

            // Try to get references - delay to allow scene to initialize
            StartCoroutine(DelayedReferenceSetup());
        }

        private System.Collections.IEnumerator DelayedReferenceSetup()
        {
            // Wait a bit for scene to initialize
            yield return new WaitForSeconds(1f);
            
            // Keep trying until we get references
            int attempts = 0;
            while (attempts < 10)
            {
                TryGetReferences();
                
                // If we got both cruisers, we're done
                if (_playerCruiser != null && _enemyCruiser != null)
                    break;
                
                attempts++;
                yield return new WaitForSeconds(0.5f);
            }
        }

        private void TryGetReferences()
        {
            // Try to get cruiser references
            var battleSceneGod = PvPBattleSceneGodClient.Instance;
            if (battleSceneGod != null)
            {
                _playerCruiser = battleSceneGod.playerCruiser;
                _enemyCruiser = battleSceneGod.enemyCruiser;

                // Hook into unit monitor events
                if (_playerCruiser != null && _playerCruiser.UnitMonitor != null)
                {
                    _playerCruiser.UnitMonitor.UnitStarted += OnUnitStarted;
                    _playerCruiser.UnitMonitor.UnitDestroyed += OnUnitDestroyedEvent;
                }

                if (_enemyCruiser != null && _enemyCruiser.UnitMonitor != null)
                {
                    _enemyCruiser.UnitMonitor.UnitStarted += OnUnitStarted;
                    _enemyCruiser.UnitMonitor.UnitDestroyed += OnUnitDestroyedEvent;
                }
            }

            // Try to get pool provider
            if (PvPFactoryProvider.PoolProviders != null)
            {
                _unitPoolProvider = PvPFactoryProvider.PoolProviders.UnitPoolProvider;
            }
        }

        private void OnUnitStarted(object sender, PvPUnitStartedEventArgs e)
        {
            OnUnitCreated(e.StartedUnit);
        }

        private void OnUnitDestroyedEvent(object sender, PvPUnitDestroyedEventArgs e)
        {
            OnUnitDestroyed(e.DestroyedUnit);
        }

        private void SetupLogFile()
        {
            #if UNITY_EDITOR
            string projectRoot = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
            _logFilePath = Path.Combine(projectRoot, "PvPPerformanceLog.log");
            #else
            _logFilePath = Path.Combine(Application.persistentDataPath, "PvPPerformanceLog.log");
            #endif

            try
            {
                _logWriter = new StreamWriter(_logFilePath, false);
                _logWriter.AutoFlush = true;
                string header = $"=== PvP BATTLE PERFORMANCE LOG - Started {DateTime.Now:yyyy-MM-dd HH:mm:ss} ===\n";
                header += $"Unity Version: {Application.unityVersion}\n";
                header += $"Platform: {Application.platform}\n";
                header += "==========================================\n\n";
                _logWriter.Write(header);
            }
            catch (Exception e)
            {
                Debug.LogError($"PvPPerformanceMonitor: Failed to create log file: {e}");
                logToFile = false;
            }
        }

        private void LogInitialState()
        {
            string message = "=== PERFORMANCE MONITORING STARTED ===\n";
            message += $"Initial Memory: {GC.GetTotalMemory(false) / (1024 * 1024)} MB\n";
            message += $"Initial GC Count: {GC.CollectionCount(0)} (Gen0), {GC.CollectionCount(1)} (Gen1), {GC.CollectionCount(2)} (Gen2)\n";
            message += "==========================================\n\n";

            if (logToConsole)
                Debug.Log(message);
            if (logToFile && _logWriter != null)
                _logWriter.Write(message);
        }

        private void Update()
        {
            if (!enableMonitoring)
                return;

            _frameCount++;
            _totalFrameTime += Time.deltaTime;

            // Check for GC collections
            CheckGarbageCollection();

            // Periodic logging
            if (Time.time - _lastLogTime >= logIntervalSeconds)
            {
                LogPerformanceMetrics();
                _lastLogTime = Time.time;
            }
        }

        private void CheckGarbageCollection()
        {
            int currentGen0 = GC.CollectionCount(0);
            int currentGen1 = GC.CollectionCount(1);
            int currentGen2 = GC.CollectionCount(2);

            // If GC counts increased, log it
            if (currentGen0 > _gcCollectionsSinceStart || currentGen1 > _gcCollectionsSinceStart || currentGen2 > _gcCollectionsSinceStart)
            {
                _gcCollectionCount++;
                float timeSinceLastGC = Time.time - _lastGCCollectionTime;
                _lastGCCollectionTime = Time.time;

                long currentMemory = GC.GetTotalMemory(false);
                long memoryDelta = currentMemory - _lastGCAllocatedMemory;

                string gcMessage = $"[GC EVENT] Time: {Time.time - _battleStartTime:F1}s | " +
                                   $"Gen0: {currentGen0} | Gen1: {currentGen1} | Gen2: {currentGen2} | " +
                                   $"Time since last GC: {timeSinceLastGC:F2}s | " +
                                   $"Memory before: {_lastGCAllocatedMemory / (1024 * 1024)} MB | " +
                                   $"Memory after: {currentMemory / (1024 * 1024)} MB | " +
                                   $"Delta: {memoryDelta / (1024 * 1024)} MB";

                if (logToConsole)
                    Debug.Log(gcMessage);
                if (logToFile && _logWriter != null)
                    _logWriter.WriteLine(gcMessage);

                _lastGCAllocatedMemory = currentMemory;
                _lastGCTotalMemory = GC.GetTotalMemory(true);
            }
        }

        private void LogPerformanceMetrics()
        {
            float battleTime = Time.time - _battleStartTime;
            float avgFrameTime = _totalFrameTime / _frameCount;
            float avgFPS = 1f / avgFrameTime;

            // Get current memory stats
            long currentAllocatedMemory = GC.GetTotalMemory(false);
            long currentTotalMemory = GC.GetTotalMemory(true);
            long memoryDelta = currentAllocatedMemory - _lastMemoryMB;

            if (currentAllocatedMemory > _peakMemoryMB)
                _peakMemoryMB = currentAllocatedMemory;

            // Get unit counts
            int activeUnits = GetActiveUnitCount();
            if (activeUnits > _peakActiveUnits)
                _peakActiveUnits = activeUnits;

            // Get GC stats
            int gen0Collections = GC.CollectionCount(0);
            int gen1Collections = GC.CollectionCount(1);
            int gen2Collections = GC.CollectionCount(2);

            // Build log message
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"=== PERFORMANCE METRICS - Battle Time: {battleTime:F1}s ===");
            sb.AppendLine($"Frame Stats: Count={_frameCount} | Avg Frame Time={avgFrameTime * 1000:F2}ms | Avg FPS={avgFPS:F1}");
            sb.AppendLine($"Memory: Allocated={currentAllocatedMemory / (1024 * 1024)} MB | Total={currentTotalMemory / (1024 * 1024)} MB | Peak={_peakMemoryMB / (1024 * 1024)} MB | Delta={memoryDelta / (1024 * 1024)} MB");
            sb.AppendLine($"GC Collections: Gen0={gen0Collections} | Gen1={gen1Collections} | Gen2={gen2Collections} | Total Events={_gcCollectionCount}");
            sb.AppendLine($"Units: Active={activeUnits} | Peak={_peakActiveUnits} | Created={_totalUnitsCreated} | Destroyed={_totalUnitsDestroyed} | Net={_totalUnitsCreated - _totalUnitsDestroyed}");

            // Unit type breakdown
            if (_unitTypeCounts.Count > 0)
            {
                sb.AppendLine("Unit Type Breakdown:");
                foreach (var kvp in _unitTypeCounts.OrderByDescending(x => x.Value))
                {
                    sb.AppendLine($"  {kvp.Key}: {kvp.Value}");
                }
            }

            // Pool stats (if we can access them)
            string poolStats = GetPoolStats();
            if (!string.IsNullOrEmpty(poolStats))
            {
                sb.AppendLine("Pool Stats:");
                sb.AppendLine(poolStats);
            }

            sb.AppendLine("==========================================");
            sb.AppendLine();

            string message = sb.ToString();

            if (logToConsole)
                Debug.Log(message);
            if (logToFile && _logWriter != null)
                _logWriter.Write(message);

            // Reset frame tracking
            _frameCount = 0;
            _totalFrameTime = 0f;
            _lastMemoryMB = currentAllocatedMemory;
        }

        private int GetActiveUnitCount()
        {
            int count = 0;

            try
            {
                if (_playerCruiser != null && _playerCruiser.UnitMonitor != null)
                {
                    count += _playerCruiser.UnitMonitor.AliveUnits.Count;
                }

                if (_enemyCruiser != null && _enemyCruiser.UnitMonitor != null)
                {
                    count += _enemyCruiser.UnitMonitor.AliveUnits.Count;
                }
            }
            catch (Exception)
            {
                // Silently handle - might not be initialized yet
            }

            return count;
        }

        private string GetPoolStats()
        {
            if (_unitPoolProvider == null)
                return "";

            StringBuilder sb = new StringBuilder();
            try
            {
                // Get pool sizes using reflection or direct access
                var pools = new Dictionary<string, Pool<PvPUnit, PvPBuildableActivationArgs>>
                {
                    { "Bomber", _unitPoolProvider.BomberPool },
                    { "Fighter", _unitPoolProvider.FighterPool },
                    { "Gunship", _unitPoolProvider.GunshipPool },
                    { "SteamCopter", _unitPoolProvider.SteamCopterPool },
                    { "Broadsword", _unitPoolProvider.BroadswordPool },
                    { "StratBomber", _unitPoolProvider.StratBomberPool },
                    { "SpyPlane", _unitPoolProvider.SpyPlanePool },
                    { "MissileFighter", _unitPoolProvider.MissileFighterPool },
                    { "TestAircraft", _unitPoolProvider.TestAircraftPool },
                    { "AttackBoat", _unitPoolProvider.AttackBoatPool },
                    { "AttackRIB", _unitPoolProvider.AttackRIBPool },
                    { "Frigate", _unitPoolProvider.FrigatePool },
                    { "Destroyer", _unitPoolProvider.DestroyerPool },
                    { "SiegeDestroyer", _unitPoolProvider.SiegeDestroyerPool },
                    { "Archon", _unitPoolProvider.ArchonPool },
                    { "GlassCannoneer", _unitPoolProvider.GlassCannoneerPool },
                    { "GunBoat", _unitPoolProvider.GunBoatPool },
                    { "RocketTurtle", _unitPoolProvider.RocketTurtlePool },
                    { "FlakTurtle", _unitPoolProvider.FlakTurtlePool }
                };

                foreach (var kvp in pools)
                {
                    if (kvp.Value != null)
                    {
                        int poolSize = kvp.Value.GetPoolSize();
                        int totalCreated = kvp.Value.GetTotalCreated();
                        if (poolSize > 0 || totalCreated > 0)
                        {
                            sb.AppendLine($"  {kvp.Key}: Pool={poolSize} | Created={totalCreated}");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                sb.AppendLine($"  Error getting pool stats: {e.Message}");
            }

            return sb.ToString();
        }

        // Public methods to track unit creation/destruction
        public void OnUnitCreated(IPvPUnit unit)
        {
            if (!enableMonitoring)
                return;

            _totalUnitsCreated++;
            string unitType = unit.GetType().Name;
            if (!_unitTypeCounts.ContainsKey(unitType))
                _unitTypeCounts[unitType] = 0;
            _unitTypeCounts[unitType]++;
        }

        public void OnUnitDestroyed(IPvPUnit unit)
        {
            if (!enableMonitoring)
                return;

            _totalUnitsDestroyed++;
            string unitType = unit.GetType().Name;
            if (_unitTypeCounts.ContainsKey(unitType) && _unitTypeCounts[unitType] > 0)
                _unitTypeCounts[unitType]--;
        }

        private void OnDestroy()
        {
            // Unhook from events
            if (_playerCruiser != null && _playerCruiser.UnitMonitor != null)
            {
                _playerCruiser.UnitMonitor.UnitStarted -= OnUnitStarted;
                _playerCruiser.UnitMonitor.UnitDestroyed -= OnUnitDestroyedEvent;
            }

            if (_enemyCruiser != null && _enemyCruiser.UnitMonitor != null)
            {
                _enemyCruiser.UnitMonitor.UnitStarted -= OnUnitStarted;
                _enemyCruiser.UnitMonitor.UnitDestroyed -= OnUnitDestroyedEvent;
            }

            if (_logWriter != null)
            {
                try
                {
                    float totalBattleTime = Time.time - _battleStartTime;
                    string footer = $"\n=== PERFORMANCE MONITORING ENDED - Total Battle Time: {totalBattleTime:F1}s ===\n";
                    footer += $"Final Memory: {GC.GetTotalMemory(false) / (1024 * 1024)} MB\n";
                    footer += $"Peak Memory: {_peakMemoryMB / (1024 * 1024)} MB\n";
                    footer += $"Total Units Created: {_totalUnitsCreated}\n";
                    footer += $"Total Units Destroyed: {_totalUnitsDestroyed}\n";
                    footer += $"Peak Active Units: {_peakActiveUnits}\n";
                    footer += $"GC Collections: {GC.CollectionCount(0)} (Gen0), {GC.CollectionCount(1)} (Gen1), {GC.CollectionCount(2)} (Gen2)\n";
                    footer += $"=== Log Ended {DateTime.Now:yyyy-MM-dd HH:mm:ss} ===\n";
                    _logWriter.Write(footer);
                    _logWriter.Close();
                    _logWriter = null;
                }
                catch { }
            }
        }

        private void OnApplicationQuit()
        {
            OnDestroy();
        }
    }
}
