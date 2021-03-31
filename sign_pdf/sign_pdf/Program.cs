using PdfSharp.Pdf;
using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using PdfSharp.Pdf.IO;
using System.IO;
using PdfSharp.Signatures;

namespace sign_pdf
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 4)
            {
                Console.WriteLine("Too few arguments. Required arguments: <original_pdf> <signed_pdf> <privatekey_file> <password> <signature_reason> <signature_location>");
                return;
            }
            string originalFile = args[0];
            string signedPdfFile = args[1];
            string privateKeyFile = args[2];
            string privateKeyPassword = args[3];
            string reason = args[4] != null ? args[4] : "";
            string location = args[5] != null ? args[5] : "";
            signPdfFile(originalFile, signedPdfFile, privateKeyFile, privateKeyPassword, reason, location);
        }
        private static void signPdfFile(string sourceDocument, string destinationPath, string privateKeyFile, string keyPassword, string reason, string location)
        {
            using (FileStream originalFileStream = new FileStream(sourceDocument, System.IO.FileMode.Open))
            {
                PdfDocument pdfDocument = PdfReader.Open(originalFileStream);

                PdfSignatureOptions options = new PdfSignatureOptions
                {
                    ContactInfo = "",
                    Location = location,
                    Reason = reason,
                    //Rectangle = new XRect(36.0, 735.0, 200.0, 50.0),
                    //AppearanceHandler = new Program.SignAppearenceHandler()
                };
                //PdfSignatureHandler pdfSignatureHandler = new PdfSignatureHandler(new DefaultSigner(Program.GetCertificate()), null, options);
                PdfSignatureHandler pdfSignatureHandler = new PdfSignatureHandler(getCertificate(privateKeyFile,keyPassword), null, options);
                pdfSignatureHandler.AttachToDocument(pdfDocument);
                pdfDocument.Save(destinationPath);
                Process.Start(destinationPath);
            }
        }

        private static X509Certificate2 getCertificate(string privateKeyFile, string privateKeyPassword)
        {
            return new X509Certificate2(privateKeyFile, privateKeyPassword, X509KeyStorageFlags.Exportable);

        }
    }
}
