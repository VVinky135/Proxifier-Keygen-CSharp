// CLI entry point. Build:
//   csc -nologo -codepage:65001 -optimize+ -target:exe -out:Pro_Keygen.exe Keygen.cs Pro_Keygen.cs

using System;
using System.Text;

class ProKeygenCli
{
    static int Main(string[] args)
    {
        try { Console.OutputEncoding = Encoding.UTF8; } catch { }

        string version = null;
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-v" || args[i] == "--version")
            {
                if (i + 1 < args.Length) version = args[i + 1];
            }
            else if (args[i].StartsWith("--version="))
            {
                version = args[i].Substring("--version=".Length);
            }
        }

        int product;
        if (version == null || version == "setup") product = 0;
        else if (version == "portable") product = 1;
        else if (version == "mac") product = 2;
        else
        {
            Console.WriteLine("版本参数错误!");
            return -1;
        }

        Console.WriteLine(Keygen.Generate(product));
        return 0;
    }
}
