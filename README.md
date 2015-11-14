# CacheManager
Unity caching library for GameObjects. Always instantiating and destroying new GameObjects can have huge performance impacts. This small library will take this task away from you.

# Components

* CacheManager implements the singleton-pattern
* Cacheable GameObjects must implement the ICacheable interface
* GameObjectStore stores cached GameObjects

# Usage
Given following Cacheable MonoBehaviour

```
using UnityEngine;
using System.Collections;

// Required usage for Cache namespace
using Letscode.Cache;
using CacheManager = Letscode.Cache.Manager;

// Must implement ICacheable interface
public class MyCacheableGO : MonoBehaviour, ICacheable
{
  bool isFree = false;

  // override interface method
  public string GetCacheCategory()
  {
    return "myCacheCategory";
  }

  // override interface method
  public bool IsFree()
  {
    return isFree;
  }

  // override interface method
  public void SetFree(bool free)
  {
    isFree = free;
  }

  // override interface method
  public void FreeItem()
  {
    CacheManager.Instance.FreeItem(GetCacheCategory(), gameObject);
    SetFree(true);
  }
}
```

The management of calling those methods is in the responsibility of its spawner.
