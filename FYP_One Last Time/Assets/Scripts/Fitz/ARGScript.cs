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
        string dir = Path.Combine(GetExeDir(), "The Lost Records");

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);

            string mainFile = Path.Combine(dir, "The Story.txt");
            File.WriteAllText(mainFile, "Five fragments of the story, scattered in the world.\n\nWill you find them all?");
        }

        string sourcePath = Path.Combine(Application.dataPath, "Delta", srcFilename[ID]); // Path to the existing PNG
        //string destinationPath = dir + "ExistingImage1.png"; // Destination path
        string destinationPath = Path.Combine(dir, destFilename[ID]); // Destination path

        if (File.Exists(sourcePath) && !File.Exists(destinationPath))
        {
            // Copy the existing PNG file to the GameLogs folder
            File.Copy(sourcePath, destinationPath);
            Debug.Log("PNG file copied to: " + destinationPath);

            // Encrypt contents using Atbash
            string content = File.ReadAllText(destinationPath);
            string decryptedContent = VigenereDecrypt(content, "anaya");
            File.WriteAllText(destinationPath, decryptedContent);
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

    private string VigenereDecrypt(string input, string key)
    {
        List<char> decrypted = new List<char>();
        int keyIndex = 0;

        foreach (char c in input)
        {
            if (char.IsLetter(c))
            {
                bool isUpper = char.IsUpper(c);
                char offset = isUpper ? 'A' : 'a';
                int k = char.ToLower(key[keyIndex % key.Length]) - 'a';
                char decryptedChar = (char)((((c - offset) - k + 26) % 26) + offset);
                decrypted.Add(decryptedChar);
                keyIndex++;
            }
            else
            {
                decrypted.Add(c); // Keep non-letter characters unchanged
            }
        }

        return new string(decrypted.ToArray());
    }


}
