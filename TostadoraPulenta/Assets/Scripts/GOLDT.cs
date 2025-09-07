using System;
using UnityEngine;

public class GOLDT : MonoBehaviour
{
	public GameObject goldt;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player" )
		{
			DesactivarObjeto();
		}
	}
	void DesactivarObjeto()
	{
		goldt.SetActive(false);
	}
}
