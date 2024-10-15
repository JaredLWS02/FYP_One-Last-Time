using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Current;

    void Awake()
    {
        if(!Current) Current=this;
    }

    ////////////////////////////////////////////////////////////////////////////////////

    void OnEnable()
    {
        // EventManager.Current.SpawnEvent += OnSpawn;
        // EventManager.Current.IdleVoiceEvent += OnIdleVoice;
        // EventManager.Current.HurtEvent += OnHurt;
        // EventManager.Current.DeathEvent += OnDeath;
        // EventManager.Current.LootEvent += OnLoot;
        // EventManager.Current.AddBuffEvent += OnAddBuff;
        // EventManager.Current.EnderPearlEvent += OnEnderPearl;
        // EventManager.Current.MaceSlamEvent += OnMaceSlam;
        // EventManager.Current.UpdateCraftEvent += OnUpdateCraft;
        // EventManager.Current.UpdateNotCraftEvent += OnUpdateNotCraft;
        // EventManager.Current.CraftedEvent += OnCrafted;
    }
    void OnDisable()
    {
        // EventManager.Current.SpawnEvent -= OnSpawn;
        // EventManager.Current.IdleVoiceEvent -= OnIdleVoice;
        // EventManager.Current.HurtEvent -= OnHurt;
        // EventManager.Current.DeathEvent -= OnDeath;
        // EventManager.Current.LootEvent -= OnLoot;
        // EventManager.Current.AddBuffEvent -= OnAddBuff;
        // EventManager.Current.EnderPearlEvent -= OnEnderPearl;
        // EventManager.Current.MaceSlamEvent -= OnMaceSlam;
        // EventManager.Current.UpdateCraftEvent -= OnUpdateCraft;
        // EventManager.Current.UpdateNotCraftEvent -= OnUpdateNotCraft;
        // EventManager.Current.CraftedEvent -= OnCrafted;
    }

    ////////////////////////////////////////////////////////////////////////////////////

    // [Header("Chivalry")]
    // public AudioClip[] sfxFireIgnite;
    // public AudioClip[] sfxFireLoop;
    // public AudioClip[] sfxHitStone;
    // public AudioClip[] sfxHitWood;

    // [Header("Grand Fantasia")]
    // public AudioClip[] sfxCraftFinish;
    // public AudioClip[] sfxCraftLoop;
    // public AudioClip[] sfxMaceCharge;
    // public AudioClip[] sfxUIBook;

    // [Header("Luxor")]
    // public AudioClip[] sfxBreakDiamond;

    // [Header("Minecraft")]
    // public AudioClip[] sfxBowDraw;
    // public AudioClip[] sfxBowShoot;
    // public AudioClip[] sfxFireIgnite2;
    // public AudioClip[] sfxFstNpc;
    // public AudioClip[] sfxFstSkeleton;
    // public AudioClip[] sfxFstSpider;
    // public AudioClip[] sfxFstZombie;
    // public AudioClip[] sfxHitArrow;
    // public AudioClip[] sfxHitFire;
    // public AudioClip[] sfxHitFist;
    // public AudioClip[] sfxHitNpc;
    // public AudioClip[] sfxHitStone2;
    // public AudioClip[] sfxHitSword;
    // public AudioClip[] sfxHitTool;
    // public AudioClip[] sfxHitWood2;
    // public AudioClip[] sfxLoot;
    // public AudioClip[] sfxLootDrink;
    // public AudioClip[] sfxLootFood;
    // public AudioClip[] sfxPearl;
    // public AudioClip[] sfxSpawnEnemy;
    // public AudioClip[] sfxSwingFist;
    // public AudioClip[] sfxSwingTool;
    // public AudioClip[] sfxThrow;
    // public AudioClip[] sfxUIBtnClick;
    // public AudioClip[] sfxUIBtnHover;
    // public AudioClip[] sfxUIInvClose;
    // public AudioClip[] sfxUIInvOpen;
    // public AudioClip[] sfxUITween;

    // public AudioClip[] voiceSkeletonDeath;
    // public AudioClip[] voiceSkeletonHurt;
    // public AudioClip[] voiceSkeletonIdle;
    // public AudioClip[] voiceSpiderDeath;
    // public AudioClip[] voiceSpiderSay;
    // public AudioClip[] voiceSteveHurt;
    // public AudioClip[] voiceZombieDeath;
    // public AudioClip[] voiceZombieHurt;
    // public AudioClip[] voiceZombieIdle;

    // [Header("PvZ")]
    // public AudioClip[] sfxLootDiamondBlock;
    // public AudioClip[] sfxLootSpeed;

    // [Header("Skyrim")]
    // public AudioClip[] sfxMaceSlam;
    // public AudioClip[] sfxMaceSlam2;
    // public AudioClip[] sfxUICraft;

    // [Header("Stardew")]
    // public AudioClip[] sfxBreakStone;
    // public AudioClip[] sfxBreakWood;
    // public AudioClip[] sfxFurnace;

    // [Header("Terraria")]
    // public AudioClip[] sfxDeathEnemy;
    // public AudioClip[] sfxDeathNpc;
    // public AudioClip[] sfxHitFlesh;
    // public AudioClip[] voiceAlexHurt;

    Dictionary<GameObject, AudioSource> soundLoops = new();

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // void OnSpawn(GameObject spawned)
    // {
    //     if(spawned.TryGetComponent(out Enemy enemy))
    //     {
    //         if(Random.Range(0,5)==0)
    //         AudioManager.Current.PlaySFX(sfxSpawnEnemy, spawned.transform.position);
    //     }
    // }

    // void OnIdleVoice(GameObject subject)
    // {
    //     if(subject.TryGetComponent(out Enemy enemy))
    //     {
    //         switch(enemy.enemyName)
    //         {
    //             case EnemyName.Zombie: AudioManager.Current.PlayVoice(enemy.voicebox, voiceZombieIdle); break;
    //             case EnemyName.Spider: AudioManager.Current.PlayVoice(enemy.voicebox, voiceSpiderSay); break;
    //             case EnemyName.Skeleton: AudioManager.Current.PlayVoice(enemy.voicebox, voiceSkeletonIdle); break;
    //         }
    //     }
    // }

    // void OnHurt(GameObject victim, GameObject attacker, HurtInfo hurtInfo)
    // {
    //     if(victim.TryGetComponent(out Steve steve))
    //     {
    //         AudioManager.Current.PlaySFX(sfxHitNpc, victim.transform.position);

    //         switch(steve.npcName)
    //         {
    //             case NPCName.Steve: AudioManager.Current.PlayVoice(steve.voicebox, voiceSteveHurt); break;
    //             case NPCName.Alex: AudioManager.Current.PlayVoice(steve.voicebox, voiceAlexHurt); break;
    //         }
    //     }

    //     else if(victim.TryGetComponent(out Enemy enemy))
    //     {
    //         if(enemy.enemyName!=EnemyName.Skeleton) AudioManager.Current.PlaySFX(sfxHitFlesh, victim.transform.position);

    //         switch(enemy.enemyName)
    //         {
    //             case EnemyName.Zombie: AudioManager.Current.PlayVoice(enemy.voicebox, voiceZombieHurt); break;
    //             case EnemyName.Spider: AudioManager.Current.PlayVoice(enemy.voicebox, voiceSpiderSay); break;
    //             case EnemyName.Skeleton: AudioManager.Current.PlayVoice(enemy.voicebox, voiceSkeletonHurt); break;
    //         }
    //     }

    //     else if(victim.TryGetComponent(out Resource resource))
    //     {
    //         switch(resource.type)
    //         {
    //             case Item.WoodLog:
    //             {
    //                 AudioManager.Current.PlaySFX(sfxHitWood, victim.transform.position);
    //                 AudioManager.Current.PlaySFX(sfxHitWood2, victim.transform.position);
    //             } break;

    //             case Item.Stone:
    //             {
    //                 AudioManager.Current.PlaySFX(sfxHitStone, victim.transform.position);
    //                 AudioManager.Current.PlaySFX(sfxHitStone2, victim.transform.position);
    //             } break;

    //             case Item.CoalOre: AudioManager.Current.PlaySFX(sfxHitStone, victim.transform.position); break;
    //             case Item.IronOre: AudioManager.Current.PlaySFX(sfxHitStone, victim.transform.position); break;
    //             case Item.Diamond: AudioManager.Current.PlaySFX(sfxHitStone, victim.transform.position); break;
    //         }
    //     }
    // }

    // void OnDeath(GameObject victim, GameObject killer, HurtInfo hurtInfo)
    // {
    //     if(victim.TryGetComponent(out Steve steve))
    //     {
    //         AudioManager.Current.PlaySFX(sfxDeathNpc, victim.transform.position, false);
    //     }

    //     else if(victim.TryGetComponent(out Enemy enemy))
    //     {
    //         if(enemy.enemyName!=EnemyName.Skeleton) AudioManager.Current.PlaySFX(sfxDeathEnemy, victim.transform.position);

    //         switch(enemy.enemyName)
    //         {
    //             case EnemyName.Zombie: AudioManager.Current.PlaySFX(voiceZombieDeath, victim.transform.position); break;
    //             case EnemyName.Spider: AudioManager.Current.PlaySFX(voiceSpiderDeath, victim.transform.position); break;
    //             case EnemyName.Skeleton: AudioManager.Current.PlaySFX(voiceSkeletonDeath, victim.transform.position); break;
    //         }
    //     }

    //     else if(victim.TryGetComponent(out Resource resource))
    //     {
    //         switch(resource.type)
    //         {
    //             case Item.WoodLog: AudioManager.Current.PlaySFX(sfxBreakWood, victim.transform.position); break;
    //             case Item.Stone: AudioManager.Current.PlaySFX(sfxBreakStone, victim.transform.position); break;
    //             case Item.CoalOre: AudioManager.Current.PlaySFX(sfxBreakStone, victim.transform.position); break;
    //             case Item.IronOre: AudioManager.Current.PlaySFX(sfxBreakStone, victim.transform.position); break;
    //             case Item.Diamond: AudioManager.Current.PlaySFX(sfxBreakDiamond, victim.transform.position); break;
    //         }
    //     }
    // }    

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // public void OnLoot(GameObject looter, GameObject loot, LootInfo lootInfo)
    // {
    //     AudioManager.Current.PlaySFX(sfxLoot, looter.transform.position);

    //     if(lootInfo.item==Item.DiamondBlock)
    //     {
    //         AudioManager.Current.PlaySFX(sfxLootDiamondBlock, looter.transform.position);
    //     }

    //     ItemFood food = ItemManager.Current.GetFood(lootInfo.item);

    //     if(food!=null)
    //     {
    //         AudioManager.Current.PlaySFX(sfxLootFood, looter.transform.position);
    //         return;
    //     }
        
    //     Potion potion = ItemManager.Current.GetPotion(lootInfo.item);

    //     if(potion!=null)
    //     {
    //         AudioManager.Current.PlaySFX(sfxLootDrink, looter.transform.position);
    //     }
    // }

    // void OnAddBuff(GameObject target, Buff newBuff, float duration)
    // {
    //     if(newBuff==Buff.Speed)
    //     {
    //         AudioManager.Current.PlaySFX(sfxLootSpeed, target.transform.position);
    //     }
    // }

    // void OnEnderPearl(GameObject teleporter, float teleportTime, Vector3 from, Vector3 to)
    // {
    //     AudioManager.Current.PlaySFX(sfxPearl, from);
    //     AudioManager.Current.PlaySFX(sfxPearl, to);
    // }

    // void OnMaceSlam(GameObject mace)
    // {
    //     AudioManager.Current.PlaySFX(sfxMaceSlam, mace.transform.position);
    //     AudioManager.Current.PlaySFX(sfxMaceSlam2, mace.transform.position);
    // }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    // Dictionary<GameObject, bool> craftingSoundPlaying = new();

    // void OnUpdateCraft(GameObject crafter, GameObject station, Recipe recipe)
    // {
    //     if(!craftingSoundPlaying.ContainsKey(station) || !craftingSoundPlaying[station])
    //     {
    //         craftingSoundPlaying[station]=true;

    //         if(recipe.craftingStation==StationType.CraftingTable)
    //         {
    //             AudioManager.Current.PlaySFX(sfxCraftLoop, station.transform.position);

    //             soundLoops[station] = AudioManager.Current.LoopSFX(station, sfxCraftLoop);
    //         }

    //         else if(recipe.craftingStation==StationType.Furnace)
    //         {
    //             AudioManager.Current.PlaySFX(sfxFireIgnite, station.transform.position);
    //             AudioManager.Current.PlaySFX(sfxFireIgnite2, station.transform.position);
    //             AudioManager.Current.PlaySFX(sfxFurnace, station.transform.position);

    //             soundLoops[station] = AudioManager.Current.LoopSFX(station, sfxFireLoop);
    //         }
    //     }
    // }

    // void OnUpdateNotCraft(GameObject station, Recipe recipe)
    // {
    //     if(craftingSoundPlaying.ContainsKey(station) && craftingSoundPlaying[station])
    //     {
    //         craftingSoundPlaying[station]=false;
    //         craftingSoundPlaying.Remove(station);
            
    //         if(recipe.craftingStation==StationType.Furnace)
    //         {
    //             AudioManager.Current.PlaySFX(sfxFireIgnite, station.transform.position);
    //             AudioManager.Current.PlaySFX(sfxFireIgnite2, station.transform.position);
    //         }
    //     }

    //     if(soundLoops.ContainsKey(station) && soundLoops[station])
    //     {
    //         AudioManager.Current.StopLoop(soundLoops[station]);
    //         soundLoops.Remove(station);
    //     }
    // }

    // void OnCrafted(GameObject crafter, GameObject station, Recipe recipe)
    // {
    //     AudioManager.Current.PlaySFX(sfxCraftFinish, station.transform.position);
    //     AudioManager.Current.PlaySFX(sfxUICraft, station.transform.position);
    // }
}
