﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using System.Runtime.Serialization.Formatters;
using System.Collections;
using System.Net.Sockets;
using System.Threading;

using SESDAD.Commons;

namespace SESDAD.Processes {

    public abstract class GenericProcess : IGenericProcess {

        private Thread waitingThread;
        private Object waitingObject;
        private ProcessHeader processHeader;
        private IMessageBrokerService parentBroker;

        public GenericProcess(ProcessHeader newProcessHeader) {
            waitingObject = new Object();
            processHeader = newProcessHeader;
            parentBroker = null;
        }
        protected Object WaitingObject {
            get { return waitingObject; }
        }
        public ProcessHeader Header {
            get { return processHeader; }
        }
        protected IMessageBrokerService ParentBroker {
            get { return parentBroker; }
        }

        public void TcpConnect(int portNumber, String channelName) {
            // Creating a custom formatter for a TcpChannel sink chain.
            BinaryServerFormatterSinkProvider provider = new BinaryServerFormatterSinkProvider();
            provider.TypeFilterLevel = TypeFilterLevel.Full;
            // Creating the IDictionary to set the port on the channel instance.
            IDictionary props = new Hashtable();
            props["port"] = portNumber;
            props["name"] = channelName;
            // Pass the properties for the port setting and the server provider in the server chain argument. (Client remains null here.)
            TcpChannel channel = new TcpChannel(portNumber);

            ChannelServices.RegisterChannel(channel, true);
        }
        public void LaunchService<Service, Interface>(Interface iProcess)
            where Service : GenericProcessService<Interface>, new()
            where Interface : IGenericProcess {

            Service service = new Service();
            service.Process = iProcess;

            Match match = Regex.Match(Header.ProcessURL, @"^tcp://[\w\.]+:(\d{4,5})/(\w+)$");
            String port = match.Groups[1].Value;
            String serviceName = match.Groups[2].Value;
            
            int portNumber = Int32.Parse(port);
            //String processURI = Header.ProcessName + "/" + serviceName;

            TcpConnect(portNumber, serviceName);

            RemotingServices.Marshal(
                service,
                serviceName,
                typeof(Service));
        }

        public virtual void ConnectToParentBroker(String parentBrokerURL) {
            parentBroker = (IMessageBrokerService)Activator.GetObject(
                typeof(IMessageBrokerService),
                parentBrokerURL);
        }

        private void Pause() {
            Monitor.Enter(waitingObject);
            waitingThread.Suspend();
            Monitor.Pulse(waitingObject);
            Monitor.Exit(waitingObject);
        }

        public void TryFreeze() {
            Monitor.Enter(waitingObject);
            Monitor.Pulse(waitingObject);
            Monitor.Exit(waitingObject);
        }

        public void Freeze() {
            ThreadStart tw = new ThreadStart(Pause);
            waitingThread = new Thread(tw);
            waitingThread.Start();
        }
        public void Unfreeze() {
            waitingThread.Resume();
        }
        public void Crash() {
            Environment.Exit(-1);
        }

        public override String ToString() {
            return Header.ToString();
        }
    }
}
