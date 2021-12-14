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
            string ip = "";
            InitializeComponent();
            BaseConstructor(ip);
        }
        public Form1(string ip)
        {
            InitializeComponent();
            BaseConstructor(ip);
        }
        public void BaseConstructor(string ip)
        {
            AutoCompleteStringCollection source = new AutoCompleteStringCollection()
        {
            "10.94.73.30",
            "10.94.",
            "10.94.1.141",
            "10.94.74.94"
        };
            TextBox[] Mass_TextBox_ip_auto = new TextBox[10] { textBox3, textBox5, textBox7, textBox9, textBox11,
                textBox13, textBox19, textBox15, textBox20, textBox17 };
            if(ip != "")
            {
                for (int i = 0; i < Mass_TextBox_ip_auto.Length; i++)
                {
                    Mass_TextBox_ip_auto[i].Text = ip;
                }
            }
            foreach (TextBox textBox in Mass_TextBox_ip_auto)
            {
                textBox.AutoCompleteCustomSource = source;
                textBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            }
            string[] mass_name_service = new string[] { "RussianPostEASconfiguration",
                        "RussianPostEASnsi", "RussianPostEASsdo", "RussianPostEAStrans", "RussianPostEASupdate", "RussianPostEASuser" };
            TextBox[] Mass_TextBox_ip = new TextBox[6] { textBox5, textBox7, textBox9, textBox11, textBox3, textBox13 };
            TextBox[] Mass_TextBox_result = new TextBox[6] { textBox6, textBox8, textBox10, textBox12, textBox4, textBox14 };
            Button[] Mass_Button_Status_Service = new Button[6] { button9, button13, button17, button21, button8, button25 };
            Button[] Mass_Button_Stopped_Service = new Button[6] { button12, button16, button20, button24, button6, button28 };
            Button[] Mass_Button_Running_Service = new Button[6] { button11, button15, button19, button23, button5, button27 };
            Button[] Mass_Button_Rebut_Service = new Button[6] { button10, button14, button18, button22, button4, button26 };
            Button[] Mass_Button = new Button[Mass_Button_Status_Service.Length + Mass_Button_Stopped_Service.Length
                + Mass_Button_Running_Service.Length + Mass_Button_Rebut_Service.Length];
            for (int i = 0; i < Mass_Button.Length; i++)
            {
                if (i > -1 & i < 6)
                    Mass_Button[i] = Mass_Button_Status_Service[i];
                if (i > 5 & i < 12)
                    Mass_Button[i] = Mass_Button_Stopped_Service[i - 6];
                if (i > 11 & i < 18)
                    Mass_Button[i] = Mass_Button_Running_Service[i - 12];
                if (i > 17 & i < 24)
                    Mass_Button[i] = Mass_Button_Rebut_Service[i - 18];
            }
            ProgressBar[] Progress_Bars = new ProgressBar[6] { progressBar3, progressBar4, progressBar5, progressBar6, progressBar2, progressBar7 };
            string result = "";
            string[] action_service = new string[2] { "Stopped", "Running" };
            try
            {
                foreach (Button button in Mass_Button)
                {
                    CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                             // отменяет отслеживание ошибок,
                                                             // но дает передать компоненты формы в другой поток 
                    button.Click += async (s, e) =>
                    {
                        for (int i = 0; i < Mass_Button_Status_Service.Length; i++)
                        {
                            if (button == Mass_Button_Status_Service[i])
                            {
                                if (Mass_TextBox_ip[i].Text != "")
                                {
                                    await Task.Run(() => Power_Shell_1($"get-service -DisplayName \"{mass_name_service[i]}\"" +
                                        $" -ComputerName " + Mass_TextBox_ip[i].Text + " | format-table Status -autosize", out result));
                                    Mass_TextBox_result[i].Text = result;
                                }
                                else
                                    MessageBox.Show("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");
                            }
                            if (button == Mass_Button_Stopped_Service[i])
                            {
                                if (Mass_TextBox_ip[i].Text != "")
                                {
                                    Progress_Bars[i].Value = 40;
                                    await Task.Run(() => Async_Power_Shell_Service(Mass_TextBox_ip[i].Text, mass_name_service[i], action_service[0],
                                    Progress_Bars[i], Mass_TextBox_result[i]));
                                }
                                else
                                    MessageBox.Show("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");
                            }
                            if (button == Mass_Button_Running_Service[i])
                            {
                                if (Mass_TextBox_ip[i].Text != "")
                                {
                                    Progress_Bars[i].Value = 40;
                                    await Task.Run(() => Async_Power_Shell_Service(Mass_TextBox_ip[i].Text, mass_name_service[i], action_service[1],
                                    Progress_Bars[i], Mass_TextBox_result[i]));
                                }
                                else
                                    MessageBox.Show("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");
                            }
                            if (button == Mass_Button_Rebut_Service[i])
                            {
                                if (Mass_TextBox_ip[i].Text != "")
                                {
                                    Progress_Bars[i].Value = 20;
                                    await Task.Run(() => Async_Power_Shell_Service_Rebut(Mass_TextBox_ip[i].Text, mass_name_service[i],
                                        Progress_Bars[i], Mass_TextBox_result[i]));

                                }
                                else
                                    MessageBox.Show("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");
                            }
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка:\n{ex}");
            }
        }

        //Метод для работы со службами - запуск или остановка
        public void Async_Power_Shell_Service(string ip, string name_service, string action_service, ProgressBar progressBar, TextBox textBox)
        {
            try
            {
                string check_action;
                Powershell_service(ip, action_service, name_service, "", out check_action);
                Thread.Sleep(500);
                if (check_action == $"\r\n{action_service}\r\n\r\n\r\n")
                {
                    progressBar.Value = 100;
                    MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - {Action_service_info(action_service)}");
                    progressBar.Value = 0;
                    textBox.Text = check_action.Replace("\r\n", "");
                }
                if (check_action == "Блок еlse")
                {
                    progressBar.Value = 100;
                    MessageBox.Show($"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - {Action_service_info(action_service)}");
                    progressBar.Value = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка:\n{ex}");
            }
        }

        //Метод для вывода состояния службы в MessageBox, исходя из значения переменной action_service
        public string Action_service_info(string action_service)
        {
            if (action_service == "Stopped")
                return "Остановлена";
            if (action_service == "Running")
                return "Запущена";
            else
                return $"\nОшибка: action_service = {action_service}\nМетод Async_Power_Shell_Service далее Action_service_info";
        }

        //Метод, представляющий запуск Power_shell, в который через параметры можно ввести команду и она отработает в Power_shell
        //и вернет значение выполнения, через выходной параметр out string result,
        //обрезанное на 18 символов, адаптированое под логику работы моей программы
        public void Power_Shell_1(string command_power_shell, out string result)
        {
            result = "";
            try
            {
                Process process = Process.Start(new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = $"/command {command_power_shell}",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true
                });
                result = process.StandardOutput.ReadToEnd().Substring(18);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка из метода Power_Shell_1 \nПк выключен или нет интернета\n\n\nОписание:\n\n{ex}");
            }
        }

        //Метод представляющий процесс, запускающий power_shell с командами,
        //редактируемыми по параметрам метода и служащими для управления службами на удаленных пк, в сетке, где находится сама программа
        //Удаленные пк задаются в виде их ip (айпи адреса), в самом Power_shell, в его команде, после объявления командлета "-ComputerName" 
        public void Powershell_service(string ip, string action, string name_service, string action_more, out string p)
        {
            p = "";
            try
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
                string k = h.Substring(18);
                Thread.Sleep(500);
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
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex}");
            }
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
            if(textBox19.Text == "")
            {
                MessageBox.Show($"\nПоле для ввода общего ip\nдля управления службыми МПК - пустое,\nвведите ip отделения почтовой связи\n");
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
            if (textBox19.Text == "")
            {
                MessageBox.Show($"\nПоле для ввода общего ip\nдля управления службыми МПК - пустое,\nвведите ip отделения почтовой связи\n");
            }
        }


        // Перезапуск всех служб МПК
        private void button38_Click(object sender, EventArgs e)
        {
            if (textBox19.Text != "")
            {
                CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                         // отменяет отслеживание ошибок,
                                                         // но дает передать компоненты формы в другой поток 

                Thread thread = new Thread(
                    () =>
                    {
                    Rebut_MPK_Service(progressBar10);
                });
                thread.Start();
            }
            else
                MessageBox.Show("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");
        }

        //Метод служащий для перезапуска сразу всех служб МПК, работает через цикл,
        //вызывающий внутри себя еще метод Powershell_service, работа которого была описана выше
        public void Rebut_MPK_Service(ProgressBar progressBar)
        {
            if (textBox19.Text != "")
            {
                try
                {
                    string ip = textBox19.Text;
                    progressBar.Value = 10;
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
                                mass_result[i] = mass_name_service[i] + " - " + "Перезапущена ,Status - Running\n";
                            else
                                mass_result[i] = mass_name_service[i] + " - " + "Не Перезапущена\n";
                        }
                        else
                            mass_result[i] = mass_name_service[i] + " - " + "Не остановлена\n";
                    }
                    progressBar.Value = 100;
                    string result = "";
                    foreach (string i in mass_result)
                        result += i;
                    MessageBox.Show($"\nCлужбы на компьютере {ip}: \n\n{ result}");
                    progressBar.Value = 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: \n{ex}");
                }
            }
            else
                MessageBox.Show("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");
        }

        // Метод для получения состояния конкретной службы , на данный момент используется другой
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
        async private void button42_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox19.Text != "")
                {
                    CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                             // отменяет отслеживание ошибок,
                                                             // но дает передать компоненты формы в другой поток 
                    listBox1.Items.Clear();
                    await Task.Run(() => Status_all_Service());
                    listBox1.Items.AddRange(Status_all_Service());
                }
                if (textBox19.Text == "")
                {
                    MessageBox.Show($"\nПоле для ввода общего ip,\nдля управления службыми МПК - пустое,\nвведите ip отделения почтовой связи\n");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка:\n {ex}");
            }
        }

        //Метод(функция, т к имеет возвращаемый тип string[]), служащая для получение строкового массива с данными,
        //описывающими состояние всех служб, адаптированый под логику программы, 
        //выводит массив имен служб "-" их состояние
        public string[] Status_all_Service()
        {
            try
            {
                string p = textBox19.Text;
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
            catch(Exception ex)
            {            
                MessageBox.Show($"Ошибка:\n {ex}");
                return new string[5];
            }
        }

        //Метод предназначеный для перезапуска конкретных служб
        public void Async_Power_Shell_Service_Rebut(string ip, string name_service, ProgressBar progressBar, TextBox textBox)
        {
            try
            {
                string check_action;
                progressBar.Value = 40;
                Powershell_service(ip, "Stopped", name_service, "", out check_action);
                if (check_action == "\r\nStopped\r\n\r\n\r\n")
                {
                    progressBar.Value = 60;
                    Powershell_service(ip, "Running", name_service, "", out check_action);
                    progressBar.Value = 100;
                    MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - Перезапущена\n");
                    progressBar.Value = 0;
                    textBox.Text = check_action.Replace("\r\n", "");
                }
                if (check_action == "Блок еlse")
                {
                    MessageBox.Show($"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - Остановлена");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка:\n {ex}");
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
                    Stop_Service_All(progressBar11, textBox19);
                });
                thread.Start();
            }
            else
                MessageBox.Show("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");
        }

        //Метод предназначенный для остановки всех служб МПК
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

        //Запуск всех служб
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
                        Start_ALL_Service(progressBar12, textBox19);
                    });
                thread.Start();
            }      
            else
                MessageBox.Show("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");
        }

        //Метод предназначенный для запуска всех служб МПК
        public void Start_ALL_Service(ProgressBar progress, TextBox text)
        {
            try
            {
                string[] mass_name_service = new string[]{ "RussianPostEASconfiguration",
                        "RussianPostEASnsi", "RussianPostEASsdo", "RussianPostEAStrans", "RussianPostEASupdate", "RussianPostEASuser" };
                string[] mass_result = new string[mass_name_service.Length];
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

        // Службы GMMQ и Sheduller написаны в виде async private void button32_Click(object sender, EventArgs e)
        //автосгенерированных методов , но добавлен  модификатор async и они стали асинхронными, т е нажимая на кнопку,
        //запускается отдельно метод через ключевое слово await и далее классTask.методRun(() =>
        //Task.Run(() => - метод запускается в отдельном потоке от основой программы,
        //Это значит, что все написанные тут кнопки, через async, могут нажиматься сразу вподряд,
        //и не ждать выполнения оперделенной операции в одной из кнопок, как если бы это было синхронное обыное выполнение программы

        // Остановка служб GMMQ и Sheduller 
        async private void button32_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                     // отменяет отслеживание ошибок,
                                                     // но дает передать компоненты формы в другой поток 
            try
            {
                if (textBox15.Text != "")
                {
                    string result = "";
                    string name_service = "GMMQ";
                    string ip = textBox15.Text;
                    progressBar8.Value = 40;
                    string action_service = "Stopped";
                    await Task.Run(() => Powershell_service_Force(ip, action_service, name_service, ""));

                    //Служба GMMQ останавливается с помощью другой команды т к имеет зависимые службы,
                    //поэтому для нее особы метод создан

                    await Task.Run(() => Power_Shell_1("get-service -" +
                                       "DisplayName \"GMMQ\"" +
                                       " -ComputerName " + textBox15.Text + "" +
                                       " | format-table Status -autosize", out result));

                    //запрос на состояние службы и потом проверка,
                    //работа с службой GMMQ специально разделена на 2 метода
                    //первый метод Powershell_service_Force - не возвращает после отработки состояние службы, просто ее останавливает
                    //второй метод Power_Shell_1 - получает просто состояние службы в переменную, записанную в выходные параметры метода
                    //далее логика работы программы, в зависимости от полученного значения состояния, из метода Power_Shell_1

                    textBox16.Text = result.Replace("\r\n", "");
                    if (result == $"\r\n{action_service}\r\n\r\n\r\n")
                    {
                        progressBar8.Value = 100;
                        MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - {Action_service_info(action_service)}");
                        progressBar8.Value = 0;
                        textBox16.Text = result.Replace("\r\n", "");
                    }
                    if (result == "Блок еlse")
                    {
                        MessageBox.Show($"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - {Action_service_info(action_service)}");
                    }
                }
                else
                    MessageBox.Show("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }

        async private void button36_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                     // отменяет отслеживание ошибок,
                                                     // но дает передать компоненты формы в другой поток 
            try
            {
                if (textBox17.Text != "")
                {
                    string name_service = "GM_SchedulerSvc";
                    string ip = textBox17.Text;
                    progressBar9.Value = 40;
                    string action_service = "Stopped";
                    string result = "";
                    await Task.Run(() => Power_Shell("Get-Service -Computer " + ip + " -Name " + name_service + " | Stop-Service -Force"));
                    await Task.Run(() => Power_Shell_1("get-service -" +
                                      "DisplayName \"GM_SchedulerSvc\"" +
                                      " -ComputerName " + textBox17.Text + "" +
                                      " | format-table Status -autosize", out result));
                    textBox18.Text = result.Replace("\r\n", "");
                    if (result == $"\r\n{action_service}\r\n\r\n\r\n")
                    {
                        progressBar9.Value = 100;
                        MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - {Action_service_info(action_service)}");
                        progressBar9.Value = 0;
                        textBox18.Text = result.Replace("\r\n", "");
                    }
                    if (result == "Блок еlse")
                    {
                        MessageBox.Show($"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - {Action_service_info(action_service)}");
                    }

                }
                else
                    MessageBox.Show("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }

        //Метод представляющий процесс, запускающий power_shell с командами, передаваемыми через параметр string action_power_shell
        //Метод ожидает своей отработки, т е пока поток не отработает, основная программа не будет выполнять код, ниже которого вызван этот метод
        public void Power_Shell(string action_power_shell)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "powershell",
                Arguments = $"/command {action_power_shell}",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true
            }).WaitForExit();
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

        // Запуск "GMMQ" и "GM_SchedulerSvc"
        async private void button31_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                     // отменяет отслеживание ошибок,
                                                     // но дает передать компоненты формы в другой поток 
            try
            {
                if(textBox15.Text != "")
                { 
                string name_service = "GMMQ";
                string action_service = "Running";
                string ip = textBox15.Text;
                progressBar8.Value = 40;
                await Task.Run(() => Async_Power_Shell_Service(ip, name_service, action_service, progressBar8, textBox16));
                }
                else
                    MessageBox.Show("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }

        async private void button35_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                     // отменяет отслеживание ошибок,
                                                     // но дает передать компоненты формы в другой поток 
            try
            {
                if (textBox15.Text != "")
                {
                    string result = "";
                    string name_service = "GM_SchedulerSvc";
                    string action_service = "Running";
                    string ip = textBox17.Text;
                    progressBar9.Value = 40;
                    await Task.Run(() => Power_Shell($"set-service {name_service} -ComputerName {ip} -Status {action_service} -PassThru | format-table Status -autosize"));
                    await Task.Run(() => Power_Shell_1("get-service -" +
                                      "DisplayName \"GM_SchedulerSvc\"" +
                                      " -ComputerName " + textBox17.Text + "" +
                                      " | format-table Status -autosize", out result));
                    textBox18.Text = result.Replace("\r\n", "");
                    if (result == $"\r\n{action_service}\r\n\r\n\r\n")
                    {
                        progressBar9.Value = 100;
                        MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - {Action_service_info(action_service)}");
                        progressBar9.Value = 0;
                        textBox18.Text = result.Replace("\r\n", "");
                    }
                    else if (result == "Блок еlse")
                        MessageBox.Show($"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - {Action_service_info(action_service)}");
                }
                else
                    MessageBox.Show("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }

        // Состояние служб
        async private void button29_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                     // отменяет отслеживание ошибок,
                                                     // но дает передать компоненты формы в другой поток 
            string p = textBox15.Text;
            if (textBox15.Text != "")
            {
                string result = "";
                await Task.Run(() => Power_Shell_1("get-service -" +
                                   "DisplayName \"GMMQ\"" +
                                   " -ComputerName " + textBox15.Text + "" +
                                   " | format-table Status -autosize", out result));
                textBox16.Text = result;
            }
            else
                MessageBox.Show("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");
        }

        async private void button33_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                     // отменяет отслеживание ошибок,
                                                     // но дает передать компоненты формы в другой поток 
            string p = textBox17.Text;
            if (textBox17.Text != "")
            {
                string result = "";
                await Task.Run(() => Power_Shell_1("get-service -" +
                                   "DisplayName \"GM_SchedulerSvc\"" +
                                   " -ComputerName " + textBox17.Text + "" +
                                   " | format-table Status -autosize", out result));
                textBox18.Text = result;
            }
            else
                MessageBox.Show("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");
        }

        // Перезапуск 
        async private void button30_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                     // отменяет отслеживание ошибок,
                                                     // но дает передать компоненты формы в другой поток 
            try
            {
                if (textBox15.Text != "")
                {
                    string result = "";
                    string name_service = "GMMQ";
                    string ip = textBox15.Text;
                    progressBar8.Value = 40;
                    string action_service_1 = "Stopped";
                    string action_service_2 = "Running";
                    await Task.Run(() => Powershell_service_Force(ip, action_service_1, name_service, ""));
                    progressBar8.Value = 70;
                    await Task.Run(() => Power_Shell_1($"set-service {name_service} " +
                     $"-ComputerName {ip} " +
                     $"-Status {action_service_2} -PassThru " +
                     "| format-table Status -autosize", out result));
                    progressBar8.Value = 90;
                    if (result == $"\r\n{action_service_2}\r\n\r\n\r\n")
                    {
                        progressBar8.Value = 100;
                        MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - Перезапущена");
                        progressBar8.Value = 0;
                        textBox16.Text = result.Replace("\r\n", "");
                    }
                    if (result == "Блок еlse")
                    {
                        MessageBox.Show($"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - Перезапущена");
                    }
                }
                else
                    MessageBox.Show("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }

      async private void button34_Click(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false; // нехороший лайфхак,
                                                     // отменяет отслеживание ошибок,
                                                     // но дает передать компоненты формы в другой поток 
            try
            {
                if(textBox10.Text != "")
                { 
                string result = "";
                string name_service = "GM_SchedulerSvc";
                string ip = textBox17.Text;
                progressBar9.Value = 40;
                string action_service_1 = "Stopped";
                string action_service_2 = "Running";
                await Task.Run(() => Powershell_service_Force(ip, action_service_1, name_service, ""));
                progressBar9.Value = 60;
                await Task.Run(() => Power_Shell($"set-service {name_service} " +
                 $"-ComputerName {ip} " +
                 $"-Status {action_service_2} -PassThru " +
                 "| format-table Status -autosize"));
                progressBar9.Value = 70;
                await Task.Run(() => Power_Shell_1("get-service -" +
                           "DisplayName \"GM_SchedulerSvc\"" +
                           " -ComputerName " + textBox17.Text + "" +
                           " | format-table Status -autosize", out result));

                //используется 3 метода, т к программа не успевает отлавливать состояние службы, т к запуск ее долгий

                textBox18.Text = result.Replace("\r\n", "");
                progressBar9.Value = 90;
                if (result == $"\r\n{action_service_2}\r\n\r\n\r\n")
                {
                    progressBar9.Value = 100;
                    MessageBox.Show($"Служба \"{name_service}\"\nНа компьютере {ip} - Перезапущена");
                    progressBar9.Value = 0;
                    textBox18.Text = result.Replace("\r\n", "");
                }
                if (result == "Блок еlse")
                {
                    MessageBox.Show($"Блок else \nСлужба \"{name_service}\"на компьютере {ip} - Перезапущена");
                }
                }
                else
                    MessageBox.Show("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: \n{ex}");
            }
        }

        //Кнопка "Сохранить" в разделе со службами GMMQ и GM_SchedulerSvc
        private void button43_Click(object sender, EventArgs e)
        {
            if (textBox20.Text != "")
            {
                textBox15.Text = textBox20.Text;
                textBox17.Text = textBox20.Text;
            }
            if(textBox20.Text == "")
                MessageBox.Show("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");
        }

        //Кнопка "Отменить" в разделе со службами GMMQ и GM_SchedulerSvc
        private void button44_Click(object sender, EventArgs e)
        {
            if (textBox15.Text != "" | textBox17.Text != "")
            {
                textBox15.Text = "";
                textBox17.Text = "";
            }
            if (textBox20.Text == "")
                MessageBox.Show("Поле для ввода пустое.\nВведите ip отделения почтовой связи.");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {

        }

        private void ClearFormField()
        {
            TextBox[] Mass_TextBox_result = new TextBox[6] { textBox6, textBox8, textBox10, textBox12, textBox4, textBox14 };
            for (int i = 0; i < Mass_TextBox_result.Length; i++)
            {
                Mass_TextBox_result[i].Clear();
            }       
            listBox1.Items.Clear();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ClearFormField();
        }
    }
}
