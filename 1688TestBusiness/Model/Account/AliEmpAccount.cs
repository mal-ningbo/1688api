using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ali1688Business.Model.Account
{
    public class AliEmpAccount:AliSecret
    {
        /// <summary>
        /// 外部系统主键
        private string _employeeId;
        public string employeeId {
            get {
                if (string.IsNullOrWhiteSpace(_employeeId))
                    throw new Exception("员工主键不能为空");
                return _employeeId;
            }
            set { 
                _employeeId = value;
            }
        }
        /// <summary>员工名称
        /// 外部系统
        /// </summary>
        private string _name;
        public string name {
            get {
                if (string.IsNullOrWhiteSpace(_name))
                    throw new Exception("员工姓名不能为空");
                return _name; }
            set {               
                _name = value;
            }
        }
        /// <summary>
        /// 手机
        /// </summary>
        public string mobileNo { get; set; }
        /// <summary>
        /// 电子邮件
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        private string _sex;
        public string sex {
            get {               
                return _sex != "2" || string.IsNullOrWhiteSpace(_sex) ? "1" : "2"; }
            set { _sex=value; }
        }
        /// <summary>
        /// 部门
        /// </summary>
        public string department { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public string role { get; set; }
    }
}
