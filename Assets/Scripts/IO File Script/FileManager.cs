using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileManager 
{
    public static List<string> ReadTextAsset(string filePath, bool includeBlankLines = true){
        TextAsset asset = Resources.Load<TextAsset>(filePath);
        if(asset == null){
            Debug.LogError($"Asset not found: '{filePath}'");
            return null;
        }
        List<string> lines = new List<string>();
        using (StringReader sr = new StringReader(asset.text)){
            while (sr.Peek() > -1){
                string line = sr.ReadLine();
                if(includeBlankLines || !string.IsNullOrWhiteSpace(line))
                    lines.Add(line);
            }
        }
        return lines;
    }
}
