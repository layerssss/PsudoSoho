using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MFLJson;
using System.Linq.Expressions;
namespace GoClassing.Internal
{
    public static class GCSearch
    {
        public static JsonArray GetAllCtypes1()
        {
            var D = new gc_localtestEntities();
            return Json.Array(D.gccon_ctype1
                .AsEnumerable()
                .Select(tc => Json.Object("type", Json.String(tc.type))));
        }
        public static JsonObject GetAllCtypes2(string ctypes1, string p)
        {
            var types = ctypes1.Split(',');
            var D = new gc_localtestEntities();
            return GCCommon.GetPagedList(types.SelectMany(type1 => D.gccon_ctype2.Where(tp => tp.ctype1_type == type1)).AsQueryable()
                , tt => tt.type, tc => Json.Object("type", Json.String(tc.type)), p, 20,true);
        }
        public static JsonObject GetAllCourses(string page, string ctype1,string ctypes2,string filter)
        {
            ctypes2 = ctypes2 ?? "";
            ctype1 = ctype1 ?? "";
            filter = filter ?? "";
            var D = new gc_localtestEntities();
            var types2 = ctypes2.Split(',').Distinct();
            var filters = filter.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries);
            var partc = Expression.Parameter(typeof(gc_course), "tc");
            Expression exp = ctype1 == "" ? (Expression)Expression.Constant(true, typeof(bool)) : Expression.Equal(Expression.Property(partc, "type1"), Expression.Constant(ctype1, typeof(string)));
            if (ctypes2.Any())
            {
                Expression nexp = Expression.Constant(false, typeof(bool));
                foreach (var type2 in types2)
                {
                    nexp = Expression.Or(nexp, Expression.Equal(Expression.Property(partc, "type2"), Expression.Constant(type2, typeof(string))));
                }
                exp = Expression.And(exp, nexp);
            }
            if (filters.Any())
            {
                var contains=typeof(string).GetMethod("Contains",new Type[]{typeof(string)});
                Expression nexp = Expression.Constant(false, typeof(bool));
                foreach (var f in filters)
                {
                    nexp = Expression.Or(nexp, Expression.Call(Expression.Property(partc, "name"), contains, Expression.Constant(f, typeof(string))));
                    nexp = Expression.Or(nexp, Expression.Call(Expression.Property(Expression.Property(partc, "gc_user"), "truename"), contains, Expression.Constant(f, typeof(string))));
                }
                exp = Expression.And(exp, nexp);
            }
            return GCCommon.GetPagedList(
                D.gc_course.Where(Expression.Lambda<Func<gc_course, bool>>(
                exp
                , partc))
                , tg => tg.name, GCCourse.GetJson, page, 30, true);
        }
        public static JsonArray GetUprovince()
        {
            return Json.Array(new gc_localtestEntities().gccon_province.AsEnumerable().Select(tp => Json.Object("uprovince", Json.String(tp.province))));
        }
        public static JsonArray GetCity(string province)
        {
            return Json.Array(new gc_localtestEntities().gccon_city.Where(tc => tc.province == province).AsEnumerable().Select(tc => Json.Object("ucity", Json.String(tc.city))));
        }
        public static JsonArray GetSex()
        {
            return Json.Array(
                Json.Object("prefix",Json.String(""),"char",Json.String("保密")),
                Json.Object("prefix",Json.String("f"),"char",Json.String("女")),
                Json.Object("prefix",Json.String("m"),"char",Json.String("男"))
                );
        }

        public static JsonObject GetAllUsers(string pagestr, string search)
        {
            var D = new gc_localtestEntities();
            if(D.gccon_coop.Any(tc=>tc.coop==search)){
                return GCCommon.GetPagedList(D.gc_user.Where(tu => tu.coop.Contains(search)), tu => tu.coopSort, tu => GCUser.GetJson(tu), pagestr, 10);
            }
            var ss = search.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct();
            var us = D.gc_user.AsQueryable();
            var partu = Expression.Parameter(typeof(gc_user));
            Expression exp = Expression.Constant(true, typeof(bool));
            if (ss.Any())
            {
                var contains = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
                Expression nexp = Expression.Constant(false, typeof(bool));
                foreach (var f in ss)
                {
                    nexp = Expression.Or(nexp, Expression.Call(Expression.Property(partu, "truename"), contains, Expression.Constant(f, typeof(string))));
                    if (D.gccon_province.Any(tp => tp.province.Contains(f)))
                    {
                        nexp = Expression.Or(nexp, Expression.Call(Expression.Property(partu, "uprovince"), contains, Expression.Constant(f, typeof(string))));
                    }
                    if (D.gccon_city.Any(tp => tp.city.Contains(f)))
                    {
                        nexp = Expression.Or(nexp, Expression.Call(Expression.Property(partu, "ucity"), contains, Expression.Constant(f, typeof(string))));
                    }
                }
                exp = Expression.And(exp, nexp);
            }
            return GCCommon.GetPagedList(us.Where(Expression.Lambda<Func<gc_user,bool>>(exp,partu)), tu => tu.truename, tu => GCUser.GetJson(tu), pagestr,8,true);
        }

        public static JsonObject QuickSearch(string pagestr,string search)
        {
            var D = new gc_localtestEntities();
            var ss = search.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct();
            var partc1 = Expression.Parameter(typeof(gccon_ctype1));
            var partc2 = Expression.Parameter(typeof(gccon_ctype2));
            var parco = Expression.Parameter(typeof(gccon_coop));
            var contains = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
            Expression nexp1 = Expression.Constant(false, typeof(bool));
            Expression nexp2 = Expression.Constant(false, typeof(bool));
            Expression nexpc = Expression.Constant(false, typeof(bool));
            foreach (var f in ss)
            {
                nexp1 = Expression.Or(nexp1, Expression.Call(Expression.Property(partc1, "type"), contains, Expression.Constant(f, typeof(string))));
                nexp2 = Expression.Or(nexp2, Expression.Call(Expression.Property(partc2, "type"), contains, Expression.Constant(f, typeof(string))));
                nexpc = Expression.Or(nexpc, Expression.Call(Expression.Property(parco, "coop"), contains, Expression.Constant(f, typeof(string))));
            }
            var c1 = D.gccon_ctype1.Where(Expression.Lambda<Func<gccon_ctype1, bool>>(nexp1, partc1)).OrderBy(tc => tc.type).AsEnumerable().Select(tc1 => Json.Object(
                "type", Json.String(tc1.type),
                "path", Json.String("/C/" + tc1.type + '/'),
                "ftype", Json.String("ctype1"),
                "icon",Json.String("blank")
                ));
            var c2 = D.gccon_ctype2.Where(Expression.Lambda<Func<gccon_ctype2, bool>>(nexp2, partc2)).OrderBy(tc => tc.type).AsEnumerable().Select(tc2 => Json.Object(
                "type", Json.String(tc2.type),
                "path", Json.String("/C/" + tc2.ctype1_type + '/' + tc2.type + '/'),
                "ftype", Json.String("ctype2"),
                "icon",Json.String("blank")
                ));
            var cc = D.gccon_coop.Where(Expression.Lambda<Func<gccon_coop, bool>>(nexpc, parco)).Select(tc =>tc.coop).OrderBy(tar => tar).AsEnumerable()
                .Select(tar => Json.Object(
                "type", Json.String(tar),
                "path", Json.String('/'+tar+'/'),
                "ftype", Json.String("removed"),
                "icon", Json.String("Tags/"+tar)
                    ));
            return GCCommon.GetPagedList(c1.Concat(c2).Concat(cc).AsQueryable(), to => (to["type"] as JsonString).Text, to => to, pagestr, 20);



        }
    }
}