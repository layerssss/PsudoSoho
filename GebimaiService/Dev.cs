using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GebimaiService
{
    /// <summary>
    /// 
    /// </summary>
    public class Dev
    {
        /// <summary>
        /// 【开发时接口】查看头哥对Actions的说明
        /// </summary>
        /// <param name="redirect">请勿更改此参数，请直接点击Execute.</param>
        [ispJs.Action]
        public void AboutActions(string redirect="/Dev/Actions.htm")
        {
        }
        /// <summary>
        /// 【开发时接口】返回首页
        /// </summary>
        /// <param name="redirect">请勿更改此参数，请直接点击Execute.</param>
        [ispJs.Action]
        public void GoHome(string redirect = "../")
        {
        }
        /// <summary>
        /// 【开发时接口】和机器人对话
        /// </summary>
        /// <param name="salut">你要对机器人说的话.</param>
        [ispJs.Action]
        public void TestRobot(string salut,out string replies)
        {
            var uname=Auth.Username;
            var u=Global.Entities.users.FirstOrDefault(tu=>tu.username==uname);
            if(u==null){
                throw(new Exception("你必须登陆先哦"));
            }
            replies = Logic.Reply(salut, u.authId).Aggregate("", (s1, s2) => s1 + ',' + s2, s1 => s1);
        }
        public static void Validate()
        {
            var uname = Auth.Username;
            var d = Global.Entities;
            var u = d.users.FirstOrDefault(tu => tu.username == uname);
            if (u == null)
            {
                throw (new Exception("你必须登陆先哦"));
            }
            if (!u.dev.Any())
            {
                throw (new Exception("亲不可以……"));
            }
        }
        /// <summary>
        /// Adds the codex.
        /// </summary>
        [ispJs.Action]
        public void AddCodex(out int id)
        {
            Validate();
            var d = Global.Entities;
            var r=new rebot()
            {
                content="亲不要啊……",
                keyword="草泥马",
                sort=0,
                locktime=DateTime.Now,
                lockdev=""
            };
            d.rebot.InsertOnSubmit(r);
            d.SubmitChanges();
            ispJs.WebApplication.SafeDelete("Dev/Default.htm");
            id=r.id;
        }
        /// <summary>
        /// Edits the codex.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="content">The content.</param>
        /// <param name="sort">The sort.</param>
        /// <param name="keyword">The keyword.</param>
        [ispJs.Action]
        public void EditCodex(int id, string content, int sort, string keyword)
        {
            Validate();
            var d = Global.Entities;
            var c = d.rebot.First(tc => tc.id == id);
            c.keyword = keyword??c.keyword;
            c.content = content??c.content;
            c.sort = sort;
            d.SubmitChanges();
            ispJs.WebApplication.SafeDelete("Dev/Default.htm");
        }
    }
}