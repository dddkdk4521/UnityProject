using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;

public class Network : MonoBehaviour
{
    public GameObject playerPrefab;

    static SocketIOComponent socket;

	// Use this for initialization
	void Start ()
    {
        socket = GetComponent<SocketIOComponent>();
        {
            socket.On("open",  OnConnected);
            socket.On("spawn", OnSpawned);
        }
    }

    private void OnSpawned(SocketIOEvent e)
    {
        Debug.Log("spawned");

        Instantiate(playerPrefab);
    }

    void OnConnected(SocketIOEvent e)
    {
        Debug.Log("connected");

        socket.Emit("move");
    }
}
