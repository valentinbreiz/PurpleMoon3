using System;
using System.Collections.Generic;
using System.Text;

namespace PurpleMoon3
{
    public enum ServiceType
    {
        KernelComponent,
        Driver,
        Utility,
        Program,
    }

    public abstract class Service
    {
        public string Name { get; protected set; }
        public uint ID { get; protected set; }
        public ServiceType Type { get; protected set; }
        public bool Initialized { get; protected set; }
        public bool Running { get; private set; }

        private static string[] TypeNames = new string[]
        {
            "KernelComponent",
            "Driver",
            "Utility",
            "Program",
            "Unknown",
        };

        public Service(string name, ServiceType type)
        {
            this.Name = name;
            this.Type = type;
            this.ID = 0;
        }

        public virtual void Initialize() { if (Initialized) { return; } Initialized = true; }
        public virtual void Start() { if (Running) { return; } Running = true; }
        public virtual void Stop() { if (!Running) { return; } Running = false; }

        public void SetName(string name) { this.Name = name; }
        public void SetID(uint id) { this.ID = id; }
        public void SetType(ServiceType type) { this.Type = type; }

        public static string GetServiceTypeString(ServiceType type)
        {
            switch (type)
            {
                case ServiceType.KernelComponent: { return TypeNames[0]; }
                case ServiceType.Driver: { return TypeNames[1]; }
                case ServiceType.Utility: { return TypeNames[2]; }
                case ServiceType.Program: { return TypeNames[3]; }
                default: { return TypeNames[4]; }
            }
        }
    }
}
