using UnityEngine;

public class DamageFood : MonoBehaviour
{
	public int damageFood;
	public void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Debug.Log("hit");
			Destroy(gameObject);
			other.gameObject.GetComponent<DesactivarJugador>().ApplyDamage(damageFood);
		}

		else
		{
			Destroy(gameObject);
		}
	}
}
/*
	public int damageFood = 5;
	private void OnCollisionEnter(Collision collision)
{
	if (collision.gameObject.CompareTag("Player"))
	{
		collision.gameObject.GetComponent<PlayerHealth>().health -= damageFood;
		Debug.Log("Daño");
	}
}
*/