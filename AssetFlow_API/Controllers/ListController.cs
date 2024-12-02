using FAMS_API.Utilities;
using FAMS_Data;
using FAMS_Models;
using FAMS_Models.Utilities;
using FAMS_ViewModels.Utilities;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FAMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiAuthorize]
    public class ListController : ControllerBase
    {
        DataEntities db = new DataEntities();

        //[HttpGet]
        //[Route("department_code")]
        //public object DepartmentCode(string q = "", int page = 0, bool init = false)
        //{
        //    int fromIdx = page > 0 ? (page * 10) : 0;
        //    int toIdx = 10;
        //    var listData = db.Department.Where(r => 1 == 2);

        //    if (init)
        //    {
        //        var listCode = string.IsNullOrEmpty(q) ? new string[] { q } : q.Split(';');

        //        listData = db.Departments
        //        .Where
        //        (
        //            r =>
        //            (string.IsNullOrEmpty(q) || listCode.Contains(r.Code.ToString()))
        //        )
        //        .OrderBy(r => r.Id);
        //    }
        //    else
        //    {
        //        listData = db.Departments
        //        .Where
        //        (
        //            r =>
        //            (string.IsNullOrEmpty(q) || ("[" + r.Code + "] " + r.Description).ToUpper().Contains(q.ToUpper())) &&
        //            r.Is_Deleted != "Y"
        //        )
        //        .OrderBy(r => r.Id);
        //    }
        //    var data = listData.Skip(fromIdx)
        //        .Take(toIdx)
        //        .Select(r => new { id = r.Code, text = "[" + r.Code + "] " + r.Description, nama = r.Description })
        //        .ToList();

        //    return new { items = data };
        //}

        //[HttpGet]
        //[Route("department_id")]
        //public object DepartmentId(string q = "", int page = 0, bool init = false)
        //{
        //    int fromIdx = page > 0 ? (page * 10) : 0;
        //    int toIdx = 10;
        //    var listData = db.Departments.Where(r => 1 == 2);

        //    if (init)
        //    {
        //        listData = db.Departments
        //        .Where
        //        (
        //            r =>
        //            (string.IsNullOrEmpty(q) || r.Id.ToString() == q)
        //        )
        //        .OrderBy(r => r.Id);
        //    }
        //    else
        //    {
        //        listData = db.Departments
        //        .Where
        //        (
        //            r =>
        //            (string.IsNullOrEmpty(q) || ("[" + r.Code + "] " + r.Description).ToUpper().Contains(q.ToUpper())) &&
        //            r.Is_Deleted != "Y"
        //        )
        //        .OrderBy(r => r.Id);
        //    }
        //    var data = listData.Skip(fromIdx)
        //        .Take(toIdx)
        //        .Select(r => new { id = r.Id, text = "[" + r.Code + "] " + r.Description, nama = r.Description })
        //        .ToList();

        //    return new { items = data };
        //}

        [HttpGet]
        [Route("group")]
        public object Group(string q = "", int page = 0, bool init = false)
        {
            int fromIdx = page > 0 ? (page * 10) : 0;
            int toIdx = 10;
            var listData = db.Group.Where(r => 1 == 2);

            if (init)
            {
                listData = db.Group
                           .Where
                           (
                               r => r.Id.ToString() == q
                           )
                           .OrderBy(r => r.Code);
            }
            else
            {
                listData = db.Group
                           .Where
                           (
                               r =>
                               (string.IsNullOrEmpty(q) || ("[" + r.Code + "] " + r.Description).ToUpper().Contains(q.ToUpper()))
                           )
                           .OrderBy(r => r.Code);
            }
            var data = listData.Skip(fromIdx)
                .Take(toIdx)
                .Select(r => new { id = r.Id, text = "[" + r.Code + "] " + r.Description })
                .ToList();

            return new { items = data };
        }


        [HttpGet]
        [Route("lookup")]
        public object Lookup(string q = "", int page = 0, bool init = false, string group = "", bool allow_all=false)
        {
            int fromIdx = page > 0 ? (page * 10) : 0;
            int toIdx = 10;
            var listData = db.Lookup.Where(r => 1 == 2);

            if (init)
            {
                listData = db.Lookup
                               .Where
                               (
                                   r =>
                                   r.LookupGroup == group &&
                                   (string.IsNullOrEmpty(q) || r.LookupKey.ToLower() == q.ToLower())
                               )
                               .OrderBy(r => r.LookupGroup).ThenBy(r => r.LookupKey);
            }
            else
            {
                listData = db.Lookup
                         .Where
                         (
                             r =>
                             r.LookupGroup == group &&
                             (string.IsNullOrEmpty(q) || (r.LookupValue).ToLower().Contains(q.ToLower()))
                             && r.IsDeleted != "Y"
                         )
                         .OrderBy(r => r.LookupGroup).ThenBy(r => r.LookupKey);
            }
            var data = listData.Skip(fromIdx)
                .Take(toIdx)
                .Select(r => new { 
                    id = r.LookupKey, 
                    text = r.LookupValue, 
                    Lookup_Info1 = string.IsNullOrEmpty(r.LookupInfo1) ? "" : r.LookupInfo1, 
                    Lookup_Info2 = string.IsNullOrEmpty(r.LookupInfo2) ? "" : r.LookupInfo2
                })
                .ToList();

            return new { items = data };
        }

        [HttpGet]
        [Route("release_status_all")]
        public object ReleaseStatusListGeneral(string q="", string initValue = "", int page = 0)
        {
            int fromIdx = page > 0 ? (page * 10) : 0;
            int toIdx = 10;
            var listStatus = new List<String>()
            {
                Constants.ReleaseStatus.FULL,
                Constants.ReleaseStatus.PARTIAL,
                Constants.ReleaseStatus.UNRELEASED
            };

            var listData = listStatus;

            if (!string.IsNullOrEmpty(initValue))
            {
                listData = listStatus
                .Where
                (
                    r =>
                    (string.IsNullOrEmpty(initValue) || (initValue == r.ToString()))
                ).ToList();
            }
            else
            {
                listData = listStatus
                    .Where
                    (
                        r =>
                        (string.IsNullOrEmpty(q) || (r).ToUpper().Contains(q.ToUpper()))
                    ).ToList();

            }
            var data = listData.Skip(fromIdx)
                .Take(toIdx)
                .Select(r => new { id = r, text = r })
                .ToList();

            return new { items = data };
        }


        [HttpGet]
        [Route("menu")]
        public object MenuList(string q = "", int page = 0, bool init = false)
        {
            int fromIdx = page > 0 ? (page * 10) : 0;
            int toIdx = 10;
            var listData = db.Menu.Where(r => 1 == 2);

            if (init)
            {
                listData = db.Menu
                .Where
                (
                    r =>
                    (string.IsNullOrEmpty(q) || (q == r.Id.ToString()))
                )
                .OrderBy(r => r.Id);
            }
            else
            {
                listData = db.Menu
                .Where
                (
                    r =>
                    (string.IsNullOrEmpty(q) || (r.MenuName).ToUpper().Contains(q.ToUpper()))
                   
                )
                .OrderBy(r => r.Id);
            }

            var data = listData.Skip(fromIdx)
                .Take(toIdx)
                .Select(r => new { id = r.Id, text = r.MenuName })
                .Distinct()
                .ToList();

            return new { items = data };
        }


        [HttpGet]
        [Route("user")]
        public object UserList(string q = "", int page = 0, bool init = false)
        {
            int fromIdx = page > 0 ? (page * 10) : 0;
            int toIdx = 10;
            var listData = db.User.Where(r => 1 == 2).OrderBy(r => r.Id);

            if (init)
            {
                listData = db.User
                .Where
                (
                    r => r.Username.ToString() == q
                )
                .OrderBy(r => r.Username);
            }
            else
            {
                listData = db.User
                    .Where
                    (
                        r =>
                        (string.IsNullOrEmpty(q) || ("[" + r.Username + "] " + r.Fullname).ToUpper().Contains(q.ToUpper()))
                        && r.IsDeleted != "Y"
                    )
                    .OrderBy(r => r.Username);

            }
            var data = listData.Skip(fromIdx)
                .Take(toIdx)
                .Select(r => new { id = r.Username, text = "[" + r.Username + "] " + r.Fullname, Email = r.Email})
                .ToList();

            return new { items = data };
        }
    }

}
