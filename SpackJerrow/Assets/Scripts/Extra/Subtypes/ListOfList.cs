using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ListOfList<T> : IEnumerable<T>, IList<T>
{

    public IEnumerator<T> GetEnumerator()
    {
        return innerList.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return innerList.GetEnumerator();
    }

    public int IndexOf(T item)
    {
        return innerList.IndexOf(item);
    }

    public void Insert(int index, T item)
    {
        innerList.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        innerList.RemoveAt(index);
    }

    public void Add(T item)
    {
        innerList.Add(item);
    }

    public void Clear()
    {
        innerList.Clear();
    }

    public bool Contains(T item)
    {
        return innerList.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        innerList.CopyTo(array, arrayIndex);
    }

    public bool Remove(T item)
    {
        return innerList.Remove(item);
    }

    public List<T> innerList;

    public int Count => innerList.Count;

    public bool IsReadOnly => false;

    public T this[int index] { get => innerList[index]; set => innerList[index] = value; }

}
