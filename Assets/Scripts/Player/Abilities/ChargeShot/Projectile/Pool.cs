using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pool<T> : MonoBehaviour where T : MonoBehaviour
{

    protected int totalObjects;
    protected string objectName;

    protected List<T> pool;
    protected int currentNode;
    

	public Pool(int totalObjects, string objectName, Transform prefabObj, Transform parentObj)
    {
        this.totalObjects = totalObjects;
        this.objectName = objectName;
        pool = new List<T>();
        for (int i = 0; i < totalObjects; i++)
        {
            Transform poolObj = (Transform)Instantiate(prefabObj);
            poolObj.parent = parentObj;
            poolObj.name = objectName + " " + i;
            poolObj.GetComponent<T>().enabled = false;
            pool.Add(poolObj.GetComponent<T>());
        }

        currentNode = 0;
	}

    /**
     * Requests an object. If it is already in use, returns null and increments the
     * pointer.
     */
    public T RequestObject()
    {
        T node = pool[currentNode];
        //Debug.Log("Current " + node.GetType() + " node: " + currentNode);
        if (!node.enabled)
        {
            currentNode++;
            if (currentNode >= totalObjects)
            {
                currentNode = 0;
            }

            node.enabled = true;

            return node;
        }
        else
        {
            currentNode++;
            if (currentNode >= totalObjects)
            {
                currentNode = 0;
            }
            return null;
        }
    }

    public void ResetPool()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            pool[i].enabled = false;
            pool[i].transform.position = new Vector3(-1000, 0, 0);
        }
    }
}
