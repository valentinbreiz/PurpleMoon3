using System;
using System.Collections.Generic;
using System.Text;

namespace PurpleMoon3
{
    public class ServiceManager
    {
        public List<Service> Services;

        public void Initialize()
        {
            Services = new List<Service>();
        }

        public bool Register(Service service)
        {
            for (int i = 0; i < Services.Count; i++)
            {
                if (Services[i] == service) { return false; }
            }

            Services.Add(service);
            Services[Services.Count - 1].SetID((uint)(Services.Count - 1));
            return true;
        }

        public bool Start(Service service)
        {
            for (int i = 0; i < Services.Count; i++)
            {
                if (Services[i] == service) { Services[i].Start(); return true; }
            }
            return false;
        }

        public bool Stop(Service service)
        {
            for (int i = 0; i < Services.Count; i++)
            {
                if (Services[i] == service) { Services[i].Stop(); return true; }
            }
            return false;
        }
    }
}
