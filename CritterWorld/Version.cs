﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CritterWorld
{
    public class Version
    {
        const int majorVersion = 2;
        const int minorVersion = 5;

        private Version() { }

        public static int MajorVersion
        {
            get
            {
                return majorVersion;
            }
        }

        public static int MinorVersion
        {
            get
            {
                return minorVersion;
            }
        }

        public static string VersionName
        {
            get
            {
                return "CritterWorld " + MajorVersion + "." + MinorVersion;
            }
        }
    }
}
