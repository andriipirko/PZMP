using System;
using System.Collections.Generic;
using System.Text;

namespace AdminAccountingApp
{
    static class GlobalConfig
    {
        static bool isAuthorized { get; set; }

        public static bool Authorized() => isAuthorized;

        public static void Authorize() => isAuthorized = true;
    }
}
