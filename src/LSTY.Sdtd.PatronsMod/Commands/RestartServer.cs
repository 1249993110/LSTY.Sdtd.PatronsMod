﻿using LSTY.Sdtd.PatronsMod.Extensions;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace LSTY.Sdtd.PatronsMod.Commands
{
    public class RestartServer : ConsoleCmdBase
    {
        private bool _isRestarting;

        public RestartServer()
        {
            ModEvents.GameShutdown.RegisterHandler(OnGameShutdown);
        }

        protected override string getDescription()
        {
            return "restart server, optional parameter -f";
        }

        protected override string[] getCommands()
        {
            return new[] { "ty-rs", "ty-RestartServer" };
        }

        public override string GetHelp()
        {
            return "Usage:\n" +
                "  1. ty-rs" +
                "  2. ty-rs -f" +
                "1. Restart server by shutdown" +
                "2. Force restart server";
        }

        public override void Execute(List<string> args, CommandSenderInfo senderInfo)
        {
            Log("Server is restarting..., please wait");

            if (args.Count > 0)
            {
                if (args[0] == "-f")
                {
                    PrepareRestart(true);
                }
            }
            else
            {
                Restart();
            }
        }

        private void PrepareRestart(bool force = false)
        {
            _isRestarting = true;

            if (force)
            {
                Restart();
            }
            else
            {
                SdtdConsole.Instance.ExecuteSync("shutdown", ClientInfoExtension.GetCmdExecuteDelegate());
            }
        }

        private void Restart()
        {
            string scriptName = null;
            string serverPath = null;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                scriptName = "restart-windows.bat";
                serverPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "startdedicated.bat");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                scriptName = "restart-linux.sh";
                serverPath = AppDomain.CurrentDomain.BaseDirectory;
                Process.Start("chmod", " +x " + Path.Combine(ModApi.ModDirectory, scriptName));
            }

            string path = Path.Combine(ModApi.ModDirectory, scriptName);
            Process.Start(path, string.Format("{0} \"{1}\"", Process.GetCurrentProcess().Id, serverPath));
        }

        private void OnGameShutdown()
        {
            if (_isRestarting)
            {
                Restart();
            }
        }
    }
}