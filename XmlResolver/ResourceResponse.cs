namespace XmlResolver;

public class ResourceResponse
{
    public readonly ResourceRequest Request;

    public ResourceResponse(ResourceRequest request) : this(request, false)
    {
        // nop
    }

    public ResourceResponse(ResourceRequest request, bool rejected)
    {
        Request = request;
        IsRejected = rejected;
    }

    public ResourceResponse(ResourceRequest request, Uri? uri)
    {
        Request = request;
        Uri = uri;
        ResolvedUri = uri;
        IsRejected = false;
        IsResolved = uri != null;
    }

    public Uri? Uri { get; internal set; } = null;
    public Uri? ResolvedUri { get; internal set; } = null;
    public bool IsRejected { get; internal set; } = false;
    public bool IsResolved { get; internal set; } = false;
    public string? ContentType { get; internal set; } = null;
    public Stream? Stream { get; internal set; } = null;
    public int StatusCode { get; internal set; } = -1;
    public Dictionary<string, List<String>> Headers { get; internal set; } = new Dictionary<string, List<string>>();

}