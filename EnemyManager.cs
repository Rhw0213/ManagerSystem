using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using SC = Assets.Scripts.StringConstant;
using System;

public class EnemyManager : ManagerBase<Enemy, GameObject> 
{
    public static EnemyManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public static Action<List<GameObject>, ObjectName> CreateEnemyInstances;

    protected override void InjectDependencies()
    {
        eventBus = DIContainer.Instance.Resolove<IEventBus>();
    }

    public override IEnumerator OnLoadStage_1()
    {
        yield return StartCoroutine(LoadAddressables(
            SC.ENEMY, 
            SC.STAGE_1
        ));

        ObjectName objectName = ObjectName.BasicSoldier;
        int count = 10;

        yield return StartCoroutine(CreateInstanceObjects(objectName, count));

        // 이벤트 발생
        eventBus.Publish(new InstanceCreteEvent(instanceObjects[objectName], objectName));

        //yield return DataManager.Instance.StartCoroutine(CreateInstanceObjects(objectName, count));
        //yield return StartCoroutine(DataManager.Instance.GetType<DataManager>().InjectionData(instanceObjects[objectName], objectName));
    }

    //public override IEnumerator OnLoadStage_2()
    //{
    //    yield return StartCoroutine(LoadAddressables(
    //        SC.ENEMY, 
    //        SC.STAGE_1
    //    ));
    //}

    //public override IEnumerator OnLoadStage_3()
    //{
    //    yield return StartCoroutine(LoadAddressables(
    //        SC.ENEMY, 
    //        SC.STAGE_1
    //    ));
    //}

}
