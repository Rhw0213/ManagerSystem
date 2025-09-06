using UnityEngine;
using System;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections;
using SC = Assets.Scripts.StringConstant;

public class DataManager : ManagerBase<MyData, ScriptableObject> , IDataLinkServive
{
    public static DataManager Instance { get; private set; }
    
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
    void Start()
    {
        //EnemyManager.CreateEnemyInstances += (List<GameObject> otherInstances, ObjectName name) => { 
        //    StartCoroutine(InjectData(otherInstances, name));
        //} ;
    }

    public IEnumerator LinkData(List<GameObject> otherInstances, ObjectName name) 
    {
        List<ScriptableObject> dataList = instanceObjects[name];

        if (dataList.Count == otherInstances.Count)
        {
            for (int i = 0; i < dataList.Count; i++)
            {
                otherInstances[i].GetComponent<MyObject>().Data = dataList[i] as MyData; 
            }
        }

        yield break;
    }
    public void OnLinkDataService(InstanceCreteEvent eventData)
    {
        StartCoroutine(LinkData(eventData.Instances, eventData.name));
    }

    protected override void InjectDependencies()
    {
        if (DIContainer.Instance != null)
        {
            eventBus = DIContainer.Instance.Resolove<IEventBus>();

            DIContainer.Instance.Register<IDataLinkServive>(this);

            eventBus.Subscribe<InstanceCreteEvent>(OnLinkDataService);
        }
    }

    //public override IEnumerator OnLoadTitle()
    //{
    //    yield return StartCoroutine(LoadAddressables(
    //        SC.DATA,
    //        SC.TITLE 
    //    )); 
    //}

    public override IEnumerator OnLoadStage_1()
    {
        yield return StartCoroutine(LoadAddressables(
            SC.DATA, 
            SC.STAGE_1
        ));
    }

    //public override IEnumerator OnLoadStage_2()
    //{
    //    yield return StartCoroutine(LoadAddressables(
    //        SC.DATA, 
    //        SC.STAGE_2
    //    ));
    //}

    //public override IEnumerator OnLoadStage_3()
    //{
    //    yield return StartCoroutine(LoadAddressables(
    //        SC.DATA, 
    //        SC.STAGE_3
    //    ));
    //}
}
