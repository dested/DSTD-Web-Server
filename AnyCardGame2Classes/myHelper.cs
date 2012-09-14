using System;
using System.Collections.Generic;
using System.Text;

namespace AnyCardGame2Classes {
    public class myHelper {
        public static string ReplaceAll(string[] aOld, string[] aNew, string s) {
            for (int i = 0; i < aOld.Length; i++) {
                s = s.Replace(aOld[i], aNew[i]);
            }
            return s;
        }


        public static string RemoveLast(string aCur, int aInt) {
            return aCur.Substring(0, aCur.Length - aInt);
        }
        public static string RemoveFirst(string aCur, int aInt) {
            return aCur.Substring(aInt, aCur.Length - aInt);
        }

    }
}
