﻿using UnityEngine;
using System.Collections;

public class PlatformGenerator : MonoBehaviour {

	public GameObject platformPrefab;
	public GameObject coinPrefab;
	public Camera mainCamera;

	public float maxPlatformXSpacementFromLast = 5f;
	public float platformYSpacement = 1f;
	public float coinChance = 0.6f;
	public float startingHeight = 2;
	public float coinOffset = 0.7f;

	private GameObject lastPlatform;
	private bool lastHadCoin = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (CanIBeSeen()) 
		{
			if(this.gameObject.transform.position.y >= startingHeight)
				spawnPlatformAndCoin();

			this.gameObject.transform.position = new Vector3(
				this.gameObject.transform.position.x,
				this.gameObject.transform.position.y + platformYSpacement,
				this.gameObject.transform.position.z
				);
		}
	}

	private void spawnPlatformAndCoin()
	{
		float x, y;
		bool hasCoin = Random.value >= coinChance;

		if (lastPlatform == null) {
			x = Random.value * 8 - 4;
		} else {
			x = lastPlatform.transform.position.x + 
				(Random.value * (2 * maxPlatformXSpacementFromLast) - maxPlatformXSpacementFromLast);
			x = Mathf.Clamp (x, -4f, 4f);
		}

		y = this.gameObject.transform.position.y + 
			(Random.value * platformYSpacement - platformYSpacement);

		if (lastPlatform != null &&
			lastHadCoin  &&
		    (y - lastPlatform.transform.position.y) < (coinOffset + 0.1f) &&
		    Mathf.Abs ((x - lastPlatform.transform.position.x)) < lastPlatform.GetComponent<Renderer> ().bounds.size.x) 
		{
			y += lastPlatform.transform.position.y + coinOffset + 0.1f;
		}
		
		lastHadCoin = hasCoin;
		lastPlatform = (GameObject)Instantiate (platformPrefab, new Vector3 (x, y, 0), Quaternion.identity);

		if (hasCoin)
			Instantiate (coinPrefab, new Vector3 (
				lastPlatform.transform.position.x,
				lastPlatform.transform.position.y + coinOffset,
				lastPlatform.transform.position.z), Quaternion.identity);
	}
	
	private bool CanIBeSeen() {
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);		

		if (GeometryUtility.TestPlanesAABB(planes, GetComponent<Collider>().bounds))
			return true;
		else
			return false;
	}
}
