using UnityEngine;
using AssemblyCSharp;
using System.Collections;
using System.Collections.Generic;

namespace Letscode.Cache
{
	/// <summary>
	/// Simple game object store.
	/// </summary>
	public class GameObjectStore
	{
		/// <summary>
		/// A list of stored gameobjects
		/// </summary>
	    List<GameObject> store;

		/// <summary>
		/// Initializes a new instance of the <see cref="Letscode.Cache.GameObjectStore"/> class.
		/// </summary>
	    public GameObjectStore()
	    {
	        store = new List<GameObject>();
	    }

		/// <summary>
		/// Add the specified gameobject.
		/// </summary>
		/// <param name="go">gameobject.</param>
	    public void Add(GameObject go)
	    {
	        store.Add(go);
	    }

		/// <summary>
		/// Determines whether this instance has unused items.
		/// </summary>
		/// <returns><c>true</c> if this instance has unused items; otherwise, <c>false</c>.</returns>
	    public bool HasUnusedItems()
	    {
	        foreach (GameObject item in store)
	        {
	            ICacheable cacheable = item.GetComponent<ICacheable>();

	            if (cacheable.IsFree())
	                return true;

	        }

	        return false;
	    }

		/// <summary>
		/// Gets one unused item.
		/// </summary>
		/// <returns>One unused item.</returns>
	    public GameObject GetUnusedItem()
	    {
	        foreach (GameObject item in store)
	        {
	            ICacheable cacheable = item.GetComponent<ICacheable>();

	            if (cacheable.IsFree())
	                return item;
	        }

	        return null;
	    }

		/// <summary>
		/// Count the amount of stored gameObjects.
		/// </summary>
	    public int Count()
	    {
	        return store.Count;
	    }

		/// <summary>
		/// Remove the specified gameobject from store.
		/// </summary>
		/// <param name="go">gameobject</param>
	    public void Remove(GameObject go)
	    {
	        if (store.Contains(go))
	            store.Remove(go);
	    }
	}
}