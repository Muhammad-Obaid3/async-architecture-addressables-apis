
[System.Serializable]
public abstract class AModelV1
{
    public int statusCode;
    public string message;
}

[System.Serializable]
public class DownloadReq : AModelV1
{
    public string url;
    public string filePath;
}
[System.Serializable]
public class DownloadRes : AModelV1
{
    public string url;
    public string filePath;
}