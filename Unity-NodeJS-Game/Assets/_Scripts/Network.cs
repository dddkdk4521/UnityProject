using System;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Network : MonoBehaviour
{
    public GameObject currentPlayer;
    public Spawner spawner;
    
    static SocketIOComponent socket;
    
    void Start()
    {
        socket = GetComponent<SocketIOComponent>();
        {
            socket.On("open", OnConnected);
            socket.On("register", OnRegister);
            socket.On("spawn", OnSpawn);
            socket.On("move", OnMove);
            socket.On("follow", OnFollow);
            socket.On("requestPosition", OnRequestPosition);
            socket.On("updatePosition", OnUpdatePosition);
		    socket.On("attack", OnAttack);
            socket.On("disconnected", OnDisconnected);
        }
    }
    
    private void OnConnected(SocketIOEvent obj)
    {
        Debug.Log("conected");
    }

    private void OnRegister(SocketIOEvent obj)
    {
        Debug.Log("registered id = " + obj.data);

        this.spawner.AddPlayer(obj.data["id"].str, this.currentPlayer);
		this.currentPlayer.GetComponent<NetworkEntity> ().id = obj.data ["id"].str;
    }
    
    private void OnSpawn(SocketIOEvent obj)
    {
        Debug.Log("Spawn " + obj.data);

        GameObject player = spawner.SpawnPlayer(obj.data["id"].str);
        if (obj.data["x"])
        {
            Vector3 movePosition = GetVectorFromJson(obj);
            Navigator navPos = player.GetComponent<Navigator>();
            navPos.NavigateTo(movePosition);
        }
    }

    private void OnMove(SocketIOEvent obj)
    {
        Vector3 position = GetVectorFromJson(obj);
        GameObject player = spawner.GetPlayer(obj.data["id"].str);

        Navigator navPos = player.GetComponent<Navigator>();
        navPos.NavigateTo(position);
    }

    private void OnFollow(SocketIOEvent obj)
    {
        Debug.Log("follow request " + obj.data);

        GameObject player = spawner.GetPlayer(obj.data["id"].str);
		Transform targetTransform = spawner.GetPlayer(obj.data["targetId"].str).transform;
        Targeter target = player.GetComponent<Targeter>();
		target.target = targetTransform;
    }

    private void OnUpdatePosition(SocketIOEvent obj)
    {
        Vector3 position = GetVectorFromJson(obj);
        GameObject player = spawner.GetPlayer(obj.data["id"].str);

        player.transform.position = position;
    }

    private void OnRequestPosition(SocketIOEvent obj)
    {
        socket.Emit("updatePosition", VectorToJson(currentPlayer.transform.position));
    }

	private void OnAttack (SocketIOEvent obj)
	{
		Debug.Log("received attack " + obj.data);
		GameObject targetPlayer = spawner.GetPlayer(obj.data["targetId"].str);
		targetPlayer.GetComponent<Hittable> ().GetHit(20f);

        GameObject attackingPlayer = spawner.GetPlayer(obj.data["id"].str);
		attackingPlayer.GetComponent<Animator> ().SetTrigger ("Attack");

	}

    private void OnDisconnected(SocketIOEvent obj)
    {
        string disconnectedId = obj.data["id"].str;
        this.spawner.Remove(disconnectedId);
    }

    // Until
    private static Vector3 GetVectorFromJson(SocketIOEvent obj)
    {
        return new Vector3(obj.data["x"].n, 0, obj.data["y"].n);
    }

    #region Static
    public static JSONObject VectorToJson(Vector3 vector)
    {
        JSONObject jsonObject = new JSONObject(JSONObject.Type.OBJECT);
        {
            jsonObject.AddField("x", vector.x);
            jsonObject.AddField("y", vector.z);
        }

        return jsonObject;
    }

    public static JSONObject PlayerIdToJson(string id)
    {
        JSONObject jsonObject = new JSONObject(JSONObject.Type.OBJECT);
        jsonObject.AddField("targetId", id);

        return jsonObject;
    }

    public static void Move(Vector3 current, Vector3 destionation)
    {
		Debug.Log("send moving to node " + Network.VectorToJson(destionation));

		JSONObject jsonObject = new JSONObject(JSONObject.Type.OBJECT);
        {
		    jsonObject.AddField("c", Network.VectorToJson(current));
		    jsonObject.AddField("d", Network.VectorToJson(destionation));
        }

		socket.Emit("move", jsonObject);
    }

    public static void Follow(string id)
    {
        Debug.Log("send follow player id " + Network.PlayerIdToJson(id));

        socket.Emit("follow", Network.PlayerIdToJson(id));
    }

	public static void Attack(string targetId)
	{
		Debug.Log("attacking player id " + Network.PlayerIdToJson(targetId));

        socket.Emit("attack", Network.PlayerIdToJson(targetId));
	}
    #endregion
}
