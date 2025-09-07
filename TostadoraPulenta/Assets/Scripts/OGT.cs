using System;
using UnityEngine;

public class OGT : MonoBehaviour
{
	public GameObject ogt;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			DesactivarObjeto();
		}
	}
	void DesactivarObjeto()
	{
		ogt.SetActive(false);
	}

}
