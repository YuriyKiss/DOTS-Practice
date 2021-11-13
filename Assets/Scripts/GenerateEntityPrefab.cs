using UnityEngine;
using Unity.Entities;

public class GenerateEntityPrefab : MonoBehaviour
{
    /* General components used to convert prefab*/
    private World defaultWorld;
    private BlobAssetStore _blobAssetStore;

    // Personal object fields
    [SerializeField] private GameObject prefab;
    private Entity entityPrefab;

    private void Start()
    {
        // Prefab conversion set up
        defaultWorld = World.DefaultGameObjectInjectionWorld;
        _blobAssetStore = new BlobAssetStore();

        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(defaultWorld, _blobAssetStore);
        entityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(prefab, settings);
        // Prefab converted and stored in entityPrefab
    }
}
