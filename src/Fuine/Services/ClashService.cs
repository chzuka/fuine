namespace Fuine.Services;

public static class ClashService
{
    #region HttpClient
    private static Dictionary<string, Proxy> Proxies { get; set; } = new();

    private static readonly HttpClient client = new(
        new HttpClientHandler
        {
            UseProxy = false,
            UseCookies = false,
        });
    #endregion

    #region GetAsync
    private static async Task<TValue?> GetAsync<TValue>(string url)
    {
        try
        {
            return await client.GetFromJsonAsync<TValue>(url);
        }
        catch (Exception)
        {
            return default;
        }
    }
    #endregion

    #region PutAsync
    private static async Task PutAsync<TValue>(string url, TValue value)
    {
        await client.PutAsJsonAsync(url, value);
    }
    #endregion

    #region PatchAsync
    private static async Task PatchAsync(string url, Dictionary<string, dynamic> pairs)
    {
        await client.PatchAsJsonAsync(url, pairs);
    }
    #endregion

    #region DeleteAsync
    private static async Task DeleteAsync(string url)
    {
        await client.DeleteAsync(url);
    }
    #endregion

    #region 递归获取延迟
    private static int? GecursiveDelay(Dictionary<string, ClashProxy> proxies, string name)
    {
        if (proxies[name].Now == null)
        {
            return proxies[name].History.LastOrDefault()?.Delay;
        }
        else
        {
            return GecursiveDelay(proxies, proxies[name].Now!);
        }
    }
    #endregion

    #region 获取代理
    private static async Task GetProxiesAsync()
    {
        var content = await GetAsync<ClashProxyData>($"{Global.Clash配置.Clash外部控制地址}/proxies");

        Dictionary<string, Proxy> proxies = new();
        foreach (var _ in content!.Proxies)
        {
            proxies.Add(_.Key, new()
            {
                Name = _.Value.Name,
                Now = _.Value.Now,
                Type = _.Value.Type,
                All = _.Value.All,
                Delay = GecursiveDelay(content.Proxies, _.Key),
            });
        }

        Proxies = proxies;
    }
    #endregion

    #region 获取代理组
    public static async Task<Dictionary<string, ProxyGroup>> GetGroupsAsync()
    {
        await GetProxiesAsync();

        Dictionary<string, ProxyGroup> groups = new();

        foreach (var proxy in Proxies)
        {
            if (proxy.Value.All == null)
            {
                continue;
            }

            if (Global.Clash配置.Mode == ClashModeType.Global ?
                proxy.Key != "GLOBAL" : proxy.Key == "GLOBAL")
            {
                continue;
            }

            groups.Add(proxy.Key, new()
            {
                Name = proxy.Value.Name,
                Now = proxy.Value.Now!,
                Type = proxy.Value.Type,
                Delay = proxy.Value.Delay,
                Proxies = Proxies.Values.
                Where(x => proxy.Value.All!.
                Any(y => y == x.Name)).
                OrderByDescending(_ => _.Name == proxy.Value.Now).
                ThenBy(_ => _.Delay == 0 ? 9000 : _.Delay).
                ToList(),
            });
        }

        return groups.
            OrderBy(_ => Proxies["GLOBAL"].All?.
            IndexOf(_.Key)).
            ToDictionary(_ => _.Key, _ => _.Value);
    }
    #endregion

    #region 获取配置
    public static async Task GetConfigAsync()
    {
        var configs = await GetAsync<ClashConfigs>($"{Global.Clash配置.Clash外部控制地址}/configs");

        if (configs == null)
        {
            return;
        }

        Global.Clash配置.AllowLan = configs.AllowLan;
        Global.Clash配置.IPV6 = configs.Ipv6;
        Global.Clash配置.LogLevel = configs.LogLevel;
        Global.Clash配置.Mode = configs.Mode;
        Global.Clash配置.TunMode = configs.Tun.Enable;

        if (configs.MixedPort == Global.Fuine配置.MixedPort)
        {
            Global.Clash配置.MixedPort = configs.MixedPort;
        }
        else
        {
            await PatchConfigAsync("mixed-port",
                Global.Clash配置.MixedPort =
                RandomPortHelper.TryUsePort(Global.Fuine配置.MixedPort));
        }
    }
    #endregion

    #region 修改配置
    public static async Task PatchConfigAsync(string key, dynamic value)
    {
        Dictionary<string, dynamic> pairs = new()
        {
            { key, value }
        };

        await PatchAsync($"{Global.Clash配置.Clash外部控制地址}/configs", pairs);
    }
    #endregion

    #region 延迟测试
    private static int isRunning = 0;
    public static async Task TestDelay()
    {
        if (Interlocked.CompareExchange(ref isRunning, 1, 0) == 0)
        {
            try
            {
                await GetAsync<object?>($"{Global.Clash配置.Clash外部控制地址}/group/GLOBAL/delay?timeout=5000&url={Global.Fuine配置.DelaySpeedTestUrl}");
            }
            finally
            {
                Interlocked.Exchange(ref isRunning, 0);
            }
        }
    }
    #endregion

    #region 切换代理
    public static async Task SelectProxy(string group, string name)
    {
        await PutAsync($"{Global.Clash配置.Clash外部控制地址}/proxies/{group}", new { name });
    }
    #endregion

    #region 重载配置
    public static async Task ReloadConfigs(string path, bool force = false)
    {
        await PutAsync($"{Global.Clash配置.Clash外部控制地址}/configs", new { force, path });
    }
    #endregion

    #region 关闭连接
    public static async Task CloseConnection(string? id = null)
    {
        await DeleteAsync($"{Global.Clash配置.Clash外部控制地址}/connections/{id}");
    }
    #endregion

    #region WebSocket
    private static async IAsyncEnumerable<TValue> GetStreamAsync<TValue>(string url)
    {
        using ClientWebSocket socket = new();
        socket.Options.Proxy = null;

        try
        {
            await socket.ConnectAsync(new Uri(url), CancellationToken.None);

            WebSocketReceiveResult result;

            var bytes = ArrayPool<byte>.Shared.Rent(1024);

            StringBuilder builder = new();

            while (!socket.CloseStatus.HasValue)
            {
                builder.Clear();

                do
                {
                    result = await socket.ReceiveAsync(new ArraySegment<byte>(bytes), CancellationToken.None);

                    builder.Append(Encoding.UTF8.GetString(bytes, 0, result.Count));

                    ArrayPool<byte>.Shared.Return(bytes);

                } while (!result.EndOfMessage);

                yield return JsonSerializer.Deserialize<TValue>(builder.ToString())!;

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    break;
                }
            }
        }
        finally
        {
            // 关闭并释放WebSocket客户端
            await socket.CloseAsync(
                WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
            socket.Dispose();
        }
    }
    #endregion

    #region 实时日志
    public static async IAsyncEnumerable<Log> GetRealTimeLogs()
    {
        await foreach (var _ in GetStreamAsync<ClashLog>($"{Global.Clash配置.ClashWebSocket控制地址}/logs"))
        {
            yield return new Log()
            {
                Type = _.Type,
                PayLoad = _.PayLoad
            };
        }
    }
    #endregion

    #region 实时连接
    public static async IAsyncEnumerable<ConnectionInfo> GetRealTimeConnections()
    {
        await foreach (var info in GetStreamAsync<ClashConnectionInfo>($"{Global.Clash配置.ClashWebSocket控制地址}/connections"))
        {
            List<Connection> connections = new();
            connections.AddRange(info.Connections.
                Select(_ => new Connection()
                {
                    Host = $"{(
                    _.MetaData?.Host != string.Empty ?
                    _.MetaData?.Host :
                    _.MetaData?.DestinationIP)}:{_.MetaData?.DestinationPort}",
                    Upload = ConvertBytesHelper.ConvertBytes(_.Upload),
                    Download = ConvertBytesHelper.ConvertBytes(_.Download),
                    Rule = $"{_.Rule}{(_.RulePayload == null ? null : $"({_.RulePayload})")}",
                    Chains = _.Chains?.Aggregate((x, y) => $"{y}=>{x}"),
                    Process = _.MetaData?.Process,
                }));

            yield return new()
            {
                UploadTotal = info.UploadTotal,
                DownloadTotal = info.DownloadTotal,
                Connections = connections,
            };
        }
    }
    #endregion

    #region 实时流量
    public static IAsyncEnumerable<ClashTraffic> GetRealTimeTraffic()
    {
        return GetStreamAsync<ClashTraffic>($"{Global.Clash配置.ClashWebSocket控制地址}/traffic");
    }
    #endregion

}
