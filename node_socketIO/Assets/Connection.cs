using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;
using socket.io;

public class Connection : MonoBehaviour
{
    string serverUrl = "http://localhost:7001";
    Socket socket;
    Client client;
    // Use this for initialization
	void Start ()
    {
        socket = Socket.Connect(serverUrl);

        socket.On("connect", () =>
        {
            Debug.Log("Hello, socket.io~");
        });

        socket.On("news", (string data) =>
        {
            Debug.Log(data);

            // "my other event" 이벤트 Send
            socket.Emit(
                "my other event",       // 이벤트명
                "{ \"my\": \"data\" }"  // 데이터 (Json 텍스트)
                );
        });

        client = new Client(serverUrl);
        client.Opened += Client_Opened;

        client.On("connect", (fn) => {
            Debug.Log("Hello, socket.io~");
        });
    }

    private void Client_Opened(object sender, System.EventArgs e)
    {
        Debug.Log("Socket Open!!!");
    }
}
