using System;
using System.ComponentModel;

#if ENABLE_UNET

namespace UnityEngine.Networking
{
    [AddComponentMenu("Network/MyNetworkManagerHUD")]
    [RequireComponent(typeof(NetworkManager))]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class MyNetworkManagerHUD : MonoBehaviour
    {
        private CustomNetworkManager manager;
        [SerializeField]
        public bool showGUI = true;
        [SerializeField]
        public int offsetX;
        [SerializeField]
        public int offsetY;

        private int buttonHeight;
        private int buttonWidth;
        private int textureSize;

        private bool malkousSelect;
        private bool bahramSelect = true;


        String heroName = "hero name";

        float sliderValue;

        public Texture bahramTexture, malkousTexture;

        CustomNetworkDiscovery networkDiscovery;

        private int playerCount = 1;

        // Runtime variable
        bool m_ShowServer;
        bool gameStarted;

        public void OnHostFound(string fromAddress, string data)
        {
            //networkDiscovery.StopBroadcast();
            String ip = fromAddress.Substring(fromAddress.LastIndexOf(':') + 1);
            Debug.Log(ip);
            manager.networkAddress = ip;
            manager.StartClient();
        }

        void Awake()
        {
            manager = GetComponent<CustomNetworkManager>();
            networkDiscovery = GetComponent<CustomNetworkDiscovery>();
            buttonHeight = Screen.height * 1 / 10;
            buttonWidth = Screen.width * 1 / 5;
            textureSize = Screen.width * 1 / 10;

        }

        void Start()
        {
            networkDiscovery.action += OnHostFound;
        }

        void Update()
        {
            if (!showGUI)
                return;
            if (NetworkServer.active && manager.IsClientConnected())
            {
                if (Input.GetKeyDown(KeyCode.X))
                {
                    manager.StopHost();
                }
            }
        }

        void OnGUI()
        {
            if (!showGUI)
                return;
            GUI.skin.label.fontSize = (int) (Screen.width) /100 + 6;
            GUI.skin.button.fontSize = (int)(Screen.width) / 100 + 6;

            int xpos = 20;
            int ypos = 5;
            const int spacing = 24;

            bool noConnection = (manager.client == null || manager.client.connection == null || manager.client.connection.connectionId == -1);

            if (!manager.IsClientConnected() && !NetworkServer.active && manager.matchMaker == null)
            {
                if (noConnection)
                {
                    if (UnityEngine.Application.platform != RuntimePlatform.WebGLPlayer)
                    {
                        if (GUI.Button(new Rect(Screen.width * 11 / 40, Screen.height * 5 / 10, buttonWidth, buttonHeight), "LAN Host"))
                        {
                            if (networkDiscovery.isClient || networkDiscovery.isServer)
                                networkDiscovery.StopBroadcast();
                            //networkDiscovery.Initialize();
                            networkDiscovery.StartAsServer();
                            manager.StartHost(playerCount);
                        }
                    }

                    if (GUI.Button(new Rect(Screen.width * 13 / 40 + buttonWidth, Screen.height * 5 / 10, buttonWidth, buttonHeight), "LAN Client"))
                    {
                        //if (networkDiscovery.isServer || networkDiscovery.isClient)
                            //networkDiscovery.StopBroadcast();
                        networkDiscovery.Initialize();
                        networkDiscovery.StartAsClient();
                    }


                   // ypos += spacing;

                    GUI.Label(new Rect(0.45f * Screen.width, 0.2f * Screen.height, 200, 50), "Choose Hero");
                    //ypos += spacing;

                    

                    GUI.Label(new Rect(0.375f * Screen.width,0.42f * Screen.height, 105, 50), "Malkous");
                    GUI.Label(new Rect(0.575f * Screen.width, 0.42f * Screen.height, 105, 50), "Bahram");
                    //ypos += spacing;

                    malkousSelect = GUI.Toggle(new Rect(7f / 20f * Screen.width, 5f / 20f * Screen.height, textureSize, textureSize),malkousSelect, malkousTexture);
                    if (malkousSelect)
                    {
                        bahramSelect = false;
                        manager.playerNumber = 0;
                    }
                    bahramSelect = GUI.Toggle(new Rect(11f / 20f * Screen.width, 5f / 20f * Screen.height, textureSize,textureSize),bahramSelect, bahramTexture);
                    if (bahramSelect)
                    {
                        malkousSelect = false;
                        manager.playerNumber = 1;
                    }

                    ypos += spacing * 2;
                    if(GUI.Button(new Rect(Screen.width * 13 / 40 + buttonWidth, Screen.height * 6.2f / 10, buttonWidth, buttonHeight), "Exit")){
                        Debug.Log("quiting");
                        Application.Quit();
                    }
                }
                else
                {
                    GUI.Label(new Rect(xpos, ypos, 200, 20), "Connecting to " + manager.networkAddress + ":" + manager.networkPort + "..");
                    ypos += spacing;


                    if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Cancel Connection Attempt"))
                    {
                        manager.StopClient();
                    }
                }
            }
            else
            {
                if (NetworkServer.active)
                {
                    string serverMsg = "Server: port=" + manager.networkPort;
                    if (manager.useWebSockets)
                    {
                        serverMsg += " (Using WebSockets)";
                    }
                    GUI.Label(new Rect(xpos, ypos, 300, 20), serverMsg);
                    ypos += spacing;
                    if (!gameStarted)
                    {
                        if (GUI.Button(new Rect(xpos, ypos + 60, 100, 20), "Start the Game"))
                        {
                            gameStarted = true;
                            ServerManager.instance.StartGame();
                        }
                    }
                }
                if (manager.IsClientConnected())
                {
                    GUI.Label(new Rect(xpos, ypos, 300, 20), "Client: address=" + manager.networkAddress + " port=" + manager.networkPort);
                    ypos += spacing;
                }
            }

            if (manager.IsClientConnected() && !ClientScene.ready)
            {
                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Client Ready"))
                {
                    ClientScene.Ready(manager.client.connection);

                    if (ClientScene.localPlayers.Count == 0)
                    {
                        ClientScene.AddPlayer(0);
                    }
                }
                ypos += spacing;
            }

            if (NetworkServer.active || manager.IsClientConnected())
            {
                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Stop (X)"))
                {
                    manager.StopHost();
                }
                ypos += spacing;
            }
        }
    }
}
#endif //ENABLE_UNET