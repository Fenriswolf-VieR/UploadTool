using System;

namespace CD_UploadTool
{
    class UserInterface_UploadTool
    {
        static void Main(string[] args)
        {
            bool endApp = false;
            Console.WriteLine("CD_UploadTool\r");
            Console.WriteLine("------------------------\n");

            while (!endApp)
            {
                try
                {
                    string input = "";
                    string type = "";
                    string username = "";
                    string password = "";
                    string host = "";
                    string folder = "";
                    string path = "";
                    string source = "";
                    Console.Write("Uploadtool>");
                    input = Console.ReadLine();
                    if ((input == "/h")||(input == "/H")||(input == "/help")||(input == "/Help")||(input == "/?"))
                    {
                        //Console.WriteLine("Here could your help text be shown! ...For the right Price of course ;) ");
                        Console.WriteLine("Verwenden Sie /h oder /H oder /help oder /Help oder /? um diese Hilfe aufzurufen.");
                        Console.WriteLine(" ");
                        Console.WriteLine("Verwenden Sie Leave oder leave um zu beenden.");
                        Console.WriteLine(" ");
                        Console.WriteLine("Verwenden Sie t=“renew“ sowie ihr passwort als pw=“secure_passwort“ , ihren Benutzernamen als un=“Benutzer-name-secure“ , den ftp-host als host=“ftp host“ , den Ordner auf dem ftp server auf dem die xml datei erstellt bzw erneuert werden soll als folder=“/folder/ist/ordner“ um eine xml datei zu erneuern. Alle eingaben in beliebiger Reihenfolge in einer Schlange und durch Leerzeichen getrennt.");
                        Console.WriteLine("---");
                        Console.WriteLine("Beispielsweise:   t=“renew“ pw=“secure_passwort“ un=“Benutzer-name-secure“ host=“ftp host“ folder=“/folder/ist/ordner“");
                        Console.WriteLine(" ");
                        Console.WriteLine("Verwenden Sie t=“upload“ sowie ihr passwort als pw=“secure_passwort“ , ihren Benutzernamen als un=“Benutzer-name-secure“ , den ftp-host als host=“ftp host“ , den Ordner auf dem ftp server auf dem die datei hochgeladen bzw ersetzt werden soll als path=“/folder/ist/ordner“ , den Pfad auf dem die Datei auf dem Computer lokal zu finden ist als src=“C:/folder/ist/ordner/example.datei“ um eine datei hochzuladen. Alle eingaben in beliebiger Reihenfolge in einer Schlange und durch Leerzeichen getrennt.");
                        Console.WriteLine("---");
                        Console.WriteLine("Beispielsweise:   t=“upload“ pw=“secure_passwort“ un=“Benutzer-name-secure“ host=“ftp host“ path=“/folder/ist/ordner“ src=“C:/folder/ist/ordner/example.datei“");
                        Console.WriteLine(" ");
                    }
                    else if((input == "Leave")||(input == "leave"))
                    {
                        endApp = true;
                    }
                    else
                    {
                        String[] inputs = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                        for(int i = 0; i < inputs.Length; i++)
                        {
                            switch (inputs[i].ToLower().Split("=",StringSplitOptions.RemoveEmptyEntries)[0])
                            {
                                case "t":
                                    if (type != "")
                                    {
                                        throw new Exception();
                                    }
                                    else
                                    {
                                        type = inputs[i].Remove(inputs[i].Length - 1, 1).Substring(3);
                                        break;
                                    }

                                case "un":
                                    if(username != "")
                                    {
                                        throw new Exception();
                                    }
                                    else
                                    {
                                        username = inputs[i].Remove(inputs[i].Length-1, 1).Substring(4);
                                        break;
                                    }
                                
                                case "pw":
                                    if (password != "")
                                    {
                                        throw new Exception();
                                    }
                                    else
                                    {
                                        password = inputs[i].Remove(inputs[i].Length - 1, 1).Substring(4);
                                        break;
                                    }

                                case "host":
                                    if (host != "")
                                    {
                                        throw new Exception();
                                    }
                                    else
                                    {
                                        host = inputs[i].Remove(inputs[i].Length - 1, 1).Substring(6);
                                        break;
                                    }

                                case "folder":
                                    if (folder != "")
                                    {
                                        throw new Exception();
                                    }
                                    else
                                    {
                                        folder = inputs[i].Remove(inputs[i].Length - 1, 1).Substring(8);
                                        break;
                                    }

                                case "path":
                                    if (path != "")
                                    {
                                        throw new Exception();
                                    }
                                    else
                                    {
                                        path = inputs[i].Remove(inputs[i].Length - 1, 1).Substring(6);
                                        break;
                                    }

                                case "src":
                                    if (source != "")
                                    {
                                        throw new Exception();
                                    }
                                    else
                                    {
                                        source = inputs[i].Remove(inputs[i].Length - 1, 1).Substring(5);
                                        break;
                                    }
                            }
                        }

                        if(type == "renew")
                        {
                            if((password == "") ||(username == "") ||(host == "") ||(folder == ""))
                            {
                                throw new Exception();
                            }
                            else
                            {
                                UploadTool.renew(username,password,host,folder);
                            }
                        }else if(type == "upload")
                        {
                            if ((password == "") ||(username == "") ||(host == "") ||(path == "") ||(source == ""))
                            {
                                throw new Exception();
                            }
                            else
                            {
                                UploadTool.upload(username,password,host,path,source);
                            }
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                }
                catch (Exception g)
                {
                    Console.WriteLine("Error! Haben Sie vielleicht eine falsche Eingabe getätigt?");
                    Console.WriteLine("Bitte überprüfen Sie ihre Eingabe und/oder verwenden Sie /? für Hilfe bezüglich der Parameter.");
                }
            }
            return;
        }
    }
}
