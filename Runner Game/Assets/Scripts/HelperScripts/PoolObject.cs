using UnityEngine;

public class PoolObject : MonoBehaviour
{
    private ObjectInstance _objectInstanceComp;

    public ObjectInstance ObjectInstanceComp { get => _objectInstanceComp; set => _objectInstanceComp = value; }

    public virtual void Reuse()
    {

    }

    public virtual void Destroy()
    {
        //Debug.Log("Calling Destroy from base class PoolObject");
        this.gameObject.SetActive(false);
        PoolManager.Instance.ReturnObjectToPool(_objectInstanceComp);
    }
}
