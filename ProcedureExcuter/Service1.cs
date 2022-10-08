using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
 
using System.IO;
 
using System.Timers;

 
using System.Data.SqlClient;
 
using System.Configuration;
namespace ProcedureExcuter
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer(); // name space(using System.Timers;)  

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            WriteToFile("Service is started at " + DateTime.Now);
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            //timer.Interval = 86400000; //number in milisecinds  //everyday
             timer.Interval = 60000; //number in milisecinds  //everyday
            timer.Enabled = true;
        }

        protected override void OnStop()
        {

            WriteToFile("Service is stopped at " + DateTime.Now);

        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ConnectionString);
          

            try
            {
                SqlCommand cmd = new SqlCommand("ProcedutreName", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                var result = (Int64)cmd.ExecuteNonQuery();               // count =0


                WriteToFile("Service is recall at " + DateTime.Now);


            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }


          





        }
        public void WriteToFile(string Message) //normal method to write in file 
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }
    }
}
