using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
//using java.sql;
//using java.util;
using SimpleNLG.Main.lexicon.util.lexAccess.Lib;
using SimpleNLG.Main.lexicon.util.lvg;
using SimpleNLG.Main.xmlrealiser;

namespace SimpleNLG.Main.lexicon.util.lexAccess.Db
{
    using Configuration = Configuration;
    using Category = Category;

    public class DbLexRecord

    {
        public static List<string> GetRecordsByBase(object conn, XMLRealiser.LexiconType lexiconType, string @base)
        {
            switch (lexiconType)
            {
                case XMLRealiser.LexiconType.NIHDB_HSQL:
                    throw new NotImplementedException("HSQLdb disabled");
                    string query = "SELECT lexRecord FROM LEX_RECORD where (base = '" + DbBase.FormatSqlStr(@base) +
                                   "' and lastAction <> 3)";

                    //Statement statement = ((Connection) conn).createStatement();
                    //ResultSet rs = statement.executeQuery(query);
                    //List<string> outs = new List<string>();
                    //while (rs.next())

                    //{
                    //    string lexRecord = rs.getString("lexRecord");
                    //    outs.Add(lexRecord);
                    //}

                    //rs.close();
                    //statement.close();
                    //return outs;

                case XMLRealiser.LexiconType.NIHDB_SQLITE:
                    List<string> recordStringList = new List<string>();
                    SQLiteCommand command = new SQLiteCommand("SELECT lexRecord FROM LEX_RECORD where (base = @BaseForm and lastAction <> 3)", (SQLiteConnection)conn);
                    command.Parameters.Add("@BaseForm", DbType.String).Value = @base;
                    SqliteAccess.ProcessDataReader((SQLiteConnection)conn, command, reader => recordStringList.Add(reader["lexRecord"] as string));
                    return recordStringList;
            }

            return null;
        }


        public static List<string> GetRecordsByBase(object conn, XMLRealiser.LexiconType lexiconType, string @base, int baseBy)
        {
            
            List<string> recordStringList = new List<string>();
            switch (lexiconType)
            {
                case XMLRealiser.LexiconType.NIHDB_HSQL:
                    throw new NotImplementedException("HSQLdb disabled");
                    string query = "SELECT lexRecord FROM LEX_RECORD where (base = '" + DbBase.FormatSqlStr(@base) +
                                   "' and lastAction <> 3)";

                    switch (baseBy)
                    {
                        case 4:
                            query = "SELECT lexRecord FROM LEX_RECORD where (base = '" + DbBase.FormatSqlStr(@base) +
                                    "' and lastAction <> 3)";
                            break;
                        case 3:
                            query = "SELECT lexRecord FROM LEX_RECORD where (base like '%" + DbBase.FormatSqlStr(@base) +
                                    "' and lastAction <> 3)";
                            break;
                        case 1:
                            query = "SELECT lexRecord FROM LEX_RECORD where (base like '" + DbBase.FormatSqlStr(@base) +
                                    "%' and lastAction <> 3)";
                            break;
                        case 2:
                            query = "SELECT lexRecord FROM LEX_RECORD where (base like '%" + DbBase.FormatSqlStr(@base) +
                                    "%' and lastAction <> 3)";
                            break;
                    }
                    //Statement statement = ((Connection)conn).createStatement();
                    //ResultSet rs = statement.executeQuery(query);
                    //while (rs.next())
                    //{
                    //    string lexRecord = rs.getString("lexRecord");
                    //    recordStringList.Add(lexRecord);
                    //}
                    break;
                case XMLRealiser.LexiconType.NIHDB_SQLITE:
                    SQLiteCommand command;
                    switch (baseBy)
                    {
                        case 3:
                            @base = "%" + @base;
                            command = new SQLiteCommand("SELECT lexRecord FROM LEX_RECORD where (base like @BaseForm and lastAction <> 3)", (SQLiteConnection)conn);
                            break;
                        case 1:
                            @base = @base + "%";
                            command = new SQLiteCommand("SELECT lexRecord FROM LEX_RECORD where (base like @BaseForm and lastAction <> 3)", (SQLiteConnection)conn);
                            break;
                        case 2:
                            @base = "%" + @base + "%";
                            command = new SQLiteCommand("SELECT lexRecord FROM LEX_RECORD where (base like @BaseForm and lastAction <> 3)", (SQLiteConnection)conn);
                            break;
                        case 4:
                        default:
                            command = new SQLiteCommand("SELECT lexRecord FROM LEX_RECORD where (base = @BaseForm and lastAction <> 3)", (SQLiteConnection)conn);
                            break;
                    }
                    command.Parameters.Add("@BaseForm", DbType.String).Value = @base;
                    SqliteAccess.ProcessDataReader((SQLiteConnection)conn, command, reader => recordStringList.Add(reader["lexRecord"] as string));
                    break;
            }

            return recordStringList;
        }


        public static List<string> GetRecordsByCat(object conn, XMLRealiser.LexiconType lexiconType, long category)
        {
            string catCondition = GetCategoryCondition(category);
            string query = "SELECT lexRecord FROM LEX_RECORD where ((lastAction <> 3)" + catCondition + ")";
            switch (lexiconType)
            {
                case XMLRealiser.LexiconType.NIHDB_HSQL:
                    throw new NotImplementedException("HSQLdb disabled");
                    //Statement statement = ((Connection)conn).createStatement();
                    //ResultSet rs = statement.executeQuery(query);
                    //List<string> outs = new List<string>();
                    //while (rs.next())

                    //{
                    //    string lexRecord = rs.getString("lexRecord");
                    //    outs.Add(lexRecord);
                    //}

                    //rs.close();
                    //statement.close();
                    //return outs;

                case XMLRealiser.LexiconType.NIHDB_SQLITE:
                    List<string> recordStringList = new List<string>();
                    SQLiteCommand command = new SQLiteCommand(query);
                    SqliteAccess.ProcessDataReader((SQLiteConnection)conn, command, reader => recordStringList.Add(reader["lexRecord"] as string));
                    return recordStringList;
            }

            return null;
        }


        public static string GetRecordByEui(object conn, XMLRealiser.LexiconType lexiconType, string eui)
        {

            switch (lexiconType)
            {
                case XMLRealiser.LexiconType.NIHDB_HSQL:
                    throw new NotImplementedException("HSQLdb disabled");
                    string query = "SELECT lexRecord FROM LEX_RECORD WHERE (eui = '" + eui + "' AND lastAction <> 3)";

                    //Statement statement = ((Connection)conn).createStatement();
                    //ResultSet rs = statement.executeQuery(query);
                    //string lexRecord = "";
                    //while (rs.next())

                    //{
                    //    lexRecord = rs.getString("lexRecord");
                    //}

                    //rs.close();
                    //statement.close();
                    //return lexRecord;

                case XMLRealiser.LexiconType.NIHDB_SQLITE:
                    List<string> recordStringList = new List<string>();
                    SQLiteCommand command = new SQLiteCommand("SELECT lexRecord FROM LEX_RECORD WHERE (eui = @ID AND lastAction <> 3)", (SQLiteConnection)conn);
                    command.Parameters.AddWithValue("@ID", eui);
                    SqliteAccess.ProcessDataReader((SQLiteConnection)conn, command, reader => recordStringList.Add(reader["lexRecord"] as string));
                    return recordStringList[0];
            }

            return null;
        }

        private static string GetCategoryCondition(long category)
        {
            string outStr = "";
            string catStr = Category.ToName(category);
            if ((ReferenceEquals(catStr, null)) || (catStr.Length == 0))

            {
                outStr = " AND (category = 'none') ";
            }
            else if (catStr.Equals("all"))

            {
                outStr = "";
            }
            else

            {
                string[] buf = catStr.Split('+').ToList().Where(x => x != "").ToArray();
                outStr = " AND (";
                bool firstTime = true;
                foreach (string curCat in buf)
                {
                    if (firstTime == true)

                    {
                        outStr = outStr + "category='" + curCat + "'";
                    }
                    else

                    {
                        outStr = outStr + " OR category='" + curCat + "'";
                    }

                    firstTime = false;
                }

                outStr = outStr + ")";
            }

            return outStr;
        }

        //public static void Main(string[] args)

        //{
        //    string eui = "E0065088";
        //    string @base = "left";

        //    Configuration conf = new Configuration("data.config.la", true);

        //    try

        //    {
        //        Connection conn = DbBase.OpenConnection(conf);
        //        if (conn != null)

        //        {
        //            string lexRecord = GetRecordByEui(conn, XMLRealiser.LexiconType.NIHDB_HSQL, eui);
        //            Console.WriteLine("--- eui: " + eui + " ---");
        //            Console.WriteLine(lexRecord);

        //            List<string> lexRecords = GetRecordsByBase(conn, XMLRealiser.LexiconType.NIHDB_HSQL, @base);
        //            DbBase.CloseConnection(conn);
        //            Console.WriteLine("-- base: " + @base + " ---");
        //            for (int i = 0; i < lexRecords.Count; i++)

        //            {
        //                Console.WriteLine((string) lexRecords[i]);
        //            }
        //        }
        //    }
        //    catch (Exception e)

        //    {
        //        Console.WriteLine("** Error: " + e.Message);
        //    }
        //}
    }


}