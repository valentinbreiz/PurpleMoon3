using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace PurpleMoon3
{
    public static class KernelInfo
    {
        public static int VersionNumberMain = 3;
        public static int VersionNumberSub = 0;

        public static string Name { get; private set; } = "PurpleMoon OS";
        public static string Version { get; private set; } = "Version " + VersionNumberMain.ToString() + "." + VersionNumberSub.ToString();

        public static void Print()
        {
            Kernel.Terminal.WriteLine(Name, Color.Magenta);
            Kernel.Terminal.WriteLine(Version, Color.DimGray);
        }
    }
}
