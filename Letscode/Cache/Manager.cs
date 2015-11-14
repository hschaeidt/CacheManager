using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

namespace Letscode.Cache {
	/// <summary>
	/// Cache manager.
	/// As it is not recommended by unity to Instantiate and Destroy many game objects, this class represents a persistent cache for those often required.
	/// </summary>
	public class Manager : ScriptableObject {
		
		/// <summary>
		/// The instance.
		/// </summary>
		static Manager instance = null;

	    Dictionary<string, GameObjectStore> cached;
		
		private Manager()
		{
			InitManager();
		}

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <value>The instance.</value>
		public static Manager Instance {
			get {
				if (instance == null) {
					instance = CreateInstance ("Manager") as Manager;
				}
				
				return instance;
			}
		}
		
		void InitManager()
		{
	        cached = new Dictionary<string, GameObjectStore>();
		}
		
		/// <summary>
		/// Determines whether this instance has inactive objects the specified category.
		/// </summary>
		/// <returns><c>true</c> if this instance has inactive objects the specified category; otherwise, <c>false</c>.</returns>
		/// <param name="category">Category.</param>
	    /// <summary>
	    /// Determines whether this instance has inactive objects the specified category.
	    /// </summary>
	    /// <returns><c>true</c> if this instance has inactive objects the specified category; otherwise, <c>false</c>.</returns>
	    /// <param name="category">Category.</param>
	    bool HasInactiveObjects(string category)
	    {
	        if (!cached.ContainsKey(category))
	            return false;

	        return cached[category].HasUnusedItems();
	    }
		
		/// <summary>
		/// Gets from cache.
		/// </summary>
		/// <returns>The from cache.</returns>
		/// <param name="category">Category.</param>
		GameObject GetFromCache(string category)
		{
	        return cached[category].GetUnusedItem();
		}
		
		/// <summary>
		/// Gets the cached gameObject of the specified category or creates a new one from the given game object.
		/// </summary>
		/// <returns>The or create.</returns>
		/// <param name="category">Category.</param>
		/// <param name="item">Item.</param>
		public GameObject GetOrCreate(GameObject item)
		{
	        string category = item.GetComponent<ICacheable>().GetCacheCategory();

			if (HasInactiveObjects(category)) {
				return GetFromCache(category);
			}
			
			GameObject newItem;
			
			if (item != null) {
				newItem = Instantiate(item);
				AddToCache(category, newItem);
			} else {
				// fallback item
				newItem = GameObject.CreatePrimitive(PrimitiveType.Cube);
			}
			
			return newItem;
		}
		
		/// <summary>
		/// Adds gameObject to cache.
		/// Cached gameObjects won't be destroyed by loading a new scene, clear this cache manually if needed.
		/// </summary>
		/// <param name="category">Category.</param>
		/// <param name="item">Item.</param>
		void AddToCache(string category, GameObject item)
		{
			ICacheable cch = item.GetComponent<ICacheable>();

			if (cch != null) {
				LevelController.OnBeforeLevelLoad += cch.HandleOnBeforeLevelLoad;
			}

	        GameObjectStore store;
	        if (!cached.ContainsKey(category))
	        {
	            store = new GameObjectStore();
	        }
	        else
	            store = cached[category];

	        store.Add(item);
	        // we re-add here, in case we entered the first case and a new store has been created
	        cached[category] = store;
		}

		/// <summary>
		/// Removes item from cache.
		/// </summary>
		/// <param name="category">Category.</param>
		/// <param name="item">Item.</param>
		public void Remove(GameObject item)
		{
			ICacheable cch = item.GetComponent<ICacheable>();
			if (cch != null) {
				LevelController.OnBeforeLevelLoad -= cch.HandleOnBeforeLevelLoad;
			}
			
			string category = cch.GetCacheCategory();

			if (cached.ContainsKey (category)) {
				cached [category].Remove (item);

				if (cached[category].Count() == 0)
					cached.Remove(category);
			}
		}

		public void PurgeCategory(string cat) {
			if (cached.ContainsKey (cat)) {
				cached[cat] = new GameObjectStore();
			}
		}

		/// <summary>
		/// Frees the item. It will be available from GetOrCreate once again.
		/// </summary>
		/// <param name="category">Category.</param>
		/// <param name="item">Item.</param>
		public void FreeItem(string category, GameObject item)
		{
			if (cached.ContainsKey(category)) {
				item.SetActive(false);
	        }
		}
	}
}