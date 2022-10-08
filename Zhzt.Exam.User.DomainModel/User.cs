﻿using Amz.EnumLib.Enums;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using SqlSugar;
using SqlSugar.Extension.DomainHelper;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Zhzt.Exam.Auth.DomainDtoModel;

namespace Zhzt.Exam.Auth.DomainModel
{
    public class User : BaseModel
    {/// <summary>
     /// 账号
     /// </summary>
        [Required]
        [SugarColumn(IndexGroupNameList = new string[] { "Username" })]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [SugarColumn(IsNullable = false)]
        [JsonIgnore]
        public string Password { get; set; } = null!;

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string Realname { get; set; } = string.Empty;

        /// <summary>
        /// 角色ID
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public long RoleId { get; set; }

        /// <summary>
        /// 角色所属部门ID
        /// </summary>
        public long DepartmentId { get; set; } = -1;

        /// <summary>
        /// 用户状态
        /// </summary>
        public UserStatusEnum UserStatus { get; set; }

        /// <summary>
        /// 部门信息
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public virtual DepartmentDto Department { get; set; } = null!;

        /// <summary>
        /// 用户角色
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public virtual RoleDto Role { get; set; } = null!;
    }
}