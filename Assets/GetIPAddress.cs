using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class GetIPAddress : MonoBehaviour
{
    private Text text;

    private float lastUpdate;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        float current = Time.realtimeSinceStartup;

        if (lastUpdate + 1.0f < current)
        {
            string[] getAddresses = GetValidIpAddress();
            text.text = "Debug Info\r\n" + getAddresses[0]
                        + "\r\n\r\nResult\r\n" + getAddresses[1];
            lastUpdate = current;
        }
    }

    public static string[] GetValidIpAddress()
    {
        StringBuilder sb = new StringBuilder();
        StringBuilder sb2 = new StringBuilder();

        var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

        foreach (var networkInterface in networkInterfaces)
        {
            sb.Append("----------------------------------------------------");
            sb.Append("statjs : " + networkInterface.OperationalStatus + "\r\n");

            var ipProperties = networkInterface.GetIPProperties();
            var unicastAddresses = ipProperties.UnicastAddresses;

            foreach (var address in unicastAddresses)
            {
                if (address.Address.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    sb.Append("not InterNetwork : " + address.Address.ToString() + "\r\n");
                    continue;
                }

                if (IPAddress.IsLoopback(address.Address))
                {
                    sb.Append("IsLoopback : " + address.Address.ToString() + "\r\n");
                    continue;
                }

                if (networkInterface.OperationalStatus == OperationalStatus.Up
                    || networkInterface.OperationalStatus == OperationalStatus.Unknown)
                {
                    sb.Append("Status OK : " + address.Address.ToString() + "\r\n");
                    sb2.Append(address.Address.ToString() + "\r\n");
                } else
                {
                    sb.Append("Status NG : " + address.Address.ToString() + "\r\n");
                }
            }
        }

        string[] result = new string[2];
        result[0] = sb.ToString();
        result[1] = sb2.ToString();

        return result;
    }
}
