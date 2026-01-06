using BattleCruisers.Scenes.BattleScene;
using UnityEngine;



public class ChainBattleController : MonoBehaviour
{
    public static BattleSceneGod BattleSceneGod;

    void Start()
    {
        BattleSceneGod = GetComponentInParent<BattleSceneGod>();
    }

    void Init(BattleSceneGod battleSceneGod)
    {
        BattleSceneGod = battleSceneGod;
    }

    public static void ApplyDamageToEnemy(float amount)
    {
        BattleSceneGod.aiCruiser.TakeDamage(amount, BattleSceneGod.playerCruiser);
    }

    public static void MakeAICruiserInvincible()
    {
        BattleSceneGod.aiCruiser.MakeInvincible();
        Debug.Log("Set enemy cruiser invincible");
    }

    public static void MakeAICruiserDamagable()
    {
        BattleSceneGod.aiCruiser.MakeDamagable();
        Debug.Log("Set enemy cruiser damagable");
    }
}