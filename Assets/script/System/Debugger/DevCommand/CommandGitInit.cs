using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Console
{
    public class CommandGitInit : ConsoleCommand
    {
        public override string Name { get; protected set; }
        public override string Command { get; protected set; }
        public override string Description { get; protected set; }
        public override string Help { get; protected set; }
        public string type { get; private set; }

        public CommandGitInit()
        {
            Name = "git init";
            Command = "git";
            Description = "found git repository under folder";
            Help = "Use this command with no arguments to found a git repository";

            AddCommandToConsole();
        }

        public override void RunCommand(string[] param)
        {
            GitSystem gitSystem = GameObject.Find("GitObject").GetComponent<GitSystem>();
            DeveloperConsole console = GameObject.Find("DeveloperConsoleObject").GetComponent<DeveloperConsole>();
            type = param[1];
            if( param.Length == 1)
            {
                console.AddMessageToConsole("Error format");
                return;
            }
            if ( param[1] == "init")
            {
                if (param.Length != 2)
                {
                    console.AddMessageToConsole("Error format");
                }
                else
                {
                    bool exist = GameObject.Find("GitObject").GetComponent<GitSystem>();
                    if (exist)
                    {
                        GameObject.Find("GitObject").GetComponent<GitSystem>().buildRepository();
                    }
                }

            }
            if (param[1] == "add")
            {
                Debug.Log("add");
                if (param.Length != 3)
                {
                    console.AddMessageToConsole("Error format");
                }
                else
                {
                    Debug.Log(param[2]);
                    gitSystem.trackFile(param[2], "test");
                }
            }
            if (param[1] == "remove")
            {
                if (param.Length != 3)
                {
                    console.AddMessageToConsole("Error format");
                }
                else
                {
                    gitSystem.untrackFile(param[2]);
                }

            }
            if (param[1] == "commit")
            {

                if (param.Length != 4 || param[2] != "-m" )
                {
                    console.AddMessageToConsole("Error format");
                }
                else
                {
                    gitSystem.Commit(param[3]);
                }

            }
            if ( param[1]  == "remote") {

                if (param[2] == "add" && param.Length == 4)
                {
                    gitSystem.addRemote(param[3]);
                }
                else
                {
                    console.AddMessageToConsole("Error format");
                }
            }
            if( param[1] == "push")
            {
                if (param.Length != 2)
                {
                    console.AddMessageToConsole("Error format");
                }
                else
                {
                    gitSystem.Push();
                }
            }
            if (param[1] == "clone")
            {
                if (param.Length != 3)
                {
                    console.AddMessageToConsole("Error format");
                }
                bool isCloneSuccessed = gitSystem.cloneRepository(param[2]);
                if (!isCloneSuccessed)
                {
                    console.AddMessageToConsole("Cannot clone");
                }
            }
            if (param[1] == "branch")
            {
                if (param.Length == 2)
                {
                    Debug.Log(gitSystem.localRepository.branches.ToString());
                }
                else if (param.Length == 3)
                {
                    gitSystem.createBranch(param[2]);
                }
                else if (param.Length == 4 && param[2] == "-D")
                {
                    gitSystem.deleteBranch(param[3]);
                }
                else
                {
                    console.AddMessageToConsole("Error format");
                }

            }
            if (param[1] == "checkout")
            {
                if (param.Length != 3)
                {
                    console.AddMessageToConsole("Error format");
                }
                else
                {
                    gitSystem.checkout(param[2]);
                }

            }
            if (param[1] == "merge")
            {
                if (param.Length != 3)
                {
                    console.AddMessageToConsole("Error format");
                }
                else
                {
                    gitSystem.Merge(param[2]);
                }

            }
            if (param[1] == "pull")
            {
                if (param.Length != 4)
                {
                    console.AddMessageToConsole("Error format");
                }
                else
                {
                    gitSystem.Pull(param[2], param[3]);
                }
            }
            if (param[1] == "stash")
            {
                if (param.Length != 2)
                {
                    console.AddMessageToConsole("Error format");
                }
                else
                {
                    gitSystem.stash();
                }
            }
            if (param[1] == "pop")
            {
                if (param.Length != 2)
                {
                    console.AddMessageToConsole("Error format");
                }
                else
                {
                    gitSystem.pop();
                }
            }
            if (param[1] == "rebase")
            {
                if (param.Length != 3)
                {
                    console.AddMessageToConsole("Error format");
                }
                else
                {
                    gitSystem.rebase(param[2]);
                }
            }
            if (param[1] == "tag")
            {
                if (param.Length != 3)
                {
                    console.AddMessageToConsole("Error format");
                }
                else
                {
                    gitSystem.tag(param[2]);
                }
            }

        }

        public static CommandGitInit CreateCommand()
        {
            return new CommandGitInit();
        }
    }
}