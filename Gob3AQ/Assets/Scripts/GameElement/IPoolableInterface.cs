using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MVerse.BaseClasses.IPoolable
{
    public interface IPoolableInterface
    {
        public void Enable_From_Pool();
        public void PresetAppearOptions(object bundle);

        public void Disable_To_Pool();
        public void Destroy();

    }


    public class Pool
    {
        private Queue<IPoolableInterface> _pooledelems;
        private int _maxsize;

        public Pool(int size, GameObject prefab)
        {
            _pooledelems = new Queue<IPoolableInterface>(size);
            _maxsize = size;

            for(int i=0;i<size;i++)
            {
                GameObject newInstance = GameObject.Instantiate(prefab);
                IPoolableInterface ipif = newInstance.GetComponentInChildren<IPoolableInterface>();

                ipif.Disable_To_Pool();

                _pooledelems.Enqueue(ipif);
            }
        }


        public IPoolableInterface TakeObjectFromPool(object bundle)
        {
            if(_pooledelems.Count > 0)
            {
                IPoolableInterface ipif = _pooledelems.Dequeue();

                ipif.PresetAppearOptions(bundle);

                ipif.Enable_From_Pool();

                return ipif;
            }
            else
            {
                throw new Exception("No more poolable objects remaining");
            }
        }

        public void InsertObjectToPool(IPoolableInterface ipif)
        {
            if(_pooledelems.Count < _maxsize)
            {
                ipif.Disable_To_Pool();
                _pooledelems.Enqueue(ipif);
            }
            else
            {
                throw new Exception("Max pooled elems reached, cannot insert anymore");
            }
        }

        public void DestroyPool()
        {
            while(_pooledelems.Count > 0)
            {
                IPoolableInterface ipif = _pooledelems.Dequeue();

                ipif.Destroy();
            }
        }
    }
}
