using System;
using System.Collections.Generic;
using System.Text;

namespace PurpleMoon3
{
    public class Runtime : Service
    {
        public Runtime() : base("runtime", ServiceType.Program)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Stop()
        {
            base.Stop();
        }
    }
}
