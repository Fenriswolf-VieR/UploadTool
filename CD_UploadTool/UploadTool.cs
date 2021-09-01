using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Text;
using System.Xml;
using Limilabs.FTP.Client;

namespace CD_UploadTool
{
    class UploadTool
    {
        //bsp: t=“renew“ pw=“3ra96erznG/e“ un=“203390-releases-intern“ host=“ftp.copadata.at“ folder=“/Dokumentation/1000SP0“
        public static void renew(string username, string password, string host, string folder)
        {
            string local_save = "C:\\ProgramData\\LANGUAGES.xml";
            using (Ftp client = new Ftp())
            {
                // Use this line to validate self-signed certificates:
                client.ServerCertificateValidate += ValidateCertificate;

                client.Connect(host);
                client.AuthTLS();
                client.Login(username, password);

                WriteXml(client, folder, local_save);
                UploadTrueFilename(client,folder,local_save);
                File.Delete(local_save);

                Console.WriteLine("Renewing of xml successful!");
                client.Close();
            }
        }

        //bsp: t=“upload“ pw=“3ra96erznG/e“ un=“203390-releases-intern“ host=“ftp.copadata.at“ path=“/Dokumentation/1000SP0/Test_upload“ src=“G:\zenon1000\FINAL\HELP\Supervisor\en.cdhelp“
        public static void upload(string username, string password, string host, string path, string src)
        {
            string[] file = src.Split('/', '\\');
            string filepath = path + "/" + file[file.Length - 1];
            using (Ftp client = new Ftp())
            {
                // Use this line to validate self-signed certificates:
                client.ServerCertificateValidate += ValidateCertificate;

                client.Connect(host);
                client.AuthTLS();
                client.Login(username, password);

                UploadTrueFilename(client, path, src);

                Console.WriteLine("Uploading of file successful!");
                client.Close();
            }
        }

        private static void WriteXml(Ftp client, string folder, string save)
        {
            XmlWriterSettings oSettings = new XmlWriterSettings();
            oSettings.Indent = true;
            oSettings.OmitXmlDeclaration = false;
            oSettings.Encoding = Encoding.UTF8;
            using (XmlWriter writer = XmlWriter.Create(save, oSettings))
            {
                writer.WriteStartDocument(false);
                writer.WriteStartElement("Languages");
                GetSubelements(client, folder, writer);
                writer.WriteEndDocument();
                writer.Flush();
            }
        }

        private static void GetSubelements(Ftp client, string folder, XmlWriter writer)
        {
            client.ChangeFolder(folder);

            foreach (FtpItem item in client.GetList())
            {
                if (item.IsFolder == true)
                {
                    if (HasFile(client, item.Name))
                    {
                        if(GetGroup(item.Name) != "")
                        {
                            writer.WriteStartElement(GetGroup(item.Name));
                            writer.WriteStartAttribute("dir");
                            writer.WriteValue(item.Name);
                            writer.WriteEndAttribute();
                        }else
                        {
                            writer.WriteStartElement(item.Name);
                        }
                        GetSubelements(client, item.Name,writer);
                    }  
                }
                else if((item.Name != ".")&&(item.Name != "..")&&(item.Name != "LANGUAGES.xml"))
                {
                    writer.WriteStartElement("file");
                    writer.WriteStartAttribute("name");
                    writer.WriteValue(item.Name);
                    writer.WriteEndAttribute();
                    writer.WriteStartAttribute("size");
                    writer.WriteValue(item.Size);
                    writer.WriteEndAttribute();
                    writer.WriteStartAttribute("date");
                    writer.WriteValue(WithoutTimeZone(item.ModifyDate));
                    writer.WriteEndAttribute();
                    writer.WriteEndElement();
                }
            }
            writer.WriteEndElement();
            client.ChangeFolderUp();
        }

        private static bool HasFile(Ftp client, string folder)
        {
            List<string> subfolders = new List<string>();
            client.ChangeFolder(folder);
            foreach (FtpItem item in client.GetList())
            {
                if (item.IsFolder)
                {
                    subfolders.Add(item.Name);
                }
                else if ((item.Name != ".") && (item.Name != "..") && (item.Name != "LANGUAGES.xml"))
                {
                    client.ChangeFolderUp();
                    return true;
                }
            }
            for (int i = 0; i < subfolders.Count; i++)
            {
                if (HasFile(client, subfolders[i]))
                {
                    client.ChangeFolderUp();
                    return true;
                }
            }
            client.ChangeFolderUp();
            return false;
        }

        private static void ValidateCertificate(object sender,ServerCertificateValidateEventArgs e)
        {
            const SslPolicyErrors ignoredErrors =
                SslPolicyErrors.RemoteCertificateChainErrors |
                SslPolicyErrors.RemoteCertificateNameMismatch;

            if ((e.SslPolicyErrors & ~ignoredErrors) == SslPolicyErrors.None)
            {
                e.IsValid = true;
                return;
            }
            e.IsValid = false;
        }

        private static string GetGroup(string folder)
        {
            folder = folder.ToLower();
            string group = "";

            if (folder.Contains("zippedhelp"))
            {
                group = "zippedHelp";
            }else if ((folder.Contains("revi"))||(folder.Contains("release")))
            {
                group = "ReleaseNotes";
            }else if (folder.Contains("onlinehelp"))
            {
                group = "OnlineHelp";
            }else if ((folder.Contains("manu")) || (folder.Contains("handbuch")))
            {
                group = "Manual";
            }else if ((folder.Contains("driver")) || (folder.Contains("treiber")))
            {
                group = "Driver";
            }else if (folder.Contains("tutorials"))
            {
                group = "Tutorials";
            }

            return group;
        }

        private static void UploadTrueFilename(Ftp client, string path, string source)
        {
            string[] file = source.Split('/', '\\');
            string filepath = path + "/" + file[file.Length - 1];
            string wrong_name = path + "/FtpTrial-" + file[file.Length - 1];
            client.Upload(filepath, source);
            if(client.FileExists(filepath) == false)
            {
                client.Rename(wrong_name, filepath);
            }
        }

        public static DateTime WithoutTimeZone(DateTime dateTime)
        {
            if (dateTime.Kind != DateTimeKind.Unspecified)
                return new DateTime(dateTime.Ticks, DateTimeKind.Unspecified);

            return dateTime;
        }
    }
}
