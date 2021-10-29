using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Sys = Cosmos.System;

namespace PurpleMoon3
{
    public class FSHost : Service
    {
        Sys.FileSystem.CosmosVFS VFS;   

        public FSHost() : base("fshost", ServiceType.Driver)
        {

        }

        public override void Initialize()
        {
            base.Initialize();

            Kernel.ServiceMgr.Register(this);
            Kernel.ServiceMgr.Start(this);
        }

        public override void Start()
        {
            base.Start();

            VFS = new Sys.FileSystem.CosmosVFS();
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(VFS);
        }

        public override void Stop()
        {
            base.Stop();

            VFS = null;
        }

        public bool PrintContents(string path)
        {
            if (!DirectoryExists(path)) { return false; }
            string[] dirs = GetDirectories(path);
            string[] files = GetFiles(path);

            for (int i = 0; i < dirs.Length; i++)
            {
                Kernel.Terminal.WriteLine("> " + dirs[i], Color.Yellow);
            }

            for (int i = 0; i < files.Length; i++)
            {
                Kernel.Terminal.WriteLine(files[i]);
            }
            return true;
        }

        public bool FileExists(string filename) { return File.Exists(filename); }
        public bool DirectoryExists(string filename) { return Directory.Exists(filename); }

        public string[] GetFiles(string path) 
        { 
            if (!Directory.Exists(path)) { return new string[1]; }
            return Directory.GetFiles(path); 
        }

        public string[] GetDirectories(string path)
        {
            if (!Directory.Exists(path)) { return new string[1]; }
            return Directory.GetDirectories(path);
        }
    }
}
