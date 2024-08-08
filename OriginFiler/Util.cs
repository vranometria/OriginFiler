using OriginFiler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.IO;

namespace OriginFiler
{
    public static class Util
    {
        public static void WriteFile(string outputFilePath, AppData appData)
        {
            //jsonに変換する
            string json = JsonSerializer.Serialize(appData);
            //ファイルに書き込む
            File.WriteAllText(outputFilePath, json);
        }
    }
}
