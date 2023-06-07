﻿namespace Fuine.Helpers;

public class RandomPortHelper
{
    private static HashSet<int> used = new();

    private static int GetRandomPort(int min = 6000, int max = 9000)
    {
        var random = new Random();

        var port = random.Next(min, max);

        while (used.Contains(port))
        {
            port = random.Next(min, max);
        }

        return port;
    }

    public static void GetUsedPort()
    {
        used = new(IPGlobalProperties.GetIPGlobalProperties().
            GetActiveTcpListeners().Select(_ => _.Port));
    }

    public static int TryUsePort(int port)
    {
        return used.Contains(port) ? GetRandomPort() : port;
    }
}

