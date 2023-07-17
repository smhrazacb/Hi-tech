// See https://aka.ms/new-console-template for more information
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

Console.WriteLine("Hello, World!");
string[] _outDir = 
    {
    "../../../../../ident/EsparkIndent.Server/",
    "../../../../../Services/Catalog/Catalog.API/",
    "../../../../../Services/Basket/Basket.API/",
    "../../../../../Services/Ordering/Ordering.API/",
    "../../../../../Services/Webhooks/Webhooks.API/",
};
GenerateEncryptionertificate(_outDir);
GenerateSigingCertificate(_outDir);
void GenerateEncryptionertificate(string[] outDir)
{
    using var algorithm = RSA.Create(keySizeInBits: 2048);

    var subject = new X500DistinguishedName("CN=Fabrikam Encryption Certificate");
    var request = new CertificateRequest(
        subject, algorithm, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    request.CertificateExtensions.Add(
        new X509KeyUsageExtension(X509KeyUsageFlags.KeyEncipherment, critical: true));

    var certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(2));
    foreach (var item in outDir)
    {
        File.WriteAllBytes(item + "_encryption-certificate.pfx",
        certificate.Export(X509ContentType.Pfx, "123456"));
    }
}
void GenerateSigingCertificate(string[] outDir)
{
    using var algorithm = RSA.Create(keySizeInBits: 2048);

    var subject = new X500DistinguishedName("CN=Fabrikam Signing Certificate");
    var request = new CertificateRequest(
        subject, algorithm, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    request.CertificateExtensions.Add(
        new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature, critical: true));

    var certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(2));
    foreach (var item in outDir)
    {
        File.WriteAllBytes(item + "_signing-certificate.pfx",
            certificate.Export(X509ContentType.Pfx, "123456"));
    }
}
