/*
 * ExtOrg - an application written in C# that organizes files based on their extensions
 * Copyright (C) 2018-2019 UnexomWid
 */
using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

namespace ExtOrg
{
    class Program
    {
        static void Main(string[] args)
        {
            bool yesToAll = false;
            bool noToAll = false;

            string app = Process.GetCurrentProcess().ProcessName + ".exe";
            string folder = AppDomain.CurrentDomain.BaseDirectory.EndsWith("/") || AppDomain.CurrentDomain.BaseDirectory.EndsWith("\\") ? AppDomain.CurrentDomain.BaseDirectory : AppDomain.CurrentDomain.BaseDirectory + "/";

            if (args.Length == 1)
            {
                string lwr = args[0].ToLower();
                if(lwr.Equals("-h") || lwr.Equals("--help"))
                {
                    Console.WriteLine("Extension Organizer (ExtOrg)\n============================\n[Description] Organizes all files in a folder by their extensions\n\n[Arguments]:\n\"FOLDER\"\n    *sets the working directory to FOLDER\n-y, --yes-to-all\n    *automatically overwrites files that already exist\n-n, --no-to-all\n    *skips files that already exist\n\n[Usage]\nextorg.exe\n    *organizes files from the current directory, doesn't skip or overwrite files automatically\nextorg.exe \"FOLDER\"\n    *organizes files from the FOLDER directory, doesn't skip or overwrite files automatically\nextorg.exe -[Y/N]\n    *organizes files from the current directory, respects the [-Y/N] flag\nextorg.exe -[Y/N] \"FOLDER\", extorg.exe \"FOLDER\" -[Y/N]\n    *organizes files from the FOLDER directory, respects the -[Y/N] flag\n");
                    return;
                }
                if (lwr.Equals("-y") || lwr.Equals("--yes-to-all"))
                    yesToAll = true;
                else if (lwr.Equals("-n") || lwr.Equals("--no-to-all"))
                    noToAll = true;
                else if(Directory.Exists(args[0]))
                {
                    folder = args[0].EndsWith("/") || args[0].EndsWith("\\") ? args[0] : args[0] + "/";
                }
                else
                {
                    Console.WriteLine("[ERROR] The folder '" + args[0] + "' does not exist");
                    return;
                }
            }

            else if (args.Length == 2)
            {
                string lwr = args[0].ToLower();
                if (lwr.Equals("-y") || lwr.Equals("--yes-to-all"))
                {
                    yesToAll = true;

                    if (Directory.Exists(args[1]))
                    {
                        folder = args[1].EndsWith("/") || args[1].EndsWith("\\") ? args[1] : args[1] + "/";
                    }
                    else
                    {
                        Console.WriteLine("[ERROR] The folder '" + args[0] + "' does not exist");
                        return;
                    }
                }
                else if (lwr.Equals("-n") || lwr.Equals("--no-to-all"))
                {
                    noToAll = true;

                    if (Directory.Exists(args[1]))
                    {
                        folder = args[1].EndsWith("/") || args[1].EndsWith("\\") ? args[1] : args[1] + "/";
                    }
                    else
                    {
                        Console.WriteLine("[ERROR] The folder '" + args[0] + "' does not exist");
                        return;
                    }
                }
                else if (Directory.Exists(args[0]))
                {
                    folder = args[0].EndsWith("/") || args[0].EndsWith("\\") ? args[0] : args[0] + "/";

                    lwr = args[1].ToLower();
                    if (lwr.Equals("-y") || lwr.Equals("--yes-to-all"))
                        yesToAll = true;
                    else if (lwr.Equals("-n") || lwr.Equals("--no-to-all"))
                        noToAll = true;
                    else
                    {
                        Console.WriteLine("[ERROR] '" + args[1] + "' is not a valid argument");
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("[ERROR] The folder '" + args[0] + "' does not exist");
                    return;
                }
            }

            List<string> files = Directory.EnumerateFiles(folder, "*.*", SearchOption.TopDirectoryOnly).ToList();
            foreach(string file in files)
            {
                string name = Path.GetFileName(file);

                if (!name.Equals(app))
                {
                    try
                    {
                        string ext = Path.GetExtension(file);

                        if (ext.Length == 0)
                        {
                            // We don't touch files with no extension
                            Console.WriteLine("[SKIP] " + Path.GetFileName(file));
                            continue;
                        }

                        ext = ext.Substring(1);

                        if (!Directory.Exists(folder + ext))
                            Directory.CreateDirectory(folder + ext);

                        string dest = folder + ext + "/" + name;

                        if(File.Exists(dest))
                        {
                            if (!yesToAll && !noToAll)
                            {
                                Console.WriteLine("The file '" + dest + "' already exists. Overwrite?\nY (Yes)\nN (No)\nYA (Yes to all)\nNA (No to all)");
                                string input = "";
                                while (true)
                                {
                                    input = Console.ReadLine();
                                    string lwr = input.ToLower();

                                    if (lwr.Equals("y"))
                                    {
                                        File.Delete(dest);
                                        File.Move(file, dest);
                                        Console.WriteLine("[SUCCESS] " + Path.GetFileName(file) + " -> " + Path.GetDirectoryName(dest));
                                        break;
                                    }
                                    else if (lwr.Equals("ya"))
                                    {
                                        yesToAll = true;
                                        File.Delete(dest);
                                        File.Move(file, dest);
                                        Console.WriteLine("[SUCCESS] " + Path.GetFileName(file) + " -> " + Path.GetDirectoryName(dest));
                                        break;
                                    }
                                    else if (lwr.Equals("n"))
                                    {
                                        break;
                                    }
                                    else if (lwr.Equals("na"))
                                    {
                                        noToAll = true;
                                        break;
                                    }
                                }
                            }
                            else if (yesToAll)
                            {
                                File.Delete(dest);
                                File.Move(file, dest);
                                Console.WriteLine("[SUCCESS] " + Path.GetFileName(file) + " -> " + Path.GetDirectoryName(dest));
                            }
                        }
                        else
                        {
                            File.Move(file, dest);
                            Console.WriteLine("[SUCCESS] " + Path.GetFileName(file) + " -> " + Path.GetDirectoryName(dest));
                        }
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("[" + ex.Message + "] " + file);
                    }
                }
            }
        }
    }
}
