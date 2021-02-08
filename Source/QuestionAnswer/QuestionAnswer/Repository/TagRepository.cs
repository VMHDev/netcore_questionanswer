using QuestionAnswer.Models;
using QuestionAnswer.Repository.Database;
using QuestionAnswer.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionAnswer.Repository
{
    public class TagRepository : DBCommand
    {
        /// <summary>
        /// Call procedure get list Tag
        /// </summary>
        /// <param name="_CompanyID">Company ID</param>
        /// <returns>List Brand</returns>
        public List<TagModel> GetListTag()
        {
            string[] arrColumns = { "tag_id", "tag_name" };
            List<TagModel> listResult = null;
            try
            {
                string sql = "[dbo].[proTag_GetAll]";
                List<SqlParameter> param = new List<SqlParameter>();
                listResult = GetList<TagModel>(sql, CommandType.StoredProcedure, param, arrColumns);
            }
            catch (Exception ex)
            {
                throw new Exception(UtilFunctions.GetDetailsException(ex));
            }
            return listResult;
        }
    }
}
