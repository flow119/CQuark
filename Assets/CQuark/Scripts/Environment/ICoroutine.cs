using System.Collections;

public interface ICoroutine {
	object StartNewCoroutine(IEnumerator method);
}
