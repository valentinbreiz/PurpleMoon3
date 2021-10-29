using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace PurpleMoon3
{
    public struct Command
    {
        public string Name;
        public string Help;
        public string Usage;
        public Action<string[]> Execute;

        public Command(string name, string help, string usage, Action<string[]> exec)
        {
            this.Name = name;
            this.Help = help;
            this.Usage = usage;
            this.Execute = exec;
        }
    }

    public class CommandLine : Service
    {
        public List<Command> Commands;

        public string[] CommandArgs;
        public string Input { get; private set; }
        public string CurrentPath;

        public const string MSG_ERROR = "Invalid command";

        public CommandLine() : base("clihost", ServiceType.Utility)
        {

        }

        public override void Initialize()
        {
            base.Initialize();

            Commands = new List<Command>();
            CommandArgs = null;

            Commands.Add(new Command("CLS", "Clear the screen", "cls", CommandActions.CLS));
            Commands.Add(new Command("HELP", "Show list of commands", "help [-u : usage]", CommandActions.HELP));
            Commands.Add(new Command("SERVICES", "Show list of registered services", "services", CommandActions.SERVICES));
            Commands.Add(new Command("CD", "Set the current directory", "cd [path]", CommandActions.CD));
            Commands.Add(new Command("DIR", "Show contents of specified directory", "dir [path]", CommandActions.DIR));

            CurrentPath = "0:\\";

            Kernel.ServiceMgr.Register(this);
            Kernel.ServiceMgr.Start(this);
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Stop()
        {
            base.Stop();
        }

        public void PrintCaret()
        {
            Kernel.Terminal.Write("root", Color.Magenta);
            Kernel.Terminal.Write("@", Color.White);
            Kernel.Terminal.Write(CurrentPath, Color.Yellow);
            Kernel.Terminal.Write("> ");
        }

        public void HandleInput()
        {
            ConsoleKeyInfo key = Kernel.Terminal.ReadKey();

            if (key.Key == ConsoleKey.Enter)
            {
                Kernel.Terminal.NewLine();
                Kernel.CLI.Execute(Input);
                PrintCaret();
                Input = "";
            }
            else if (key.Key == ConsoleKey.Backspace)
            {
                if (Input.Length == 0) { return; }
                Kernel.Terminal.Delete();
                Input = Input.Remove(Input.Length - 1, 1);

            }
            else if (key.KeyChar >= 32 && key.KeyChar < 126) { Kernel.Terminal.WriteChar(key.KeyChar); Input += key.KeyChar; }
        }

        public void Execute(string input)
        {
            if (input.Length == 0) { return; }

            CommandArgs = input.Split(' ');
            if (CommandArgs.Length == 0) { return; }

            string cmd = CommandArgs[0].ToUpper();

            for (int i = 0; i < Commands.Count; i++)
            {
                if (Commands[i].Name == cmd)
                {
                    if (Commands[i].Execute != null) { Commands[i].Execute(CommandArgs); }
                    return;
                }
            }

            Kernel.Terminal.WriteLine(MSG_ERROR, Color.Red);
        }
    }

    public static class CommandActions
    {
        public static void CLS(string[] args)
        {
            Kernel.Terminal.Clear();
        }

        public static void HELP(string[] args)
        {
            bool usage = false;
            if (args.Length > 1) { if (args[1] == "-u") { usage = true; } }

            for (int i = 0; i < Kernel.CLI.Commands.Count; i++)
            {
                Kernel.Terminal.Write(Kernel.CLI.Commands[i].Name);
                while (Kernel.Terminal.CursorX < 22) { Kernel.Terminal.WriteChar(' '); }
                if (usage) { Kernel.Terminal.WriteLine(Kernel.CLI.Commands[i].Usage, Color.DimGray); }
                else { Kernel.Terminal.WriteLine(Kernel.CLI.Commands[i].Help, Color.DimGray); }
            }
        }

        public static void SERVICES(string[] args)
        {
            Kernel.Terminal.Write("------ ", Color.DimGray);
            Kernel.Terminal.Write("SERVICES", Color.Lime);
            Kernel.Terminal.WriteLine(" ---------------------------", Color.DimGray);
            Kernel.Terminal.WriteLine("ID      TYPE    STATE    NAME", Color.DimGray);

            for (int i = 0; i < Kernel.ServiceMgr.Services.Count; i++)
            {
                Kernel.Terminal.Write(Kernel.ServiceMgr.Services[i].ID.ToString().PadRight(8, ' '));
                Kernel.Terminal.Write(((int)Kernel.ServiceMgr.Services[i].Type).ToString().PadRight(8, ' '));
                Kernel.Terminal.Write((Kernel.ServiceMgr.Services[i].Running ? 1 : 0).ToString().PadRight(9, ' '));
                Kernel.Terminal.Write(Kernel.ServiceMgr.Services[i].Name.ToString().PadRight(24, ' '));

                Kernel.Terminal.NewLine();
            }
        }

        public static void CD(string[] args)
        {
            if (args.Length < 2) { Kernel.Terminal.WriteLine("Expected path name"); return; }
            string input = Kernel.CLI.Input;
            input = input.Substring(3, input.Length - 3);
            if (input.StartsWith("0:\\"))
            {
                if (!Kernel.FileSys.DirectoryExists(input)) { Kernel.Terminal.WriteLine("Unable to locate path '" + input + "'"); return; }
                Kernel.CLI.CurrentPath = input.Replace("/", "\\");
                if (!Kernel.CLI.CurrentPath.EndsWith("\\")) { Kernel.CLI.CurrentPath += "\\"; }
            }
            else
            {
                string full = Kernel.CLI.CurrentPath + input.Replace("/", "\\");
                if (!full.EndsWith("\\")) { full += "\\"; }
                if (!Kernel.FileSys.DirectoryExists(full)) { Kernel.Terminal.WriteLine("Unable to locate path '" + input + "'"); return; }
                Kernel.CLI.CurrentPath = full;
            }
        }

        public static void DIR(string[] args)
        {
            if (args.Length < 2) { if (!Kernel.FileSys.PrintContents(Kernel.CLI.CurrentPath)) { Kernel.Terminal.WriteLine("Unable to locate path '" + Kernel.CLI.CurrentPath + "'"); return; } }
            string input = Kernel.CLI.Input;
            input = input.Substring(4, input.Length - 4);
            if (input.StartsWith("0:\\")) { if (!Kernel.FileSys.PrintContents(input)) { Kernel.Terminal.WriteLine("Unable to locate path '" + Kernel.CLI.CurrentPath + "'"); return; } }
            else
            {
                string full = Kernel.CLI.CurrentPath + input.Replace("/", "\\");
                if (!full.EndsWith("\\")) { full += "\\"; }
                if (!Kernel.FileSys.PrintContents(full)) { Kernel.Terminal.WriteLine("Unable to locate path '" + Kernel.CLI.CurrentPath + "'"); return; }
            }
        }
    }
}