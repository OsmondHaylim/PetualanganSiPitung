using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CommandData
{
    public List<Command> commands;
    private const char CommandSplitter = ',';
    private const char argumentsContainer = '(';

    public struct Command{
        public string name;
        public string[] arguments;
    }

    public CommandData(string rawCommands){
        commands = RipCommands(rawCommands);
    }

    private List<Command> RipCommands(string rawCommands){
        string[] data = rawCommands.Split(CommandSplitter, System.StringSplitOptions.RemoveEmptyEntries);
        List<Command> result = new List<Command>();

        foreach (string item in data){
            Command command = new Command();
            int index = item.IndexOf(argumentsContainer);
            command.name = item.Substring(0, index).Trim();
            command.arguments = GetArgs(item.Substring(index + 1, item.Length - index - 2));
            result.Add(command);
        }

        return result;
    }

    private string[] GetArgs(string args){
        List<string> argList = new List<string>();
        StringBuilder currentArg = new StringBuilder();
        bool inQuotes = false;

        for (int i = 0; i < args.Length; i++){
            if(args[i] == '"'){
                inQuotes = !inQuotes;
                continue;
            }
            if(!inQuotes && args[i] == ' '){
                argList.Add(currentArg.ToString());
                currentArg.Clear();
                continue;
            }

            currentArg.Append(args[i]);
        }

        if(currentArg.Length > 0)
            argList.Add(currentArg.ToString());

        return argList.ToArray();
    }
}
