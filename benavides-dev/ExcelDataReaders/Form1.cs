using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BenavidesFarm.DataModels.Models;
using System.IO;
using BenavidesFarm.DataModels.Models.Pages.Elements;
using System.Net.Mail;
using System.Net;

namespace ExcelDataReaders
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonLoadCitiesInformation_Click(object sender, EventArgs e)
        {

            
            string fileName = "K:/csv.csv";
            MyApplicationDbContext context = new MyApplicationDbContext();
            //List<String> messages = new List<string>();
            Dictionary<String, Estados> states = new Dictionary<string, Estados>();
            Dictionary<String, Municipios> cities = new Dictionary<string, Municipios>();


            Dictionary<String, int> MiArreglo = new Dictionary<String, int>();
            MiArreglo.Add("roger", 2);



            StreamReader fileReader = new StreamReader(fileName,Encoding.UTF8);
            while (!fileReader.EndOfStream)
            {
                string line = fileReader.ReadLine();

                string[] lineComponents = this.getLineComponents(line);
                string ceco = lineComponents[0];
                string branchName = lineComponents[1].Trim();
                string stateName = lineComponents[2].Trim();
                string cityName = lineComponents[3].Trim();
                string latitude = lineComponents[4].Trim();
                string longitude = lineComponents[5].Trim();

                Estados state = null;

                if (!states.ContainsKey(stateName))
                {
                    state = new Estados() { Name = stateName };
                    context.Estados.Add(state);
                    context.SaveChanges();
                    states[stateName] = state;
                }
                else
                {
                    state = states[stateName];
                }

                Municipios city = null;
                if (!cities.ContainsKey(cityName))
                {
                    city = new Municipios() { Name = cityName };
                    var cs = new EstadosMunicipios()
                    {
                        Estado = state,
                        Municipio = city
                    };
                    context.Municipios.Add(city);
                    context.EstadosMunicipios.Add(cs);
                    context.SaveChanges();
                    cities[cityName] = city;
                }
                else
                {
                    city = cities[cityName];
                }

                Branch b = new Branch()
                {
                    BranchName = branchName,
                    BranchLatitude = latitude,
                    BranchLongitude = longitude,
                    BranchCeco = ceco,
                    BranchActive = true,
                    BranchConsult = true,
                    BranchTwentyFourHours = true,
                    BranchFose = true,
                    City = city,
                    State = state
                };
                context.Branchs.Add(b);
                context.SaveChanges();

            }

            //listBox1.DataSource = messages;

            fileReader.Close();

        }

        private string[] getLineComponents(string line)
        {
            string[] result;

            if(line.Contains("\""))
            {
                string[] tmpResult = line.Split('\"');
                result = new string[] { tmpResult[0].Replace(",","").Trim(), tmpResult[1]};
                string thirdElement = tmpResult[2].Trim().Remove(0, 1).Trim();
                result = result.Union(thirdElement.Split(',')).ToArray();
            }
            else
            {
                result = line.Split(',');
            }


            return result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string from = "yamonteagudo@uci.cu";
            string server = "smtp.uci.cu";
            string username = "yamonteagudo@uci.cu";
            string password = "L@ s1ngandinga779*";

            MailMessage message = new MailMessage();
          
            message.To.Add("jmsillero@uci.cu");
            message.To.Add("yamonteagudo@uci.cu");

            message.From = new MailAddress(from);


            message.Subject = "Using the new SMTP client.";
            message.Body = @"Using this new feature, you can send an e-mail message from an application very easily.";
            SmtpClient client = new SmtpClient(server);
            ServicePointManager.ServerCertificateValidationCallback = (obj, certificate, chain, sslPolicyErrors) => true;
            client.Credentials = new NetworkCredential(username, password);
            client.EnableSsl = true;

            
            client.Send(message);
           
           
        }
    }
}
