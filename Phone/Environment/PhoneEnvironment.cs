﻿using Microsoft.Phone.Net.NetworkInformation;

namespace MyToolkit.Environment
{
	public static class PhoneEnvironment
	{
		public static bool IsMobileConnected
		{
			get
			{
				return NetworkInterface.NetworkInterfaceType == NetworkInterfaceType.MobileBroadbandGsm ||
					NetworkInterface.NetworkInterfaceType == NetworkInterfaceType.MobileBroadbandCdma;
			}
		}

		public static bool IsWirelessConnected
		{
			get
			{
				return NetworkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211;
			}
		}

		public static bool IsConnected
		{
			get { return NetworkInterface.NetworkInterfaceType != NetworkInterfaceType.None; }
		}
	}
}