using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.U2D;

public class AssetManager : MonoBehaviour
{
    public Subject<bool> LoadComplete = new Subject<bool>();
    static AssetManager instance;
    SpriteAtlas atlas;
    public static AssetManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new GameObject("AssetManager", typeof(AssetManager)).GetComponent<AssetManager>();
                DontDestroyOnLoad(instance.gameObject);
               
            }
            return instance;
        }
    }
    public void Init()
    {
        ResourceRequest resourceRequest = Resources.LoadAsync<SpriteAtlas>("atlas");
        resourceRequest.completed += async => {
            atlas = (SpriteAtlas)resourceRequest.asset;
            LoadComplete.OnNext(true);
            LoadComplete.Dispose();
        };
    }
    public static Sprite GetSprite(string spriteName)
    {
        Debug.Log("getsprite " + spriteName);
        return Instance.atlas.GetSprite(spriteName);
    }

    public static T GetResource<T>(string path) where T : UnityEngine.Object
    {
        return Resources.Load<T>(path);
    }
    public void Dispose()
    {
        Destroy(this.gameObject);
    }
}
