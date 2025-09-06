using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Assets.Scripts;

public abstract class ManagerBase<TValue, TInstanceObjectsType> : MonoBehaviour , IOnLoadable
    where TValue : Object
    where TInstanceObjectsType : Object
{
    protected readonly Dictionary<ObjectName, TValue> dic = new();
    protected readonly Dictionary<ObjectName, List<TInstanceObjectsType>> instanceObjects = new();
    protected IEventBus eventBus;

    private void Start()
    {
        InjectDependencies();

        LoadingManager.Instance.SceneLoadingEvents[GameManager.SceneName.Title_Scene].Add(OnLoadTitle);
        LoadingManager.Instance.SceneLoadingEvents[GameManager.SceneName.Stage_1_Scene].Add(OnLoadStage_1);
        LoadingManager.Instance.SceneLoadingEvents[GameManager.SceneName.Stage_2_Scene].Add(OnLoadStage_2);
        LoadingManager.Instance.SceneLoadingEvents[GameManager.SceneName.Stage_3_Scene].Add(OnLoadStage_3);
    }

    public IEnumerator LoadAddressables(params string[] tags)
    {
        dic.Clear();

        var handle = Addressables.LoadAssetsAsync<TValue>(tags, null, Addressables.MergeMode.Union);

        yield return handle;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            var list = handle.Result;

            foreach (var item in list)
            {
                ObjectName name = MyCommon.FindEnumNameByString<ObjectName>(item.name, ObjectName.End);

                if (!dic.ContainsKey(name))
                {
                    dic[name] = item;
                }
            }
        }
        else
        {
            Debug.LogError("Addressables Load Error!!");
        }

        Addressables.Release(handle);
    }
    public TValue GetValue(ObjectName key)
    {
        if (dic.ContainsKey(key))
        {
            return dic[key];
        }

        return null;
    }

    protected virtual IEnumerator CreateInstanceObjects(ObjectName name, int count)
    {
        if (dic.TryGetValue(name, out TValue obj))
        {
            if (!instanceObjects.ContainsKey(name))
            {
                instanceObjects[name] = new List<TInstanceObjectsType>();
            }

            for (int i = 0; i < count; i++)
            {
                if (typeof(TInstanceObjectsType) == typeof(GameObject))
                {
                    if (obj is TInstanceObjectsType instanceObj)
                    {
                        TInstanceObjectsType newInstanceObj = Instantiate(instanceObj);

                        if (newInstanceObj is GameObject gameObj)
                        {
                            gameObj.name = $"{name}_{i}";
                            gameObj.SetActive(false);

                            instanceObjects[name].Add(newInstanceObj);
                        }
                    }
                }
                else if (typeof(TInstanceObjectsType) == typeof(ScriptableObject))
                {
                    if (obj is TInstanceObjectsType instanceObj)
                    {
                        TInstanceObjectsType newInstanceObj = Instantiate(instanceObj);

                        if (newInstanceObj is ScriptableObject gameObj)
                        {
                            instanceObjects[name].Add(newInstanceObj);
                        }
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Object not found and Instance fail");
        }

        yield break;
    }

    public virtual IEnumerator OnLoadTitle() { yield break; }
    public virtual IEnumerator OnLoadStage_1(){ yield break; }
    public virtual IEnumerator OnLoadStage_2(){ yield break; }
    public virtual IEnumerator OnLoadStage_3(){ yield break; }

    protected abstract void InjectDependencies();
}

