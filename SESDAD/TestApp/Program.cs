﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp {
    public class Program {
        static void Main(string[] args) {
            Object testLock = new Object();
            System.Diagnostics.Process.Start("SESDAD.MessageBroker.exe", "broker0 site0 tcp://localhost:8080/broker tcp://localhost:8085/broker");
                System.Diagnostics.Process.Start("SESDAD.Subscriber.exe", "subscriber0 site0 tcp://localhost:8081/sub tcp://localhost:8080/broker");
                System.Diagnostics.Process.Start("SESDAD.Subscriber.exe", "subscriber1 site0 tcp://localhost:8082/sub tcp://localhost:8080/broker");
                System.Diagnostics.Process.Start("SESDAD.Subscriber.exe", "subscriber2 site0 tcp://localhost:8083/sub tcp://localhost:8080/broker");
                System.Diagnostics.Process.Start("SESDAD.Subscriber.exe", "subscriber3 site0 tcp://localhost:8086/sub tcp://localhost:8080/broker");
                System.Diagnostics.Process.Start("SESDAD.Subscriber.exe", "subscriber4 site0 tcp://localhost:8087/sub tcp://localhost:8080/broker");
                System.Diagnostics.Process.Start("SESDAD.Subscriber.exe", "subscriber5 site0 tcp://localhost:8088/sub tcp://localhost:8080/broker");
            System.Diagnostics.Process.Start("SESDAD.Publisher.exe", "publisher0 site0 tcp://localhost:8084/pub tcp://localhost:8080/broker");
            /*System.Diagnostics.Process.Start("SESDAD.PuppetMaster.exe", "Site0 1000 sesdadrc");
            System.Diagnostics.Process.Start("SESDAD.PuppetMaster.exe", "Site1 1001");*/
        }
    }
}
