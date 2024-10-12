/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GathererAI : MonoBehaviour {

    //[SerializeField] private Transform goldNodeTransform;
    //[SerializeField] private Transform storageTransform;//  1장

    //private enum State
    //{
    //    Idle,
    //     MovingToResourceNode,
    //     GatheringResources,
    //     MovingToStorage,
    //}

    //private IUnit unit;
    //private State state;
    //private Transform resourceNodeTransform;
    //private Transform storageTransform;
    //private int goldInventoryAmount;


    //private void Awake()
    //{
    //    unit = gameObject.GetComponent<IUnit>();
    //    state = State.Idle;

    //    //unit.MoveTo(goldNodeTransform.position, 10f, () => {
    //    //    unit.PlayAnimationMine(goldNodeTransform.position, () =>
    //    //    {
    //    //        unit.MoveTo(storageTransform.position, 5f, null);
    //    //    });
    //    //}); 1장
    //}

    //private void Update()
    //{
    //    switch (state) { 
    //    case State.Idle:
    //            GameHandler.GetResourceNode_Static();
    //            state = State.MovingToResourceNode;
    //            break;
    //    case State.MovingToResourceNode:
    //            if (unit.IsIdle())
    //            {
    //                unit.MoveTo(resourceNodeTransform.position, 10f, () => {
    //                    state = State.GatheringResources;
    //                });
    //            }
    //            break;
    //    case State.GatheringResources:
    //            if (unit.IsIdle())
    //            {
    //                if (goldInventoryAmount > 0)
    //                {
    //                    // Move to storage
    //                    storageTransform = GameHandler.GetStorage_Static();
    //                    state = State.MovingToStorage;
    //                }
    //                else
    //                {
    //                    unit.PlayAnimationMine(resourceNodeTransform.position, () =>
    //                    {
    //                        goldInventoryAmount++;
    //                    });
    //                }
    //            }
    //            break;
    //    case State.MovingToStorage:
    //            if (unit.IsIdle())
    //            {
    //                unit.MoveTo(storageTransform.position, 10f, () => {
    //                    GameResources.AddGoldAmount(goldInventoryAmount);
    //                    Debug.Log("Gold Amount: " + GameResources.GetGoldAmount()); 
    //                    goldInventoryAmount = 0;
    //                    state = State.Idle;
    //                });
    //            }
    //            break;

    //    }
    //}


    private enum State
    {
        Idle,
        MovingToResourceNode,
        GatheringResources,
        MovingToStorage,
    }

    private IUnit unit;
    private State state;
    private Transform resourceNodeTransform;
    private Transform storageTransform;
    private int goldInventoryAmount;


    //LootTable
    //[Header("Loot")]
    //public List<LootItem> lootTable = new List<LootItem>();

    //void InstantiateLoot(GameObject loot)
    //{
    //    if (loot)
    //    {
    //        GameObject droppedLoot = Instantiate(loot, transform.position, Quaternion.identity);

    //        droppedLoot.GetComponent<SpriteRenderer>().color = Color.red;
    //    }
    //}




    private void Awake()
    {
        unit = gameObject.GetComponent<IUnit>();
        state = State.Idle;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                resourceNodeTransform = GameHandler.GetResourceNode_Static();
                state = State.MovingToResourceNode;
                break;
            case State.MovingToResourceNode:
                if (unit.IsIdle())
                {
                    unit.MoveTo(resourceNodeTransform.position, 10f, () =>
                    {
                        state = State.GatheringResources;
                    });
                }
                break;
            case State.GatheringResources:
                if (unit.IsIdle())
                {
                    if (goldInventoryAmount > 3)
                    {
                        ////Item drop
                        //foreach(LootItem lootItem in lootTable)
                        //{
                        //    if(Random.Range(0f,100f) <= lootItem.dropChance)
                        //    {
                        //        InstantiateLoot(lootItem.itemPrefab);
                        //    }
                        //    break;
                        //}
                        // Move to storage
                        storageTransform = GameHandler.GetStorage_Static();
                        state = State.MovingToStorage;
                    }
                    else
                    {
                        // Gather resources
                        unit.PlayAnimationMine(resourceNodeTransform.position, () =>
                        {
                            goldInventoryAmount++;
                        });
                    }
                }
                break;
            case State.MovingToStorage:
                if (unit.IsIdle())
                {
                    unit.MoveTo(storageTransform.position, 10f, () =>
                    {
                        goldInventoryAmount = 0;
                        state = State.Idle;
                    });
                }
                break;
        }
    }
}
