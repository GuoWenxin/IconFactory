using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IconFactory
{
    class Program
    {
        private static string currentAndroidFolder = "./icons/Android/";
        private static string currentIosFolder = "./icons/ios/";
        static void Main(string[] args)
        {
            Console.WriteLine("请输入需要的功能:\n1：生成自定义的图标;\n2：一键生成安卓的图标(需要512的原图);\n3:一键生成IOS的图标(需要1024的原图)");
            string cmd = Console.ReadLine();
            if(cmd=="1")
            {
                Console.WriteLine("请输入源文件名：");
                string resname = Console.ReadLine();
                Console.WriteLine("请输入需要生成的文件大小(整数,多个之间用','隔开)：");
                string newsize = Console.ReadLine();
                CreateNewSize(resname,newsize);
            }
            else if (cmd == "2")
            {
                int[] sizes = new[] { 36, 48, 72, 96, 144, 192 };
                string[] folderNames = new[]
                {
                    "drawable-ldpi", "drawable-mdpi", "drawable-hdpi", "drawable-xhdpi", "drawable-xxhdpi",
                    "drawable-xxxhdpi"
                };
                string normalFolderName = "drawable";
                string path = "./512.png";
                if (!File.Exists(path))
                {
                    Console.WriteLine("512.png文件不存在!");
                }
                else
                {
                    Image img = GetFile(path);
                    for (int i = 0; i < sizes.Length; i++)
                    {
                        Bitmap bit = GetNewSizeBitmap(img, sizes[i]);
                        SaveImage(bit, sizes[i].ToString(), ".png");
                    }
                    for (int i = 0; i < sizes.Length; i++)
                    {
                        CreateFolder(folderNames[i], sizes[i]);
                        if (sizes[i] == 48)
                        {
                            CreateFolder(normalFolderName, sizes[i]);
                        }
                        File.Delete(sizes[i] + ".png");
                        /*switch (sizes[i])
                        {
                            case 36:
                                CreateFolder("drawable-ldpi", 36);
                                File.Delete("36.png");
                                break;
                            case 48:
                                CreateFolder("drawable-mdpi", 48);
                                CreateFolder("drawable", 48);
                                File.Delete("48.png");
                                break;
                            case 72:
                                CreateFolder("drawable-hdpi", 72);
                                File.Delete("72.png");
                                break;
                            case 96:
                                CreateFolder("drawable-xhdpi", 96);
                                File.Delete("96.png");
                                break;
                            case 144:
                                CreateFolder("drawable-xxhdpi", 144);
                                File.Delete("144.png");
                                break;
                            case 192:
                                CreateFolder("drawable-xxxhdpi", 192);
                                File.Delete("192.png")l;
                                break;
                        }*/
                    }
                }
            }
            else if (cmd == "3")
            {
                int[] sizes = new[]
                {
                    40, 40, 60, 20, 29, 58,
                    58, 87, 29, 40, 80, 120,80,
                    50, 100, 57, 114, 120, 180,
                    72, 144, 76, 152, 167,1024
                };
                string[] names = new[]
                {
                    "icon-20@2x", "icon-20@2x-ipad", "icon-20@3x", "icon-20-ipad", "icon-29","icon-29@2x",
                    "icon-29@2x-ipad","icon-29@3x","icon-29-ipad","icon-40","icon-40@2x","icon-40@3x","icon-40@2x",
                    "icon-50","icon-50@2x","icon-57","icon-57@2x","icon-60@2x","icon-60@3x",
                    "icon-72","icon-72@2x","icon-76","icon-76@2x","icon-83.5@2x","icon-1024"
                };
                string[] scales = new[]
                {
                    "2x", "2x", "3x", "1x", "1x", "2x",
                    "2x", "3x", "1x", "1x", "2x", "3x","2x",
                    "1x", "2x", "1x", "2x", "2x", "3x",
                    "1x", "2x", "1x", "2x", "2x", "1x"
                };
                string[] idioms = new string[]
                {
                    "iphone","ipad","iphone","ipad","iphone","iphone",
                    "ipad","iphone","ipad","ipad","iphone","iphone","ipad",
                    "ipad","ipad","iphone","iphone","iphone","iphone",
                    "ipad","ipad","ipad","ipad","ipad","ios-marketing",
                };
                string[] showsize = new string[]
                {
                    "20","20","20","20","29","29",
                    "29","29","29","40","40","40","40",
                    "50","50","57","57","60","60",
                    "72","72","76","76","83.5","1024",
                };
                string path = "./1024.png";
                string alliocnsPath = currentIosFolder + "AppIcon.appiconset/";
                if (!File.Exists(path))
                {
                    Console.WriteLine("1024.png文件不存在!");
                    return;
                }
                Image img = GetFile(path);
                if (!Directory.Exists(currentIosFolder))
                {
                    Directory.CreateDirectory(currentIosFolder);
                }
                Bitmap bit1 = GetNewSizeBitmap(img, 512);
                SaveImage(bit1, currentIosFolder+"iTunesArtwork", ".png");
                Console.WriteLine("iTunesArtwork" + ".png生成成功！");
                Bitmap bit2 = GetNewSizeBitmap(img, 1024);
                SaveImage(bit2, currentIosFolder+"iTunesArtwork@2x", ".png");
                Console.WriteLine("iTunesArtwork@2x" + ".png生成成功！");
                if (!Directory.Exists(alliocnsPath))
                {
                    Directory.CreateDirectory(alliocnsPath);
                }
                for (int i = 0; i < sizes.Length; i++)
                {
                    Bitmap bit = GetNewSizeBitmap(img, sizes[i]);
                    SaveImage(bit, alliocnsPath+names[i], ".png");
                    //File.Move(names[i],alliocnsPath+names[i]);
                    Console.WriteLine(names[i] + ".png生成成功！");
                }
                StringBuilder sb=new StringBuilder();
                sb.Append("{\"images\":[");
                for (int i = 0; i < sizes.Length; i++)
                {
                    sb.Append("{");
                    sb.Append("\"size\":\"");
                    sb.Append(showsize[i]);
                    sb.Append("x");
                    sb.Append(showsize[i]);
                    sb.Append("\",");
                    sb.Append("\"idiom\":\"");
                    sb.Append(idioms[i]);
                    sb.Append("\",");
                    sb.Append("\"filename\":\"");
                    sb.Append(names[i]);
                    sb.Append(".png\",");
                    sb.Append("\"scale\":\"");
                    sb.Append(scales[i]);
                    sb.Append("\"}");
                    if (i<sizes.Length-1)
                    {
                        sb.Append(",");
                    }
                }
                sb.Append("],");
                sb.Append("\"info\":{");
                sb.Append("\"version\":1,");
                sb.Append("\"author\":\"www.didida.com\",");
                sb.Append("\"data\":\"");
                sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sb.Append("\"}}");
                FileStream fs=new FileStream(alliocnsPath+ "Contents.json",FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(sb.ToString());
                sw.Dispose();
                sw.Close();
                fs.Dispose();
                Console.WriteLine("json字符串生成完成!");
            }
            Console.WriteLine("按任意键关闭!");
            Console.ReadKey();
        }

        static void CreateNewSize(string resName,string newSize)
        {
            string[] sizes = newSize.Split(',');
            if (sizes.Length > 0)
            {
                foreach (var s in sizes)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        int size = 0;
                        if (int.TryParse(s, out size))
                        {
                            string path = "./" + resName;
                            if (!File.Exists(path))
                            {
                                Console.WriteLine(resName + "文件不存在!");
                                return;
                            }
                            Image img = GetFile(path);
                            Bitmap bit = GetNewSizeBitmap(img, size);
                            SaveImage(bit, size.ToString(), ".png");
                            Console.WriteLine(size + ".png生成成功！");
                        }
                        else
                        {
                            Console.WriteLine(s+"尺寸输入有误！");
                            //Console.WriteLine("输入尺寸有误，请重新输入(整数)：");
                            //string newsize = Console.ReadLine();
                            //CreateNewSize(resName, newsize);
                        }
                    }
                }
            }
        }
        static void CreateFolder(string name,int size)
        {
            FileInfo fi = new FileInfo("./" + size + ".png");
            if (!fi.Exists)
            {
                Console.WriteLine(size + ".png文件不存在");
                return;
            }
            string newName = currentAndroidFolder + name + "/app_icon.png";
            /*if (File.Exists(newName))
            {
                File.Delete(newName);
            }*/
            DirectoryInfo di = new DirectoryInfo(currentAndroidFolder + name);
            //if (di.Exists)
            //{
            //    di.Delete(true);
            //}
            di.Create();
            fi.CopyTo(newName,true);
            Console.WriteLine(size + ".png生成完成");
        }
        static public Image GetFile(string path)
        {
            FileStream stream = File.OpenRead(path);
            int fileLength = 0;
            fileLength = (int)stream.Length;
            Byte[] image = new Byte[fileLength];
            stream.Read(image, 0, fileLength);
            System.Drawing.Image result = System.Drawing.Image.FromStream(stream);
            stream.Close();
            return result;
        }
        static public Bitmap GetNewSizeBitmap(Image img, int size)
        {
            //int newWidth = Convert.ToInt32(img.Width / size);
            //int newHeight = Convert.ToInt32(img.Height / size);
            Size s = new Size(size, size);
            Bitmap newBit = new Bitmap(img, s);
            return newBit;
        }
        static public void SaveImage(Bitmap bit, string name, string ext)
        {
            bit.Save(name+ext);
            bit.Dispose();
            //Console.WriteLine("已处理:" + name);
        }
    }
}
