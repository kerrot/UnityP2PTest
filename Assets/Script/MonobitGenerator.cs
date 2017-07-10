using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using MonobitEngine;

public class MonobitGenerator : UnityEngine.MonoBehaviour {
	[SerializeField]
	private GameObject prefab;
	[SerializeField]
	private float period;
	[SerializeField]
	private float range;
	[SerializeField]
	private int amount;

	void Start()
	{
		if (period > 0 && amount > 0) {
			Observable.Interval (System.TimeSpan.FromSeconds (period))
				.Where(_ => MonobitNetwork.inRoom && MonobitNetwork.isHost).Subscribe (_ => {
					Observable.Repeat(0, amount).Subscribe(r => {
						GameObject obj = MonobitNetwork.Instantiate(prefab.name, transform.position + Random.insideUnitSphere * range, Random.rotation, 0);
						if (obj){
							obj.transform.parent = transform;	
						}
					});
			});	
		}
	}
		
}
