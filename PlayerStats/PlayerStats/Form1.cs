using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Top200
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Read the previous weeks file
            DataTable tbl_previous = new DataTable();
            DataRow dr;
            DateTime dt;
            tbl_previous.Columns.Add("uid");
            tbl_previous.Columns.Add("points");
            tbl_previous.PrimaryKey = new DataColumn[] { tbl_previous.Columns["uid"] };

            string fileroot = "C:\\Users\\jnicol\\Documents\\Personal\\eterna\\top200\\";
            string filename = fileroot + DateTime.Today.AddDays(-7).ToString("yyyyMMdd") + ".csv";
            System.IO.StreamReader sr = new System.IO.StreamReader(filename);
            string stUsers, stPuzzles, st;
            int i1, i2, i3, uid, nid, solves, solves_10, solves_3, solves_sw;

            while ((stUsers = sr.ReadLine()) != null)
            {
                dr = tbl_previous.NewRow();
                i1 = stUsers.IndexOf("&uid=");
                i2 = stUsers.IndexOf("\"", i1);
                dr["uid"] = int.Parse(stUsers.Substring(i1 + 5, i2 - i1 - 5));
                i1 = stUsers.IndexOf(",", i2 + 1);
                i1 = stUsers.IndexOf(",", i1 + 1);
                i2 = stUsers.IndexOf(",", i1 + 1);
                dr["points"] = int.Parse(stUsers.Substring(i1 + 1, i2 - i1 - 1));
                tbl_previous.Rows.Add(dr);
            }
            sr.Close();

            // Get all puzzles that have 10 or less solvers
            DataTable tbl_puzzle = new DataTable();
            System.Net.WebRequest req;
            System.IO.StreamReader rsp;
            tbl_puzzle.Columns.Add("nid");
            tbl_puzzle.Columns.Add("solves");
            tbl_puzzle.PrimaryKey = new DataColumn[] { tbl_puzzle.Columns["nid"] };
            i2 = 0; i3 = 4000; stPuzzles = ""; // skip the first 4000 puzzles since they have 13 or more solvers

            while (stPuzzles == "" || stPuzzles.IndexOf("\"id\"") > 0)
            {
                while ((i1 = stPuzzles.IndexOf("\"id\"", i2)) > 0)
                {
                    i2 = stPuzzles.IndexOf(",", i1);
                    nid = int.Parse(stPuzzles.Substring(i1 + 6, i2 - i1 - 7));
                    i1 = stPuzzles.IndexOf("\"created\"", i2);
                    i2 = stPuzzles.IndexOf(",", i1);
                    dt = new DateTime(long.Parse(stPuzzles.Substring(i1 + 11, i2 - i1 - 12)) * 10000000 + DateTime.Parse("1-1-1970").Ticks);
                    i1 = stPuzzles.IndexOf("\"num-cleared\"", i2);
                    i2 = stPuzzles.IndexOf(",", i1);
                    st = stPuzzles.Substring(i1 + 15, i2 - i1 - 16);
                    if (int.TryParse(st, out i1)) { solves = int.Parse(st); } else { solves = 0; }
                    if (solves <= 10 && dt.CompareTo(DateTime.Today.AddDays(-7)) == -1) // only get puzzles with 10 or less solves that are older than 1 week
                    {
                        dr = tbl_puzzle.NewRow();
                        dr["nid"] = nid;
                        dr["solves"] = solves;
                        tbl_puzzle.Rows.Add(dr);
                    }
                }
                req = System.Net.WebRequest.Create("http://eterna.cmu.edu/get/?type=puzzles&sort=solved&puzzle_type=PlayerPuzzle&size=100&skip=" + i3.ToString());
                rsp = new System.IO.StreamReader(req.GetResponse().GetResponseStream());
                stPuzzles = rsp.ReadToEnd();
                i3 = i3 + 100; i2 = 0;
            }

            // Get all switch puzzles
            DataTable tbl_switches = new DataTable();
            tbl_switches.Columns.Add("nid");
            tbl_switches.Columns.Add("solves");
            tbl_switches.PrimaryKey = new DataColumn[] { tbl_switches.Columns["nid"] };
            i2 = 0; i3 = 0; stPuzzles = "";

            while (stPuzzles == "" || stPuzzles.IndexOf("\"id\"") > 0)
            {
                while ((i1 = stPuzzles.IndexOf("\"id\"", i2)) > 0)
                {
                    i2 = stPuzzles.IndexOf(",", i1);
                    nid = int.Parse(stPuzzles.Substring(i1 + 6, i2 - i1 - 7));
                    i1 = stPuzzles.IndexOf("\"created\"", i2);
                    i2 = stPuzzles.IndexOf(",", i1);
                    dt = new DateTime(long.Parse(stPuzzles.Substring(i1 + 11, i2 - i1 - 12)) * 10000000 + DateTime.Parse("1-1-1970").Ticks);
                    i1 = stPuzzles.IndexOf("\"num-cleared\"", i2);
                    i2 = stPuzzles.IndexOf(",", i1);
                    st = stPuzzles.Substring(i1 + 15, i2 - i1 - 16);
                    if (int.TryParse(st, out i1)) { solves = int.Parse(st); } else { solves = 0; }
                    dr = tbl_switches.NewRow();
                    dr["nid"] = nid;
                    dr["solves"] = solves;
                    tbl_switches.Rows.Add(dr);
                }
                req = System.Net.WebRequest.Create("http://eterna.cmu.edu/get/?type=puzzles&sort=solved&puzzle_type=PlayerPuzzle&switch=checked&size=100&skip=" + i3.ToString());
                rsp = new System.IO.StreamReader(req.GetResponse().GetResponseStream());
                stPuzzles = rsp.ReadToEnd();
                i3 = i3 + 100; i2 = 0;
            }

            // Get the top 200 players
            DataTable tbl_current = new DataTable();
            tbl_current.Columns.Add("uid");
            tbl_current.Columns.Add("uname");
            tbl_current.Columns.Add("points");
            tbl_current.Columns.Add("started");
            tbl_current.Columns.Add("solves");
            tbl_current.Columns.Add("solves_cbns");
            tbl_current.Columns.Add("solves_sw");
            tbl_current.Columns.Add("solves_10");
            tbl_current.Columns.Add("solves_3");

            DataTable tbl_created = new DataTable();
            tbl_created.Columns.Add("nid");
            tbl_created.PrimaryKey = new DataColumn[] { tbl_created.Columns["nid"] };

            req = System.Net.WebRequest.Create("http://eterna.cmu.edu/get/?type=users&sort=point&size=200");
            rsp = new System.IO.StreamReader(req.GetResponse().GetResponseStream());
            stUsers = rsp.ReadToEnd(); i2 = 0;
            while ((i1 = stUsers.IndexOf("\"uid\"", i2)) > 0)
            {
                i2 = stUsers.IndexOf(",", i1);
                uid = int.Parse(stUsers.Substring(i1 + 7, i2 - i1 - 8));

                // Get each players created puzzle list
                req = System.Net.WebRequest.Create("http://eterna.cmu.edu//get/?type=user&tab_type=created&uid=" + uid.ToString());
                rsp = new System.IO.StreamReader(req.GetResponse().GetResponseStream());
                stPuzzles = rsp.ReadToEnd(); i3 = 0; tbl_created.Clear();
                while ((i1 = stPuzzles.IndexOf("\"id\"", i3 + 1)) > 0)
                {
                    dr = tbl_created.NewRow();
                    i3 = stPuzzles.IndexOf(",", i1);
                    dr["nid"] = int.Parse(stPuzzles.Substring(i1 + 6, i3 - i1 - 7));
                    tbl_created.Rows.Add(dr);
                }

                // Get each players solved puzzle list
                req = System.Net.WebRequest.Create("http://eterna.cmu.edu//get/?type=user&tab_type=cleared&uid=" + uid.ToString());
                rsp = new System.IO.StreamReader(req.GetResponse().GetResponseStream());
                stPuzzles = rsp.ReadToEnd(); i3 = 0;
                solves = 0; solves_10 = 0; solves_3 = 0; solves_sw = 0;
                while ((i1 = stPuzzles.IndexOf("\"nid\"", i3 + 1)) > 0)
                {
                    solves++;
                    i3 = stPuzzles.IndexOf(",", i1);
                    nid = int.Parse(stPuzzles.Substring(i1 + 7, i3 - i1 - 8));
                    dr = tbl_puzzle.Rows.Find(nid); // find 10 or less puzzles solved
                    if (dr != null) { solves_10++; if (int.Parse(dr["solves"].ToString()) <= 3) { solves_3++; } }
                    dr = tbl_created.Rows.Find(nid); // remove from puzzles created list if it has been solved
                    if (dr != null) { tbl_created.Rows.Remove(dr); }
                    dr = tbl_switches.Rows.Find(nid); // find switches solved
                    if (dr != null) { solves_sw++; }
                }

                // Check each players created but not solved puzzle list for 10 or less puzzles solved and switches solved
                for (i1 = 0; i1 < tbl_created.Rows.Count; i1++)
                {
                    nid = int.Parse(tbl_created.Rows[i1]["nid"].ToString());
                    dr = tbl_puzzle.Rows.Find(nid);
                    if (dr != null) { solves_10++; if (int.Parse(dr["solves"].ToString()) <= 3) { solves_3++; } }
                    dr = tbl_switches.Rows.Find(nid);
                    if (dr != null) { solves_sw++; }
                }

                dr = tbl_current.NewRow();
                dr["uid"] = uid;
                i1 = i2 + 9;
                i2 = stUsers.IndexOf(",", i1);
                dr["uname"] = stUsers.Substring(i1, i2 - i1 - 1);
                i1 = stUsers.IndexOf("\"points\"", i2);
                i2 = stUsers.IndexOf(",", i1);
                dr["points"] = stUsers.Substring(i1 + 10, i2 - i1 - 11);
                i1 = i2 + 12;
                i2 = stUsers.IndexOf(",", i1);
                dr["started"] = stUsers.Substring(i1, i2 - i1 - 1);
                dr["solves"] = solves;
                dr["solves_cbns"] = tbl_created.Rows.Count;
                dr["solves_sw"] = solves_sw;
                dr["solves_10"] = solves_10;
                dr["solves_3"] = solves_3;
                tbl_current.Rows.Add(dr);
            }

            // Write the stats to a new file
            filename = fileroot + DateTime.Today.ToString("yyyyMMdd") + ".csv";
            System.IO.StreamWriter sw = new System.IO.StreamWriter(filename);
            for (i1 = 0; i1 < tbl_current.Rows.Count; i1++)
            {
                stUsers = "\"=hyperlink(\"\"http://eterna.cmu.edu/eterna_page.php?page=player&uid=";
                stUsers += tbl_current.Rows[i1]["uid"] + "\"\";\"\"" + tbl_current.Rows[i1]["uname"] + "\"\")\",";
                stUsers += tbl_current.Rows[i1]["started"] + ",";
                stUsers += tbl_current.Rows[i1]["points"] + ",";
                dr = tbl_previous.Rows.Find(tbl_current.Rows[i1]["uid"]);
                if (dr == null) { i2 = 0; } else { i2 = int.Parse(dr["points"].ToString()); }
                stUsers += i2.ToString() + ",";
                dt = DateTime.Parse(tbl_current.Rows[i1]["started"].ToString());
                stUsers += (int.Parse(tbl_current.Rows[i1]["points"].ToString()) / DateTime.Today.Subtract(dt).Days).ToString() + ",";
                stUsers += (int.Parse(tbl_current.Rows[i1]["points"].ToString()) - i2).ToString() + ",";
                stUsers += tbl_current.Rows[i1]["solves"] + ",";
                stUsers += tbl_current.Rows[i1]["solves_cbns"] + ",";
                stUsers += tbl_current.Rows[i1]["solves_sw"] + ",";
                stUsers += tbl_current.Rows[i1]["solves_10"] + ",";
                stUsers += tbl_current.Rows[i1]["solves_3"];
                sw.WriteLine(stUsers);
            }
            sw.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult r = openFileDialog1.ShowDialog();
            System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.OpenFile());
            string st = sr.ReadLine();
        }
    }
}
