using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class ColliderManager
    {
        private static ColliderManager instance;

        public static ColliderManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ColliderManager();
                }
                return instance;
            }
        }

        private Dictionary<GameObject, List<Collider>> colliders;

        private ColliderManager()
        {
            colliders = new Dictionary<GameObject, List<Collider>>();
        }

        public void Register(GameObject obj)
        {
            if (!colliders.ContainsKey(obj))
            {
                colliders.Add(obj, new List<Collider>());
            }
            Collider[] cols = obj.GetComponentsInChildren<Collider>();
            colliders[obj].AddRange(cols);
        }

        public void Register(GameObject obj, Collider col)
        {
            if (!colliders.ContainsKey(obj))
            {
                colliders.Add(obj, new List<Collider>());
            }
            colliders[obj].Add(col);
        }

        public void Unregister(GameObject obj)
        {
            if (colliders.ContainsKey(obj))
            {
                colliders[obj].Clear();
                colliders.Remove(obj);
            }
        }

        public void UnregisterAll()
        {
            colliders.Clear();
        }

        public void CancelCollisionBetween(GameObject mine, GameObject other)
        {
            if(colliders.ContainsKey(mine) && colliders.ContainsKey(other))
            {
                List<Collider> mines = colliders[mine];
                List<Collider> others = colliders[other];
                for (int i = 0; i < mines.Count; i++)
                {
                    if (mines[i] == null) continue;
                    for (int j = 0; j < others.Count; j++)
                    {
                        if (others[j] == null) continue;
                        Physics.IgnoreCollision(mines[i], others[j], true);
                    }
                }
            }
        }

        public void CancelCollisionWithOthers(GameObject obj)
        {
            if (colliders.ContainsKey(obj))
            {
                foreach (GameObject other in colliders.Keys)
                {
                    if (other != obj)
                    {
                        CancelCollisionBetween(obj, other);
                    }
                }
            }

        }

        public void ResumeCollisionBetween(GameObject mine, GameObject other)
        {
            if (colliders.ContainsKey(mine) && colliders.ContainsKey(other))
            {
                List<Collider> mines = colliders[mine];
                List<Collider> others = colliders[other];
                for (int i = 0; i < mines.Count; i++)
                {
                    if (mines[i] == null) continue;
                    for (int j = 0; j < others.Count; j++)
                    {
                        if (others[j] == null) continue;
                        Physics.IgnoreCollision(mines[i], others[j], false);
                    }
                }
            }
        }

        public void ResumeCollisionWithOthers(GameObject obj)
        {
            if (colliders.ContainsKey(obj))
            {
                foreach (GameObject other in colliders.Keys)
                {
                    if (other != obj)
                    {
                        ResumeCollisionBetween(obj, other);
                    }
                }
            }
        }

        ~ColliderManager()
        {
            colliders.Clear();
            colliders = null;
        }

    }
}