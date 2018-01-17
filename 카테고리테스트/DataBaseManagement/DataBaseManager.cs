using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace 카테고리테스트.DataBaseManagement
{
    public class DataBaseManager
    {
        /*
         * 데이터베이스에서 값을 가져오는 클래스로 방식은 다음과 같다.
         * 1. DB에 연결해 테이블을 찾는다.
         * 2. 부모인덱스를 0으로 주는 재귀 함수를 실행한다.
         * 
         ** 재귀 함수
         * 1. DB에서 부모인덱스의 값을 가진 데이터들을 데이터포맷에 올린다.
         * 2. 데이터포맷의 숫자만큼 반복한다
         *   1. 부모인덱스를 I튜플의 인덱스로 주는 재귀 함수를 실행한다.
         ** 재귀함수 끝
         * 
         * 위처럼 하면 대/중/소의 3분류가 아닌 1~N까지의 분류를 만들 수 있다.
         */

        public MySqlConnection conn;
        public List<DataFormat> list;
        public string dbLoginInfo;

        public DataBaseManager()
        {
            list = new List<DataFormat>();
        }

        public bool SetDB(string address, string port, string id, string pw)
        {
            /*
             * DB에 접속을 하고 DB를 선택한다.
             */
            dbLoginInfo = string.Format("Server={0};Port={1};Database=davala;Uid={2};Pwd={3}", address, port, id, pw);
            using (conn = new MySqlConnection(dbLoginInfo))
            {
                try
                {
                    conn.Open();
                    conn.Close();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public void GetCategoryStart()
        {

            list = GetCategory(0);


        }

        private List<DataFormat> GetCategory(int parentIndex)
        {
            /*
            **재귀 함수
            * 1.DB에서 부모인덱스의 값을 가진 데이터들을 데이터포맷에 올린다.
            * 2.데이터포맷의 숫자만큼 반복한다
            *   1.부모인덱스를 I튜플의 인덱스로 주는 재귀 함수를 실행한다.
            **재귀함수 끝
            */
            List<DataFormat> tmpList = new List<DataFormat>();


            using (conn = new MySqlConnection(dbLoginInfo))
            {
                conn.Open();
                //데이터 포맷에 해당 부모인덱스 값을 가진 데이터들을 올린다.
                MySqlCommand cmd = new MySqlCommand(string.Format("SELECT * FROM davala.ProductCategory WHERE parentIndex={0};", parentIndex), conn);

                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    tmpList.Add(new DataFormat(Convert.ToInt32(rdr[0]), rdr[1].ToString(), Convert.ToInt32(rdr[2])));
                }

                //데이터 포맷의 숫자만큼 반복한다
                for (int i = 0; i < tmpList.Count; i++)
                {
                    //부모인덱스를 자신의 인덱스로 주는 재귀 함수를 실행한다.
                    tmpList[i].childList = GetCategory(tmpList[i].index);
                }
                conn.Close();
            }
            return tmpList;
        }
    }
}