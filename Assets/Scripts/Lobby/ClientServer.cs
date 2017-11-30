using System;
using UnityEngine;
using UnityEngine.Networking;

namespace FCS
{
    public class NetClient : NetworkDiscovery
    {
        public Action<string> Callback;

        void Awake()
        {
            useNetworkManager = true;
            showGUI = false;
            StartClient();
        }

        public void StartClient()
        {
            Initialize();
            StartAsClient();
        }

        public override void OnReceivedBroadcast(string fromAddress, string data)
        {
            if (Callback != null)
            {
                Callback(fromAddress);
            }
            Callback = null;
        }
    }

    public class NetClientSingleton : Singleton<NetClientSingleton>
    {
        private NetClient _client;

        private NetClient Client
        {
            get { return _client ?? (_client = gameObject.AddComponent<NetClient>()); }
        }

        public void StartClient(Action<string> callback)
        {
            if (!Client.running)
            {
                Client.StartClient();
                Client.Callback += callback;
            }
        }

        public void StopClient()
        {
            Client.StopBroadcast();
        }
    }
    

    public class NetServer : NetworkDiscovery
    {
        // Use this for initialization
        void Awake()
        {
            useNetworkManager = true;
            showGUI = false;
            StartServer();
        }

        //Call to create a server
        public void StartServer()
        {
            int serverPort = CreateServer();
            if (serverPort != -1)
            {
                Debug.Log("Server created on port : " + serverPort);
                broadcastData = serverPort.ToString();
                Initialize();
                StartAsServer();
            }
            else
            {
                Debug.Log("Failed to create Server");
            }
        }

        public void StopServer()
        {
            StopBroadcast();
        }

        const int _minPort = 8082;
        const int _maxPort = 8888;
        const int _defaultPort = 8081;

        public static int Port
        {
            get { return 8080; }
        }

        //Creates a server then returns the port the server is created with. Returns -1 if server is not created
        private int CreateServer()
        {
            int serverPort = -1;
            //Connect to default port
            bool serverCreated = NetworkServer.Listen(_defaultPort);
            if (serverCreated)
            {
                serverPort = _defaultPort;
                Debug.Log("Server Created with deafault port");
            }
            else
            {
                Debug.Log("Failed to create with the default port");
                //Try to create server with other port from min to max except the default port which we trid already
                for (int tempPort = _minPort; tempPort <= _maxPort; tempPort++)
                {
                    //Skip the default port since we have already tried it
                    if (tempPort != _defaultPort)
                    {
                        //Exit loop if successfully create a server
                        if (NetworkServer.Listen(tempPort))
                        {
                            serverPort = tempPort;
                            break;
                        }

                        //If this is the max port and server is not still created, show, failed to create server error
                        if (tempPort == _maxPort)
                        {
                            Debug.LogError("Failed to create server");
                        }
                    }
                }
            }
            return serverPort;
        }
    }

    public class NetServerSingleton : Singleton<NetServerSingleton>
    {
        private NetServer _server;

        private NetServer Server
        {
            get { return _server ?? (_server = gameObject.AddComponent<NetServer>()); }
        }

        public void StartServer()
        {
            if (!Server.running)
                Server.StartServer();
        }

        public void StopServer()
        {
            Server.StopBroadcast();
        }
    }
}
