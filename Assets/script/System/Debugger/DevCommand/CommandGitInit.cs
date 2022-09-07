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
            // for(int i=0;i<param.Length;i++){
            //     Debug.Log(param[i]);
            // }
            
            GitSystem gitSystem = GameObject.Find("GitObject").GetComponent<GitSystem>();
            DeveloperConsole console = GameObject.Find("DeveloperConsoleObject").GetComponent<DeveloperConsole>();
            // type = param[1];
            if(param.Length == 1 && param[0] == "git"){
                console.AddMessageToConsole("請看右方的卡片教學");
                return;
            }

            if( param.Length == 1)
            {
                console.AddMessageToConsole("Error format");
                return;
            }
            switch(param[1]){
                case "init":
                    if (param.Length != 2){
                        console.AddMessageToConsole("Error format");
                    }else{
                        bool exist = GameObject.Find("GitObject").GetComponent<GitSystem>();
                        if (exist)
                        {
                            GameObject.Find("GitObject").GetComponent<GitSystem>().buildRepository();
                        }
                    }
                    break;
                case "add":
                    if (param.Length != 3){
                        console.AddMessageToConsole("Error format");
                    }else{
                        //Debug.Log(param[2]);
                        gitSystem.trackFile(param[2], "test");
                    }
                    break;
                case "remove":
                    if (param.Length != 3){
                        console.AddMessageToConsole("Error format");
                    }else{
                        gitSystem.untrackFile(param[2]);
                    }
                    break;
                case "commit":
                    if (param.Length != 4 || param[2] != "-m" )
                    {
                        console.AddMessageToConsole("Error format");
                    }
                    else
                    {
                        Debug.Log("Commit");
                        gitSystem.Commit(param[3]);
                    }
                    break;
                case "remote":
                    if (param[2] == "add" && param.Length == 4)
                    {
                        gitSystem.addRemote(param[3]);
                    }
                    else
                    {
                        console.AddMessageToConsole("Error format");
                    }
                    break;
                case "push":
                    if (param.Length != 2)
                    {
                        console.AddMessageToConsole("Error format");
                    }
                    else
                    {
                        gitSystem.Push();
                    }
                    break;
                case "clone":
                    if (param.Length != 3)
                    {
                        console.AddMessageToConsole("Error format");
                    }
                    bool isCloneSuccessed = gitSystem.cloneRepository(param[2]);
                    if (!isCloneSuccessed)
                    {
                        console.AddMessageToConsole("Cannot clone");
                    }
                    break;
                case "branch":
                    if (param.Length == 2)
                    {
                        Debug.Log(gitSystem.localRepository.branches.ToString());
                    }
                    else if (param.Length == 3)
                    {
                        gitSystem.createBranch(param[2]);
                    }
                    else if (param.Length == 4 && param[2] == "-D" || param[2] == "-d")
                    {
                        gitSystem.deleteBranch(param[3]);
                    }
                    else
                    {
                        console.AddMessageToConsole("Error format");
                    }
                    break;
                case "checkout":
                    if (param.Length != 3)
                    {
                        console.AddMessageToConsole("Error format");
                    }
                    else
                    {
                        gitSystem.checkout(param[2]);
                    }
                    break;
                case "merge":
                    if (param.Length != 3)
                    {
                        console.AddMessageToConsole("Error format");
                    }
                    else
                    {
                        gitSystem.Merge(param[2]);
                    }
                    break;
                case "pull":
                    if (param.Length != 4)
                    {
                        console.AddMessageToConsole("Error format");
                    }
                    else
                    {
                        gitSystem.Pull(param[2], param[3]);
                    }
                    break;
                case "stash":
                    if (param.Length != 2)
                    {
                        console.AddMessageToConsole("Error format");
                    }
                    else
                    {
                        gitSystem.stash();
                    }
                    break;
                case "pop":
                    if (param.Length != 2)
                    {
                        console.AddMessageToConsole("Error format");
                    }
                    else
                    {
                        gitSystem.pop();
                    }
                    break;
                case "rebase":
                    if (param.Length != 3)
                    {   
                        console.AddMessageToConsole("Error format");
                    }
                    else
                    {
                        // Debug.Log("rebase: " + param[2]);
                        gitSystem.rebase(param[2]);
                    }
                    break;
                case "tag":
                    if (param.Length != 3)
                    {
                        console.AddMessageToConsole("Error format");
                    }
                    else
                    {
                        gitSystem.tag(param[2]);
                    }
                    break;
                case "reset":
                    gitSystem.reset(param[2],param[3]);
                    break;
                default:
                    console.AddMessageToConsole( "\"" + param[1] + "\"" + " is not a git command.");
                    break;
            }
        }

        public static CommandGitInit CreateCommand()
        {
            return new CommandGitInit();
        }
    }
}