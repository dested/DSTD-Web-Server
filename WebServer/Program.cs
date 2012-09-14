using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using DSTDControls;
using Control=DSTDControls.Control;

namespace WebServer {
    class Program {
        static void Main(string[] args) {

            try
            {
          //  Server server = new Server("../../");
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();


        }

    }
   public class Server {

        String GetIP() {
            String strHostName = Dns.GetHostName();

            // Find host by name
            IPHostEntry iphostentry = Dns.GetHostByName(strHostName);

            // Grab the first IP addresses
            String IPStr = "";
            foreach (IPAddress ipaddress in iphostentry.AddressList) {
                IPStr = ipaddress.ToString();
                return IPStr;
            }
            return IPStr;
        }


       private Guid ServerGUID = Guid.NewGuid();

 
        private Socket mainSocket; 
        private int m_clientCount = 0;

       public Func<string, Type> GetTypeFromString;

        public Server(Func<string, Type> getTypeFromString, string localDir) {

           GetTypeFromString = getTypeFromString;
            localDir = localDir.EndsWith("\\") ? "" : "\\";
            localDirectory = localDir;

            int port= 9099;
            mainSocket = new Socket(AddressFamily.Unspecified, SocketType.Stream, ProtocolType.Tcp);
            mainSocket.Bind(new IPEndPoint(IPAddress.Any, port));
            mainSocket.Listen(200);
            
            mainSocket.BeginAccept(new AsyncCallback(OnClientConnect), Guid.NewGuid());
            while (true)
            {
                Thread.Sleep(2000);
            }

        }
        public Dictionary<string, Dictionary<string, object>> sessions = new Dictionary<string, Dictionary<string, object>>();

       public Dictionary<string, Dictionary<string, object>> privateSessions = new Dictionary<string, Dictionary<string, object>>();

        public Dictionary<string, object> application =new Dictionary<string, object>();
        
        public void SetSession(Guid g, Dictionary<string, object> o,Dictionary<string, object> o2) {
            if (!sessions.ContainsKey(g.ToString()))
                sessions.Add(g.ToString(), o);
            else
                sessions[g.ToString()] = o;

            
            if (!privateSessions.ContainsKey(g.ToString()))
                privateSessions.Add(g.ToString(), o2);
            else
                privateSessions[g.ToString()] = o2;

        }
        public void GrabSession(Page h, Guid guid) {
            if (sessions.ContainsKey(guid.ToString()))
                h.Session = sessions[guid.ToString()];

            if (privateSessions.ContainsKey(guid.ToString()))
                h.SetPrivateSession(privateSessions[guid.ToString()]);
        }

       public void SetPrivateSession(Guid g, Dictionary<string, object> o) {
            if (!privateSessions.ContainsKey(g.ToString()))
                privateSessions.Add(g.ToString(), o);
            else
                privateSessions[g.ToString()] = o;
        }
        public void GraPrivatebSession(Page h, Guid guid) {
           if (privateSessions.ContainsKey(guid.ToString()))
               h.SetPrivateSession(privateSessions[guid.ToString()]);
        }
   
        public Dictionary<string,object> GrabApplication() {
            return application;
        }

        public void OnClientConnect(IAsyncResult asyn) {
            try {
                ConnectionData.Add((Guid) asyn.AsyncState, new DSTDData());
                Socket m_workerSocket = mainSocket.EndAccept(asyn);
                WaitForData(m_workerSocket,asyn.AsyncState);

                mainSocket.BeginAccept(new AsyncCallback(OnClientConnect), Guid.NewGuid());
            }
            catch (ObjectDisposedException) {
                System.Diagnostics.Debugger.Log(0, "1", "\n OnClientConnection: Socket has been closed\n");
            }
            catch (SocketException se) {

            }

        }
        public class SocketPacket {
            public System.Net.Sockets.Socket m_currentSocket;
            public byte[] dataBuffer = new byte[1];
            public object state;
        }
        public void WaitForData(System.Net.Sockets.Socket soc, object state)
        {
                AsyncCallback pfnWorkerCallBack = new AsyncCallback(OnDataReceived);
                SocketPacket theSocPkt = new SocketPacket();
                theSocPkt.state = state;
                theSocPkt.m_currentSocket = soc;
                soc.BeginReceive(theSocPkt.dataBuffer, 0, theSocPkt.dataBuffer.Length, SocketFlags.None, pfnWorkerCallBack, theSocPkt);
        }

       public void OnDataReceived(IAsyncResult asyn) {
            try {
                SocketPacket socketData = (SocketPacket)asyn.AsyncState;
                
                int iRx = 0;
                // Complete the BeginReceive() asynchronous call by EndReceive() method
                // which will return the number of characters written to the stream 
                // by the client
                iRx = socketData.m_currentSocket.EndReceive(asyn);
                char[] chars = new char[iRx + 1];
                System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
                int charLen = d.GetChars(socketData.dataBuffer,
                                         0, iRx, chars, 0);
                System.String szData = new System.String(chars);
                szData = szData.Replace("\0", "");
                AppendDataToStream(socketData.m_currentSocket, szData, socketData.m_currentSocket.Available==0,socketData.state);

                // Continue the waiting for data on the Socket
                WaitForData(socketData.m_currentSocket, socketData.state);
            }
            catch (ObjectDisposedException) {
                System.Diagnostics.Debugger.Log(0, "1", "\nOnDataReceived: Socket has been closed\n");
            }
            catch (SocketException se) { 
            }
        }

        private void AppendDataToStream(Socket socket, string data, bool update, object state)
        {
            ConnectionData[(Guid) state].sb.Append(data);
            if (update)
            {
                AppendToLog(ConnectionData[(Guid)state].sb.ToString()+Lines);
                try
                {
                    ReadHeader(socket, ConnectionData[(Guid) state]);
             //       ConnectionData.Remove((Guid) state);
                }
                catch(Exception e)
                {
                    AppendToLog(e.Message);
                    XmlDocument doc = new XmlDocument();
                    XmlNode head = doc.CreateElement("head");
                    XmlNode node = doc.CreateElement("error");
                    node.InnerText = e.Message;
                    head.AppendChild(node);
                    doc.AppendChild(head);

                    string s = GetHeader(doc.OuterXml.Length, "200", "text/xml");
                    s += doc.OuterXml;
                    writeBack(s, socket);
                }
            }
        }

       private string Lines
       {
            get { string s = "";
                for (int i = 0; i < 50; i++)
                {
                    s += "_";
                }
                return  s ;
            }
       }

       void AppendToLog(string s)
        {
           lock (fw)
           {
               fw.write(ServerGUID, s);

           }
        }

       FileWriter fw=new FileWriter();

       class FileWriter
       {
           public FileWriter()
           {
           }
           public void write(Guid ServerGUID, string s) {
               if (!File.Exists("Log" + ServerGUID.ToString() + ".txt"))
                   File.Create("Log" + ServerGUID.ToString() + ".txt").Close();
               s = DateTime.Now.ToString() + " " + DateTime.Now.Ticks + "    " + s + Control.NEWLINE;

               using (FileStream fs = new FileStream("Log" + ServerGUID.ToString() + ".txt", FileMode.Append)) {
                   using (StreamWriter sw = new StreamWriter(fs)) {
                       sw.Write(Control.NEWLINE + s + Control.NEWLINE);
                   }
               }
           }
       }
       private Dictionary<Guid, DSTDData> ConnectionData = new Dictionary<Guid, DSTDData>();
    
        public string GetHeader(int iTotBytes, string sStatusCode,string type) {

            String sBuffer = "";


            sBuffer = sBuffer + "HTTP/1.1" + sStatusCode + "\r\n";
            sBuffer = sBuffer + "Server: AjaxServer 0.01\r\n";
            sBuffer = sBuffer + "Content-Type: " + type + " \r\n";
            sBuffer = sBuffer + "Accept-Ranges: none\r\n";
            sBuffer = sBuffer + "Content-Length: " + iTotBytes + "\r\n\r\n";


            //            SendToBrowser(bSendData, ref mySocket);


 
            return sBuffer;
        }

        //http://www.youtube.com/watch?v=wxThNQdQ-dE
        private double average=0;
        private long averagecount = 0;
       public string localDirectory;//= @"K:\My Applications\Sals\WebServer\AnyCardGame2\";
 
        private void ReadHeader(Socket sock, DSTDData ds) {

            string d = ds.sb.ToString();
            if (d.Contains("<top>") && !d.Contains("</top>"))
                d += "</top>";

            DSTDQuery q = new DSTDQuery(DSTDContext.getURL(d));

            if (q.File.TrimStart('/')=="favicon.ico")
                return;

            localDirectory = Directory.GetCurrentDirectory() + "\\";
            if (File.Exists(localDirectory + q.File.TrimStart('/'))) {
                StreamReader sr = new StreamReader(File.OpenRead(localDirectory + q.File.TrimStart('/')));
                writeBack(sr.ReadToEnd(), sock);
                sr.Close();
                return;
            }
            //K:\My Applications\Sals\WebServer\WebServer\
            if (File.Exists(@"" + q.File.TrimStart('/'))) {
                StreamReader sr = new StreamReader(File.OpenRead(@"" + q.File.TrimStart('/')));
                writeBack(sr.ReadToEnd(), sock);
                sr.Close();
                return;
            }


            DSTDContext context = new DSTDContext(this,ds.sb.ToString(), GrabSession, SetSession, GrabApplication);
            string str = context.Request.CurrentPage.OnRender();

            string ty = "";
            if (context.Request.CurrentPage.PanelToUpdate != "" || !context.Request.CurrentPage.FullRender )
                ty = "text/xml";
            else
                ty = "text/html";

            context.SetSession(context.Request.CurrentPage.CurrentGUID, context.Request.CurrentPage.Session, context.Request.CurrentPage.GetPrivateSession());

            string s = GetHeader(str.Length, "200", ty);
            s += str;
            writeBack(s, sock);
            ds.Start.Stop();
            average += ds.Start.Elapsed.TotalMilliseconds;
            averagecount++;
            Console.WriteLine(ds.Start.Elapsed.TotalMilliseconds.ToString() + "       " + (average / averagecount));
        }

        private void writeBack(string s, Socket sock)
        {
         

            Object objData = s;
            byte[] byData = System.Text.Encoding.ASCII.GetBytes(objData.ToString());
            try
            {
                if (sock != null)
                {
                    if (sock.Connected)
                    {
                        sock.Send(byData);
                    }
                    sock.Close();

                }
            }
            catch (SocketException se)
            {
                Console.WriteLine(se.Message);
            }
        }
    }
    public class DSTDData
    {
        public StringBuilder sb = new StringBuilder();

        public List<int> highest = new List<int>();

        public Stopwatch Start = new Stopwatch();

        public DSTDData()
        {
            Start.Start();
        }

    }

}