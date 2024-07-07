using UnityEngine;

using Zenject;

public class DemoPrefab : MonoBehaviour
{

    public void SetPosition(Vector3 position)
    {
        this.transform.position = position;
    }

    public void SetRotation(Quaternion rotation)
    {
        this.transform.rotation = rotation;
    }

    public class Factory : PlaceholderFactory<DemoPrefab>
    {
        // this is factory to create prefabs
    }
}
