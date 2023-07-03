using System;
using System.IO;

namespace VirtualKeyboard.Models {
    public abstract class VLogger{
        public static void Exception(string errCode, Exception e) { Exception(errCode, e, ""); }
        public static void Exception(string errCode, Exception e, string custom) {
            try {
                int i = 0;
                do {
                    Console.WriteLine($"ERROR {errCode} : {e.Message} : {e.StackTrace}\n\r{custom}");
                    FileAppend(string.Format("<err code=\"{0}\" dt=\"{1}\" ver=\"{5}\"><msg>{2}</msg><trace>{3}</trace><custom>{4}</custom></err>"
                        , e.InnerException != null || i > 0 ? errCode + "." + i : errCode
                        , GetDate()
                        , EscapeXml(e.Message)
                        , EscapeXml(e.StackTrace)
                        , EscapeXml(custom)
                        , System.Reflection.Assembly.GetExecutingAssembly().GetName().Version
                    ));
                    i++; e = e.InnerException;
                } while(e != null);
            } catch(Exception) {
                System.Windows.MessageBox.Show(String.Format("ERROR ON SAVING ERROR : <err code=\"{0}\" dt=\"{1}\"><msg>{2}</msg><trace>{3}</trace><custom>{4}</custom></err>"
                    , errCode
                    , GetDate()
                    , EscapeXml(e.Message)
                    , EscapeXml(e.StackTrace)
                    , EscapeXml(custom)
                ));
            }//try
        }//func

        #region Support Function
            public static string GetLogFilePath() {
                string rtn = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName);
                if(!rtn.EndsWith("\\")) rtn += "\\";
                return rtn + "error_log.txt";
            }//func

            public static string GetDate() { return DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"); }
            public static string EscapeXml(string txt) { return System.Security.SecurityElement.Escape(txt); }

            public static bool FileAppend(string txt) { return FileAppend(txt, 1); }
            public static bool FileAppend(string txt, int step) {
                string path = GetLogFilePath();
                StreamWriter sw = null;
                var stat = true;
            Console.WriteLine(path);
                try {
                    sw = new StreamWriter(path, true);
                    sw.WriteLine(txt);
                } catch(Exception ex) {
                    //Error saving to Progress files, Try to save it to AppData before we quit.
                    if(step <= 1) stat = FileAppend(txt, 2);
                    else {
                        System.Windows.MessageBox.Show("ERROR ON SAVING ERROR : " + txt + " :: " + ex.Message);
                        stat = false;
                    }
                } finally
                {
                    sw?.Close();
                }

                return stat;
            }
        #endregion
    }
}