using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using WebServer;

namespace AnyCardGame2 {
    public class Class1 {
        public static void Main(string[] args) {

            try {
                string s = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                Server server = new Server(GetTypeFromString, "");
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();


        }
        static Type GetTypeFromString(string s) {
            Assembly asm = Assembly.GetExecutingAssembly();

            foreach (Type type in asm.GetTypes()) {
                if (type.Name == s)
                    return type;
            }
            return null;
        }
    }
}