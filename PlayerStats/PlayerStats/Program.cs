using System;
using System.Data;

namespace PlayerStats
{
    class Program
    {
        // This program takes around 4 minutes to run
        static void Main(string[] args)
        {
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
            int i1, i2, i3, uid, nid, solves, solves_10, solves_3, solves_switch;
            int smalleasy_left, specials_left, spores_left, bugs_left, spugs_left;
            int smallswitches_left, switched_left, minibits_left, mirrors_left, rsod_left;
            int basicshapes_left, tricksters_left, puzzlets_left, chainletters_left;
            int smallshape_left, funandeasy_left, boostme_left, structuralrepresentation_left;
            int puzzles_created, puzzles_created_10, puzzles_created_3, puzzles_created_0;
            stUsers = sr.ReadLine(); // read header row

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

            // Build list of Small and Easy puzzles
            int[] list_smalleasy = { 628737, 640504, 670886, 688515, 688808, 699722, 702500, 727172, 729351, 745238, 764359, 774860, 774863, 784583, 832357, 856771, 865997, 872070, 885411, 889665, 898373, 910810, 930923, 939688, 939735, 972680, 972682, 1006862, 1007132, 1007826, 1010502, 1228731, 1440832 };
            // Build list of Special Knowledge puzzles
            int[] list_special = { 5223102, 5223098, 885402, 885457, 888973, 888975, 888976, 899958, 899967, 899970, 901494, 963091, 963104, 1884715, 1897861 };
            // Build list of Spore puzzles
            int[] list_spore = { 460876, 461243, 461244, 461261, 465866, 465868, 466455, 466483, 466733, 466938, 467048, 467168, 467411, 467590, 467765, 778671, 876296, 876345, 876359 };
            // Build list of Bug puzzles
            int[] list_bug = { 5530420, 5525720, 5509425, 5505453, 5496863, 5486447, 5481670, 5481045, 5440694, 921867, 922555, 922563, 924832, 933750, 934778, 936999, 938388, 939793, 941255, 943678, 949886, 960437, 960440, 966416, 967786, 969286, 970812, 973763, 974760, 975528, 978079, 980508, 982159, 983653, 987506, 989689, 991842, 993722, 998516, 999201, 999856, 1000520, 1001273, 1001452, 1004465, 1005229, 1005956, 1006649, 1006697, 1007275, 1009216, 1009765, 1010399, 1027476, 1040119, 1059293, 1072878, 1088244, 1102242, 1132731, 1140523, 1271121, 1297872, 1333522, 1352620, 1372006, 1392204, 1419605, 1448148, 1464685, 1468791, 1504044, 1504102, 1543791, 1586778, 1594807, 1850224, 1852817, 1855316, 1857288, 1864825, 1868210, 1869954, 1872173, 1883111 };
            // Build list of Spug puzzles
            int[] list_spug = { 2439674, 2441085, 2449037, 2449362, 2453379, 2453413, 2463213, 2466841, 2468887, 2524875, 2524934, 2524972, 2541563, 2541569, 2541588, 2587536, 2587549 };
            // Build list of Small Switch Training puzzles
            int[] list_smallswitch = { 2533540, 2533544, 2533546, 2538013, 2538015, 2538017, 2542160, 2542162, 2545789, 2545791, 2545793, 2549593, 2549595, 2549597, 2553603, 2553605, 2553607, 2559356, 2559358, 2559360, 2563392, 2567228, 2567230, 2567234, 2571137, 2571139, 2571141, 2575665, 2575667, 2575669, 2580108 };
            // Build list of Switched puzzles
            int[] list_switched = { 3705313, 3705311, 3702514, 3702512, 3700303, 3700074, 3698049, 3698047, 3695997, 3695995, 3693541, 3693530, 3691506, 3691504, 3689550, 3689503, 3686825, 3686823, 3683760, 3683758, 3682384, 3682370, 3679656, 3679654, 3676781, 3676779, 3673650, 3673648, 3667963, 3667961, 3665533, 3665511, 3663088, 3663086, 3661424, 3661414, 3659819, 3659783, 3658499, 3658497, 3657188, 3657186, 3655482, 3655480, 3654255, 3654221, 3652459, 3652457, 3650370, 3650342, 3647918, 3647916, 3645882, 3645862, 3643790, 3643788, 3641700, 3641693, 3639394, 3639339, 3637494, 3637493, 3635588, 3635564, 3633640, 3633638, 3631248, 3631246, 3628339, 3628337, 3626230, 3625951, 3623546, 3623544, 3618698, 3618696, 3614581, 3614579, 3609678, 3609634, 3607565, 3607563, 3604910, 3604908, 3602898, 3602884, 3600202, 3600194, 3597844, 3597842, 3596083, 3596081, 3593915, 3593913, 3591908, 3591906, 3589496, 3589494, 3586262, 3586260 };
            // Build list of Minibit puzzles
            int[] list_minibit = { 3194553, 3194555, 3194557, 3196597, 3196599, 3196601, 3199302, 3199304, 3199306, 3201634, 3201638, 3201641, 3203863, 3203865, 3203867, 3205977, 3205979, 3205981, 3207952, 3207954, 3207956, 3209906, 3209908, 3209910, 3211446, 3211448, 3211450, 3214259, 3214261, 3214263, 3216473, 3216475, 3216477, 3221494, 3221496, 3221498, 3223907, 3223909, 3223911 };
            // Build list of Mirror puzzles
            int[] list_mirror = { 2335599, 2338875, 2339567, 2340979, 2341643, 2369997, 2370153, 2373022, 2373374, 2373407, 2376546, 2377064, 2377197, 2380827, 2382071, 2382120, 2384531, 2386304, 2386481, 2386913, 2388507, 2411141, 2423683, 2426603, 2429500, 2431208, 2431423, 2434137, 2434605, 2437476, 2443488, 2444502, 2445030, 2448260, 2448783, 2449592, 2450948, 2453341, 2454292, 2455815, 2455886, 2458323, 2460051, 2462604, 2476060, 2498108, 2502516, 2520278, 2520326, 2520940, 2545244, 2545409, 2553058, 2575235, 2579034, 2579990, 2580963, 2584160, 2585311, 2588855, 2627082, 2643182, 2648696, 2648914, 2648933, 2652503, 2659887, 2661246, 2668638, 2670271, 2674992, 2675488, 2675535, 2686833, 2701431, 2701511, 2702128, 2715708, 2749275, 2811274, 2820413, 2820486, 2820629, 2823117, 2824303, 2824342, 2824485, 2829857, 2831459, 2832205, 2838539, 2838547, 2838578, 2842197, 2842623, 2842834, 2846271, 2846307, 2846318, 2849613, 2849619, 2849632, 2853805, 2853819, 2853838, 3399967, 3401552, 3426467, 3480879 };
            // Build list of RSOD puzzles
            int[] list_rsod = { 3393925, 3394089, 3394117, 3396457, 3396533, 3399345, 3401274, 3403367, 3404848, 3406585, 3407488, 3409371, 3409377, 3412912, 3416592, 3416714, 3421857, 3421866, 3424676, 3426814, 3450032, 3450037, 3450046, 3454012, 3454014, 3457291, 3457293, 3462543, 3462558, 3462577, 3465664, 3465669, 3470194, 3470196, 3474134, 3474137, 3477169, 3477171, 3479768, 3479770, 3479772, 3482668, 3482671, 3488870, 3488872, 3488878, 3492812, 3492814, 3492816, 3495694 };
            // Build list of Basic Shapes puzzles
            int[] list_basicshapes = { 4186996, 4178237, 4172190, 4172108, 4156377, 4147399, 4147194, 4142431, 4138842, 4138782 };
            // Build list of Trickster puzzles
            int[] list_tricksters = { 3225468, 3225500, 3226151, 3229453, 3233285, 3233537, 3266478, 3237235, 3236996, 3237312, 3239434, 3239490, 3239528, 3241848, 3241890, 3242035, 3243833, 3245958, 3246013, 3246015, 3248262, 3248719, 3250817, 3252065, 3252980, 3257059, 3258384, 3260582, 3267592, 3273924, 3273936, 3276889, 3276988, 3284843, 3297637, 3307368, 3319288, 3327649 };
            // Build list of Puzzlet puzzles
            int[] list_puzzlets = { 5198481, 4930143, 4897017, 4886806, 4828344, 4783555, 4776227, 4756464, 4645515, 4634091, 4590639, 4494158, 4368285, 4360718, 4315075, 4263194, 4227188, 4225993, 4219585, 4182414, 4120729, 4105616, 4072226, 4043234, 3950819, 3923361, 3822755, 3756527, 3734129, 3722616, 3712599, 3709868, 3694841, 3686673, 3651180, 3637090, 3602614, 3581766, 3559860, 3538808, 3494415, 3488747, 3482930, 3471681, 3394672, 3365126, 3339877, 3282479, 3232217, 3060315, 2725418 };
            // Build list of Chain Letter puzzles
            int[] list_chainletters = { 4330993, 4326759, 4304242, 4298776, 4286424, 4277890, 4270923, 4270435, 4270026, 4260214, 4260089, 4243747, 4239119, 4231840, 4229935, 4218598, 4217256, 4215977, 4215035, 4180071, 4170805, 4170728, 4170704, 4170031, 4169065, 4162378, 4159873, 4159234, 4156764, 4147338, 4138661, 4137798, 4136281, 4135015, 4061187, 4033926, 3985684, 3981699, 3970582, 3967229, 3965981, 3965816, 3965181, 3921786, 3921662 };
            // Build list of Small Shape puzzles
            int[] list_smallshape = { 4772231, 4769685, 4244663, 4228114, 3932181, 3279461, 3279438, 3240251, 3236236, 3222762, 3063908, 2844270, 2628059, 1656115, 1651904, 1651802, 1651792, 1642982, 1636377, 1626167, 1611914, 1591298, 1480361, 1474060, 1441422, 1391383, 1381029, 1010333, 1002989, 999017, 996310, 992679 };
            // Build list of Fun and Easy puzzles
            int[] list_funandeasy = { 5281156, 5105539, 5026553, 5014799, 4979935, 4979895, 4883332, 4783904, 4729246, 4724109, 4694891, 4678864, 4674492, 4674384, 4668765, 4666355, 4664286, 4629746, 4478518, 4314613, 4314460, 4292190, 4285613, 4274825, 4267575, 4230426, 4226698, 4222487, 4217147, 4213427, 4213395, 4182418, 4180191, 4166011, 4157365, 4151044, 4147695 };
            // Build list of Boost Me puzzles
            int[] list_boostme = { 5613941, 5583207, 5529736, 5279262, 5121238, 4946971, 4856002, 4855990, 4847938, 4734631, 4729443, 4728241, 4728231, 4662025, 4660092, 4537091, 4314813, 4156764, 4147747, 4142946, 4124882, 4120153, 4120117, 4047253, 4044779, 4042666, 3993116, 3737063, 3737060, 3736687, 3679326, 3679036, 3679026, 3579951, 3507264, 3503132, 3453294, 3449552, 3435216, 3435181, 3296525, 3288665, 3277396, 3242065, 3195259, 3158139, 3134772, 3105234, 3086598, 3043459, 3043106, 3015187, 2942052, 2941941, 2899022, 2873700, 2873693, 2870396, 2870321, 2870306, 2648450, 2644112, 2570264, 2569947, 2564567, 2564440, 2564119, 2532266, 2504087, 2486427 };
            // Build list of Structural Representation puzzles
            int[] list_structuralrepresentation = { 5660043, 5657210, 5654891, 5654259, 5645689, 5635123, 5618938, 5618266, 5616125, 5613679, 5610873, 5608529, 5608370, 5605703, 5600472, 5591881, 5586320, 5581188, 5439579, 5385961, 5374293, 5322111, 5131139, 5124253, 5124201, 5101218, 5073097, 5059595, 5056041, 5053782, 5051829, 5048047, 5047877, 5047128, 5044902, 5041827, 5031725, 5028157, 5028048, 5026729, 5022815, 5022758, 5021911, 5016432, 5014369, 5011439, 5005929, 5005358, 5001488, 5000710, 5000520, 4997070, 4987260, 4980809, 4978220, 4976363, 4968741, 4964531, 4960368, 4955951, 4945820, 4937703, 4926788, 4923214, 4921006, 4919751, 4914184, 4908922, 4906780, 4902098, 4891834, 4885180, 4881673, 4878721, 4873229, 4869706, 4867445, 4858412, 4853858, 4841319, 4841197, 4833286, 4827975, 4809285, 4793787, 4782674, 4780635, 4780516, 4774127, 4773812, 4751705, 4751447, 4746999, 4743342, 4742777, 4738347, 4706909, 4705281, 4701173, 4698233, 4697123, 4696738, 4693666, 4692254, 4689362, 4684360, 4684273, 4654489, 4654014, 4647200, 4646468, 4643852, 4640099, 4639237, 4638101, 4634619, 4634331, 4632800, 4628539, 4625824, 4625345, 4622632, 4616282, 4611379, 4610972, 4609606, 4580740, 4546466, 4534090, 4526464, 4525234, 4522394, 4516551, 4516334, 4513428, 4506931, 4505449, 4498411, 4495188, 4490158, 4482931, 4482310, 4480623, 4467937, 4466852, 4458253, 4422345, 4420974, 4413623, 4342791, 4342361, 4341547, 4329159, 4327990, 4327576, 4323762, 4322254, 4322018, 4316121, 4315886 };

            // Get all puzzles that have 10 or less solvers
            DataTable tbl_puzzle = new DataTable();
            System.Net.WebRequest req;
            System.IO.StreamReader rsp;
            tbl_puzzle.Columns.Add("nid");
            tbl_puzzle.Columns.Add("solves");
            tbl_puzzle.PrimaryKey = new DataColumn[] { tbl_puzzle.Columns["nid"] };
            i2 = 0; i3 = 5000; stPuzzles = ""; // skip the first 5000 puzzles since they have 16 or more solvers

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
            tbl_current.Columns.Add("solves_switch");
            tbl_current.Columns.Add("solves_10");
            tbl_current.Columns.Add("solves_3");
            tbl_current.Columns.Add("puzzles_created");
            tbl_current.Columns.Add("puzzles_created_bns");
            tbl_current.Columns.Add("puzzles_created_10");
            tbl_current.Columns.Add("puzzles_created_3");
            tbl_current.Columns.Add("puzzles_created_0");
            tbl_current.Columns.Add("smalleasy_left");
            tbl_current.Columns.Add("specials_left");
            tbl_current.Columns.Add("spores_left");
            tbl_current.Columns.Add("bugs_left");
            tbl_current.Columns.Add("spugs_left");
            tbl_current.Columns.Add("smallswitches_left");
            tbl_current.Columns.Add("switched_left");
            tbl_current.Columns.Add("minibits_left");
            tbl_current.Columns.Add("mirrors_left");
            tbl_current.Columns.Add("rsod_left");
            tbl_current.Columns.Add("basicshapes_left");
            tbl_current.Columns.Add("tricksters_left");
            tbl_current.Columns.Add("puzzlets_left");
            tbl_current.Columns.Add("chainletters_left");
            tbl_current.Columns.Add("smallshape_left");
            tbl_current.Columns.Add("funandeasy_left");
            tbl_current.Columns.Add("boostme_left");
            tbl_current.Columns.Add("structuralrepresentation_left");

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
                puzzles_created = 0; puzzles_created_10 = 0; puzzles_created_3 = 0; puzzles_created_0 = 0;
                while ((i1 = stPuzzles.IndexOf("\"id\"", i3 + 1)) > 0)
                {
                    dr = tbl_created.NewRow();
                    i3 = stPuzzles.IndexOf(",", i1);
                    nid = int.Parse(stPuzzles.Substring(i1 + 6, i3 - i1 - 7));
                    dr["nid"] = nid;
                    tbl_created.Rows.Add(dr);
                    puzzles_created++;
                    dr = tbl_puzzle.Rows.Find(nid); // find 10 or less puzzles solved
                    if (dr != null) {
                        puzzles_created_10++;
                        if (int.Parse(dr["solves"].ToString()) <= 3) {
                            puzzles_created_3++;
                            if (int.Parse(dr["solves"].ToString()) == 0) {
                                puzzles_created_0++;
                            }
                        }
                    }
                }

                // Get each players solved puzzle list
                req = System.Net.WebRequest.Create("http://eterna.cmu.edu//get/?type=user&tab_type=cleared&uid=" + uid.ToString());
                rsp = new System.IO.StreamReader(req.GetResponse().GetResponseStream());
                stPuzzles = rsp.ReadToEnd(); i3 = 0;
                solves = 0; solves_10 = 0; solves_3 = 0; solves_switch = 0;
                smalleasy_left = list_smalleasy.Length; specials_left = list_special.Length; spores_left = list_spore.Length;
                bugs_left = list_bug.Length; spugs_left = list_spug.Length; smallswitches_left = list_smallswitch.Length; switched_left = list_switched.Length;
                minibits_left = list_minibit.Length; mirrors_left = list_mirror.Length; rsod_left = list_rsod.Length; basicshapes_left = list_basicshapes.Length;
                tricksters_left = list_tricksters.Length; puzzlets_left = list_puzzlets.Length; chainletters_left = list_chainletters.Length;
                smallshape_left = list_smallshape.Length; funandeasy_left = list_funandeasy.Length;
                boostme_left = list_boostme.Length; structuralrepresentation_left = list_structuralrepresentation.Length;
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
                    if (dr != null) { solves_switch++; }
                    if (Array.Find(list_smalleasy, id => id == nid) > 0) { smalleasy_left--; }
                    else if (Array.Find(list_special, id => id == nid) > 0) { specials_left--; }
                    else if (Array.Find(list_spore, id => id == nid) > 0) { spores_left--; }
                    else if (Array.Find(list_bug, id => id == nid) > 0) { bugs_left--; }
                    else if (Array.Find(list_spug, id => id == nid) > 0) { spugs_left--; }
                    else if (Array.Find(list_smallswitch, id => id == nid) > 0) { smallswitches_left--; }
                    else if (Array.Find(list_switched, id => id == nid) > 0) { switched_left--; }
                    else if (Array.Find(list_minibit, id => id == nid) > 0) { minibits_left--; }
                    else if (Array.Find(list_mirror, id => id == nid) > 0) { mirrors_left--; }
                    else if (Array.Find(list_rsod, id => id == nid) > 0) { rsod_left--; }
                    else if (Array.Find(list_basicshapes, id => id == nid) > 0) { basicshapes_left--; }
                    else if (Array.Find(list_tricksters, id => id == nid) > 0) { tricksters_left--; }
                    else if (Array.Find(list_puzzlets, id => id == nid) > 0) { puzzlets_left--; }
                    else if (Array.Find(list_chainletters, id => id == nid) > 0) { chainletters_left--; }
                    else if (Array.Find(list_smallshape, id => id == nid) > 0) { smallshape_left--; }
                    else if (Array.Find(list_funandeasy, id => id == nid) > 0) { funandeasy_left--; }
                    else if (Array.Find(list_boostme, id => id == nid) > 0) { boostme_left--; }
                    else if (Array.Find(list_structuralrepresentation, id => id == nid) > 0) { structuralrepresentation_left--; }
                }

                // Check each players created but not solved puzzle list for 10 or less puzzles solved and switches solved
                for (i1 = 0; i1 < tbl_created.Rows.Count; i1++)
                {
                    nid = int.Parse(tbl_created.Rows[i1]["nid"].ToString());
                    dr = tbl_puzzle.Rows.Find(nid);
                    if (dr != null) { solves_10++; if (int.Parse(dr["solves"].ToString()) <= 3) { solves_3++; } }
                    dr = tbl_switches.Rows.Find(nid);
                    if (dr != null) { solves_switch++; }
                }

                // Save each players solved stats
                dr = tbl_current.NewRow();
                dr["uid"] = uid;
                i1 = i2 + 9;
                i2 = stUsers.IndexOf(",", i1);
                dr["uname"] = stUsers.Substring(i1, i2 - i1 - 1);
                i1 = stUsers.IndexOf("\"points\"", i2);
                i2 = stUsers.IndexOf(",", i1);
                dr["points"] = stUsers.Substring(i1 + 10, i2 - i1 - 11);
                i1 = stUsers.IndexOf("\"created\"", i2);
                i2 = stUsers.IndexOf(",", i1);
                dr["started"] = stUsers.Substring(i1 + 11, i2 - i1 - 12);
                dr["solves"] = solves;
                dr["solves_switch"] = solves_switch;
                dr["solves_10"] = solves_10;
                dr["solves_3"] = solves_3;
                dr["puzzles_created"] = puzzles_created;
                dr["puzzles_created_bns"] = tbl_created.Rows.Count;
                dr["puzzles_created_10"] = puzzles_created_10;
                dr["puzzles_created_3"] = puzzles_created_3;
                dr["puzzles_created_0"] = puzzles_created_0;
                dr["smalleasy_left"] = smalleasy_left;
                dr["specials_left"] = specials_left;
                dr["spores_left"] = spores_left;
                dr["bugs_left"] = bugs_left;
                dr["spugs_left"] = spugs_left;
                dr["smallswitches_left"] = smallswitches_left;
                dr["switched_left"] = switched_left;
                dr["minibits_left"] = minibits_left;
                dr["mirrors_left"] = mirrors_left;
                dr["rsod_left"] = rsod_left;
                dr["basicshapes_left"] = basicshapes_left;
                dr["tricksters_left"] = tricksters_left;
                dr["puzzlets_left"] = puzzlets_left;
                dr["chainletters_left"] = chainletters_left;
                dr["smallshape_left"] = smallshape_left;
                dr["funandeasy_left"] = funandeasy_left;
                dr["boostme_left"] = boostme_left;
                dr["structuralrepresentation_left"] = structuralrepresentation_left;
                tbl_current.Rows.Add(dr);
            }

            // Write the stats to a new file
            filename = fileroot + DateTime.Today.ToString("yyyyMMdd") + ".csv";
            System.IO.StreamWriter sw = new System.IO.StreamWriter(filename);
            sw.WriteLine("Rank,Player,Playing Since,Points as of,Points as of,Points / Day,Last Week,Puzzles Solved,Switches,Puzzles Solved 10 or less solvers,Puzzles Solved 3 or less solvers,Puzzles Created,Puzzles Created But Not Solved,Puzzles Created 10 or less solvers,Puzzles Created 3 or less solvers,Puzzles Created with 0 solvers,Small and Easy left,Special Knowledge left,Spores left,Bugs left,Spugs left,Basic Shapes left,Small Switches left,Switched left,Minibits left,Mirrors left,RSOD left,Tricksters left,Puzzlets left,Chain Letters left,Small Shape left,Fun and Easy left,Boost Me left,Structural Representation left");
            for (i1 = 0; i1 < tbl_current.Rows.Count; i1++)
            {
                stUsers = (i1+1).ToString() + ",\"=hyperlink(\"\"http://eterna.cmu.edu/eterna_page.php?page=player&uid=";
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
                stUsers += tbl_current.Rows[i1]["solves_switch"] + ",";
                stUsers += tbl_current.Rows[i1]["solves_10"] + ",";
                stUsers += tbl_current.Rows[i1]["solves_3"] + ",";
                stUsers += tbl_current.Rows[i1]["puzzles_created"] + ",";
                stUsers += tbl_current.Rows[i1]["puzzles_created_bns"] + ",";
                stUsers += tbl_current.Rows[i1]["puzzles_created_10"] + ",";
                stUsers += tbl_current.Rows[i1]["puzzles_created_3"] + ",";
                stUsers += tbl_current.Rows[i1]["puzzles_created_0"] + ",";
                stUsers += tbl_current.Rows[i1]["smalleasy_left"] + ",";
                stUsers += tbl_current.Rows[i1]["specials_left"] + ",";
                stUsers += tbl_current.Rows[i1]["spores_left"] + ",";
                stUsers += tbl_current.Rows[i1]["bugs_left"] + ",";
                stUsers += tbl_current.Rows[i1]["spugs_left"] + ",";
                stUsers += tbl_current.Rows[i1]["basicshapes_left"] + ",";
                stUsers += tbl_current.Rows[i1]["smallswitches_left"] + ",";
                stUsers += tbl_current.Rows[i1]["switched_left"] + ",";
                stUsers += tbl_current.Rows[i1]["minibits_left"] + ",";
                stUsers += tbl_current.Rows[i1]["mirrors_left"] + ",";
                stUsers += tbl_current.Rows[i1]["rsod_left"] + ",";
                stUsers += tbl_current.Rows[i1]["tricksters_left"] + ",";
                stUsers += tbl_current.Rows[i1]["puzzlets_left"] + ",";
                stUsers += tbl_current.Rows[i1]["chainletters_left"] + ",";
                stUsers += tbl_current.Rows[i1]["smallshape_left"] + ",";
                stUsers += tbl_current.Rows[i1]["funandeasy_left"] + ",";
                stUsers += tbl_current.Rows[i1]["boostme_left"] + ",";
                stUsers += tbl_current.Rows[i1]["structuralrepresentation_left"];
                sw.WriteLine(stUsers);
            }
            sw.Close();
        }
    }
}
