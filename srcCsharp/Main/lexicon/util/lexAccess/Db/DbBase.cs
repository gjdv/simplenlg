using System;
using System.Collections.Generic;
//using java.sql;
using SimpleNLG.Main.lexicon.util.lexAccess.Lib;

namespace SimpleNLG.Main.lexicon.util.lexAccess.Db
{
    using Configuration = Configuration;
    using GlobalVars = GlobalVars;


    public class DbBase

    {

        //public static Connection OpenConnection(Configuration config)
        //{
        //    string driverName = GetDbDriverFromConfig(config);
        //    string url = GetDbUrlFromConfig(config) + ";ifexists=true";
        //    Connection conn = null;
        //    LoadDbDriver(driverName);
        //    string userName = GetDbUserNameFromConfig(config);
        //    string password = GetDbPasswordFromConfig(config);
        //    conn = DriverManager.getConnection(url, userName, password);

        //    return conn;
        //}


        //public static Connection OpenConnection(Configuration config, string dbVersion)
        //{
        //    string driverName = GetDbDriverFromConfig(config);
        //    string dbDir = "HSqlDb." + dbVersion + "/";
        //    string dbName = "lexAccess" + dbVersion;

        //    string url = GetDbUrlFromConfig(config, dbDir, dbName) + ";create=false";


        //    Connection conn = null;
        //    LoadDbDriver(driverName);
        //    string userName = GetDbUserNameFromConfig(config);
        //    string password = GetDbPasswordFromConfig(config);
        //    try

        //    {
        //        conn = DriverManager.getConnection(url, userName, password);
        //    }
        //    catch (Exception)

        //    {
        //        Console.Error.WriteLine("** Err: Can't Open DB connection at URL:" + url);
        //    }

        //    return conn;
        //}


        //public static Connection OpenConnection(string driverName, string url, string userName, string password)
        //{
        //    Connection conn = null;
        //    LoadDbDriver(driverName);
        //    conn = DriverManager.getConnection(url, userName, password);
        //    return conn;
        //}


        //public static void CloseConnection(Connection conn)
        //{
        //    conn.close();
        //}


        
        //public static void CloseConnection(Connection conn, Configuration config)
        //{
        //    string dbStr = config.GetConfiguration("DB_TYPE");

        //    if (dbStr.Equals("JAVADB") == true)

        //    {
        //        string connUrl = GetDbUrlFromConfig(config);
        //        ShutDownJavaDb(connUrl);
        //    }

        //    conn.close();
        //}


        public static string FormatSqlStr(string inStr)

        {
            string newStr = Replace(inStr, "'", "''");

            return newStr;
        }


        public static string ToSqlStr(string @in)

        {
            string @out = @in;
            int curIndex = @in.Length - 1;

            Dictionary<char, string> escapeCharacters = GlobalVars.GetInstance().GetEscapeCharacters();
            while (curIndex >= 0)

            {
                char curChar = @in[curIndex];
                if (escapeCharacters.ContainsKey(curChar) == true)

                {
                    string before = @out.Substring(0, curIndex);
                    string after = @out.Substring(curIndex + 1);
                    string current = (string) escapeCharacters[curChar];
                    @out = before + current + after;
                }

                curIndex--;
            }

            return @out;
        }


        //internal static void SubmitDML(string query, Configuration config)

        //{
        //    Connection conn = null;

        //    string driverName = GetDbDriverFromConfig(config);
        //    LoadDbDriver(driverName);

        //    try

        //    {
        //        string url = GetDbUrlFromConfig(config);

        //        string userName = GetDbUserNameFromConfig(config);
        //        string password = GetDbPasswordFromConfig(config);
        //        conn = DriverManager.getConnection(url, userName, password);

        //        Statement statement = conn.createStatement();
        //        statement.executeQuery(query);

        //        statement.close();
        //        conn.close();
        //    }
        //    catch (SQLException sqle)

        //    {
        //        Console.Error.WriteLine(sqle.Message);
        //        if (conn != null)

        //        {
        //            try

        //            {
        //                conn.rollback();
        //            }
        //            catch (SQLException ex)

        //            {
        //                Console.Error.WriteLine("SQLException: " + ex.Message);
        //            }
        //        }
        //    }
        //    catch (Exception e)

        //    {
        //        Console.Error.WriteLine(e.Message);
        //    }
        //}

        //private static void ShutDownJavaDb(string connUrl)

        //{
        //    bool suceessfulShutDown = false;
        //    try

        //    {
        //        DriverManager.getConnection(connUrl + ";shutdown=true");
        //    }
        //    catch (SQLException se)

        //    {
        //        //if ((se.SQLState.Equals("XJ015")) || (se.SQLState.Equals("08006")))
        //        //{
        //        //    suceessfulShutDown = true;
        //        //}
        //    }

        //    if (!suceessfulShutDown)

        //    {
        //        Console.Error.WriteLine("JavaDb Database did not shut down normally");
        //    }
        //}

        private static void LoadDbDriver(string driverName)

        {
            try

            {
                Activator.CreateInstance(Type.GetType(driverName));
            }
            catch (Exception e)

            {
                Console.Error.WriteLine("** Error: Unable to load driver(" + driverName + ").");

                Console.Error.WriteLine(e.Message);
                Console.WriteLine(e.ToString());
                Console.Write(e.StackTrace);
            }
        }

        private static string GetDbUserNameFromConfig(Configuration config)

        {
            string userName = config.GetConfiguration("DB_USERNAME");
            return userName;
        }

        private static string GetDbPasswordFromConfig(Configuration config)
        {
            string password = config.GetConfiguration("DB_PASSWORD");
            return password;
        }

        private static string GetDbDriverFromConfig(Configuration config)
        {
            string driverName = config.GetConfiguration("DB_DRIVER");
            return driverName;
        }

        private static string GetDbUrlFromConfig(Configuration config)
        {
            string dbDir = config.GetConfiguration("DB_DIR");
            string dbName = config.GetConfiguration("DB_NAME");
            string url = GetDbUrlFromConfig(config, dbDir, dbName);
            return url;
        }

        private static string GetDbUrlFromConfig(Configuration config, string dbDir, string dbName)

        {
            string dbStr = config.GetConfiguration("DB_TYPE");
            string url = null;

            if (dbStr.Equals("JAVADB") == true)

            {
                string laDir = config.GetConfiguration("LA_DIR");
                string dbHome = laDir + "/data/JavaDb/";
                string dbTemp = dbHome + "temp/laTemp";
                string dbLog = dbHome + "temp/laLog.LOG";
                //Properties p = System.Properties;
                //p.put("derby.system.home", dbHome);
                //p.put("derby.storage.tempDirectory", dbTemp);
                //p.put("derby.stream.error.file", dbLog);

                url = "jdbc:derby:" + config.GetConfiguration("DB_NAME");
            }
            else if (dbStr.Equals("HSQLDB") == true)

            {
                url = "jdbc:hsqldb:" + config.GetConfiguration("LA_DIR") + "data/" + dbDir + dbName;
            }
            else if (dbStr.Equals("JAVADB_SERVER") == true)

            {
                string dbHost = config.GetConfiguration("DB_HOST");
                string dbPortNum = config.GetConfiguration("DB_PORT_NUM");
                string userName = config.GetConfiguration("DB_USERNAME");
                string passwd = config.GetConfiguration("DB_PASSWORD");

                url = "jdbc:derby://" + dbHost + ":" + dbPortNum + "/" + dbName + ";user=" + userName + ";password=" +
                      passwd;
            }
            else if (dbStr.Equals("MYSQL") == true)

            {
                url = "jdbc:mysql://" + config.GetConfiguration("DB_HOST") + "/" + config.GetConfiguration("DB_NAME");
            }
            else if (dbStr.Equals("OTHER") == true)

            {
                url = config.GetConfiguration("JDBC_URL");
            }

            return url;
        }

        private static string Replace(string source, string oldString, string newString)

        {
            int curIndex = 0;
            int lastIndex = 0;
            string target = source;
            curIndex = source.IndexOf(oldString, StringComparison.Ordinal);
            while (curIndex > -1)

            {
                string before = source.Substring(0, curIndex);

                string after = StringHelper.SubstringSpecial(source, curIndex + oldString.Length, source.Length);
                target = before + newString + after;
                source = target;
                lastIndex = curIndex + newString.Length;
                curIndex = source.IndexOf(oldString, lastIndex, StringComparison.Ordinal);
            }

            return target;
        }
    }


}