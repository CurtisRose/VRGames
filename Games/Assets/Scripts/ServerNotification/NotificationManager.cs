using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationManager: MonoBehaviour {

	private static NotificationManager instance;
	private Hashtable notifiers = new Hashtable();


	public static NotificationManager GetInstance() {
		if (instance == null) {
			instance = new NotificationManager ();
			//notifiers = new Hashtable ();
		}
		return instance;
	}

	public void AddNotifier(Notifier notifier) {
		notifiers[notifier.id + ""] = notifier;
	}
		
	private string message; // DEBUG - skip server
	public void createNotification(string encodedString) {
		message = encodedString;
		SocketSimulator ();
	}

	// DEBUG - simulate socket
	private void SocketSimulator() {
		if (message != null) {
			Notifier notifier = (Notifier)notifiers[message.Split(AttributeNames.delimiter)[0] + ""];
			notifier.decode (message);
		}
	}
}