using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Console
{
    public class CommandCopy : ConsoleCommand
    {
        public override string Name { get; protected set; }
        public override string Command { get; protected set; }
        public override string Description { get; protected set; }
        public override string Help { get; protected set; }

        public CommandCopy()
        {
            Name = "Copy";
            Command = "cp";
            Description = "copy file";
            Help = "Use this command with no arguments to force Unity to quit!";

            AddCommandToConsole();
        }

        public override void RunCommand(string[] param)
        {
            if ( param.Length != 3)
            {
                GameSystemManager.GetSystem<DeveloperConsole>().AddMessageToConsole("Error format");
            }
            else
            {
                Tuple<bool,string> end = GameObject.Find("FileObject").GetComponent<FileSystem>().CopyFile(param[1],param[2]);
                if (!end.Item1)
                {
                    GameSystemManager.GetSystem<DeveloperConsole>().AddMessageToConsole(end.Item2);
                }
            }
        }

        public static CommandCopy CreateCommand()
        {
            return new CommandCopy();
        }
    }
}