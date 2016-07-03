using UnityEngine;

public class Old_Exit : MonoBehaviour
{
	private bool _open = true;
	public bool Open
	{
		get
		{
			return _open;
		}
		set
		{
			_open = value;
			Blocker.SetActive(_open);
		}
	}
	public GameObject Blocker;
}