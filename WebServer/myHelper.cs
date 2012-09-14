using System;

using System.Collections.Generic;
using System.Text;

namespace DSTDControls {
    public static class myHelper {
        private static Random ran = new Random();
        public static int RANDOM(int small, int big) {
            return ran.Next(small, big);
        }

        public static string Flatten(this List<string> strs) {
            string s = "";
            foreach (string c in strs) {
                s += c;
            }

            return s;
        }

        public static string ReturnHash(string s) {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(s, "MD5");
        }
        public static bool CheckAgainstHash(string aString,  string aLastString) {
            if (myHelper.ReturnHash(aString) == aLastString || aString == "") { 
                return false;
            }
            else { 
                return true;
            }
        }
    }
}
