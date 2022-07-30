using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Console
{
    public class CommandDelete : ConsoleCommand
    {
        public override string Name { get; protected set; }
        public override string Command { get; protected set; }
        public override string Description { get; protected set; }
        public override string Help { get; protected set; }

        public CommandDelete()
        {
            Name = "Delete";
            Command = "rm";
            Description = "remove files";
            Help = "Use this command to delete files";

            AddCommandToConsole();
        }

        public override void RunCommand(string[] param)
        {
            if ( param.Length != 2 )
            {
                GameSystemManager.GetSystem<DeveloperConsole>().AddMessageToConsole("Error format");
            }
            else
            {
                bool end = GameObject.Find("FileObject").GetComponent<FileSystem>().DeleteFile(param[1]);
                if (!end)
                {
                    GameSystemManager.GetSystem<DeveloperConsole>().AddMessageToConsole("Cannot find file");
                }
            }
        }

        public static CommandDelete CreateCommand()
        {
            return new CommandDelete();
        }
    }
}