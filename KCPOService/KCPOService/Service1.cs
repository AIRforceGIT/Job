using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using BufferingLib;
using System.Runtime.Serialization.Formatters.Binary;

namespace KCPOService
{
   
    public partial class KCPOService : ServiceBase
    {
        Logger logger;
        public KCPOService()
        {
            InitializeComponent();
            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this.AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
            logger = new Logger();
            Thread loggerThread = new Thread(new ThreadStart(logger.Start));
            loggerThread.Start();
        }

        protected override void OnStop()
        {
            logger.Stop();
            Thread.Sleep(2000);
        }
    }
    class Logger
    {
        private static string[] MassGenerator()
        {
            string Value1 = ".1.3.6.1.4.1.44345.0.0.3.1.1";
            string Value2 = ".1.3.6.1.4.1.44345.0.0.3.1.2";
            string Value3 = ".1.3.6.1.4.1.44345.0.0.3.2.1";
            string Value4 = ".1.3.6.1.4.1.44345.0.0.3.2.2";
            string Value5 = ".1.3.6.1.4.1.44345.0.0.3.2.3";
            string Value6 = ".1.3.6.1.4.1.44345.0.0.3.2.4";
            string Value7 = ".1.3.6.1.4.1.44345.0.0.3.2.5";
            string Value8 = ".1.3.6.1.4.1.44345.0.0.3.3.1";
            string Value9 = ".1.3.6.1.4.1.44345.0.0.3.3.2";
            string Value10 = ".1.3.6.1.4.1.44345.0.0.3.3.3";
            string Value11 = ".1.3.6.1.4.1.44345.0.0.3.3.4";
            string Value12 = ".1.3.6.1.4.1.44345.0.0.3.3.5";
            string Value13 = ".1.3.6.1.4.1.44345.0.0.3.4.1";
            string Value14 = ".1.3.6.1.4.1.44345.0.0.3.4.2";
            string Value15 = ".1.3.6.1.4.1.44345.0.0.3.4.3";
            string Value16 = ".1.3.6.1.4.1.44345.0.0.3.4.4";
            string Value17 = ".1.3.6.1.4.1.44345.0.0.3.4.5";
            string Value18 = ".1.3.6.1.4.1.44345.0.0.3.5.1";
            string[] mass = new string[18] { Value1, Value2, Value3, Value4, Value5, Value6, Value7, Value8, Value9, Value10, Value11, Value12, Value13, Value14, Value15, Value16, Value17, Value18 };
            return mass;
        }
        private static ReportMessage EstablishingConnection()
        {

            //Инициализация объекта для подключения к хосту SNMP
            Discovery discovery = Messenger.GetNextDiscovery(SnmpType.GetRequestPdu);

            ReportMessage report = null;

            //Бинд подключения
            try
            {
                report = discovery.GetResponse(10000, new IPEndPoint(IPAddress.Parse("127.0.0.1"), 161));
            }
            catch (Exception error)
            {
                using (EventLog myLogger = new EventLog("KCPOEventLog", ".", "KCPOEventLog"))
                {
                    myLogger.WriteEntry(error.Message + "\nСлужба SNMP не запущена.", EventLogEntryType.Error);
                }               
            }

            return report;
        }

        protected static ISnmpMessage OIDRecieve(ReportMessage rep, string inp)
        {

            //Хранение пароля в чистом виде - уязвимость. Придумать решение и исправить
            var auth = new SHA1AuthenticationProvider(new OctetString(/*password*/));
            var priv = new DefaultPrivacyProvider(auth);
            ISnmpMessage reply = null;
            GetRequestMessage request = null;

            //Опрос хоста оп OID'ам
            try
            {
                //Ответ от хоста
                request = new GetRequestMessage(VersionCode.V3, Messenger.NextMessageId, Messenger.NextRequestId, new OctetString(/*login*/), new List<Variable> { new Variable(new ObjectIdentifier(inp)) }, priv, Messenger.MaxMessageSize, rep);
            }
            catch (Exception error)
            {
                using (EventLog myLogger = new EventLog("KCPOEventLog", ".", "KCPOEventLog"))
                {
                    myLogger.WriteEntry(error.Message + "\nНе удалось получить OID.", EventLogEntryType.Error);
                }
            }

            try
            {
                //Ответ от хоста
                reply = request.GetResponse(10000, new IPEndPoint(IPAddress.Parse("127.0.0.1"), 161));
            }
            catch (Exception error)
            {
                using(EventLog myLogger = new EventLog("KCPOEventLog", ".", "KCPOEventLog"))
                {
                    myLogger.WriteEntry(error.Message + "\nТаймаут при ожидании ответа от хоста SNMP.", EventLogEntryType.Error);
                }
            }

            return reply;
        }
        public static ISnmpMessage OIDTransmit(ReportMessage rep, int cmd)
        {
            var auth = new SHA1AuthenticationProvider(new OctetString("Emicon123!"));
            var priv = new DefaultPrivacyProvider(auth);

            SetRequestMessage request = new SetRequestMessage(VersionCode.V3, Messenger.NextMessageId, Messenger.NextRequestId, new OctetString(/*login*/), new List<Variable> { new Variable(new ObjectIdentifier("1.3.6.1.4.1.44345.0.0.3.5.1"), new Integer32(cmd)) }, priv, Messenger.MaxMessageSize, rep); ;
            ISnmpMessage reply = null;

            try
            {
                reply = request.GetResponse(10000, new IPEndPoint(IPAddress.Parse("127.0.0.1"), 161));
            }
            catch (Exception error)
            {
                using (EventLog myLogger = new EventLog("KCPOEventLog", ".", "KCPOEventLog"))
                {
                    myLogger.WriteEntry(error.Message + "\nУровень доступа SNMP не установлен на READ_WRITE.", EventLogEntryType.Error);
                }
            }

            return reply;
        }
        private static string String_Format(ref string formatted_output, string inp)
        {
            //Вывод элементов, с использованием регулярных выражений для удаления специальных символов, появившихся после s.Data.ToBytes();
            //В регулярном выражении исключается удаление символов: {., *, |, :,  , \, (, )}; \w - означает любой символ, кроме букв и цифр.
            int indexC = 0;
            formatted_output = Regex.Replace(inp, "[^\\w\\.\\*\\|\\:\\ \\\\\\(\\)]", "");

            //Необходимо добавить пайп для корректной работы парсера с последним набором значений. см. FileProperties.GetParse()
            formatted_output = formatted_output + "|";

            indexC = formatted_output.IndexOf(@"C:\");

            if (indexC > 0)
                formatted_output = formatted_output.Remove(0, indexC);

            return formatted_output;
        }

        bool enabled = true;


        public void Start()
        {
            byte[] bytes;
            string output;
            string Output_Formatted = null;
            string[] mass = MassGenerator();
            string EtalonVersion = null;
            

            FileProperties info = new FileProperties();

            List<FileProperties> BufferList = new List<FileProperties>();

            List<FileProperties> SystemList = new List<FileProperties>();

            List<FileProperties> ProgramFilesList = new List<FileProperties>();

            List<FileProperties> ProgramFiles86List = new List<FileProperties>();

            List<FileProperties> CorruptedFilesList = new List<FileProperties>();
            bool FilesAreCorrupted = false;

            List<FileProperties> _messageCorruptedFilesList = new List<FileProperties>();
            _messageCorruptedFilesList.Clear();

            List<FileProperties> CorruptedDateFilesList = new List<FileProperties>();
            bool DateFilesAreCorrupted = false;

            List<FileProperties> _messageCorruptedDateFilesList = new List<FileProperties>();

            using (EventLog myLogger = new EventLog("KCPOEventLog", ".", "KCPOEventLog"))
            {
                myLogger.WriteEntry("Service has been started", EventLogEntryType.Warning);
            }
            while (enabled)
            {
                Program.report = EstablishingConnection();

                //Общение по СНМП и обработка данных в оидах(17 итераций = 17 оидов(не все оиды здесь))
                for (int i = 0; i < 18; i++)
                {
                    Program.reply = OIDRecieve(Program.report, mass[i]);

                    if (Program.reply != null)
                    {
                        foreach (var oid in Program.reply.Pdu().Variables)
                        {
                            //не содержит символов !латиница, не требует перекодировки
                            if (oid.Id.ToString() == "1.3.6.1.4.1.44345.0.0.3.2.3")
                            {
                                output = oid.Data.ToString();
                            }
                            else
                            {
                                //Изменение кодировки для вывода кириллицы
                                bytes = oid.Data.ToBytes();
                                output = Encoding.UTF8.GetString(bytes);
                            }
                            //Форматирование строки
                            String_Format(ref Output_Formatted, output);


                            //Получение версии эталона
                            if (oid.Id.ToString() == "1.3.6.1.4.1.44345.0.0.3.1.1")
                            {
                                Output_Formatted = Output_Formatted.Replace("|", "");
                                EtalonVersion = Output_Formatted;

                            }

                            //Получение даты создания эталона
                            if (oid.Id.ToString() == "1.3.6.1.4.1.44345.0.0.3.1.2")
                            {

                                if (Output_Formatted != "|")
                                {
                                    Output_Formatted = Output_Formatted.Replace("|", "");
                                    Program.lastEtalonTime = DateTime.Parse(Output_Formatted);
                                }
                                else
                                {
                                    using (EventLog myLogger = new EventLog("KCPOEventLog", ".", "KCPOEventLog"))
                                    {
                                        myLogger.WriteEntry("Отсутсвует дата создания эталона! Убедитесь, что файлы добавлены в конфигурацию Alpha.Hashcheck и создан эталон.", EventLogEntryType.Error);
                                    }
                                }

                            }

                            //Заведение Имен отслеживаемых файлов в лист
                            if (oid.Id.ToString() == "1.3.6.1.4.1.44345.0.0.3.2.2")
                            {
                                using (StreamWriter sw = new StreamWriter(/*path*/, false, System.Text.Encoding.UTF8))
                                {
                                    sw.WriteLine(Output_Formatted);
                                }
                                if (Output_Formatted != "|")
                                    while (Output_Formatted != "")
                                    {
                                        info.GetParse(ref Output_Formatted);
                                        BufferList.Add(new FileProperties() { Name = info.Name, Hash_Standard = info.Hash_Standard });
                                    }
                                else
                                {
                                    using (EventLog myLogger = new EventLog("KCPOEventLog", ".", "KCPOEventLog"))
                                    {
                                        myLogger.WriteEntry("Не найдены отслеживаемые файлы!\nПроверьте, что файлы добавлены в конфигурацию Alpha.Hashcheck и создан эталон.", EventLogEntryType.Error);
                                    }
                                }

                                //Сортировка элементов по имени
                                BufferList.Sort();

                                //Добавление заголовка
                                BufferList.Insert(0, new FileProperties() { Name = @"C:\" });
                                BufferList.Insert(1, new FileProperties() { Name = "      Прикладное ПО" });
                            }

                            //Заполнение листа подробной информацией по отслеживаемым файлам(без имени)
                            if (oid.Id.ToString() == "1.3.6.1.4.1.44345.0.0.3.2.3")
                            {
                                if (Output_Formatted != "|")
                                    while (Output_Formatted != "")
                                    {
                                        info.GetListedParse(ref Output_Formatted, ref BufferList);

                                    }

                                //Проверка на наличие измененной даты последнего изменения
                                foreach (var item in BufferList)
                                {
                                    if ((item.Hash_Standard == item.Hash_Fact) & (item.DateOfModification_Standard != item.DateOfModification_Fact))
                                    {
                                        DateFilesAreCorrupted = true;
                                        CorruptedDateFilesList.Add(item);

                                    }                                   
                                }
                                
                            }

                            //Проверка на наличие измененных файлов
                            if ((oid.Id.ToString() == "1.3.6.1.4.1.44345.0.0.3.2.4") & (oid.Data.ToString() != "0"))
                                FilesAreCorrupted = true;
                            //Здесь добавится выброс сообщения после оборота в службу

                            //Обработка измененных файлов
                            if ((oid.Id.ToString() == "1.3.6.1.4.1.44345.0.0.3.2.5") & FilesAreCorrupted)
                            {
                                while (Output_Formatted != "")
                                {
                                    info.GetParse(ref Output_Formatted);
                                    CorruptedFilesList.Add(new FileProperties() { Name = info.Name, Hash_Standard = info.Hash_Standard });
                                }
                                
                            }
                        }
                    }
                    else
                    {
                        //Если пришел нулевой reply - выход 
                        using (EventLog myLogger = new EventLog("KCPOEventLog", ".", "KCPOEventLog"))
                        {
                            myLogger.WriteEntry("Пустой ответ от сервера SNMP.", EventLogEntryType.Error);
                        }
                        break;

                    }
                }
                foreach (var msg in CorruptedFilesList)
                {
                    if (!_messageCorruptedFilesList.Exists(e => e.Name == msg.Name))
                    {
                     
                        _messageCorruptedFilesList.Add(new FileProperties() { Name = msg.Name, Hash_Standard = msg.Hash_Standard });

                        using (EventLog myLogger = new EventLog("KCPOEventLog", ".", "KCPOEventLog"))
                        {
                            myLogger.WriteEntry("ИБ. Файл [" + msg.Name + "] - содержимое перезаписано, изменена контрольная сумма.", EventLogEntryType.Warning);
                        }

                    }
                }
                foreach (var msg in CorruptedDateFilesList)
                {
                    if (!_messageCorruptedDateFilesList.Exists(e => e.Name == msg.Name))
                    {

                        _messageCorruptedDateFilesList.Add(new FileProperties() { Name = msg.Name, Hash_Standard = msg.Hash_Standard });

                        using (EventLog myLogger = new EventLog("KCPOEventLog", ".", "KCPOEventLog"))
                        {
                            myLogger.WriteEntry("ИБ. Файл [" + msg.Name + "] - содержимое перезаписано, изменена контрольная сумма.", EventLogEntryType.Warning);
                        }

                    }
                }
                //Группировка Системного ПО
                foreach (var item in BufferList.ToArray())
                {
                    if (item.Name.Contains(@"C:\Windows"))
                    {
                        SystemList.Add(item);
                        BufferList.Remove(item);
                    }

                }
                SystemList.Sort();

                //Группировка ProgramFiles
                foreach (var item in BufferList.ToArray())
                {
                    if ((item.Name.Contains(@"C:\Program Files")) && !(item.Name.Contains(@"C:\Program Files x86")))
                    {

                        ProgramFilesList.Add(item);
                        BufferList.Remove(item);

                    }
                }
                ProgramFilesList.Sort();
                //Группировка ProgramFiles x86

                foreach (var item in BufferList.ToArray())
                {
                    if (item.Name.Contains(@"C:\Program Files x86"))
                    {
                        ProgramFiles86List.Add(item);
                        BufferList.Remove(item);
                    }
                }
                ProgramFiles86List.Sort();

                //Склейка коллекций Program Files
                BufferList.AddRange(ProgramFilesList);

                //Склейка коллекций Program Files
                BufferList.AddRange(ProgramFiles86List);

                //Добавление заголовка Системного ПО в конец BufferList
                BufferList.Add(new FileProperties() { Name = "      Системное ПО" });
                BufferList.AddRange(SystemList);

                //Запись в лог
                using (EventLog myLogger = new EventLog("KCPOEventLog", ".", "KCPOEventLog"))
                {
                    // myLogger.WriteEntry("Error message", EventLogEntryType.Error);
                    myLogger.WriteEntry("Starting BufferList writing", EventLogEntryType.Information);
                }

                //Запись коллекций в файлы
                try
                {
                    //Запись коллекции отслеживаемых файлов
                    Stream stream = File.Open(/*BufferPath*/\BufferList.osl", FileMode.Create);
                    BinaryFormatter bFormatter = new BinaryFormatter();
                    bFormatter.Serialize(stream, BufferList);
                    stream.Close();
                   

                    //Запись коллекции измененных хэшей
                    stream = File.Open(/*BufferPath*/\CorruptedFilesList.osl", FileMode.Create);
                    bFormatter = new BinaryFormatter();
                    bFormatter.Serialize(stream, CorruptedFilesList);
                    stream.Close();

                    //Запись коллекции измененных дат
                    stream = File.Open(/*BufferPath*/\CorruptedDateFilesList.osl", FileMode.Create);
                    bFormatter = new BinaryFormatter();
                    bFormatter.Serialize(stream, CorruptedDateFilesList);
                    stream.Close();

                    //Отправка даты создания последнего эталона и его версии
                    stream = File.Open(/*BufferPath*/\LastEtalonTime.osl", FileMode.Create);
                    bFormatter = new BinaryFormatter();
                    bFormatter.Serialize(stream, Program.lastEtalonTime);
                    stream.Close();
                    stream = File.Open(/*BufferPath*/\LastEtalonVersion.osl", FileMode.Create);
                    bFormatter = new BinaryFormatter();
                    bFormatter.Serialize(stream, EtalonVersion);
                    stream.Close();

                }
                catch (Exception expt)
                {
                    using (EventLog myLogger = new EventLog("KCPOEventLog", ".", "KCPOEventLog"))
                    {
                        myLogger.WriteEntry(expt.Message, EventLogEntryType.Error);
                    }

                }
                using (EventLog myLogger = new EventLog("KCPOEventLog", ".", "KCPOEventLog"))
                {
                    myLogger.WriteEntry("Service have worked fine, data written", EventLogEntryType.Information);
                }
                //Исправление увеличения интервала работы службы.(возможно перенется в начало  функции при добавлении кода отправки объекта)
                BufferList.Clear();
                ProgramFiles86List.Clear();
                ProgramFilesList.Clear();
                SystemList.Clear();
                CorruptedDateFilesList.Clear();
                CorruptedFilesList.Clear();

                Thread.Sleep(5000);
            }
        }    

        public void Stop()
        {
            enabled = false;
            using (EventLog myLogger = new EventLog("KCPOEventLog", ".", "KCPOEventLog"))
            {
                myLogger.WriteEntry("Service have been stopped", EventLogEntryType.Warning);
            }
        }
    }
}
