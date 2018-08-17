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

        string maxPlayerCount = "1";

        private int playerCount = 1;
        public int maxPlayer;

        // Runtime variable
        bool m_ShowServer;

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

            if (!manager.IsClientConnected() && !NetworkServer.active && manager.matchMaker == null)
            {
                if (UnityEngine.Application.platform != RuntimePlatform.WebGLPlayer)
                {
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        //manager.StartServer();
                    }
                    if (Input.GetKeyDown(KeyCode.H))
                    {
                        //manager.StartHost();
                    }
                }
                if (Input.GetKeyDown(KeyCode.C))
                {
                    //manager.StartClient();
                }
            }
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

            bool noConnection = (manager.client == null || manager.client.connection == null ||
                                 manager.client.connection.connectionId == -1);

            if (!manager.IsClientConnected() && !NetworkServer.active && manager.matchMaker == null)
            {
                if (noConnection)
                {
                    if (UnityEngine.Application.platform != RuntimePlatform.WebGLPlayer)
                    {
                        if (GUI.Button(new Rect(Screen.width * 11 / 40, Screen.height * 5 / 10, buttonWidth, buttonHeight), "LAN Host"))
                        {
                            if (networkDiscovery.isClient)
                                networkDiscovery.StopBroadcast();
                            networkDiscovery.StartAsServer();
                            manager.StartHost(playerCount);
                        }
                        //maxPlayerCount = GUI.TextField(new Rect(xpos + 200, ypos, 50, 20), maxPlayerCount);

                        GUI.Label(new Rect(Screen.width * 11 / 40, 0.605f * Screen.height, 0.11f * Screen.width, buttonHeight), "Player Count: " + playerCount);
                        playerCount =(int) GUI.HorizontalSlider(new Rect(Screen.width *0.395f,0.62f * Screen.height , 0.08f * Screen.width, buttonHeight), playerCount, 1f,maxPlayer);

                        // ypos += spacing;
                    }

                    if (GUI.Button(new Rect(Screen.width * 13 / 40 + buttonWidth, Screen.height * 5 / 10, buttonWidth, buttonHeight), "LAN Client"))
                    {
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
                    if(GUI.Button(new Rect(Screen.width * 13 / 40 + buttonWidth, Screen.height * 5 / 10 + ypos, buttonWidth, buttonHeight), "Exit")){
                        Debug.Log("quiting");
                        Application.Quit();
                    }

                    //sliderValue = GUI.HorizontalSlider(new Rect(xpos + 65, ypos, 60, 20), sliderValue, 0f, 1f);

                    //if (sliderValue > 0.5f)
                    //{
                    //    sliderValue = 1f;
                    //    if (manager.playerNumber != 1)
                    //        manager.playerNumber = 1;
                    //}
                    //if (sliderValue <= 0.5f)
                    //{
                    //    sliderValue = 0f;
                    //    if (manager.playerNumber != 0)
                    //        manager.playerNumber = 0;
                    //}


                   // ypos += spacing;

                    /*if (UnityEngine.Application.platform == RuntimePlatform.WebGLPlayer)
                    {
                        // cant be a server in webgl build
                        GUI.Box(new Rect(xpos, ypos, 200, 25), "(  WebGL cannot be server  )");
                        ypos += spacing;
                    }
                    else
                    {
                        if (GUI.Button(new Rect(xpos, ypos, 200, 20), "LAN Server Only(S)"))
                        {
                            manager.StartServer();
                        }
                        ypos += spacing;
                    }*/
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
                    networkDiscovery.StopBroadcast();
                }
                ypos += spacing;
            }

            if (!NetworkServer.active && !manager.IsClientConnected() && noConnection)
            {
                ypos += 10;

                /*if (UnityEngine.Application.platform == RuntimePlatform.WebGLPlayer)
                {
                    GUI.Box(new Rect(xpos - 5, ypos, 220, 25), "(WebGL cannot use Match Maker)");
                    return;
                }

                if (manager.matchMaker == null)
                {
                    if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Enable Match Maker (M)"))
                    {
                        manager.StartMatchMaker();
                    }
                    ypos += spacing;
                }
                else
                {
                    if (manager.matchInfo == null)
                    {
                        if (manager.matches == null)
                        {
                            if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Create Internet Match"))
                            {
                                manager.matchMaker.CreateMatch(manager.matchName, manager.matchSize, true, "", "", "", 0, 0, manager.OnMatchCreate);
                            }
                            ypos += spacing;

                            GUI.Label(new Rect(xpos, ypos, 100, 20), "Room Name:");
                            manager.matchName = GUI.TextField(new Rect(xpos + 100, ypos, 100, 20), manager.matchName);
                            ypos += spacing;

                            ypos += 10;

                            if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Find Internet Match"))
                            {
                                manager.matchMaker.ListMatches(0, 20, "", false, 0, 0, manager.OnMatchList);
                            }
                            ypos += spacing;
                        }
                        else
                        {
                            for (int i = 0; i < manager.matches.Count; i++)
                            {
                                var match = manager.matches[i];
                                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Join Match:" + match.name))
                                {
                                    manager.matchName = match.name;
                                    manager.matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, manager.OnMatchJoined);
                                }
                                ypos += spacing;
                            }

                            if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Back to Match Menu"))
                            {
                                manager.matches = null;
                            }
                            ypos += spacing;
                        }
                    }

                    if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Change MM server"))
                    {
                        m_ShowServer = !m_ShowServer;
                    }
                    if (m_ShowServer)
                    {
                        ypos += spacing;
                        if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Local"))
                        {
                            manager.SetMatchHost("localhost", 1337, false);
                            m_ShowServer = false;
                        }
                        ypos += spacing;
                        if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Internet"))
                        {
                            manager.SetMatchHost("mm.unet.unity3d.com", 443, true);
                            m_ShowServer = false;
                        }
                        ypos += spacing;
                        if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Staging"))
                        {
                            manager.SetMatchHost("staging-mm.unet.unity3d.com", 443, true);
                            m_ShowServer = false;
                        }
                    }

                    ypos += spacing;

                    GUI.Label(new Rect(xpos, ypos, 300, 20), "MM Uri: " + manager.matchMaker.baseUri);
                    ypos += spacing;

                    if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Disable Match Maker"))
                    {
                        manager.StopMatchMaker();
                    }
                    ypos += spacing;
                }*/
            }
        }
    }
}
#endif //ENABLE_UNET