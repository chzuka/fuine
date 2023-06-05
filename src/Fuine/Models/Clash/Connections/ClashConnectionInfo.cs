namespace Fuine.Models.Clash.Connections;

public class ClashConnectionInfo
{
    [JsonPropertyName("uploadTotal")]
    public long UploadTotal { get; set; }

    [JsonPropertyName("downloadTotal")]
    public long DownloadTotal { get; set; }

    [JsonPropertyName("connections")]
    public List<ClashConnection> Connections { get; set; } = null!;
}
