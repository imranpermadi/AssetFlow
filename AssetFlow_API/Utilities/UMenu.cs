using FAMS_Data;
using FAMS_Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAMS_API.Utilities
{
    public class UMenu
    {
        public static List<Menu> GetAuthorizedMenu(string username, bool isAdmin)
        {
            DataEntities db = new DataEntities();
            Menu[] menu = new Menu[0];
            string query = @";with name_tree as 
                            (
                                SELECT m.*
	                            FROM Menu m
	                            {0}
                                union all
                                select C.*
                                from menu c 

                                inner join name_tree p on c.id = p.parent
	                            --and c.id <> c.parent
                            ) 

                            SELECT distinct * FROM name_tree";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@EMP", username));

            if (!isAdmin) query = string.Format(query, @"INNER JOIN GroupMenu gm ON gm.MenuId = m.Id
	                            INNER JOIN [Group] g ON g.Id = gm.GroupId
	                            INNER JOIN [UserGroup] ug ON ug.GroupId = g.Id
	                            INNER JOIN [User] u ON u.Id = ug.UserId
	                            WHERE u.Username = @EMP AND m.Visible = 1");
            else query = string.Format(query, " WHERE m.MenuController IS NOT NULL AND m.Visible = 1 ");



            List<Menu> listOfData_ = db.Database.SqlQuery<Menu>(query, parameters.ToArray()).ToList();

            return listOfData_;
        }

        private static IList<Menu> GetChildrenMenu(IList<Menu> menuList, int parentId = 0)
        {
            return menuList.Where(x => x.Parent == parentId).OrderBy(x => x.Ordinal).ThenBy(r => r.MenuName).ToList();
        }
        private static Menu GetMenuItem(IList<Menu> menuList, int id)
        {
            return menuList.FirstOrDefault(x => x.Id == id);
        }

        //public static string GetMenuString(Menu[] menuItems)
        //{
        //    //var menuItems = GetAuthorizedMenu(username);

        //    var builder = new StringBuilder();
        //    builder.Append(GetMenuLiString(menuItems, 0));
        //    return builder.ToString();

        //}

        public static List<object> GetUserMenu(string username, bool isAdmin)
        {
            var authorizedMenus = GetAuthorizedMenu(username, isAdmin);

            //Add Dashboard on top
            var dashboard = new MenuItemModel()
            {
                label = "Dashboard",
                to = "/",
                icon = "pi pi-fw pi-chart-bar"
            };

            var menus = GetMenuLiString(authorizedMenus, 0);
            //menus.Insert(0, dashboard);
            //var a = menus.ToString();

            return menus;
        }



        private static List<object> GetMenuLiString(IList<Menu> menuList, int parentId)
        {
            var children = GetChildrenMenu(menuList, parentId);

            if (children.Count <= 0)
            {
                return new List<object>();
            }

            var builder = new List<object>();

            foreach (var item in children)
            {
                var childStr = GetMenuLiString(menuList, (int)item.Id);

                if (childStr.Count > 0)
                {
                    var menu = new MenuItemModel();

                    menu.alias = item.MenuController;
                    //menu.alias = item.Menu_Alias;
                    menu.icon = item.MenuIcon;
                    menu.items = childStr;
                    menu.label = item.MenuName;

                    builder.Add(menu);

                    //builder.Append("<li class=\"treeview\">");
                    //builder.Append("<a href=\"#\">");
                    //builder.AppendFormat("<i class=\"{0}\"></i> <span>{1}</span>", item.Menu_Icon, item.Menu_Name);
                    //builder.Append("<span class=\"pull-right-container\">");
                    //builder.Append("<i class=\"fa fa-angle-left pull-right\"></i>");
                    //builder.Append("</span>");
                    //builder.Append("</a>");
                    //builder.Append("<ul class=\"treeview-menu\">");
                    //builder.Append(childStr);
                    //builder.Append("</ul>");
                    //builder.Append("</li>");
                }
                else
                {
                    var menu = new MenuModel();
                    string[] splitted = item.MenuController.Split('?');
                    string param = "";
                    string controller = item.MenuController;
                    string action = item.MenuAction;

                    string to = null;
                    if (splitted.Length > 1)
                    {
                        to = "{path : '$path', query : {$query} }";

                        string query = "";
                        string fullQuery = splitted[1];
                        var splittedQuery = fullQuery.Split('&');
                        dynamic myDynamic = new ExpandoObject();

                        for (int i = 0; i < splittedQuery.Length; i++)
                        {
                            string key = splittedQuery[i].Split('=')[0];
                            string value = splittedQuery[i].Split('=')[1];

                            query += key + ":'" + value + "',";

                            Type t = Type.GetType(key);
                            myDynamic.t = value;
                        }

                        query = query.TrimEnd(',');

                        to = to.Replace("$path", action).Replace("$query", query);

                        menu.to = $"/{controller}/{action}";
                        menu.query = myDynamic;
                    }
                    else
                    {
                        to = "{ path: '$path' }";
                        to = to.Replace("$path", $"/{controller}/{action}");
                        menu.to = $"/{controller}/{action}";

                    }

                    //menu.to = to;
                    menu.id = item.Id;
                    menu.label = item.MenuName;
                    menu.icon = item.MenuIcon;
                    //menu.alias = item.Alias;
                    menu.alias = item.MenuController;

                    builder.Add(menu);
                }
            }

            return builder;
        }

        public static bool UserHasAuthorization(string username, string action, string controller, out GroupMenu menuAuth)
        {
            DataEntities db = new DataEntities();

            menuAuth = new GroupMenu();
            bool hasAuth = false;
            string query = @"select gm.*
                                from GroupMenu gm
		                        INNER JOIN Menu m ON m.Id = gm.MenuId
	                            INNER JOIN [Group] g ON g.Id = gm.GroupId
	                            INNER JOIN [UserGroup] ug ON ug.GroupId = g.Id
	                            INNER JOIN [User] u ON u.Id = ug.UserId
	                            WHERE u.Username = @emp
			                        AND m.MenuAction = @action
			                        AND m.MenuController = @controller ";
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            sqlParams.Add(new SqlParameter("@emp", username));
            sqlParams.Add(new SqlParameter("@action", action));
            sqlParams.Add(new SqlParameter("@controller", controller));

            try
            {
                var user = db.User.Where(r => r.Username == username && r.IsDeleted != "Y").FirstOrDefault();
                if (user != null)
                {
                    if (user.IsAdmin == "Y")
                    {
                        menuAuth.Create = true;
                        menuAuth.Edit = true;
                        menuAuth.View = true;
                        menuAuth.Delete = true;
                        menuAuth.Print = true;
                        return true;
                    }
                }
                else
                {
                    return false;
                }

                GroupMenu[] groupMenus = db.Database.SqlQuery<GroupMenu>(query, sqlParams.ToArray()).ToArray();

                if (groupMenus.Length > 0)
                {
                    hasAuth = true;

                    menuAuth.MenuId = groupMenus[0].MenuId;

                    int countView = groupMenus.Where(r => r.View == true).Count();
                    int countEdit = groupMenus.Where(r => r.Edit == true).Count();
                    int countDelete = groupMenus.Where(r => r.Delete == true).Count();
                    int countCreate = groupMenus.Where(r => r.Create == true).Count();
                    int countPrint = groupMenus.Where(r => r.Print == true).Count();

                    if (countView > 0) menuAuth.View = true;
                    else menuAuth.View = false;

                    if (countEdit > 0) menuAuth.Edit = true;
                    else menuAuth.Edit = false;

                    if (countDelete > 0) menuAuth.Delete = true;
                    else menuAuth.Delete = false;

                    if (countCreate > 0) menuAuth.Create = true;
                    else menuAuth.Create = false;

                    if (countPrint > 0) menuAuth.Print = true;
                    else menuAuth.Print = false;
                }
            }
            catch (Exception ex)
            {
                string a = ex.Message;
                hasAuth = false;
            }

            return hasAuth;
        }


    }
}
