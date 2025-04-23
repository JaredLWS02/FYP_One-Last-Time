using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ARGScript : MonoBehaviour
{
    [HideInInspector] public int ID;
    public string[] srcFilename;
    public string[] destFilename;
    private string GetExeDir()
    {
        string exeFolder = Path.GetDirectoryName(Application.dataPath);
        return exeFolder;
    }

    // Copy an existing PNG file to the target directory
    public void CopyPngFile()
    {
        //string dir = GetExeDir() + "/GameLogs/";
        string dir = Path.Combine(GetExeDir(), "GameLogs");

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        string sourcePath = Path.Combine(Application.dataPath, "New folder", srcFilename[ID]); // Path to the existing PNG
        //string destinationPath = dir + "ExistingImage1.png"; // Destination path
        string destinationPath = Path.Combine(dir, destFilename[ID]); // Destination path

        if (File.Exists(sourcePath) && !File.Exists(destinationPath))
        {
            // Copy the existing PNG file to the GameLogs folder
            File.Copy(sourcePath, destinationPath);
            Debug.Log("PNG file copied to: " + destinationPath);

            // Encrypt contents using Atbash
            string content = File.ReadAllText(destinationPath);
            string encryptedContent = AtbashEncrypt(content);
            File.WriteAllText(destinationPath, encryptedContent);
        }
        else if (File.Exists(destinationPath))
        {
            Debug.Log("File already exists at destination: " + destinationPath);
        }
        else
        {
            Debug.LogError("Source file does not exist at: " + sourcePath);
        }
    }

    private string AtbashEncrypt(string input)
    {
        char[] buffer = input.ToCharArray();

        for (int i = 0; i < buffer.Length; i++)
        {
            char letter = buffer[i];

            if (char.IsUpper(letter))
            {
                buffer[i] = (char)('Z' - (letter - 'A')); // Uppercase Atbash
            }
            else if (char.IsLower(letter))
            {
                buffer[i] = (char)('z' - (letter - 'a')); // Lowercase Atbash
            }
            // else leave punctuation, numbers, and whitespace untouched
        }

        return new string(buffer);
    }

}
