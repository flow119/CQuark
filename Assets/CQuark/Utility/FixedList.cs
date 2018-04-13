using System.Collections.Generic;
using System.Diagnostics;

/// <summary>
/// 一种固定长度的List形式，只能对最后一位做写操作（但和Queue不同，可以取前面的位）。声明的时候必须知道最大大小（不存在Alloc）
/// </summary>
public struct FixedList<T> {

    public static FixedList<T> Null {
        get {
            return new FixedList<T>(0);
        }
    }

	public int Count {get; private set;}
	private T[] buffer;

    public FixedList (int maxCount) {
		buffer = new T[maxCount];
		Count = 0;
	}

	/// <summary>
	/// Convenience function. I recommend using .buffer instead.
	/// </summary>
	[DebuggerHidden]
	public T this[int i]{
		get { return buffer[i]; }
		set { buffer[i] = value; }
	}

	/// <summary>
	/// For 'foreach' functionality.
	/// </summary>
	[DebuggerHidden]
	[DebuggerStepThrough]
	public IEnumerator<T> GetEnumerator (){
		for (int i = 0; i < Count; ++i){
			yield return buffer[i];
		}
	}

	/// <summary>
	/// Add the specified item to the end of the list.
	/// </summary>
	public void Add (T item){
		buffer[Count] = item;
		Count++;
	}

	/// <summary>
	/// Remove an item from the end.
	/// </summary>
	public T Pop (){
		if (Count != 0){
			Count--;
			return buffer[Count];
		}
		return default(T);
	}

	/// <summary>
	/// Clear the array by resetting its size to zero. Note that the memory is not actually released.
	/// </summary>
	public void Clear () { Count = 0; }

	/// <summary>
	/// Returns 'true' if the specified item is within the list.
	/// </summary>
	public bool Contains (T item){
		for (int i = 0; i < Count; ++i) if (buffer[i].Equals(item)) return true;
		return false;
	}

	/// <summary>
	/// Return the index of the specified item.
	/// </summary>
	public int IndexOf (T item){
		for (int i = 0; i < Count; ++i) if (buffer[i].Equals(item)) return i;
		return -1;
	}

	/// <summary>
	/// Mimic List's ToArray() functionality, except that in this case the list is resized to match the current size.
	/// </summary>
	public T[] ToArray () {
		T[] newList = new T[Count];
		for (int i = 0; i < Count; ++i) 
			newList[i] = buffer[i];
		return newList;
	}
}
