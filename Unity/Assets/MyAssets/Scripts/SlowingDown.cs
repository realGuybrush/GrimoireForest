using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SlowingDown : MonoBehaviour {

	public int SlowPercentAmount;
	// Use this for initialization
	void Start () 
	{
		if (SlowPercentAmount < 0)
			SlowPercentAmount = 0;
		if (SlowPercentAmount > 100)
			SlowPercentAmount = 100;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay2D(Collider2D collider)
	{
		var col = collider.gameObject.GetComponent<Rigidbody2D> ();
		if( col!= null)
			col.velocity *= (float)((100f - (float)SlowPercentAmount) / 100f);
	}
}
