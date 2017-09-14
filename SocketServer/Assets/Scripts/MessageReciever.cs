using System;
using UnityEngine;

public abstract class MessageReciever : MonoBehaviour
{
	public static MessageReciever singleton;
	public abstract void RecieveMessage (Message message);



}

