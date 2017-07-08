using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Notifier : MonoBehaviour{

	public int id;
	private NotificationManager manager;

	// DEBUG: id creator
	private static int NEXT_ID = 0;

	protected virtual void Start() {
		this.id = NEXT_ID;
		NEXT_ID++;
		//this.manager = NotificationManager.GetInstance();
		//this.manager.AddNotifier (this);
	}

	public abstract void decode (string message);

	protected string encode(string id, string attribute, params string[] values) {
		string encodedString = id + ':' + attribute;
		for (int i = 0; i < values.Length; i++) {
			encodedString += (':' + values [i]);
		}
		return encodedString;
	}

	protected void notify(string message) {
		//manager.createNotification (message);
	}
}
