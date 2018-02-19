using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace h92html
{
    class Program
    {
        static void Main(string[] args)
        {
            string tmp;
            Console.WriteLine("Tell me where your h9 file is");
            string h9File = Console.ReadLine();
            string htmlFile = h9File.Replace(".txt",".html").Replace(".h9", ".html");
            Console.WriteLine("output will be at {0}",htmlFile);
            List<string> ls = new List<string>();
            string[] split;
            int remove = -1;

            using (StreamReader sr = new StreamReader(h9File))
            {
                bool waiting= true;
                while (sr.EndOfStream == false)
                {
                    if (waiting)
                    {
                        tmp = sr.ReadLine();
                        if (tmp.StartsWith("; Pl"))
                        {
                            waiting = false;
                            ls.Add(tmp);
                        }
                    }
                    else
                    {
                        tmp = sr.ReadLine();
                        if(tmp!="")
                            ls.Add(tmp);
                    }                    
                }
                Console.WriteLine("You appear to have had {0} players in your event", ls.Count - 1);
            }

            using (StreamWriter sw = new StreamWriter(htmlFile))
            {            
                //Make header
                sw.WriteLine("<table class=\"simple\" align=\"center\">");
                sw.WriteLine("<tbody>");
                sw.WriteLine("<tr>");
                split = ls[0].Split(new string[] { " ","\t" },StringSplitOptions.RemoveEmptyEntries);
                //Prompt to remove a column
                Console.WriteLine("Would you like to remove any column? ");
                Console.WriteLine("If not, type 'NO' ");
                Console.WriteLine("else, type the header string ");
                string answer = Console.ReadLine();
                if (answer.Equals("NO"))
                    remove = -1;
                else
                {
                    for (int j = 0; j < split.Length; j++)
                        if (split[j].Equals(answer))
                            remove = j;
                }
                for (int i = 1; i < split.Length; i++)
                {
                    if (i != remove) { 
                        if (i < 4)
                            sw.WriteLine("<th class=\"left\">" + split[i] + "</th>");
                        else
                        {
                            if (i < split.Length - 1)
                                sw.WriteLine("<th class=\"middle\">" + split[i] + "</th>");
                            else
                                sw.WriteLine("<th class=\"right\">" + split[i] + "</th>");
                        }
                    }
                }
                sw.WriteLine("</tr>");
                //write body

                for (int j = 1; j < ls.Count; j++)
                {
                    split = ls[j].Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
                    bool toJoinName = true;
                    //concatenate the first non numeric 
                    for (int i = 1; i < split.Length -1; i++) //scrub EGD Pin
                    {
                        if (i != remove + 1)
                        {
                            string parity = "class=\"impair\" ";
                            if (j % 2 == 0)
                                parity = "class=\"pair\" ";

                            if (toJoinName)
                            {
                                try
                                {
                                    int test = int.Parse(split[i]);
                                    sw.WriteLine("<td " + parity + " align=\"right\">" + split[i] + "</td>");
                                }

                                catch
                                {
                                    string playerName = split[i] + ", " + split[i + 1];
                                    i = i + 1;
                                    sw.WriteLine("<td " + parity + ">" + playerName + "</td>");
                                    toJoinName = false;
                                }
                            }
                            else
                            {
                                sw.WriteLine("<td " + parity + " align =\"center\">" + split[i] + "</td>");
                            }
                        }
                    }
                    sw.WriteLine("</tr>");
                }

                sw.WriteLine("</tbody>");
                sw.WriteLine("</table>");
            }

        }
    }
}
