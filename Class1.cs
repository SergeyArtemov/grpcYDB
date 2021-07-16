/*public static async Task Main1(string[] args)
{
    await FullFrameworkSample();
}

private static async Task FullFrameworkSample()
{
    Uri host = new Uri("https://sv-xxx-dev-cpu-01:44301");
    int port = host.Port;

    (string publicKeyInPemFormat, string commonName) = await GetCertificateInformationFromServer(host);

    //note: in the full framework implementation it's very important that the casing of the target is correct (the same casing as in the CN name of the certificate)
    string target = $"{commonName}:{port}";

    //note: thats only needed in our case, because we have a server side interceptor, that checks if the secureKey is valid.
    CallCredentials credentials = CallCredentials.FromInterceptor((context, metadata) =>
    {
        metadata.Add("SecurityTokenId", "someSecureKey");

        return Task.CompletedTask;
    });

    ChannelCredentials channelCredentials = ChannelCredentials.Create(new SslCredentials(publicKeyInPemFormat), credentials);

    Channel channel = new Channel(target, channelCredentials);

    ProjectInlayDataService.ProjectInlayDataServiceClient client = new ProjectInlayDataService.ProjectInlayDataServiceClient(channel);

    GetProjectInlayDataResponse result = await client.GetProjectInlayDataAsync(new GetProjectInlayDataRequest());

    await channel.ShutdownAsync();

    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
}

private static async Task<(string PublicKeyInPemFormat, string CommonName)> GetCertificateInformationFromServer(Uri host)
{
    Regex commonNameRegex = new Regex("CN=([\\w\\-.]*),?", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    StringBuilder builder = new StringBuilder();
    const string newline = "\n";

    X509Certificate certFromServer;

    using (HttpClient client = new HttpClient())
    {
        using (HttpResponseMessage _ = await client.GetAsync(host))
        {
            //get the certificate from the server, so we don't need to store the pem.
            certFromServer = ServicePointManager.FindServicePoint(host).Certificate;
            if (certFromServer == null)
                throw new InvalidOperationException($"Could not get certificate from server ({host}).");
        }
    }

    Match match = commonNameRegex.Match(certFromServer.Subject);
    if (!match.Success)
        throw new InvalidOperationException($"Could not extract CN (Common Name) from server certificate ({certFromServer.Subject}).");

    string commonName = match.Groups[1].Captures[0].Value;

    X509Certificate2 certificate = new X509Certificate2(certFromServer);
    string pem = ExportToPem(certificate);

    builder.AppendLine(
        "# Issuer: " + certificate.Issuer + newline +
        "# Subject: " + certificate.Subject + newline +
        "# Label: " + certificate.FriendlyName + newline +
        "# Serial: " + certificate.SerialNumber + newline +
        "# SHA1 Fingerprint: " + certificate.GetCertHashString() + newline +
        pem + newline);

    return (builder.ToString(), commonName);
}

/// <summary>
/// Export a certificate to a PEM format string
/// </summary>
/// <param name="cert">The certificate to export</param>
/// <returns>A PEM encoded string</returns>
private static string ExportToPem(X509Certificate cert)
{
    StringBuilder builder = new StringBuilder();

    builder.AppendLine("-----BEGIN CERTIFICATE-----");

    builder.AppendLine(Convert.ToBase64String(cert.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks));
    builder.AppendLine("-----END CERTIFICATE-----");

    return builder.ToString();
}
    }
*/