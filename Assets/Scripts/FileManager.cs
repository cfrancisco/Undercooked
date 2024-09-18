using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Undercooked
{
    public class FileManager
    {
        [DllImport("__Internal")]
        private static extern void JS_FileSystem_Sync();
        private string filename = "";

        public FileManager(string _filename)
        {
            this.filename = _filename;
            Debug.Log("[FileManager] Data will be saved at: " + this.filename);
            this.startFile();
        }

        public void cleanFile()
        {
            // Just to clean up the file. 
            TextWriter writer = new StreamWriter(this.filename, false);
            writer.Close();
        }
        public void startFile()
        {
            if (!File.Exists(this.filename))
            {
                File.Create(this.filename).Close();

            }
        }


        public void writeLineInWeb(string text)
        {
            //FileStream fs = null;
            Debug.Log("writeLineInWeb: " + text);

            try
            {
                //fs = new FileStream(this.filename, FileMode.Open);
                if (!File.Exists(this.filename))
                {

                    var crFile = File.CreateText(this.filename);
                    crFile.WriteLine(text);
                    if (crFile != null) crFile.Close();

                    //File.WriteAllText(this.filename, text);
                }
                else
                {
                    File.AppendAllText(this.filename, text);

                }
                //string jsonDataString = JsonUtility.ToJson(text, true);
                Debug.Log("text: " + text);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        public void writeLine(string text)
        {
            //  Debug.Log(">>writeLine>>>>>>>");
            //#if !UNITY_WEBGL
            TextWriter writer = new StreamWriter(this.filename, true);
            writer.WriteLine(text);
            writer.Close();
            /*#endif
            #if UNITY_WEBGL && !UNITY_EDITOR
                        this.writeLineInWeb(text);
                        this.SyncFS();
            #endif
             */
        }

        [System.Obsolete]
        public void SyncFS()
        {
            Debug.Log("SyncFS called.");
            Application.ExternalEval("_JS_FileSystem_Sync();");

            Debug.Log("SyncFS called - part 2.");
            JS_FileSystem_Sync();
        }

        public TextWriter getWriter()
        {
            return new StreamWriter(this.filename, true);
        }
    }
}
