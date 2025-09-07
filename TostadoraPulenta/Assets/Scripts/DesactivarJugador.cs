using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class DesactivarJugador : MonoBehaviour
{
    [Header("Refs (del mismo jugador)")]
    public PlayerInput playerInput;          
    public Rigidbody rb;                     
    public Camera playerCamera;              
    public Transform respawnPoint;           

    [Header("Qu� apagar al morir")]
    public GameObject[] visualsToToggle;     
    public Collider[] collidersToToggle;     
    public Behaviour[] scriptsToDisable;     

    [Header("Vida")]
    public int maxHealth = 10;
    public int health = 10;
    public float respawnDelay = 5f;

    bool isDead;

    void Reset()
    {
        playerInput ??= GetComponent<PlayerInput>();
        rb ??= GetComponent<Rigidbody>();
        playerCamera ??= GetComponentInChildren<Camera>();
    }

    public void ApplyDamage(int amount)
    {
        if (isDead) return;
        health -= amount;
        if (health <= 0) StartCoroutine(DeathRoutine());
    }

    IEnumerator DeathRoutine()
    {
        isDead = true;

        
        playerInput?.DeactivateInput();

        foreach (var b in scriptsToDisable) if (b) b.enabled = false;
        foreach (var c in collidersToToggle) if (c) c.enabled = false;
        foreach (var go in visualsToToggle) if (go) go.SetActive(false);
        if (playerCamera) playerCamera.enabled = false;


        if (rb)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        

        yield return new WaitForSeconds(respawnDelay);

        // Respawn
        if (respawnPoint) transform.position = respawnPoint.position;
        health = maxHealth;

        if (rb)
        {
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;
        }

        foreach (var c in collidersToToggle) if (c) c.enabled = true;
        foreach (var go in visualsToToggle) if (go) go.SetActive(true);
        foreach (var b in scriptsToDisable) if (b) b.enabled = true;
        if (playerCamera) playerCamera.enabled = true;

        
        playerInput?.ActivateInput();

        isDead = false;
    }

	private void Awake()
	{
		if(respawnPoint == null) 
        {
            var rp = GameObject.FindGameObjectWithTag("ReSpawn");
            respawnPoint = rp.transform;

		}
	}
}
