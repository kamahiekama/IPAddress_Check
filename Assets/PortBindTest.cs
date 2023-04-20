using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class PortBindTest : MonoBehaviour
{
    public int Port = 35900;

    public Text text;

    private UdpClient client;

    public Toggle toggle;

    // Start is called before the first frame update
    void Start()
    {
        text.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Check(){
        bool inuse = IsPortInUse(Port);

        log("in use: " + inuse);
    }

    public void Bind(){

        bool check = toggle.isOn;

        try
        {
            if (check){
                client = new UdpClient();
                client.Client.Bind(new IPEndPoint(System.Net.IPAddress.Any, Port));
                log("bind: " + Port);
            } else {
                client.Close();
                log("socket close");
            }
        }
        catch (SocketException ex) 
        {
            log(ex.StackTrace);
        }
    }

    private void log(string s){
        Debug.Log(s);
        text.text += s + "\r\n";
    }

    public static bool IsPortInUse(int port){
        UdpClient udp = new UdpClient();

        bool inuse = false;

        try
        {
            udp.Client.Bind(new IPEndPoint(System.Net.IPAddress.Any, port));
        }
        catch (SocketException ex) 
        {
            if (ex.ErrorCode == 10048){
            inuse = true;
            }
        }

        udp.Close();

        return inuse;
    }
}
