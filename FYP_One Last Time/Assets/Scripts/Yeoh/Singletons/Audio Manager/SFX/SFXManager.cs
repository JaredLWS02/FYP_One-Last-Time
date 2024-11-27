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

    AudioManager AudioM;
    EventManager EventM;

    void OnEnable()
    {
        AudioM = AudioManager.Current;
        EventM = EventManager.Current;
        
        // EventM.SpawnEvent += OnSpawn;
        // EventM.IdleVoiceEvent += OnIdleVoice;
        // EventM.HurtEvent += OnHurt;
        // EventM.DeathEvent += OnDeath;
        // EventM.LootEvent += OnLoot;
        // EventM.AddBuffEvent += OnAddBuff;
        // EventM.EnderPearlEvent += OnEnderPearl;
        // EventM.MaceSlamEvent += OnMaceSlam;
        // EventM.UpdateCraftEvent += OnUpdateCraft;
        // EventM.UpdateNotCraftEvent += OnUpdateNotCraft;
        // EventM.CraftedEvent += OnCrafted;
    }
    void OnDisable()
    {
        // EventM.SpawnEvent -= OnSpawn;
        // EventM.IdleVoiceEvent -= OnIdleVoice;
        // EventM.HurtEvent -= OnHurt;
        // EventM.DeathEvent -= OnDeath;
        // EventM.LootEvent -= OnLoot;
        // EventM.AddBuffEvent -= OnAddBuff;
        // EventM.EnderPearlEvent -= OnEnderPearl;
        // EventM.MaceSlamEvent -= OnMaceSlam;
        // EventM.UpdateCraftEvent -= OnUpdateCraft;
        // EventM.UpdateNotCraftEvent -= OnUpdateNotCraft;
        // EventM.CraftedEvent -= OnCrafted;
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

    // [Header("Main Menu UI")]
    // public AudioClip[] sfxUISelectButton;
    // public AudioClip[] sfxUIHoverButton;

    // Dictionary<GameObject, AudioSource> soundLoops = new();

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // void OnSpawn(GameObject spawned)
    // {
    //     if(spawned.TryGetComponent(out Enemy enemy))
    //     {
    //         if(Random.Range(0,5)==0)
    //         AudioM.PlaySFX(sfxSpawnEnemy, spawned.transform.position);
    //     }
    // }

    // void OnIdleVoice(GameObject subject)
    // {
    //     if(subject.TryGetComponent(out Enemy enemy))
    //     {
    //         switch(enemy.enemyName)
    //         {
    //             case EnemyName.Zombie: AudioM.PlayVoice(enemy.voicebox, voiceZombieIdle); break;
    //             case EnemyName.Spider: AudioM.PlayVoice(enemy.voicebox, voiceSpiderSay); break;
    //             case EnemyName.Skeleton: AudioM.PlayVoice(enemy.voicebox, voiceSkeletonIdle); break;
    //         }
    //     }
    // }

    // void OnHurt(GameObject victim, GameObject attacker, HurtInfo hurtInfo)
    // {
    //     if(victim.TryGetComponent(out Steve steve))
    //     {
    //         AudioM.PlaySFX(sfxHitNpc, victim.transform.position);

    //         switch(steve.npcName)
    //         {
    //             case NPCName.Steve: AudioM.PlayVoice(steve.voicebox, voiceSteveHurt); break;
    //             case NPCName.Alex: AudioM.PlayVoice(steve.voicebox, voiceAlexHurt); break;
    //         }
    //     }

    //     else if(victim.TryGetComponent(out Enemy enemy))
    //     {
    //         if(enemy.enemyName!=EnemyName.Skeleton) AudioM.PlaySFX(sfxHitFlesh, victim.transform.position);

    //         switch(enemy.enemyName)
    //         {
    //             case EnemyName.Zombie: AudioM.PlayVoice(enemy.voicebox, voiceZombieHurt); break;
    //             case EnemyName.Spider: AudioM.PlayVoice(enemy.voicebox, voiceSpiderSay); break;
    //             case EnemyName.Skeleton: AudioM.PlayVoice(enemy.voicebox, voiceSkeletonHurt); break;
    //         }
    //     }

    //     else if(victim.TryGetComponent(out Resource resource))
    //     {
    //         switch(resource.type)
    //         {
    //             case Item.WoodLog:
    //             {
    //                 AudioM.PlaySFX(sfxHitWood, victim.transform.position);
    //                 AudioM.PlaySFX(sfxHitWood2, victim.transform.position);
    //             } break;

    //             case Item.Stone:
    //             {
    //                 AudioM.PlaySFX(sfxHitStone, victim.transform.position);
    //                 AudioM.PlaySFX(sfxHitStone2, victim.transform.position);
    //             } break;

    //             case Item.CoalOre: AudioM.PlaySFX(sfxHitStone, victim.transform.position); break;
    //             case Item.IronOre: AudioM.PlaySFX(sfxHitStone, victim.transform.position); break;
    //             case Item.Diamond: AudioM.PlaySFX(sfxHitStone, victim.transform.position); break;
    //         }
    //     }
    // }

    // void OnDeath(GameObject victim, GameObject killer, HurtInfo hurtInfo)
    // {
    //     if(victim.TryGetComponent(out Steve steve))
    //     {
    //         AudioM.PlaySFX(sfxDeathNpc, victim.transform.position, false);
    //     }

    //     else if(victim.TryGetComponent(out Enemy enemy))
    //     {
    //         if(enemy.enemyName!=EnemyName.Skeleton) AudioM.PlaySFX(sfxDeathEnemy, victim.transform.position);

    //         switch(enemy.enemyName)
    //         {
    //             case EnemyName.Zombie: AudioM.PlaySFX(voiceZombieDeath, victim.transform.position); break;
    //             case EnemyName.Spider: AudioM.PlaySFX(voiceSpiderDeath, victim.transform.position); break;
    //             case EnemyName.Skeleton: AudioM.PlaySFX(voiceSkeletonDeath, victim.transform.position); break;
    //         }
    //     }

    //     else if(victim.TryGetComponent(out Resource resource))
    //     {
    //         switch(resource.type)
    //         {
    //             case Item.WoodLog: AudioM.PlaySFX(sfxBreakWood, victim.transform.position); break;
    //             case Item.Stone: AudioM.PlaySFX(sfxBreakStone, victim.transform.position); break;
    //             case Item.CoalOre: AudioM.PlaySFX(sfxBreakStone, victim.transform.position); break;
    //             case Item.IronOre: AudioM.PlaySFX(sfxBreakStone, victim.transform.position); break;
    //             case Item.Diamond: AudioM.PlaySFX(sfxBreakDiamond, victim.transform.position); break;
    //         }
    //     }
    // }    

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // public void OnLoot(GameObject looter, GameObject loot, LootInfo lootInfo)
    // {
    //     AudioM.PlaySFX(sfxLoot, looter.transform.position);

    //     if(lootInfo.item==Item.DiamondBlock)
    //     {
    //         AudioM.PlaySFX(sfxLootDiamondBlock, looter.transform.position);
    //     }

    //     ItemFood food = ItemManager.Current.GetFood(lootInfo.item);

    //     if(food!=null)
    //     {
    //         AudioM.PlaySFX(sfxLootFood, looter.transform.position);
    //         return;
    //     }
        
    //     Potion potion = ItemManager.Current.GetPotion(lootInfo.item);

    //     if(potion!=null)
    //     {
    //         AudioM.PlaySFX(sfxLootDrink, looter.transform.position);
    //     }
    // }

    // void OnAddBuff(GameObject target, Buff newBuff, float duration)
    // {
    //     if(newBuff==Buff.Speed)
    //     {
    //         AudioM.PlaySFX(sfxLootSpeed, target.transform.position);
    //     }
    // }

    // void OnEnderPearl(GameObject teleporter, float teleportTime, Vector3 from, Vector3 to)
    // {
    //     AudioM.PlaySFX(sfxPearl, from);
    //     AudioM.PlaySFX(sfxPearl, to);
    // }

    // void OnMaceSlam(GameObject mace)
    // {
    //     AudioM.PlaySFX(sfxMaceSlam, mace.transform.position);
    //     AudioM.PlaySFX(sfxMaceSlam2, mace.transform.position);
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
    //             AudioM.PlaySFX(sfxCraftLoop, station.transform.position);

    //             soundLoops[station] = AudioM.LoopSFX(station, sfxCraftLoop);
    //         }

    //         else if(recipe.craftingStation==StationType.Furnace)
    //         {
    //             AudioM.PlaySFX(sfxFireIgnite, station.transform.position);
    //             AudioM.PlaySFX(sfxFireIgnite2, station.transform.position);
    //             AudioM.PlaySFX(sfxFurnace, station.transform.position);

    //             soundLoops[station] = AudioM.LoopSFX(station, sfxFireLoop);
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
    //             AudioM.PlaySFX(sfxFireIgnite, station.transform.position);
    //             AudioM.PlaySFX(sfxFireIgnite2, station.transform.position);
    //         }
    //     }

    //     if(soundLoops.ContainsKey(station) && soundLoops[station])
    //     {
    //         AudioM.StopLoop(soundLoops[station]);
    //         soundLoops.Remove(station);
    //     }
    // }

    // void OnCrafted(GameObject crafter, GameObject station, Recipe recipe)
    // {
    //     AudioM.PlaySFX(sfxCraftFinish, station.transform.position);
    //     AudioM.PlaySFX(sfxUICraft, station.transform.position);
    // }
}
