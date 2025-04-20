using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using Server.Model;

namespace Server.Controller
{
    public class DBC
    {
        public MySqlConnection Conn { get; set; }
        public string dateFormat = "yyyy-MM-dd HH:mm:ss";
        public DBC()
        {
            string ip = "127.0.0.1";
            int port = 3306;
            string uid = "root";
            string pwd = "1234";
            string dbname = "PARKING_LOT";
            
            string connectString = $"Server={ip};Port={port};Database={dbname};Uid={uid};Pwd={pwd};CharSet=utf8;";
            // 연결 확인
            Conn = new MySqlConnection(connectString);
            Conn.Open();
            Conn.Ping();
        }

        /* 입차 시 정기 차량 구분 조회
         * 차량구분 반환
        */
        public byte Select_expDate(string vehicleNum)
        {
            byte cls = 0;
            string query = $"SELECT EXP_DATE FROM USER WHERE VEHICLE_NUM = '{vehicleNum}'";
            MySqlDataReader? dr = null;
            MySqlCommand cmd = new MySqlCommand(query, Conn);
            try
            {
                dr = cmd.ExecuteReader();
                // 정기 등록된 차량이면
                if (dr.Read())
                {
                    DateTime exp_date = dr.GetDateTime(0);
                    DateTime now = DateTime.Now;
                    // 정기등록기간이 남아 있으면
                    if (DateTime.Compare(exp_date.Date, now.Date) >= 0)
                    {
                        cls = 2;
                    }
                    else
                    {
                        cls = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Select_expDate" + ex.Message);
            }
            finally
            {
                dr.Close();
            }
            return cls;
        }

        // 입차 기록 삽입
        public void Insert_Entry_record(byte cls, string vehicleNum)
        {
            
            Console.WriteLine("차량 번호 : " + vehicleNum);
            
            string query = $"INSERT INTO ENTRY_EXIT_RECORD (VEHICLE_NUM, ENTRY_DATE, CLASSIFICATION) VALUES(" +
                $"'{vehicleNum}', '{DateTime.Now.ToString(dateFormat)}', {cls});";
            Console.WriteLine("쿼리문 : " + query);

            MySqlCommand cmd = new MySqlCommand( query, Conn);
            try { cmd.ExecuteNonQuery(); }
            catch (Exception ex) { Console.WriteLine("Insert_Entry_record" + ex.Message ); }
        }

        // 출차 시 차량 구분 조회(출차 시점과, 입/출차 기록의 정기 만료 일자 차이 문제)
        public byte Select_classification(string vehicleNum)
        {
            byte cls = 0;
            string query = $"SELECT CLASSIFICATION FROM ENTRY_EXIT_RECORD WHERE VEHICLE_NUM = '{vehicleNum}' AND EXIT_DATE IS NULL";
            MySqlDataReader? dr = null;
            MySqlCommand cmd = new MySqlCommand(query, Conn);
            try
            {
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    cls = dr.GetByte(0);
                }
            }
            catch (Exception ex) { Console.WriteLine("Select_classfication" + ex.Message); }
            finally { dr.Close(); }
            return cls;
        }

        // 차량 구분 업데이트
        public void Update_classification(string vehicleNum, byte cls)
        {
            string query = $"UPDATE ENTRY_EXIT_RECORD SET CLASSIFICATION = {cls} " +
                $"WHERE VEHICLE_NUM = '{vehicleNum}' AND EXIT_DATE IS NULL";
            MySqlCommand cmd = new MySqlCommand(query , Conn);
            try { cmd.ExecuteNonQuery(); }
            catch(Exception ex) { Console.WriteLine("Update_classification" + ex.Message); }
        }

        //  출차하지 않은 차량의 입차 일시, 차량 구분 조회
        public Entry_exit_record Select_record(string vehicleNum)
        {
            Entry_exit_record record = new()
            {
                VehicleNum = vehicleNum
            };

            string query = $"SELECT ENTRY_DATE, CLASSIFICATION FROM ENTRY_EXIT_RECORD WHERE VEHICLE_NUM = '{vehicleNum}' AND EXIT_DATE IS NULL;";
            MySqlDataReader? dr = null;
            MySqlCommand cmd = new MySqlCommand(query, Conn);

            try
            {
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    record.EntryDate = dr.GetDateTime(0);
                    record.Classification = dr.GetByte(1);
                }
            }
            catch (Exception ex) { Console.WriteLine("Select_record" + ex.Message); }
            finally { dr.Close(); }
            return record;
        }

        public void Update_exitRecord(string vehicleNum, DateTime ExitDate, int TotalFee)
        {
            string query =  $"UPDATE ENTRY_EXIT_RECORD " +
                            $"SET EXIT_DATE = '{ExitDate.ToString(dateFormat)}', " +
                            $"FEE = {TotalFee} " +
                            $"WHERE VEHICLE_NUM = '{vehicleNum}' AND EXIT_DATE IS NULL;";
            Console.WriteLine("출차 쿼리 : " + query);
            MySqlCommand cmd = new MySqlCommand(query, Conn);
            try { cmd.ExecuteNonQuery(); }
            catch (Exception ex) { Console.WriteLine("Update_exitRecord"+ ex.Message) ; }
        }

        public void Insert_regInfo(User user)
        {
            DateTime First_reg_date = DateTime.Now;
            DateTime exp_date = First_reg_date.AddDays(user.Reg_period);
            string query = $"INSERT INTO USER VALUES " +
                            $"('{user.VehicleNum}', '{user.Pw}', {First_reg_date.ToString(dateFormat)}', '{exp_date.ToString(dateFormat)}', {user.Fee});";
            MySqlCommand cmd = new MySqlCommand(query, Conn);

            try { cmd.ExecuteNonQuery(); }
            catch (Exception ex) { Console.WriteLine("Insert_regInfo"+ex.Message); }
        }

        public void Update_regInfo(User user)
        {
            string query = $"UPDATE USER SET EXP_DATE = date_add((SELECT EXP_DATE FROM (SELECT EXP_DATE FROM USER WHERE VEHICLE_NUM = '{user.VehicleNum}') AS A)," +
                $"INTERVAL {user.Reg_period} DAY)," +
                $"TOTAL_FEE = (SELECT TOTAL_FEE FROM (SELECT TOTAL_FEE FROM USER WHERE VEHICLE_NUM = '{user.VehicleNum}') AS B) + {user.Fee} WHERE VEHICLE_NUM = '{user.VehicleNum}';";
            MySqlCommand cmd = new MySqlCommand( query, Conn);
            try { cmd.ExecuteNonQuery(); }
            catch (Exception ex) { Console.WriteLine("Update_regInfo" + ex.Message); }
        }

        public List<Entry_exit_record> Select_all_record()
        {
            List<Entry_exit_record> parkingList = [];
            string query = $"SELECT VEHICLE_NUM, ENTRY_DATE, CLASSIFICATION FROM ENTRY_EXIT_RECORD;";
            MySqlDataReader dr = null;
            MySqlCommand cmd = new MySqlCommand(query, Conn);
            try
            {
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Entry_exit_record record = new()
                    {
                        VehicleNum = dr.GetString(0),
                        EntryDate = dr.GetDateTime(1),
                        Classification = dr.GetByte(2),
                    };
                    parkingList.Add(record);
                }
            }catch (Exception ex) { Console.WriteLine("Select_all_record" + ex.Message);}
            finally { dr.Close(); }
            return parkingList;
        }
    }
}
