﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Permissions;

using SESDAD.Commons;

namespace SESDAD.Processes {
    public abstract class GenericProcessService<I> : MarshalByRefObject
        where I : IGenericProcess {
        private I process;
        private IPuppetMasterService puppetMaster;

        public I Process {
            get { return process; }
            set { process = value; }
        }
        public IPuppetMasterService PuppetMaster {
            get { return puppetMaster; }
        }
        public ProcessHeader Header {
            get { return process.Header; }
        }

        public void ConnectToPuppetMaster(String puppetMasterURL) {
            puppetMaster = (IPuppetMasterService)Activator.GetObject(
                 typeof(IPuppetMasterService),
                 puppetMasterURL);
        }
        public void ConnectToParentBroker(String parentbrokerURL) {
            process.ConnectToParentBroker(parentbrokerURL);
        }

        public void ForceFreeze() {
            process.Freeze();
        }
        public void ForceUnfreeze() {
            process.Unfreeze();
        }
        public void ForceCrash() {
            process.Crash();
        }

        public String GetStatus() {
            return process.ToString();
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, Flags = SecurityPermissionFlag.Infrastructure)]
        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
