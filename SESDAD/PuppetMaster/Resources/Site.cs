﻿using System;

namespace SESDAD.Managing {

    internal class Site : Node<Site> {
        private readonly String siteName;
        private String brokerURL;

        internal Site(String newSiteName, Site newParent) :
            base(newParent) {
                siteName = newSiteName;
        }

        internal String SiteName {
            get { return siteName; }
        }

        internal String ParentBrokerURL {
            get {
                if (Parent == null) {
                    return null;
                }
                return Parent.BrokerURL;
            }
        }

        internal String BrokerURL {
            get { return brokerURL; }
            set { brokerURL = value; }
        }
    }
}