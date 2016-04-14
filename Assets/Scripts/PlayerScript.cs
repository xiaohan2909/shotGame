﻿using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	/// <summary>
	/// 1 - The speed of the ship
	/// </summary>
	public Vector2 speed = new Vector2(50, 50);
	// 2 - Store the movement
	private Vector2 movement;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// 3 - Retrieve axis information
		float inputX = Input.GetAxis("Horizontal");
		float inputY = Input.GetAxis("Vertical");	
		// 4 - Movement per direction
		movement = new Vector2(
			speed.x * inputX,
			speed.y * inputY);
		// 5 - Shooting
		bool shoot = Input.GetButtonDown("Fire1");
		shoot |= Input.GetButtonDown("Fire2");
		// Careful: For Mac users, ctrl + arrow is a bad idea
		if (shoot)
		{
			WeaponScript weapon = GetComponent<WeaponScript>();
			if (weapon != null)
			{
				// false because the player is not an enemy
				weapon.Attack(false);
				//sound
				SoundEffectsHelper.Instance.MakePlayerShotSound();
			}
		}
		// 6 - Make sure we are not outside the camera bounds
		var dist = (transform.position - Camera.main.transform.position).z;
		
		var leftBorder = Camera.main.ViewportToWorldPoint(
			new Vector3(0, 0, dist)
			).x;
		
		var rightBorder = Camera.main.ViewportToWorldPoint(
			new Vector3(1, 0, dist)
			).x;
		
		var topBorder = Camera.main.ViewportToWorldPoint(
			new Vector3(0, 0, dist)
			).y;
		
		var bottomBorder = Camera.main.ViewportToWorldPoint(
			new Vector3(0, 1, dist)
			).y;
		
		transform.position = new Vector3(
			Mathf.Clamp(transform.position.x, leftBorder, rightBorder),
			Mathf.Clamp(transform.position.y, topBorder, bottomBorder),
			transform.position.z
			);

	}
	void FixedUpdate()
	{
		// 5 - Move the game object
		rigidbody2D.velocity = movement;
	}
	//碰撞的话俩都死
	void OnCollisionEnter2D(Collision2D collision)
	{
		bool damagePlayer = false;
		
		// Collision with enemy
		EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript>();
		if (enemy != null)
		{
			// Kill the enemy
			HealthScript enemyHealth = enemy.GetComponent<HealthScript>();
			if (enemyHealth != null) enemyHealth.Damage(enemyHealth.hp);
			
			damagePlayer = true;
		}
		
		// Damage the player
		if (damagePlayer)
		{
			HealthScript playerHealth = this.GetComponent<HealthScript>();
			if (playerHealth != null) playerHealth.Damage(1);
		}
	}
	void OnDestroy()
	{
		// Game Over.
		// Add the script to the parent because the current game
		// object is likely going to be destroyed immediately.
		transform.parent.gameObject.AddComponent<GameOverScript>();
	}
}
