using UnityEngine;

public class SpawnTostadores : MonoBehaviour
{
	public OGT ogt;
	public GOLDT goldt;
	void ActivarT()
	{
		Invoke("TostadorasOGT", 60);
		Invoke("TostadorasGOLDT", 120);
	}
	void TostadorasOGT()
	{
		ogt.ogt.SetActive(true);
	}

	void TostadorasGOLDT()
	{
		goldt.goldt.SetActive(true);
	}
	private void Start()
	{
		ActivarT();
	}
}
