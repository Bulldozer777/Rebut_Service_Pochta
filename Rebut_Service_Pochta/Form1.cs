using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rebut_Service_Pochta
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            AutoCompleteStringCollection source = new AutoCompleteStringCollection()
        {
            "10.94.73.30",
            "10.94.",
            "10.94.1.141",
            "Кустов"
        };
            textBox2.AutoCompleteCustomSource = source;
            textBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBox3.AutoCompleteCustomSource = source;
            textBox3.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox3.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBox19.AutoCompleteCustomSource = source;
            textBox19.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox19.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBox15.AutoCompleteCustomSource = source;
            textBox15.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox15.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBox17.AutoCompleteCustomSource = source;
            textBox17.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox17.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBox20.AutoCompleteCustomSource = source;
            textBox20.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox20.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }




        // Код основной программы



        public void Powershell_service(string ip, string action, string name_service, string action_more, out string p)     
        {         
            Process process1 = Process.Start(new ProcessStartInfo
            {
                FileName = "powershell",
                Arguments = "/command set-service " + name_service + " -ComputerName " + ip + " -Status " + action + " -PassThru | format-table Status -autosize" + action_more,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true
            });
            Thread.Sleep(500);
            string h = process1.StandardOutput.ReadToEnd();
            Thread.Sleep(500);
            string k = h.Substring(18);
            if (k == "\r\nStopped\r\n\r\n\r\n")
            {
                p = k;
            }
            else if (k == "\r\nRunning\r\n\r\n\r\n")
            {
                p = k;
            }
            else
                p = "Блок еlse";
        }

    

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        // Кнопка "Сохранить"

        private void button37_Click(object sender, EventArgs e)
        {
            if(textBox19.Text != "")
            {
                textBox5.Text = textBox19.Text;
                textBox7.Text = textBox19.Text;
                textBox9.Text = textBox19.Text;
                textBox11.Text = textBox19.Text;
                textBox3.Text = textBox19.Text;
                textBox13.Text = textBox19.Text;
            }
        }

        // Кнопка "Отменить"

        private void button41_Click(object sender, EventArgs e)
        {
            if (textBox5.Text != "" | textBox7.Text != ""| textBox9.Text != "" | textBox11.Text != "" | textBox3.Text != "" | textBox13.Text != "")
            {
                textBox5.Text = "";
                textBox7.Text = "";
                textBox9.Text = "";
                textBox11.Text = "";
                textBox3.Text = "";
                textBox13.Text = "";
            }
        }


        // Перезапуск всех служб МПК


        
        private void button38_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                     // отменяет отслеживание ошибок,
                                                     // но дает передать компоненты формы в другой поток 

            Thread thread = new Thread(
                () =>
                {
                    //Action action = () =>
                    //{
                    Rebut_MPK_Service(progressBar10);
                    //};
                    //if (InvokeRequired)
                    //    Invoke(action);
                    //else
                    //    action(); /*реализация через делегат action*/

                    // Invoke((MethodInvoker)(() =>
                    //{
                    //    Rebut_MPK_Service(progressBar10);
                    //}));

                });
            thread.Start();
            //try
            //{
            //    progressBar10.Minimum = 0;
            //    progressBar10.Maximum = 100;
            //    string ip = textBox19.Text;
            //    progressBar10.Value = 10;
            //    progressBar10.Value = 20;
            //    progressBar10.Value = 30;
            //    progressBar10.Value = 40;
            //    string check_action;
            //    string[] mass_name_service = new string[] { "RussianPostEASconfiguration",
            //            "RussianPostEASnsi", "RussianPostEASsdo", "RussianPostEAStrans", "RussianPostEASupdate", "RussianPostEASuser" };
            //    string[] mass_result = new string[mass_name_service.Length];
            //    for (int i = 0; i < mass_name_service.Length; i++)
            //    {
            //        Powershell_service(ip, "Stopped", mass_name_service[i], "", out check_action);
            //        if (check_action == "\r\nStopped\r\n\r\n\r\n")
            //        {
            //            progressBar10.Value += 10;
            //            Powershell_service(ip, "Running", mass_name_service[i], "", out check_action);
            //            if (check_action == "\r\nRunning\r\n\r\n\r\n")
            //                mass_result[i] = $"\nСлужба \"{mass_name_service[i]}\"\n\nНа компьютере {ip} - Перезапущена\n";
            //            else
            //                mass_result[i] = $"\nСлужба \"{mass_name_service[i]}\"\n\nНа компьютере {ip} - Не перезапущена\ncheck_action != Running";
            //        }
            //        else
            //            mass_result[i] = $"\nСлужба \"{mass_name_service[i]}\"\n\nНа компьютере {ip} - Не перезапущена\ncheck_action != Stopped";
            //    }
            //    progressBar10.Value = 100;
            //    string result = "";
            //    foreach (string i in mass_result)
            //        result += i;
            //        MessageBox.Show($"{result} ");
            //    progressBar10.Value = 0;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Ошибка: \n{ex}");
            //}
        }
        public void Rebut_MPK_Service(ProgressBar progressBar)
        {
            if (textBox19.Text != "")
            {
                try
                {
                    progressBar.Minimum = 0;
                    progressBar.Maximum = 100;
                    string ip = textBox19.Text;
                    progressBar.Value = 10;
                    progressBar.Value = 20;
                    progressBar.Value = 30;
                    progressBar.Value = 40;
                    string check_action;
                    string[] mass_name_service = new string[] { "RussianPostEASconfiguration",
                        "RussianPostEASnsi", "RussianPostEASsdo", "RussianPostEAStrans", "RussianPostEASupdate", "RussianPostEASuser" };
                    string[] mass_result = new string[mass_name_service.Length];
                    for (int i = 0; i < mass_name_service.Length; i++)
                    {
                        Powershell_service(ip, "Stopped", mass_name_service[i], "", out check_action);
                        if (check_action == "\r\nStopped\r\n\r\n\r\n")
                        {
                            progressBar.Value += 10;
                            Powershell_service(ip, "Running", mass_name_service[i], "", out check_action);
                            if (check_action == "\r\nRunning\r\n\r\n\r\n")
                                mass_result[i] = $"\nСлужба \"{mass_name_service[i]}\"\n\nНа компьютере {ip} - Перезапущена\n";
                            else
                                mass_result[i] = $"\nСлужба \"{mass_name_service[i]}\"\n\nНа компьютере {ip} - Не перезапущена\ncheck_action != Running";
                        }
                        else
                            mass_result[i] = $"\nСлужба \"{mass_name_service[i]}\"\n\nНа компьютере {ip} - Не перезапущена\ncheck_action != Stopped";
                    }
                    progressBar.Value = 100;
                    string result = "";
                    foreach (string i in mass_result)
                        result += i;
                    MessageBox.Show($"{result} ");
                    progressBar.Value = 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: \n{ex}");
                }
            }
            else
                MessageBox.Show("Поле пустое");
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

      

       // Использование метода служащего для получения состояния конкретной службы

        private void button25_Click(object sender, EventArgs e)
        {
            string p = textBox13.Text;
            if (textBox13.Text != "")
            {
                Process process = Process.Start(new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = "/command get-service -DisplayName \"RussianPostEASuser\" -ComputerName " + p + " | format-table Status -autosize",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true
                });
                textBox14.Text = process.StandardOutput.ReadToEnd().Substring(18);
            }
            else
                MessageBox.Show("Поле для ввода пустое = \"\"");
        }
        private void button9_Click(object sender, EventArgs e)
        {
            string p = textBox5.Text;
            if (textBox5.Text != "")
            {
                Process process = Process.Start(new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = "/command get-service -DisplayName \"RussianPostEASconfiguration\" -ComputerName " + p + " | format-table Status -autosize",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true
                });
                textBox6.Text = process.StandardOutput.ReadToEnd().Substring(18);
            }
            else
                MessageBox.Show("Поле для ввода пустое = \"\"");
        }
        private void button13_Click(object sender, EventArgs e)
        {
            string p = textBox7.Text;
            if (textBox7.Text != "")
            {
                Process process = Process.Start(new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = "/command get-service -DisplayName \"RussianPostEASnsi\" -ComputerName " + p + " | format-table Status -autosize",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true
                });
                textBox8.Text = process.StandardOutput.ReadToEnd().Substring(18);
            }
            else
                MessageBox.Show("Поле для ввода пустое = \"\"");
        }
        private void button17_Click(object sender, EventArgs e)
        {
            string p = textBox9.Text;
            if (textBox9.Text != "")
            {
                Process process = Process.Start(new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = "/command get-service -DisplayName \"RussianPostEASsdo\" -ComputerName " + p + " | format-table Status -autosize",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true
                });
                textBox10.Text = process.StandardOutput.ReadToEnd().Substring(18);
            }
            else
                MessageBox.Show("Поле для ввода пустое = \"\"");
        }
        private void button21_Click(object sender, EventArgs e)
        {
            string p = textBox11.Text;
            if (textBox11.Text != "")
            {
                Process process = Process.Start(new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = "/command get-service -DisplayName \"RussianPostEAStrans\" -ComputerName " + p + " | format-table Status -autosize",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true
                });
                textBox12.Text = process.StandardOutput.ReadToEnd().Substring(18);
            }
            else
                MessageBox.Show("Поле для ввода пустое = \"\"");
        }
        private void button8_Click(object sender, EventArgs e)
        {
            string p = textBox3.Text;
            if (textBox3.Text != "")
            {
                Process process = Process.Start(new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = "/command get-service -DisplayName \"RussianPostEASupdate\" -ComputerName " + p + " | format-table Status -autosize",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true
                });
                textBox4.Text = process.StandardOutput.ReadToEnd().Substring(18);
            }
            else
                MessageBox.Show("textBox3.Text = \"\"");
        }

        // Метод для получения состояния конкретной службы

        public void Status_Service(string ip, string service_name, out string status_service)
        {
            status_service = "";
            Process process = Process.Start(new ProcessStartInfo
            {
                FileName = "powershell",
                Arguments = $"/command get-service -DisplayName \"{service_name}\" -ComputerName " + ip + " | format-table Status -autosize",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true
            });
            if (process.StandardOutput.ReadToEnd() == "")
            {
                MessageBox.Show($"Нет подключения к пк - {ip}");
            
            }
            else
                status_service = process.StandardOutput.ReadToEnd();
        }


        // Состояние всех служб

        private void button42_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            Thread myThread_1 = new Thread(new ThreadStart(Status_all_Service));
            myThread_1.Start();
            listBox1.Items.AddRange(Status_all_Service_1());
        }

        public string[] Status_all_Service_1()
        {     
            string p = textBox19.Text;
            if (textBox19.Text != "")
            {
                Process process = Process.Start(new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = $"/command get-service -DisplayName RussianPostEAS* -ComputerName {p} | format-table Status -autosize",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true
                });
                string status_service = process.StandardOutput.ReadToEnd().Substring(18);
                string[] mass_status = new string[6];
                string status_service_1 = status_service.Replace("\r\n", " ");
                string[] status_service_mass = status_service_1.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string[] mass_name_service = new string[]{ "RussianPostEASconfiguration",
                        "RussianPostEASnsi", "RussianPostEASsdo", "RussianPostEAStrans", "RussianPostEASupdate", "RussianPostEASuser" };
                for (int i = 0; i < mass_status.Length; i++)
                {
                    if (status_service_mass[i] == "------")
                    {
                        status_service_mass[i] = "Running";
                        mass_status[i] = mass_name_service[i] + " - " + status_service_mass[i];
                    }
                    else
                        mass_status[i] = mass_name_service[i] + " - " + status_service_mass[i];
                }
                return mass_status;
            }
            else
                MessageBox.Show("Поле для ввода пустое = \"\"");
            string[] m = new string[1];
            return m;
        }
        public void Status_all_Service()
        {
            Status_all_Service_1();
        }



        //Остановка всех служб

        private void button16_Click_1(object sender, EventArgs e)
        {
            try
            {
                string name_service = "RussianPostEASnsi";
                progressBar4.Minimum = 0;
                progressBar4.Maximum = 100;
                string ip = textBox7.Text;
                progressBar4.Value = 40;
                string check_action;
                Powershell_service(ip, "Stopped", name_service, "", out check_action);
                if (check_action == "\r\nStopped\r\n\r\n\r\n")
                {
                    progressBar4.Value = 100;
                    MessageBox.Show($"Служба \"name_service\"\nНа компьютере {ip} - Остановлена");
                    progressBar4.Value = 0;
                    textBox8.Text = check_action.Replace("\r\n", "");
                }
                if (check_action == "Блок еlse")
                {
                    MessageBox.Show($"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - Остановлена");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            try
            {
                string name_service = "RussianPostEASsdo";
                progressBar5.Minimum = 0;
                progressBar5.Maximum = 100;
                string ip = textBox9.Text;
                progressBar5.Value = 40;
                string check_action;
                Powershell_service(ip, "Stopped", name_service, "", out check_action);
                if (check_action == "\r\nStopped\r\n\r\n\r\n")
                {
                    progressBar5.Value = 100;
                    MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - Остановлена");
                    progressBar5.Value = 0;
                    textBox10.Text = check_action.Replace("\r\n", "");
                }
                if (check_action == "Блок еlse")
                {
                    MessageBox.Show($"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - Остановлена");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }

        private void button24_Click(object sender, EventArgs e)
        {
            try
            {
                string name_service = "RussianPostEAStrans";
                progressBar6.Minimum = 0;
                progressBar6.Maximum = 100;
                string ip = textBox11.Text;
                progressBar6.Value = 40;
                string check_action;
                Powershell_service(ip, "Stopped", name_service, "", out check_action);
                if (check_action == "\r\nStopped\r\n\r\n\r\n")
                {
                    progressBar6.Value = 100;
                    MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - Остановлена");
                    progressBar6.Value = 0;
                    textBox12.Text = check_action.Replace("\r\n", "");
                }
                if (check_action == "Блок еlse")
                {
                    MessageBox.Show($"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - Остановлена");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }
        private void button28_Click(object sender, EventArgs e)
        {
            try
            {
                string name_service = "RussianPostEASuser";
                progressBar7.Minimum = 0;
                progressBar7.Maximum = 100;
                string ip = textBox13.Text;
                progressBar7.Value = 40;
                string check_action;
                Powershell_service(ip, "Stopped", name_service, "", out check_action);
                if (check_action == "\r\nStopped\r\n\r\n\r\n")
                {
                    progressBar7.Value = 100;
                    MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - Остановлена");
                    progressBar7.Value = 0;
                    textBox14.Text = check_action.Replace("\r\n", "");
                }
                if (check_action == "Блок еlse")
                {
                    MessageBox.Show($"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - Остановлена");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }
        private void button6_Click_1(object sender, EventArgs e)
        {
            try
            {
                string name_service = "RussianPostEASupdate";
                progressBar2.Minimum = 0;
                progressBar2.Maximum = 100;
                string ip = textBox3.Text;
                progressBar2.Value = 10;
                progressBar2.Value = 20;
                progressBar2.Value = 30;
                progressBar2.Value = 40;
                string check_action;
                Powershell_service(ip, "Stopped", name_service, "", out check_action);
                if (check_action == "\r\nStopped\r\n\r\n\r\n")
                {
                    progressBar2.Value = 100;
                    MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - Остановлена");
                    progressBar2.Value = 0;
                    textBox4.Text = check_action.Replace("\r\n", "");
                }
                if (check_action == "Блок еlse")
                {
                    MessageBox.Show($"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - Остановлена");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }
        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                string name_service = "RussianPostEASconfiguration";
                progressBar3.Minimum = 0;
                progressBar3.Maximum = 100;
                string ip = textBox5.Text;
                progressBar3.Value = 40;
                string check_action;
                Powershell_service(ip, "Stopped", name_service, "", out check_action);
                if (check_action == "\r\nStopped\r\n\r\n\r\n")
                {
                    progressBar3.Value = 100;
                    MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - Остановлена");
                    progressBar3.Value = 0;
                    textBox6.Text = check_action.Replace("\r\n", "");
                }
                if (check_action == "Блок еlse")
                {
                    MessageBox.Show($"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - Остановлена");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }

        // Запуск конкретных служб

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                string name_service = "RussianPostEASupdate";
                progressBar2.Minimum = 0;
                progressBar2.Maximum = 100;
                string ip = textBox3.Text;
                progressBar2.Value = 40;
                string check_action;
                Powershell_service(ip, "Running", name_service, "", out check_action);
                if (check_action == "\r\nRunning\r\n\r\n\r\n")
                {
                    progressBar2.Value = 100;
                    MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - Запущена");
                    progressBar2.Value = 0;
                    textBox4.Text = check_action.Replace("\r\n", "");
                }
                if (check_action == "Блок еlse")
                {
                    MessageBox.Show($"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - Запущена");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                string name_service = "RussianPostEASconfiguration";
                progressBar3.Minimum = 0;
                progressBar3.Maximum = 100;
                string ip = textBox5.Text;
                progressBar3.Value = 40;
                string check_action;
                Powershell_service(ip, "Running", name_service, "", out check_action);
                if (check_action == "\r\nRunning\r\n\r\n\r\n")
                {
                    progressBar3.Value = 100;
                    MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - Запущена");
                    progressBar3.Value = 0;
                    textBox6.Text = check_action.Replace("\r\n", "");
                }
                if (check_action == "Блок еlse")
                {
                    MessageBox.Show($"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - Запущена");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                string name_service = "RussianPostEASnsi";
                progressBar4.Minimum = 0;
                progressBar4.Maximum = 100;
                string ip = textBox7.Text;
                progressBar4.Value = 40;
                string check_action;
                Powershell_service(ip, "Running", name_service, "", out check_action);
                if (check_action == "\r\nRunning\r\n\r\n\r\n")
                {
                    progressBar4.Value = 100;
                    MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - Запущена");
                    progressBar4.Value = 0;
                    textBox8.Text = check_action.Replace("\r\n", "");
                }
                if (check_action == "Блок еlse")
                {
                    MessageBox.Show($"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - Запущена");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            try
            {
                string name_service = "RussianPostEASsdo";
                progressBar5.Minimum = 0;
                progressBar5.Maximum = 100;
                string ip = textBox9.Text;
                progressBar5.Value = 40;
                string check_action;
                Powershell_service(ip, "Running", name_service, "", out check_action);
                if (check_action == "\r\nRunning\r\n\r\n\r\n")
                {
                    progressBar5.Value = 100;
                    MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - Запущена");
                    progressBar5.Value = 0;
                    textBox10.Text = check_action.Replace("\r\n", "");
                }
                if (check_action == "Блок еlse")
                {
                    MessageBox.Show($"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - Запущена");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            try
            {
                string name_service = "RussianPostEAStrans";
                progressBar6.Minimum = 0;
                progressBar6.Maximum = 100;
                string ip = textBox11.Text;
                progressBar6.Value = 40;
                string check_action;
                Powershell_service(ip, "Running", name_service, "", out check_action);
                if (check_action == "\r\nRunning\r\n\r\n\r\n")
                {
                    progressBar6.Value = 100;
                    MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - Запущена");
                    progressBar6.Value = 0;
                    textBox12.Text = check_action.Replace("\r\n", "");
                }
                if (check_action == "Блок еlse")
                {
                    MessageBox.Show($"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - Запущена");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }

        private void button27_Click(object sender, EventArgs e)
        {
            try
            {
                string name_service = "RussianPostEASuser";
                progressBar7.Minimum = 0;
                progressBar7.Maximum = 100;
                string ip = textBox13.Text;
                progressBar7.Value = 40;
                string check_action;
                Powershell_service(ip, "Running", name_service, "", out check_action);
                if (check_action == "\r\nRunning\r\n\r\n\r\n")
                {
                    progressBar7.Value = 100;
                    MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - Запущена");
                    progressBar7.Value = 0;
                    textBox14.Text = check_action.Replace("\r\n", "");
                }
                if (check_action == "Блок еlse")
                {
                    MessageBox.Show($"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - Запущена");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }


        // Перезапуск конкретных служб

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string name_service = "RussianPostEASupdate";
                progressBar2.Minimum = 0;
                progressBar2.Maximum = 100;
                string ip = textBox3.Text;
                progressBar2.Value = 10;
                progressBar2.Value = 20;
                progressBar2.Value = 30;
                progressBar2.Value = 40;
                string check_action;
                Powershell_service(ip, "Stopped", name_service, "", out check_action);
                if (check_action == "\r\nStopped\r\n\r\n\r\n")
                {
                    Powershell_service(ip, "Running", name_service, "", out check_action);
                    progressBar2.Value = 100;
                    MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - Перезапущена\n");
                    progressBar2.Value = 0;
                    textBox4.Text = check_action.Replace("\r\n", "");
                }
                if (check_action == "Блок еlse")
                {
                    MessageBox.Show($"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - Остановлена");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {              
                string name_service = "RussianPostEASconfiguration";
                progressBar3.Minimum = 0;
                progressBar3.Maximum = 100;
                string ip = textBox5.Text;
                progressBar3.Value = 40;
                string check_action;
                Powershell_service(ip, "Stopped", name_service, "", out check_action);
                if (check_action == "\r\nStopped\r\n\r\n\r\n")
                {
                    Powershell_service(ip, "Running", name_service, "", out check_action);
                    progressBar3.Value = 100;
                    MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - Перезапущена\n");
                    progressBar3.Value = 0;
                    textBox6.Text = check_action.Replace("\r\n", "");
                }
                if (check_action == "Блок еlse")
                {
                    MessageBox.Show($"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - Остановлена");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            try
            {
                string name_service = "RussianPostEASnsi";
                progressBar4.Minimum = 0;
                progressBar4.Maximum = 100;
                string ip = textBox7.Text;
                progressBar4.Value = 40;
                string check_action;
                Powershell_service(ip, "Stopped", name_service, "", out check_action);
                if (check_action == "\r\nStopped\r\n\r\n\r\n")
                {
                    Powershell_service(ip, "Running", name_service, "", out check_action);
                    progressBar4.Value = 100;
                    MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - Перезапущена\n");
                    progressBar4.Value = 0;
                    textBox8.Text = check_action.Replace("\r\n", "");
                }
                if (check_action == "Блок еlse")
                {
                    MessageBox.Show($"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - Остановлена");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            try
            {
                string name_service = "RussianPostEASsdo";
                progressBar5.Minimum = 0;
                progressBar5.Maximum = 100;
                string ip = textBox9.Text;
                progressBar5.Value = 40;
                string check_action;
                Powershell_service(ip, "Stopped", name_service, "", out check_action);
                if (check_action == "\r\nStopped\r\n\r\n\r\n")
                {
                    Powershell_service(ip, "Running", name_service, "", out check_action);
                    progressBar5.Value = 100;
                    MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - Перезапущена\n");
                    progressBar5.Value = 0;
                    textBox10.Text = check_action.Replace("\r\n", "");
                }
                if (check_action == "Блок еlse")
                {
                    MessageBox.Show($"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - Остановлена");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            try
            {
                string name_service = "RussianPostEAStrans";
                progressBar6.Minimum = 0;
                progressBar6.Maximum = 100;
                string ip = textBox11.Text;
                progressBar6.Value = 40;
                string check_action;
                Powershell_service(ip, "Stopped", name_service, "", out check_action);
                if (check_action == "\r\nStopped\r\n\r\n\r\n")
                {
                    Powershell_service(ip, "Running", name_service, "", out check_action);
                    progressBar6.Value = 100;
                    MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - Перезапущена\n");
                    progressBar6.Value = 0;
                    textBox12.Text = check_action.Replace("\r\n", "");
                }
                if (check_action == "Блок еlse")
                {
                    MessageBox.Show($"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - Остановлена");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }

        private void button26_Click(object sender, EventArgs e)
        {
            try
            {
                string name_service = "RussianPostEASuser";
                progressBar7.Minimum = 0;
                progressBar7.Maximum = 100;
                string ip = textBox13.Text;
                progressBar7.Value = 40;
                string check_action;
                Powershell_service(ip, "Stopped", name_service, "", out check_action);
                if (check_action == "\r\nStopped\r\n\r\n\r\n")
                {
                    Powershell_service(ip, "Running", name_service, "", out check_action);
                    progressBar7.Value = 100;
                    MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - Перезапущена\n");
                    progressBar7.Value = 0;
                    textBox14.Text = check_action.Replace("\r\n", "");
                }
                if (check_action == "Блок еlse")
                {
                    MessageBox.Show($"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - Остановлена");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }

        // Остановка всех служб

        private void button39_Click(object sender, EventArgs e)
        {
            if (textBox19.Text != "")
            {
                CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                         // отменяет отслеживание ошибок,
                                                         // но дает передать компоненты формы в другой поток 

                Thread thread = new Thread(
                    () =>
                    {
                    //Action action = () =>
                    //{
                    Stop_Service_All(progressBar11, textBox19);
                    //};
                    //if (InvokeRequired)
                    //    Invoke(action);
                    //else
                    //    action(); /*реализация через делегат action*/

                    // Invoke((MethodInvoker)(() =>
                    //{
                    //    Rebut_MPK_Service(progressBar10);
                    //}));

                });
                thread.Start();
            }
            else
                MessageBox.Show($"Поле для ввода пустое");

            //Stop_Service_All()
        }

        public void Stop_Service_All(ProgressBar progress, TextBox text)
        {
                try
                {
                    string[] mass_name_service = new string[]{ "RussianPostEASconfiguration",
                        "RussianPostEASnsi", "RussianPostEASsdo", "RussianPostEAStrans", "RussianPostEASupdate", "RussianPostEASuser" };
                    string[] mass_result = new string[mass_name_service.Length];
                    progress.Minimum = 0;
                    progress.Maximum = 100;
                    string ip = text.Text;
                    progress.Value = 40;
                    string check_action;
                    for (int i = 0; i < mass_name_service.Length; i++)
                    {
                        Powershell_service(ip, "Stopped", mass_name_service[i], "", out check_action);
                        if (check_action == "\r\nStopped\r\n\r\n\r\n")
                        {
                            progress.Value += 10;
                            mass_result[i] = mass_name_service[i] + " - " + "Stopped (остановлена)\n";
                        }
                        if (check_action == "Блок еlse")
                        {
                            mass_result[i] = $"\nСлужба \"{ mass_name_service[i]}\"на компьютере {ip} - Остановлена";
                        }
                    }
                    progress.Value = 100;
                    string result = "";
                    foreach (string i in mass_result)
                        result += i;
                    MessageBox.Show($"\nCлужбы на компьютере {ip}: \n\n{result} ");
                    progress.Value = 0;

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: \n{ex}");
                }
        }

        // Запуск всех служб

        private void button40_Click(object sender, EventArgs e)
        {
            if (textBox19.Text != "")
            {
                CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                         // отменяет отслеживание ошибок,
                                                         // но дает передать компоненты формы в другой поток 

                Thread thread = new Thread(
                    () =>
                    {
                        //Action action = () =>
                        //{
                        Start_ALL_Service(progressBar12, textBox19);
                        //};
                        //if (InvokeRequired)
                        //    Invoke(action);
                        //else
                        //    action(); /*реализация через делегат action*/

                        // Invoke((MethodInvoker)(() =>
                        //{
                        //    Rebut_MPK_Service(progressBar10);
                        //}));

                    });
                thread.Start();
            }
            else
                MessageBox.Show($"Поле для ввода пустое");
        }
        public void Start_ALL_Service(ProgressBar progress, TextBox text)
        {
            try
            {
                string[] mass_name_service = new string[]{ "RussianPostEASconfiguration",
                        "RussianPostEASnsi", "RussianPostEASsdo", "RussianPostEAStrans", "RussianPostEASupdate", "RussianPostEASuser" };
                string[] mass_result = new string[mass_name_service.Length];
                progress.Minimum = 0;
                progress.Maximum = 100;
                string ip = text.Text;
                progress.Value = 40;
                string check_action;
                for (int i = 0; i < mass_name_service.Length; i++)
                {
                    Powershell_service(ip, "Running", mass_name_service[i], "", out check_action);
                    if (check_action == "\r\nRunning\r\n\r\n\r\n")
                    {
                        progress.Value += 10;
                        mass_result[i] = mass_name_service[i] + " - " + "Running (запущена)\n";
                    }
                    if (check_action == "Блок еlse")
                    {
                        mass_result[i] = $"\nСлужба \"{ mass_name_service[i]}\"на компьютере {ip} - Запущена";
                    }
                }
                progress.Value = 100;
                string result = "";
                foreach (string i in mass_result)
                    result += i;
                MessageBox.Show($"\nCлужбы на компьютере {ip}: \n\n{result} ");
                progress.Value = 0;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }
        // Службы GMMQ и Sheduller 

        // Остановка 

        private void button32_Click(object sender, EventArgs e)
        {
            try
            {
                string name_service = "GMMQ";
                progressBar8.Minimum = 0;
                progressBar8.Maximum = 100;
                string ip = textBox15.Text;
                progressBar8.Value = 40;
                Powershell_service_Force(ip, "Stopped", name_service, "");
                progressBar8.Value = 100;
                MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - Остановлена");
                progressBar8.Value = 0;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }

        private void button36_Click(object sender, EventArgs e)
        {
            try
            {
                string name_service = "GM_SchedulerSvc";
                progressBar9.Minimum = 0;
                progressBar9.Maximum = 100;
                string ip = textBox17.Text;
                progressBar9.Value = 40;
                Powershell_service_Force(ip, "Stopped", name_service, "");
                    progressBar9.Value = 100;
                    MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - Остановлена");
                    progressBar9.Value = 0;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }
        public void Powershell_service_Force(string ip, string action, string name_service, string action_more)
        {
           Process.Start(new ProcessStartInfo
            {
                FileName = "powershell",
                Arguments = "/command Get-Service -Computer " + ip + " -Name " + name_service + " | Stop-Service -Force" + action_more,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true
            }).WaitForExit();
        }

        // Запуск 

        private void button31_Click(object sender, EventArgs e)
        {
            try
            {
                string name_service = "GMMQ";
                progressBar8.Minimum = 0;
                progressBar8.Maximum = 100;
                string ip = textBox15.Text;
                progressBar8.Value = 40;
                string check_action;
                Powershell_service(ip, "Running", name_service, "", out check_action);
                if (check_action == "\r\nRunning\r\n\r\n\r\n")
                {
                    progressBar8.Value = 100;
                    MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - Запущена");
                    progressBar8.Value = 0;
                    textBox16.Text = check_action.Replace("\r\n", "");
                }
                if (check_action == "Блок еlse")
                {
                    MessageBox.Show($"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - Запущена");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }

        private void button35_Click(object sender, EventArgs e)
        {
            try
            {
                string name_service = "GM_SchedulerSvc";
                progressBar9.Minimum = 0;
                progressBar9.Maximum = 100;
                string ip = textBox17.Text;
                progressBar9.Value = 40;
                string check_action;
                Powershell_service(ip, "Running", name_service, "", out check_action);
                if (check_action == "\r\nRunning\r\n\r\n\r\n")
                {
                    progressBar9.Value = 100;
                    MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - Запущена");
                    progressBar9.Value = 0;
                    textBox18.Text = check_action.Replace("\r\n", "");
                }
                if (check_action == "Блок еlse")
                {
                    MessageBox.Show($"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - Запущена");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }

        // Состояние служб


        private void button29_Click(object sender, EventArgs e)
        {
            string p = textBox15.Text;
            if (textBox15.Text != "")
            {
                Process process = Process.Start(new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = "/command get-service -DisplayName \"GMMQ\" -ComputerName " + p + " | format-table Status -autosize",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true
                });
                textBox16.Text = process.StandardOutput.ReadToEnd().Substring(18);
            }
            else
                MessageBox.Show("Поле для ввода пустое = \"\"");
        }

        private void button33_Click(object sender, EventArgs e)
        {
            string p = textBox17.Text;
            if (textBox17.Text != "")
            {
                Process process = Process.Start(new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = "/command get-service -DisplayName \"GM_SchedulerSvc\" -ComputerName " + p + " | format-table Status -autosize",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true
                });
                textBox18.Text = process.StandardOutput.ReadToEnd().Substring(18);
            }
            else
                MessageBox.Show("Поле для ввода пустое = \"\"");
        }

        // Перезапуск 

        private void button30_Click(object sender, EventArgs e)
        {
            try
            {
                string name_service = "GMMQ";
                progressBar8.Minimum = 0;
                progressBar8.Maximum = 100;
                string ip = textBox15.Text;
                progressBar8.Value = 40;
                string check_action;
                Powershell_service(ip, "Stopped", name_service, "", out check_action);
                if (check_action == "\r\nStopped\r\n\r\n\r\n")
                {
                    progressBar8.Value = 70;
                    Powershell_service(ip, "Running", name_service, "", out check_action);
                    progressBar8.Value = 100;
                    MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - Перезапущена\n");
                    progressBar8.Value = 0;
                    textBox16.Text = check_action.Replace("\r\n", "");
                }
                if (check_action == "Блок еlse")
                {
                    MessageBox.Show($"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - Остановлена");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
            try
            {
                string name_service = "RussianPostEASconfiguration";
                progressBar3.Minimum = 0;
                progressBar3.Maximum = 100;
                string ip = textBox5.Text;
                progressBar3.Value = 40;
                string check_action;
                Powershell_service(ip, "Stopped", name_service, "", out check_action);
                if (check_action == "\r\nStopped\r\n\r\n\r\n")
                {
                    Powershell_service(ip, "Running", name_service, "", out check_action);
                    progressBar3.Value = 100;
                    MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - Перезапущена\n");
                    progressBar3.Value = 0;
                    textBox6.Text = check_action.Replace("\r\n", "");
                }
                if (check_action == "Блок еlse")
                {
                    MessageBox.Show($"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - Остановлена");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }

        private void button34_Click(object sender, EventArgs e)
        {
            try
            {
                string name_service = "GM_SchedulerSvc";
                progressBar9.Minimum = 0;
                progressBar9.Maximum = 100;
                string ip = textBox17.Text;
                progressBar9.Value = 40;
                string check_action;
                Powershell_service(ip, "Stopped", name_service, "", out check_action);
                if (check_action == "\r\nStopped\r\n\r\n\r\n")
                {
                    progressBar9.Value = 70;
                    Powershell_service(ip, "Running", name_service, "", out check_action);
                    progressBar9.Value = 100;
                    MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - Перезапущена\n");
                    progressBar9.Value = 0;
                    textBox18.Text = check_action.Replace("\r\n", "");
                }
                if (check_action == "Блок еlse")
                {
                    MessageBox.Show($"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - Остановлена");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }


        private void button43_Click(object sender, EventArgs e)
        {
            if (textBox20.Text != "")
            {
                textBox15.Text = textBox20.Text;
                textBox17.Text = textBox20.Text;
            }
        }

        private void button44_Click(object sender, EventArgs e)
        {
            if (textBox15.Text != "" | textBox17.Text != "")
            {
                textBox15.Text = "";
                textBox17.Text = "";
            }
        }




        // Основная программа закончена





        //static int x = 0;
        //Form1 i = new Form1();
        static object locker = new object();
        private void button1_Click(object sender, EventArgs e)
        {
            //textBox1.Clear();
            ////string ip = textBox2.Text;
            ////ProcessStartInfo procInfo;
            ////procInfo = new ProcessStartInfo("C://Windows//System32//cmd.exe",
            ////    $@"/k cd C:\Users\Eduard.Karpov\Downloads\PSToolssc && PsExec.exe \\{ip} net stop RussianPostEASupdate");
            ////Process.Start(procInfo);
            ////MessageBox.Show("Служба остановлена");
            ////textBox1.Text = "Служба остановлена";
            //progressBar1.Minimum = 0;
            //progressBar1.Maximum = 100;
            //string ip = textBox2.Text;
            //string action = "stop";
            //string name_service = "RussianPostEASupdate";
            //string action_more = "";
            //progressBar1.Value = 10;
            ////Service_Action_Class op = new Service_Action_Class(ip, action, name_service, action_more);
            //progressBar1.Value = 20;
            ////Thread myThread = new Thread((new ThreadStart(op.Service_Action)));
            //progressBar1.Value = 30;
            ////myThread.Start();
            //progressBar1.Value = 40;
            //int p;
            //Service_Action_1(ip, action, name_service, action_more, out p );
            //Thread.Sleep(500);
            //progressBar1.Value = 98;
            //string path = @"C:\Users\Eduard.Karpov\source\repos\Rebut_Service_Pochta\Rebut_Service_Pochta\trans\note.txt";
            //FileInfo fileInf = new FileInfo(path);
            //if (fileInf.Exists == false)
            //{
            //    progressBar1.Value = 100;
            //    MessageBox.Show("Служба остановлена");
            //    progressBar1.Value = 0;
            //    textBox2.Clear();
            //    textBox1.Text = "Служба остановлена";
            //}
            ////if (p == 100)
            ////{
            //progressBar1.Value = 100;
            ////    MessageBox.Show("Служба остановлена");
            ////    progressBar1.Value = 0;
            //textBox2.Clear();
            ////    textBox1.Text = "Служба остановлена";
            ////}

            ////op.Service_Action();
            ///

            Operation_Service("stop", "RussianPostEASupdate", "");
        }
        void Service()
        {

        }
        public void Service_Action_1(string ip, string action, string name_service, string action_more, out int p)
        {
            p = 10;
            lock (locker)
            {
                try
                {
                    ProcessStartInfo procInfo = new ProcessStartInfo();
                    progressBar1.Value = 50;
                    progressBar1.Value = 60;
                    procInfo.FileName = @"C:\Users\Eduard.Karpov\source\repos\Action_Service\Action_Service\bin\Debug\net5.0\Action_Service.exe";

                    if (action_more != "")
                    {
                        Operation_Service(ip, action, name_service, action_more, procInfo);
                    }
                    else
                    {
                        Operation_Service(ip, action, name_service, action_more, procInfo);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: \n{ex}");
                }
            }
        }
        public void Operation_Service(string ip, string action, string name_service, string action_more, ProcessStartInfo procInfo)
        {
            string path = @"C:\Users\Eduard.Karpov\source\repos\Rebut_Service_Pochta\Rebut_Service_Pochta\trans";
            string text = $"{ip} net {action} {name_service} {action_more}";
            using (FileStream fstream = new FileStream(path + @"\note.txt", FileMode.OpenOrCreate))
            {
                byte[] array = System.Text.Encoding.Default.GetBytes(text);
                fstream.Write(array, 0, array.Length);
            }
            Process.Start(procInfo);
        }
        class Service_Action_Class : Form1
        {
            private string ip;
            private string action;
            private string name_service;
            private string action_more;
            //private Form1 i;
            public Service_Action_Class(string _ip, string _action, string _name_service, string _action_more)
            {
                this.ip = _ip;
                this.action = _action;
                this.name_service = _name_service;
                this.action_more = _action_more;
                //this.i = _i;
            }
            public void Service_Action()
            {
                lock (locker)
                {
                    try
                    {

                        //progressBar1.Minimum = 0;
                        //progressBar1.Maximum = 100;
                        //ip = i.textBox2.Text;

                        ProcessStartInfo procInfo = new ProcessStartInfo();
                        //i.progressBar1.Value = 10;
                        //procInfo = new ProcessStartInfo("C://Windows//System32//cmd.exe",
                        //    @"\k cd C:\Users\Eduard.Karpov\Downloads\PSToolssc && PsExec.exe \\" + ip + " net " + action + " " + name_service + " " + action_more);
                        //i.progressBar1.Value = 20;
                        procInfo.FileName = "C://Users//Eduard.Karpov//source//repos//Action_Service//Action_Service//bin//Debug//net5.0//Action_Service.exe";
                        //procInfo.Arguments = @"\k cd C:\Users\Eduard.Karpov\source\repos\Rebut_Service_Pochta\Rebut_Service_Pochta\PSTools && PsExec.exe \\" + ip + " net " + action + " " + name_service + " " + action_more;
                        Process.Start(procInfo);
                        //i.progressBar1.Value = 100;


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка: \n{ex}");
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Operation_Service("start", "RussianPostEASupdate", "");
        }
        private void button3_Click(object sender, EventArgs e)
        {
            Operation_Service("stop", "RussianPostEASupdate", "&& net start RussianPostEASupdate");
        }
        public void Operation_Service(string action, string name_service, string action_more)
        {
            try
            {
                textBox1.Clear();
                progressBar1.Minimum = 0;
                progressBar1.Maximum = 100;
                string ip = textBox2.Text;
                progressBar1.Value = 10;
                progressBar1.Value = 20;
                progressBar1.Value = 30;
                progressBar1.Value = 40;
                int p;
                Service_Action_1(ip, action, name_service, action_more, out p);
                Thread.Sleep(500);
                progressBar1.Value = 98;
                if (action_more == "")
                {
                    if (action == "stop")
                        End_Operation_Service("oстановлена");
                    else if (action == "start")
                        End_Operation_Service("запущена");
                    else
                        MessageBox.Show($"Ошибка : action = {action}");
                }
                else
                    End_Operation_Service("перезапущена");

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }
        public virtual void End_Operation_Service(string end_action)
        {
            string path = @"C:\Users\Eduard.Karpov\source\repos\Rebut_Service_Pochta\Rebut_Service_Pochta\trans\note.txt";
            FileInfo fileInf = new FileInfo(path);
            if (fileInf.Exists == false)
            {
                progressBar1.Value = 100;
                textBox1.Text = $"Служба {end_action}";
                MessageBox.Show($"Служба {end_action}");
                progressBar1.Value = 0;
                textBox2.Clear();
                textBox1.Clear();
                //textBox1.Text = "Служба запущена";
            }
            //progressBar1.Value = 100;
            //MessageBox.Show($"Служба {end_action}");
            //textBox1.Text = $"Служба {end_action}";
        }
        private void button7_Click(object sender, EventArgs e)
        {
            string p = textBox2.Text;
            Process process = Process.Start(new ProcessStartInfo
            {
                FileName = "powershell",
                Arguments = "/command get-service -DisplayName \"RussianPostEASupdate\" -ComputerName " + p + " | format-table Status -autosize",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true
            });
            textBox1.Text = process.StandardOutput.ReadToEnd().Substring(18);
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }



















        //private void button16_Click(object sender, EventArgs e)
        //{
        //    string p = textBox2.Text;
        //    Process process = Process.Start(new ProcessStartInfo
        //    {
        //        FileName = "powershell",
        //        Arguments = "/command get-service -DisplayName \"RussianPostEASupdate\" -ComputerName " + p + " | format-table Status -autosize",
        //        UseShellExecute = false,
        //        CreateNoWindow = true,
        //        RedirectStandardOutput = true
        //    });
        //    textBox1.Text = process.StandardOutput.ReadToEnd().Substring(18);
        //    //ServiceController ser = new ServiceController("RussianPostEASupdate");

        //    //textBox1.Text = ser.Status.ToString();
        //}
    }
}
