using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace BlackListAddSQL
{
    class Program
    {
        private static string[] SerchText = new string[] { "в ч/с добавлен в г.", "фио водителя", "Дата рождения водителя", "Комментарий парка", "ч/с добавлен в г.", "г.", "г.р", "г.р.", "Дата рождения водителя:", "Дата рождения", "в ч/с добавлена в", "ч/с добавлен в", "ч/с добавлен в г", "в ч/с добавлен в г" };
        public static List<CityClass1> restoredPerson = new List<CityClass1>();
        public static PersonClass personClass;// = new PersonClass();
        private static int Start = 0;
        static void Main(string[] args)
        {
            if (Start == 1)
            {
                #region
                //Console.WriteLine("Читаем файл городов");
                using (FileStream fs = new FileStream("e:\\russian-cities.json", FileMode.OpenOrCreate))
                {
                    StreamReader streamReader = new StreamReader(fs);
                    string json = streamReader.ReadToEnd();
                    restoredPerson = JsonConvert.DeserializeObject<List<CityClass1>>(json);

                }
                #endregion
                #region Объявляем все переменные
                string serchStart = "<div class=\"text\">";
                string serchEnd = @"</div>";
                int indexStart = 0;
                int indexEnd = 0;
                List<string> stroki = new List<string>();
                List<PersonClass> personClasses = new List<PersonClass>();
                List<string> bredList = new List<string>();
                #endregion

                string[] failName = new string[] { "e:\\messages", "e:\\messages2",
                "e:\\messages3","e:\\messages4","e:\\messages5","e:\\messages6","e:\\messages7","e:\\messages8","e:\\messages9","e:\\messages10","e:\\messages11",
                "e:\\messages12","e:\\messages13","e:\\messages14","e:\\messages15","e:\\messages16","e:\\messages17","e:\\messages18","e:\\messages19","e:\\messages20" };

                for (int lenfail = 9; lenfail < failName.Length; lenfail++)
                {
                    personClasses = new List<PersonClass>();
                    #region Считываем и перебираем по заданным параметрам HTML файл
                    FileStream file1 = new FileStream(failName[lenfail] + ".html", FileMode.Open); //создаем файловый поток
                    StreamReader reader = new StreamReader(file1); // создаем «потоковый читатель» и связываем его с файловым потоком
                    string html = reader.ReadToEnd();
                    string[] htmlArray = html.Split(' ');
                    /// Читаем файл и выбираем нужное вхождение между стартом и ендом
                    indexStart = 0;
                    for (int i = 0; i < html.Length; i++)
                    {

                        indexStart = html.IndexOf(serchStart, indexStart);//ищем первое вхождение
                        if (indexStart < 0)
                            break;

                        indexEnd = html.IndexOf(serchEnd, indexStart);//Ищем второе вхождение
                        string test = html.Substring((indexStart + serchStart.Length), (indexEnd - indexStart - serchEnd.Length - 12)); //Определяем длину искомой подстроки.
                        stroki.Add(test);
                        indexStart = indexEnd + serchEnd.Length;
                    }
                    reader.Close(); //закрываем поток
                    #endregion

                    #region Перебираем данные и записываем нужные строки в таблицу
                    foreach (var item in stroki)
                    {
                        personClass = new PersonClass();
                        string listStr = "";
                        Match match = Regex.Match(item, @"<[^>]+>");
                        listStr = item;
                        while (Regex.Match(listStr, @"<[^>]+>").Success)
                        {
                            string[] m = new string[] { match.Value };
                            listStr = String.Join(" ", listStr.Split(m, StringSplitOptions.None).Distinct().ToArray());
                            match = Regex.Match(listStr, @"<[^>]+>");
                        }

                        match = Regex.Match(listStr, @"\p{Cs}");
                        while (Regex.Match(listStr, @"\p{Cs}").Success)
                        {
                            string[] m = new string[] { match.Value };
                            listStr = String.Join(" ", listStr.Split(m, StringSplitOptions.None).Distinct().ToArray());
                            match = Regex.Match(listStr, @"\p{Cs}");
                        }



                        string[] tempMasiv = listStr.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

                        string xxx = GetFIOFullInt(tempMasiv[0]);
                        if (int.TryParse(xxx, out int tt))
                        {
                            personClass.ID = Convert.ToInt32(xxx);
                            List<string> rem = tempMasiv.ToList();
                            rem.Remove(tempMasiv[0]);
                            tempMasiv = rem.ToArray();
                            if (personClass.ID == 2592)
                            {

                            }
                        }
                        //var tempv = tempMasiv.Where(x => !string.IsNullOrWhiteSpace(x));
                        //tempMasiv = tempv.ToArray();

                        stroki = GetFIO(tempMasiv);
                        Array.Reverse(stroki.ToArray());
                        listStr = String.Join(" ", stroki);

                        listStr = GetFullCity(listStr);

                        listStr = GetDateTime(listStr);
                        //if (DateTime.TryParse(dat, out DateTime dateTime))
                        //{
                        //    personClass.DateTimes = Convert.ToDateTime(dat);
                        //}
                        //else
                        //{
                        //    personClass.DateTimes = Convert.ToDateTime("01.01.1901");
                        //}



                        //personClass.Phone = GetPhone(listStr);
                        if (personClass.FirstName == "" || personClass.FirstName == null)
                        {
                            bredList.Add(item);

                        }
                        else
                        {
                            listStr = GetNewTempMassiv(listStr, personClass);
                        }

                        tempMasiv = listStr.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

                        for (int i = 0; i < tempMasiv.Length; i++)
                        {
                            string tempStr = GetFIOFullInt(tempMasiv[i]);
                            if (i == 0)
                            {
                                if (int.TryParse(tempStr, out int tt1))
                                {
                                    personClass.ID = Convert.ToInt32(tempStr);
                                }
                                else if (Regex.Match(tempStr, @"\w*").Success)
                                {
                                    personClass.Opisanie += tempStr + " ";
                                }
                            }
                            else
                            {
                                personClass.Opisanie += tempStr + " ";
                            }
                        }
                        if (personClass.FirstName == "" || personClass.FirstName == null)
                        {
                            bredList.Add(item);

                        }
                        else
                        {
                            personClasses.Add(personClass);

                        }
                    }
                    #endregion
                    //Console.WriteLine("**********************Выводим полученный список*********************************************************************");
                    //foreach (var item in personClasses)
                    //{
                    //    Console.WriteLine("{0}***{1}***{2}***{3}***{4}***{5}***{6}", item.ID, item.FirstName, item.LastName, item.SecondName, item.City, item.DateTimes, item.Opisanie);
                    //    Console.WriteLine("____________________________________________________________________________________________________________________");
                    //    if (item.DateTimes.Year == 0001 || item.DateTimes.Year == 1001)
                    //    {
                    //        Console.ReadLine();
                    //    }
                    //    if (item.ID == 2592)
                    //    {
                    //        Console.ReadLine();
                    //    }
                    //}

                    ////Console.WriteLine("Нажмите педаль для записи в базу");
                    //Console.ReadLine();
                    //Console.Clear();
                    Console.WriteLine("*****************_____Пишем в базу_______{0}_____******************************************", failName[lenfail]);




                    Console.CursorVisible = false;
                    #region Записываем полученный результат в базу
                    int procent = personClasses.Count;
                    using (var dbcontext = new MyDbContext())
                    {
                        for (int i = 0; i < personClasses.Count; i++)
                        {
                            Driver driver = new Driver();
                            driver.Avatar = 1;
                            driver.BlackList = true;
                            driver.DriverslicenseID = 1;
                            driver.FirstName = personClasses[i].FirstName;
                            driver.LastName = personClasses[i].LastName;
                            driver.SecondName = personClasses[i].SecondName;
                            driver.PassportID = 1;
                            driver.INN = 0;

                            dbcontext.Drivers.Add(driver);
                            dbcontext.SaveChanges();
                            int id = driver.ID;

                            FeedBack feedBack = new FeedBack();
                            feedBack.DriverID = id;
                            feedBack.TaxiPoolID = 1;
                            feedBack.Subjest = personClasses[i].Opisanie;
                            feedBack.DateADD = personClasses[i].DateTimes;

                            string cit = personClasses[i].City;

                            var users = (from user in dbcontext.GetCityNames
                                         where user.Name == cit
                                         select user).ToList();
                            feedBack.CityID = users[0].ID;

                            dbcontext.feedBacks.Add(feedBack);
                            dbcontext.SaveChanges();

                            if (i < procent / 4)
                                Console.ForegroundColor = ConsoleColor.Red;
                            else if (i < procent / 2)
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                            else if (i < ((procent / 4) + (procent / 2)))
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                            else if (i < procent)
                                Console.ForegroundColor = ConsoleColor.Yellow;
                            else
                                Console.ForegroundColor = ConsoleColor.Green;

                            string pct = string.Format("{0,-30} {1,3}%", new string((char)0x2592, i * (procent / 100) / 100), i);
                            Console.CursorLeft = 0;
                            Console.Write(pct);
                        }
                    }


                    //Console.WriteLine("**********************Выводим полученный бред*********************************************************************");
                    //foreach (var item in bredList)
                    //{
                    //    Console.WriteLine(item);
                    //}
                    //Console.WriteLine("записей в базу не вошло {0}", bredList.Count);

                    //Console.ReadLine();
                    #endregion
                }
                Console.WriteLine("Запись завершена. Благодарю за терпение!");

                Console.ReadLine();
            }


            #region Запись для приставов
            string jsonrez = "";
            using (FileStream fstr = new FileStream("e:\\region.txt", FileMode.OpenOrCreate))
            {
                StreamReader streamReader = new StreamReader(fstr);
                jsonrez = streamReader.ReadToEnd();
            }

            string[] masivfsspcode = jsonrez.Split('\n');
            //for(int i = 0; i < masivfsspcode.Length; i++)
            //{
            //".*?"
            //}
            List<FsspCode> fsspCodes = new List<FsspCode>();
            foreach(string r in masivfsspcode)
            {
                
                if (r != "" && r != " ")
                {
                    Match match = Regex.Match(r, @".*");
                    var fssp = match.ToString().Split(',');
                    fsspCodes.Add(new FsspCode { Regumber = Convert.ToInt32(fssp[0]), Name = fssp[1].Trim('"') });
                }
            }
            using (var dbcontext = new MyDbContext())
            {
                foreach (FsspCode code in fsspCodes)
                {
                    dbcontext.FsspCodes.Add(code);
                    dbcontext.SaveChanges();
                }
            }

            foreach(var re in fsspCodes)
            {
                Console.WriteLine($"{re.ID}*****{re.Name}");
            }

            
            Console.ReadLine();
            #endregion
        }


        /***************************************************************************************************************************************************************/



        static void Print(string message, int delay)
        {
            Thread.Sleep(delay);
            Console.WriteLine(message);
        }
        private static string GetFIOFullInt(string text)
        {
            char[] charsTrim = { ',', '.', ' ', '.', '<', '>', '\'', '"', '\\', '{', '}', '/', '\n', ':', '-' };
            string ret = text.Trim(charsTrim);
            ret = ret.TrimStart('\\');
            return ret;
        }


        private static List<string> GetFIO(string[] fioin)
        {
            List<string> fio = fioin.ToList();
            //for (int i = 0; i < fio.Count; i++)
            //{
            //    fio[i] = GetFIOFullInt(fio[i]);
            //}
            string items = "";
            fio = (from x in fio where !items.Contains(x) select x).ToList();
            for (int i = 0; i < fio.Count; i++)
            {
                if (fio[i] != "")
                {
                    string _fio = GetFIOFullInt(fio[i]);
                    if (char.IsUpper(fio[i][0]))
                    {
                        if (_fio.ToLower().EndsWith("вич") || _fio.ToLower().EndsWith("евна") || _fio.ToLower().EndsWith("евич") || _fio.ToLower().EndsWith("овна") || _fio.ToLower().EndsWith("тич"))
                        {
                            if (fio.Count > 2)
                            {
                                if ((i - 2) < 0)
                                {
                                    _fio = GetFIOFullInt(fio[i + 2]);
                                    if (_fio.ToLower().EndsWith("ович") || _fio.ToLower().EndsWith("евич") || _fio.ToLower().EndsWith("евна"))
                                    {
                                        personClass.FirstName = fio[i];
                                        personClass.LastName = fio[i + 1];
                                        personClass.SecondName = fio[i + 2];
                                        fio = GetListMassivFIO(3, i, fio);
                                    }
                                }
                                else
                                {
                                    personClass.FirstName = fio[i - 2];
                                    personClass.LastName = fio[i - 1];
                                    personClass.SecondName = fio[i];
                                    fio = GetListMassivFIO(3, i-2, fio);
                                }
                            }
                            else
                            {
                                personClass.FirstName = fio[0];
                                personClass.LastName = "No Name";
                                personClass.SecondName = fio[1];
                                fio = GetListMassivFIO(2, 0, fio);
                                
                            }
                        }

                    }
                    if (_fio.ToLower() == "оглы" || _fio.ToLower() == "углы" || _fio.ToLower() == "уулу" || _fio.ToLower() == "угли" || _fio.ToLower() == "огли" || _fio.ToLower() == "улги")
                    {
                        if (fio.Count > 3)
                        {
                            if (i > 0)
                            {
                                if (i <= 2)
                                {
                                    personClass.FirstName = fio[i - 1];
                                    personClass.LastName = fio[i + 1];
                                    personClass.SecondName = fio[i];
                                    if (i + 2 < fio.Count)
                                    {
                                        personClass.SecondName += " " + fio[i + 2];
                                        fio = GetListMassivFIO(4, i - 1, fio);
                                    }
                                    else
                                        fio = GetListMassivFIO(3, i - 1, fio);

                                }
                                else
                                {
                                    personClass.FirstName = fio[i - 3];
                                    personClass.LastName = fio[i - 2];
                                    personClass.SecondName = fio[i - 1] + " " + fio[i];
                                    fio = GetListMassivFIO(4, i - 3, fio);
                                }
                            }
                            else
                            {
                                personClass.FirstName = fio[i +1];
                                personClass.LastName = fio[i + 2];
                                personClass.SecondName = fio[i];
                                fio = GetListMassivFIO(3, i, fio);
                            }
                        }
                        else
                        {
                            if (i < 2)
                            {
                                personClass.FirstName = fio[i - 1];
                                personClass.LastName = fio[i + 1];
                                personClass.SecondName = fio[i];
                                fio = GetListMassivFIO(3, i-1, fio);
                            }
                            else
                            {
                                personClass.FirstName = fio[i - 2];
                                personClass.LastName = fio[i - 1];
                                personClass.SecondName = fio[i];
                                fio = GetListMassivFIO(3, i-2, fio);
                            }
                        }
                    }
                }
            }
            return fio;
        }

        private static List<string> GetListMassivFIO(int start, int delall, List<string> fio)
        {
            for(int i = 0; i < start; i++)
            {
                fio.RemoveAt(delall);
            }
            return fio;
        }

        private static string GetFullCity(string temp)
        {
            for (int i = 0; i < restoredPerson.Count; i++)
            {
                string texts = "\\b" + restoredPerson[i].Name + "\\b";
                if (Regex.IsMatch(temp, texts))
                {
                    personClass.City= restoredPerson[i].Name;
                    int ind = 0;
                    ind = temp.IndexOf(restoredPerson[i].Name, 0);
                    string str = temp.Remove(ind, restoredPerson[i].Name.Length);
                    //return restoredPerson[i].Name;
                    return str;
                }
            }
            personClass.City = "No City";
            return temp;
        }


        private static string GetDateTime(string tempString)
        {
            char[] sp = new char[] { ' ', '.' };
            string ful = "";
            string[] dat;
            Match match = Regex.Match(tempString, @"\d\d[.]\d\d[.]\d\d\d\d");
            Match match1 = Regex.Match(tempString, @"\d\d[.]\d\d[.]\d\d");
            if (match.Success == true)
            {
                ful = match.Value;
                dat = match.Value.Split(sp);
                if (Convert.ToInt32(dat[1]) > 12)
                {
                    ful = dat[1] + "." + dat[0] + "." + dat[2];
                }
                if (Convert.ToInt32(dat[2]) == 0001 || Convert.ToInt32(dat[2]) == 1001 || Convert.ToInt32(dat[2]) == 1 || Convert.ToInt32(dat[2]) < 1900 || Convert.ToInt32(dat[2]) > 2020)
                {
                    personClass.DateTimes = Convert.ToDateTime("01.01.1901");
                    //string str = CleerDate(tempString, ful);
                    return tempString;
                }
                if (DateTime.TryParse(ful, out DateTime dateTime))
                {
                    personClass.DateTimes = Convert.ToDateTime(ful);
                    string str = CleerDate(tempString, ful);
                    return str;
                }
            }
            else if (match1.Success == true)
            {
                ful = match1.Value;
                dat = match1.Value.Split(sp);
                if (dat[2].Length < 3)
                {
                    dat[2] = "19" + dat[2];
                    ful = dat[0] + "." + dat[1] + "." + dat[2];
                }
                if (Convert.ToInt32(dat[1]) > 12)
                {
                    ful = dat[1] + "." + dat[0] + "." + dat[2];
                }
                if (Convert.ToInt32(dat[2]) == 0001 || Convert.ToInt32(dat[2]) == 1001 || Convert.ToInt32(dat[2]) == 1 || Convert.ToInt32(dat[2]) < 1900)
                {
                    personClass.DateTimes = Convert.ToDateTime("01.01.1901");
                    string str = CleerDate(tempString, ful);
                    return str;
                }
                if (DateTime.TryParse(ful, out DateTime dateTime))
                {
                    personClass.DateTimes = Convert.ToDateTime(ful);
                    string str = CleerDate(tempString, match1.Value);
                    return str;
                }
            }
            else
            {
                string tempString1 = Regex.Replace(tempString, "[. ]+[ ]", ".");
                string[] tempString2 = tempString.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                match = Regex.Match(tempString1, @"\d\d[.]\d\d[.]\d\d\d\d");
                if (match.Success)
                {
                    ful = match.Value;
                    dat = ful.Split('.');
                    if (Convert.ToInt32(dat[1]) > 12)
                    {
                        ful = dat[1] + "." + dat[0] + "." + dat[2];
                    }
                    if (Convert.ToInt32(dat[2]) == 0001 || Convert.ToInt32(dat[2]) == 1001 || Convert.ToInt32(dat[2]) == 1 || Convert.ToInt32(dat[2]) < 1900)
                    {
                        personClass.DateTimes = Convert.ToDateTime("01.01.1901");
                        string str = CleerDate(tempString1, ful);
                        return str;
                    }
                    if (DateTime.TryParse(ful, out DateTime dateTime))
                    {
                        personClass.DateTimes = Convert.ToDateTime(ful);
                        string str = CleerDate(tempString1);
                        return str;
                    }
                }
                else
                {
                    match = Regex.Match(tempString1, @"\d\d[.]\d\d[.]\d\d");
                    if (match.Success)
                    {
                        ful = match.Value;
                        if (DateTime.TryParse(ful, out DateTime dateTime))
                        {
                            personClass.DateTimes = Convert.ToDateTime(ful);
                            string str = CleerDate(tempString1);
                            return str;
                        }
                        else
                        {
                            int year = 0, month = 0, days = 0;
                            DateTime dateTime1 = new DateTime();
                            dat = ful.Split('.');
                            if (Convert.ToInt32(dat[0]) > 31)
                            {
                                year= Convert.ToInt32("19" + dat[0]);
                            }
                            if(Convert.ToInt32(dat[1])> Convert.ToInt32(dat[2]) && Convert.ToInt32(dat[1]) >12)
                            {
                                days = Convert.ToInt32(dat[1]);
                                month = Convert.ToInt32(dat[2]);
                            }
                            else if (Convert.ToInt32(dat[2]) > 12)
                            {
                                days = Convert.ToInt32(dat[2]);
                                month = Convert.ToInt32(dat[1]);
                            }
                            ful = days + "." + month + "." + year;
                            match = Regex.Match(tempString, @"\d\d[\W]\d\d[\w]\d\d\d\d");
                            personClass.DateTimes = Convert.ToDateTime(ful);
                            string str = CleerDate(tempString1,dat[0]+ dat[1] + dat[2]);
                            return str;
                        }
                    }
                    match = Regex.Match(tempString, @"\d\d[. ]\d\d[. ]\d\d\d\d");
                }
            }
            personClass.DateTimes = Convert.ToDateTime("01.01.1901");
            return tempString;
        }

        private static string CleerDate(string tempString)
        {
            return CleerDate(tempString, personClass.DateTimes.ToShortDateString());
        }
        private static string CleerDate(string tempString, string deldate)
        {

            int indexS = Regex.Replace(tempString, "[.]+[ ]", ".").IndexOf(deldate, 0);
            if (indexS > -1)
                tempString = Regex.Replace(tempString, "[.]+[ ]", ".").Remove(indexS, deldate.Length);
            tempString = Regex.Replace(tempString, "[.]+", ". ");
            return tempString;
        }
        private static string GetPhone(string tempString)
        {
            //+7-911-767-21-21
            Match match = Regex.Match(tempString, @"[+]\d[-]\d\d\d[-]\d\d\d[-]\d\d[-]\d\d");
            Match match2 = Regex.Match(tempString, @"\d[-]\d\d\d[-]\d\d\d[-]\d\d[-]\d\d");
            if (match.Success)
            {
                return match.Value;

            }
            else if (match2.Success)
            {
                return match2.Value;
            }
            return "";
        }


        private static string GetNewTempMassiv(string tempString, PersonClass personClass)
        {
            int indexS = 0;

            //indexS = tempString.IndexOf(personClass.City, 0);
            //if (indexS > -1)
            //    tempString = tempString.Remove(indexS, personClass.City.Length);

            //indexS = tempString.IndexOf(personClass.FirstName, 0);
            //if (indexS > -1)
            //    tempString = tempString.Remove(indexS, personClass.FirstName.Length);

            //indexS = tempString.IndexOf(personClass.LastName, 0);
            //if (indexS > -1)
            //    tempString = tempString.Remove(indexS, personClass.LastName.Length);

            //indexS = tempString.IndexOf(personClass.SecondName, 0);
            //if (indexS > -1)
            //    tempString = tempString.Remove(indexS, personClass.SecondName.Length);


            //indexS = Regex.Replace(tempString, "[.]+[ ]", ".").IndexOf(personClass.DateTimes.ToShortDateString(), 0);
            //if (indexS > -1)
            //    tempString = Regex.Replace(tempString, "[.]+[ ]", ".").Remove(indexS, personClass.DateTimes.ToShortDateString().Length);
            //tempString = Regex.Replace(tempString, "[.]+", ". ");

            for (int i = 0; i < SerchText.Length; i++)
            {
                indexS = tempString.ToLower().IndexOf(SerchText[i].ToLower(), 0);
                if (indexS > -1)
                {
                    tempString = tempString.Remove(indexS, SerchText[i].Length);
                }
            }
            return tempString;
        }
    }
}
