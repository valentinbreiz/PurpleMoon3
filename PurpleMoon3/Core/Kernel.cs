using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Sys = Cosmos.System;

namespace PurpleMoon3
{
    public class Kernel : Sys.Kernel
    {
        public static ServiceManager ServiceMgr;
        public static SVGADriver SVGA;
        public static TerminalManager Terminal;
        public static CommandLine CLI;
        public static FSHost FileSys;

        protected override void BeforeRun()
        {
            ServiceMgr = new ServiceManager();
            ServiceMgr.Initialize();

            SVGA = new SVGADriver();
            SVGA.Initialize();

            Terminal = new TerminalManager();
            Terminal.Initialize();

            FileSys = new FSHost();
            FileSys.Initialize();

            CLI = new CommandLine();
            CLI.Initialize();

            Terminal.WriteLine("PurpleMoon OS", Color.Magenta);
            Terminal.WriteLine("Version 3.0", Color.Silver);
            CLI.PrintCaret();
        }

        protected override void Run()
        {
            CLI.HandleInput();
        }
    }
}
