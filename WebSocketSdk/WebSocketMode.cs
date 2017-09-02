using Fleck;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketSdk
{

    /// <summary>
    /// 实现WebSocket通信的具体模式
    /// 
    /// </summary>
    public partial class WebSocketMode
    {

        /// <summary>
        /// Fleck实现模式
        /// 其功能并不强大，且可配置的地方太少
        /// 简单,无依赖项
        /// 对于简单快速的项目我会用它，
        /// 如果你不需要用WebSocket发送太复杂的数据结构、命令一样的消息、或在客户端无WebSocket支持时的备选方式，这就是你要的了。
        /// </summary>
        public void FleckSocket()
        {
            FleckLog.Level = LogLevel.Debug;
            var allSockets = new List<IWebSocketConnection>();
            var server = new WebSocketServer("http://localhost:14319");
            server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    //Console.WriteLine("Open!");
                    allSockets.Add(socket);
                };
                socket.OnClose = () =>
                {
                   // Console.WriteLine("Close!");
                    allSockets.Remove(socket);
                };
                socket.OnMessage = message =>
                {
                    //Console.WriteLine(message);
                    allSockets.ToList().ForEach(s => s.Send("Echo: " + message));
                };
                //socket.OnBinary = file =>
                //{
                //    string path = ("D:/test.txt");
                //    //创建一个文件流
                //    FileStream fs = new FileStream(path, FileMode.Create);

                //    //将byte数组写入文件中
                //    fs.Write(file, 0, file.Length);
                //    //所有流类型都要关闭流，否则会出现内存泄露问题
                //    fs.Close();
                //};
            });


            var input = Console.ReadLine();
            while (input != "exit")
            {
                foreach (var socket in allSockets.ToList())
                {
                    socket.Send(input);
                }
                input = Console.ReadLine();
            }

        }
    }
}
