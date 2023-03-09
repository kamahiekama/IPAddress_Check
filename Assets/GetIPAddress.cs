using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System;

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
            List<string> getAddresses = GetValidIpAddress();
            string s = "";
            for(int i = 1; i < getAddresses.Count; i++)
            {
                s += getAddresses[i] + "\r\n";
            }
            s += "\r\n\r\nDebug Info\r\n" + getAddresses[0];

            text.text = s;
            lastUpdate = current;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>
    /// 0: Debug Log
    /// 1~: IP Address (óDêÊìxèá)
    /// </returns>
    public static List<string> GetValidIpAddress()
    {
        StringBuilder sb = new StringBuilder();

        // 192.168 ÇÃóLê¸
        List<string> addresses0 = new List<string>();
        // 192.168 ÇÃñ≥ê¸
        List<string> addresses1 = new List<string>();
        // ÇªÇÃëºÇÃóLê¸
        List<string> addresses2 = new List<string>();
        // ÇªÇÃëºÇÃñ≥ê¸
        List<string> addresses3 = new List<string>();

        var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

        foreach (var networkInterface in networkInterfaces)
        {
            sb.Append("----------------------------------------------------");
            sb.Append("status : " + networkInterface.OperationalStatus + "\r\n");

            string connectionType = networkInterface.NetworkInterfaceType.ToString();

            bool isCable = false;
            switch (networkInterface.NetworkInterfaceType)
            {
                case NetworkInterfaceType.Wireless80211:
                    connectionType = "Wi-Fi";
                    break;
                case NetworkInterfaceType.Ethernet:
                case NetworkInterfaceType.GigabitEthernet:
                    connectionType = "Ethernet";
                    isCable = true;
                    break;
            }

            sb.Append("connect type : " + connectionType + "\r\n");

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
                    string addr = address.Address.ToString();
                    sb.Append("Status OK : " + addr + "\r\n");

                    if (addr.StartsWith("192.168."))
                    {
                        if (isCable)
                        {
                            addresses0.Add(addr);
                        }
                        else
                        {
                            addresses1.Add(addr);
                        }
                    }
                    else
                    {
                        if (isCable)
                        {
                            addresses2.Add(addr);
                        }
                        else
                        {
                            addresses3.Add(addr);
                        }
                    }
                } else
                {
                    sb.Append("Status NG : " + address.Address.ToString() + "\r\n");
                }
            }
        }

        List<string> result = new List<string>();

        result.Add(sb.ToString());
        result.AddRange(addresses0);
        result.AddRange(addresses1);
        result.AddRange(addresses2);
        result.AddRange(addresses3);

        return result;
    }
}
