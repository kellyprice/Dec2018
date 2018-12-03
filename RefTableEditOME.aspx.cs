using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using Lab.Data;

namespace Lab
{
    public partial class RefTableEditOME : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateTables();
            }

            CheckEvent();
        }

        private void CheckEvent()
        {
            switch (Event__.Value)
            {
                case "SaveStandard":
                    SaveStandard();
                    break;
                case "SaveOther":
                    SaveOther();
                    break;
                case "Tables_Changed":
                    Tables_Changed();
                    break;
            }

            Event__.Value = "";
            Value__.Value = "";
        }

        private void PopulateTables()
        {
            var tables = GetTableDefs().Select(x => x.Table).ToList();

            Tables.Items.Add(new ListItem(""));

            foreach (var table in tables)
            {
                Tables.Items.Add(new ListItem(table));
            }
        }

        private void Tables_Changed()
        {
            TableHead.Text = "";
            TableBody.Text = "";

            if (Tables.SelectedIndex > 0)
            {
                SetTableHeader();
                SetTableBody();
            }
        }

        #region setHTML
        private void SetTableHeader()
        {
            var table = Tables.Items[Tables.SelectedIndex].Value;
            var s = new StringBuilder();
            var standard = GetTableDefs().Where(x => x.Table == table).Select(x => x.Standard).Single();

            if (standard)
            {
                s.Append("<tr>");
                s.AppendFormat("<th style=\"width:180px\">{0}</th>", table);
                s.Append("<th style=\"width:25px\"></td>");
                s.Append("<th style=\"width:25px\"></td>");
                s.Append("</tr>");
            }
            else if (table == "Currency")
            {
                s.Append("<tr>");
                s.Append("<th style=\"width:70px\">Code</th>");
                s.Append("<th style=\"width:180px\">Currency</th>");
                s.Append("<th style=\"width:25px\"></td>");
                s.Append("<th style=\"width:25px\"></td>");
                s.Append("</tr>");
            }
            else if (table == "Issue Type")
            {
                s.Append("<tr>");
                s.Append("<th style=\"width:180px\">Issue Type</th>");
                s.Append("<th style=\"width:300px\">Description</th>");
                s.Append("<th style=\"width:25px\"></td>");
                s.Append("<th style=\"width:25px\"></td>");
                s.Append("</tr>");
            }

            TableHead.Text = s.ToString();
        }

        private void SetTableBody()
        {
            var table = Tables.Items[Tables.SelectedIndex].Value;

            if (table == "Activity")
                GetActivity();
            else if (table == "Budget Status")
                GetBudgetStatus();
            else if (table == "Country")
                GetCountry();
            else if (table == "Currency")
                GetCurrency();
            else if (table == "Incident Source")
                GetIncidentSource();
            else if (table == "Inherent Risk Category")
                GetInherentRiskCategory();
            else if (table == "Issue Source")
                GetIssueSource();
            else if (table == "Issue Type")
                GetIssueType();
            else if (table == "Party Entity Region")
                GetPartyEntityRegion();
            else if (table == "Region")
                GetRegion();
        }

        private string StandardFirstRow()
        {
            var s = new StringBuilder();

            s.Append("<tr data-id=\"0\">");
            s.Append("<td style=\"width:180px\"><input type=\"text\" class=\"form-control val\" maxlength=\"100\" /></td>");
            s.Append("<td class=\"center\" style=\"width:25px\"><span class=\"glyphicon glyphicon-floppy-disk text-primary pointer\" onclick=\"saveStandard(this)\"></span></td>");
            s.Append("<td class=\"center\" style=\"width:25px\"></td>");
            s.Append("</tr>");

            return s.ToString();
        }

        private string IssueTypeFirstRow()
        {
            var s = new StringBuilder();

            s.Append("<tr data-id=\"0\">");
            s.Append("<td style=\"width:180px\"><input type=\"text\" class=\"form-control val\" maxlength=\"100\" /></td>");
            s.Append("<td style=\"width:300px\"><textarea rows=\"2\" class=\"form-control other\"></textarea></td>");
            s.Append("<td class=\"center\" style=\"width:25px\"><span class=\"glyphicon glyphicon-floppy-disk text-primary pointer\" onclick=\"saveOther(this)\"></span></td>");
            s.Append("<td class=\"center\" style=\"width:25px\"></td>");
            s.Append("</tr>");

            return s.ToString();
        }

        private string CurrencyFirstRow()
        {
            var s = new StringBuilder();

            s.Append("<tr data-id=\"0\">");
            s.Append("<td style=\"width:70px\"><input type=\"text\" class=\"form-control other\" maxlength=\"4\"></td>");
            s.Append("<td style=\"width:180px\"><input type=\"text\" class=\"form-control val\" maxlength=\"100\" /></td>");
            s.Append("<td class=\"center\" style=\"width:25px\"><span class=\"glyphicon glyphicon-floppy-disk text-primary pointer\" onclick=\"saveOther(this)\"></span></td>");
            s.Append("<td class=\"center\" style=\"width:25px\"></td>");
            s.Append("</tr>");

            return s.ToString();
        }

        private string StandardRow(int id, string val, bool isDeleted)
        {
            var s = new StringBuilder();

            s.AppendFormat("<tr data-id=\"{0}\">", id);
            s.AppendFormat("<td style=\"width:180px\"><input type=\"text\" class=\"form-control val {0}\" maxlength=\"100\" value=\"{1}\" /></td>", isDeleted ? "item-deleted" : "", val);
            s.Append("<td class=\"center\" style=\"width:25px\"><span class=\"glyphicon glyphicon-floppy-disk text-primary pointer\" onclick=\"saveStandard(this)\"></span></td>");
            if (!isDeleted) s.Append("<td class=\"center\" style=\"width:25px\"><span class=\"glyphicon glyphicon-minus text-danger pointer\" onclick=\"saveStandard(this)\"></span></td>");
            else s.Append("<td class=\"center\" style=\"width:25px\"></td>");
            s.Append("</tr>");

            return s.ToString();
        }

        private string IssueTypeRow(int id, string val, string description, bool isDeleted)
        {
            var s = new StringBuilder();

            s.AppendFormat("<tr data-id=\"{0}\">", id);
            s.AppendFormat("<td style=\"width:180px\"><input type=\"text\" class=\"form-control val {0}\" maxlength=\"100\" value=\"{1}\" /></td>", isDeleted ? "item-deleted" : "", val);
            s.AppendFormat("<td style=\"width:300px\"><textarea rows=\"2\" class=\"form-control other {0}\" />{1}</textarea></td>", isDeleted ? "item-deleted" : "", description);
            s.Append("<td class=\"center\" style=\"width:25px\"><span class=\"glyphicon glyphicon-floppy-disk text-primary pointer\" onclick=\"saveOther(this)\"></span></td>");
            if (!isDeleted) s.Append("<td class=\"center\" style=\"width:25px\"><span class=\"glyphicon glyphicon-minus text-danger pointer\" onclick=\"saveOther(this)\"></span></td>");
            else s.Append("<td class=\"center\" style=\"width:25px\"></td>");
            s.Append("</tr>");

            return s.ToString();
        }

        private string CurrencyRow(int id, string val, string description, bool isDeleted)
        {
            var s = new StringBuilder();

            s.AppendFormat("<tr data-id=\"{0}\">", id);
            s.AppendFormat("<td style=\"width:70px\"><input type=\"text\" class=\"form-control other {0}\" maxlength=\"4\" value=\"{1}\" /></td>", isDeleted ? "item-deleted" : "", description);
            s.AppendFormat("<td style=\"width:180px\"><input type=\"text\" class=\"form-control val {0}\" maxlength=\"100\" value=\"{1}\" /></td>", isDeleted ? "item-deleted" : "", val);
            s.Append("<td class=\"center\" style=\"width:25px\"><span class=\"glyphicon glyphicon-floppy-disk text-primary pointer\" onclick=\"saveOther(this)\"></span></td>");
            if (!isDeleted) s.Append("<td class=\"center\" style=\"width:25px\"><span class=\"glyphicon glyphicon-minus text-danger pointer\" onclick=\"saveOther(this)\"></span></td>");
            else s.Append("<td class=\"center\" style=\"width:25px\"></td>");
            s.Append("</tr>");

            return s.ToString();
        }
        #endregion

        #region Get
        private void GetActivity()
        {
            using (var db = new OMEEntities())
            {
                var s = new StringBuilder();
                var dt = DateTime.UtcNow.AddSeconds(-10);

                var data = from act in db.ActivityNs
                           orderby act.ModifiedOn > dt ? 0 : 1, act.IsDeleted, act.Activity
                           select new
                           {
                               act.ActivityId,
                               act.Activity,
                               act.IsDeleted
                           };

                s.Append(StandardFirstRow());

                foreach (var t in data)
                {
                    s.Append(StandardRow(t.ActivityId, t.Activity, t.IsDeleted));
                }

                TableBody.Text = s.ToString();
            }
        }

        private void GetBudgetStatus()
        {
            using (var db = new OMEEntities())
            {
                var s = new StringBuilder();
                var dt = DateTime.UtcNow.AddSeconds(-10);

                var data = from bst in db.BudgetStatusNs
                           orderby bst.ModifiedOn > dt ? 0 : 1, bst.IsDeleted, bst.BudgetStatus
                           select new
                           {
                               bst.BudgetStatusId,
                               bst.BudgetStatus,
                               bst.IsDeleted
                           };

                s.Append(StandardFirstRow());

                foreach (var t in data)
                {
                    s.Append(StandardRow(t.BudgetStatusId, t.BudgetStatus, t.IsDeleted));
                }

                TableBody.Text = s.ToString();
            }
        }

        private void GetCountry()
        {
            using (var db = new OMEEntities())
            {
                var s = new StringBuilder();
                var dt = DateTime.UtcNow.AddSeconds(-10);

                var data = from cnt in db.CountryNs
                           orderby cnt.ModifiedOn > dt ? 0 : 1, cnt.IsDeleted, cnt.Country
                           select new
                           {
                               cnt.CountryId,
                               cnt.Country,
                               cnt.IsDeleted
                           };

                s.Append(StandardFirstRow());

                foreach (var t in data)
                {
                    s.Append(StandardRow(t.CountryId, t.Country, t.IsDeleted));
                }

                TableBody.Text = s.ToString();
            }
        }

        private void GetCurrency()
        {
            using (var db = new OMEEntities())
            {
                var s = new StringBuilder();
                var dt = DateTime.UtcNow.AddSeconds(-10);

                var data = from cur in db.CurrencyNs
                           orderby cur.ModifiedOn > dt ? 0 : 1, cur.IsDeleted, cur.Currency
                           select new
                           {
                               cur.CurrencyId,
                               cur.Code,
                               cur.Currency,
                               cur.IsDeleted
                           };

                s.Append(CurrencyFirstRow());

                foreach (var t in data)
                {
                    s.Append(CurrencyRow(t.CurrencyId, t.Currency, t.Code, t.IsDeleted));
                }

                TableBody.Text = s.ToString();
            }
        }

        private void GetIncidentSource()
        {
            using (var db = new OMEEntities())
            {
                var s = new StringBuilder();
                var dt = DateTime.UtcNow.AddSeconds(-10);

                var data = from ins in db.IncidentSourceNs
                           orderby ins.ModifiedOn > dt ? 0 : 1, ins.IsDeleted, ins.IncidentSource
                           select new
                           {
                               ins.IncidentSourceId,
                               ins.IncidentSource,
                               ins.IsDeleted
                           };

                s.Append(StandardFirstRow());

                foreach (var t in data)
                {
                    s.Append(StandardRow(t.IncidentSourceId, t.IncidentSource, t.IsDeleted));
                }

                TableBody.Text = s.ToString();
            }
        }

        private void GetInherentRiskCategory()
        {
            using (var db = new OMEEntities())
            {
                var s = new StringBuilder();
                var dt = DateTime.UtcNow.AddSeconds(-10);

                var data = from irc in db.InherentRiskCategoryNs
                           orderby irc.ModifiedOn > dt ? 0 : 1, irc.IsDeleted, irc.InherentRiskCategory
                           select new
                           {
                               irc.InherentRiskCategoryId,
                               irc.InherentRiskCategory,
                               irc.IsDeleted
                           };

                s.Append(StandardFirstRow());

                foreach (var t in data)
                {
                    s.Append(StandardRow(t.InherentRiskCategoryId, t.InherentRiskCategory, t.IsDeleted));
                }

                TableBody.Text = s.ToString();
            }
        }

        private void GetIssueSource()
        {
            using (var db = new OMEEntities())
            {
                var s = new StringBuilder();
                var dt = DateTime.UtcNow.AddSeconds(-10);

                var data = from iss in db.IssueSourceNs
                           orderby iss.ModifiedOn > dt ? 0 : 1, iss.IsDeleted, iss.IssueSource
                           select new
                           {
                               iss.IssueSourceId,
                               iss.IssueSource,
                               iss.IsDeleted
                           };

                s.Append(StandardFirstRow());

                foreach (var t in data)
                {
                    s.Append(StandardRow(t.IssueSourceId, t.IssueSource, t.IsDeleted));
                }

                TableBody.Text = s.ToString();
            }
        }

        private void GetIssueType()
        {
            using (var db = new OMEEntities())
            {
                var s = new StringBuilder();
                var dt = DateTime.UtcNow.AddSeconds(-10);

                var data = from ist in db.IssueTypeNs
                           orderby ist.ModifiedOn > dt ? 0 : 1, ist.IsDeleted, ist.IssueType
                           select new
                           {
                               ist.IssueTypeId,
                               ist.IssueType,
                               ist.Description,
                               ist.IsDeleted
                           };

                s.Append(IssueTypeFirstRow());

                foreach (var t in data)
                {
                    s.Append(IssueTypeRow(t.IssueTypeId, t.IssueType, t.Description, t.IsDeleted));
                }

                TableBody.Text = s.ToString();
            }
        }

        private void GetPartyEntityRegion()
        {
            using (var db = new OMEEntities())
            {
                var s = new StringBuilder();
                var dt = DateTime.UtcNow.AddSeconds(-10);

                var data = from per in db.PartyEntityRegionNs
                           orderby per.ModifiedOn > dt ? 0 : 1, per.IsDeleted, per.PartyEntityRegion
                           select new
                           {
                               per.PartyEntityRegionId,
                               per.PartyEntityRegion,
                               per.IsDeleted
                           };

                s.Append(StandardFirstRow());

                foreach (var t in data)
                {
                    s.Append(StandardRow(t.PartyEntityRegionId, t.PartyEntityRegion, t.IsDeleted));
                }

                TableBody.Text = s.ToString();
            }
        }

        private void GetRegion()
        {
            using (var db = new OMEEntities())
            {
                var s = new StringBuilder();
                var dt = DateTime.UtcNow.AddSeconds(-10);

                var data = from reg in db.RegionNs
                           orderby reg.ModifiedOn > dt ? 0 : 1, reg.IsDeleted, reg.Region
                           select new
                           {
                               reg.RegionId,
                               reg.Region,
                               reg.IsDeleted
                           };

                s.Append(StandardFirstRow());

                foreach (var t in data)
                {
                    s.Append(StandardRow(t.RegionId, t.Region, t.IsDeleted));
                }

                TableBody.Text = s.ToString();
            }
        }
        #endregion

        #region Save
        private void SaveStandard()
        {
            var ser = new JavaScriptSerializer();
            var item = new StandardItem();
            item = ser.Deserialize<StandardItem>(Value__.Value);
            item.Value = HttpUtility.UrlDecode(item.Value);

            if (item.Value.Length > 0)
            {
                if (item.Id == 0)
                {
                    if (item.Table == "Activity")
                    {
                        if (!ExistsActivity(0, item.Value))
                            CreateActivity(item);
                    }
                    else if (item.Table == "Budget Status")
                    {
                        if (!ExistsBudgetStatus(0, item.Value))
                            CreateBudgetStatus(item);
                    }
                    else if (item.Table == "Country")
                    {
                        if (!ExistsCountry(0, item.Value))
                            CreateCountry(item);
                    }
                    else if (item.Table == "Incident Source")
                    {
                        if (!ExistsIncidentSource(0, item.Value))
                            CreateIncidentSource(item);
                    }
                    else if (item.Table == "Inherent Risk Category")
                    {
                        if (!ExistsInherentRiskCategory(0, item.Value))
                            CreateInherentRiskCategory(item);
                    }
                    else if (item.Table == "Issue Source")
                    {
                        if (!ExistsIssueSource(0, item.Value))
                            CreateIssueSource(item);
                    }
                    else if (item.Table == "Party Entity Region")
                    {
                        if (!ExistsPartyEntityRegion(0, item.Value))
                            CreatePartyEntityRegion(item);
                    }
                    else if (item.Table == "Region")
                    {
                        if (!ExistsRegion(0, item.Value))
                            CreateRegion(item);
                    }
                }
                else
                {
                    if (item.Table == "Activity")
                    {
                        if (!ExistsActivity(item.Id, item.Value))
                            UpdateActivity(item);
                    }
                    else if (item.Table == "Budget Status")
                    {
                        if (!ExistsBudgetStatus(item.Id, item.Value))
                            UpdateBudgetStatus(item);
                    }
                    else if (item.Table == "Country")
                    {
                        if (!ExistsCountry(item.Id, item.Value))
                            UpdateCountry(item);
                    }
                    else if (item.Table == "Incident Source")
                    {
                        if (!ExistsIncidentSource(item.Id, item.Value))
                            UpdateIncidentSource(item);
                    }
                    else if (item.Table == "Inherent Risk Category")
                    {
                        if (!ExistsInherentRiskCategory(item.Id, item.Value))
                            UpdateInherentRiskCategory(item);
                    }
                    else if (item.Table == "Issue Source")
                    {
                        if (!ExistsIssueSource(item.Id, item.Value))
                            UpdateIssueSource(item);
                    }
                    else if (item.Table == "Party Entity Region")
                    {
                        if (!ExistsPartyEntityRegion(item.Id, item.Value))
                            UpdatePartyEntityRegion(item);
                    }
                    else if (item.Table == "Region")
                    {
                        if (!ExistsRegion(item.Id, item.Value))
                            UpdateRegion(item);
                    }
                }
            }

            SetTableHeader();
            SetTableBody();
        }

        private void SaveOther()
        {
            var ser = new JavaScriptSerializer();
            var item = new OtherItem();
            item = ser.Deserialize<OtherItem>(Value__.Value);
            item.Value = HttpUtility.UrlDecode(item.Value);
            item.Other = HttpUtility.UrlDecode(item.Other);

            if (item.Value.Length > 0)
            {
                if (item.Id == 0)
                {
                    if (item.Table == "Currency")
                    {
                        if (!ExistsCurrency(0, item.Other))
                            CreateCurrency(item);
                    }
                    else if (item.Table == "Issue Type")
                    {
                        if (!ExistsIssueType(0, item.Value))
                            CreateIssueType(item);
                    }
                }
                else
                {
                    if (item.Table == "Currency")
                    {
                        if (!ExistsCurrency(item.Id, item.Other))
                            UpdateCurrency(item);
                    }
                    else if (item.Table == "Issue Type")
                    {
                        if (!ExistsIssueType(item.Id, item.Value))
                            UpdateIssueType(item);
                    }
                }
            }

            SetTableHeader();
            SetTableBody();
        }
        #endregion

        #region Exists
        private bool ExistsActivity(int id, string val)
        {
            using (var db = new OMEEntities())
            {
                return db.ActivityNs.Any(x => x.ActivityId != id && x.Activity == val);
            }
        }

        private bool ExistsBudgetStatus(int id, string val)
        {
            using (var db = new OMEEntities())
            {
                return db.BudgetStatusNs.Any(x => x.BudgetStatusId != id && x.BudgetStatus == val);
            }
        }

        private bool ExistsCountry(int id, string val)
        {
            using (var db = new OMEEntities())
            {
                return db.CountryNs.Any(x => x.CountryId != id && x.Country == val);
            }
        }

        private bool ExistsCurrency(int id, string val)
        {
            using (var db = new OMEEntities())
            {
                return db.CurrencyNs.Any(x => x.CurrencyId != id && x.Code == val);
            }
        }

        private bool ExistsIncidentSource(int id, string val)
        {
            using (var db = new OMEEntities())
            {
                return db.IncidentSourceNs.Any(x => x.IncidentSourceId != id && x.IncidentSource == val);
            }
        }

        private bool ExistsInherentRiskCategory(int id, string val)
        {
            using (var db = new OMEEntities())
            {
                return db.InherentRiskCategoryNs.Any(x => x.InherentRiskCategoryId != id && x.InherentRiskCategory == val);
            }
        }

        private bool ExistsIssueSource(int id, string val)
        {
            using (var db = new OMEEntities())
            {
                return db.IssueSourceNs.Any(x => x.IssueSourceId != id && x.IssueSource == val);
            }
        }

        private bool ExistsIssueType(int id, string val)
        {
            using (var db = new OMEEntities())
            {
                return db.IssueTypeNs.Any(x => x.IssueTypeId != id && x.IssueType == val);
            }
        }

        private bool ExistsPartyEntityRegion(int id, string val)
        {
            using (var db = new OMEEntities())
            {
                return db.PartyEntityRegionNs.Any(x => x.PartyEntityRegionId != id && x.PartyEntityRegion == val);
            }
        }

        private bool ExistsRegion(int id, string val)
        {
            using (var db = new OMEEntities())
            {
                return db.RegionNs.Any(x => x.RegionId != id && x.Region == val);
            }
        }
        #endregion

        #region Create
        private void CreateActivity(StandardItem item)
        {
            using (var db = new OMEEntities())
            {
                var userId = GetUserId();

                var act = new ActivityN
                {
                    Activity = item.Value,
                    IsDeleted = false,
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = userId,
                    ModifiedOn = DateTime.UtcNow,
                    SysId = Guid.NewGuid()
                };

                db.ActivityNs.Add(act);

                db.SaveChanges();
            }
        }

        private void CreateBudgetStatus(StandardItem item)
        {
            using (var db = new OMEEntities())
            {
                var userId = GetUserId();

                var bgs = new BudgetStatusN
                {
                    BudgetStatus = item.Value,
                    IsDeleted = false,
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = userId,
                    ModifiedOn = DateTime.UtcNow,
                    SysId = Guid.NewGuid()
                };

                db.BudgetStatusNs.Add(bgs);

                db.SaveChanges();
            }
        }

        private void CreateCountry(StandardItem item)
        {
            using (var db = new OMEEntities())
            {
                var userId = GetUserId();

                var cnt = new CountryN
                {
                    Country = item.Value,
                    IsDeleted = false,
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = userId,
                    ModifiedOn = DateTime.UtcNow,
                    SysId = Guid.NewGuid()
                };

                db.CountryNs.Add(cnt);

                db.SaveChanges();
            }
        }

        private void CreateCurrency(OtherItem item)
        {
            using (var db = new OMEEntities())
            {
                var userId = GetUserId();

                var cur = new CurrencyN
                {
                    Code = item.Other,
                    Currency = item.Value,
                    IsDeleted = false,
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = userId,
                    ModifiedOn = DateTime.UtcNow,
                    SysId = Guid.NewGuid()
                };

                db.CurrencyNs.Add(cur);

                db.SaveChanges();
            }
        }

        private void CreateIncidentSource(StandardItem item)
        {
            using (var db = new OMEEntities())
            {
                var userId = GetUserId();

                var ins = new IncidentSourceN
                {
                    IncidentSource = item.Value,
                    IsDeleted = false,
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = userId,
                    ModifiedOn = DateTime.UtcNow,
                    SysId = Guid.NewGuid()
                };

                db.IncidentSourceNs.Add(ins);

                db.SaveChanges();
            }
        }

        private void CreateInherentRiskCategory(StandardItem item)
        {
            using (var db = new OMEEntities())
            {
                var userId = GetUserId();

                var ihc = new InherentRiskCategoryN
                {
                    InherentRiskCategory = item.Value,
                    IsDeleted = false,
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = userId,
                    ModifiedOn = DateTime.UtcNow,
                    SysId = Guid.NewGuid()
                };

                db.InherentRiskCategoryNs.Add(ihc);

                db.SaveChanges();
            }
        }

        private void CreateIssueSource(StandardItem item)
        {
            using (var db = new OMEEntities())
            {
                var userId = GetUserId();

                var iss = new IssueSourceN
                {
                    IssueSource = item.Value,
                    IsDeleted = false,
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = userId,
                    ModifiedOn = DateTime.UtcNow,
                    SysId = Guid.NewGuid()
                };

                db.IssueSourceNs.Add(iss);

                db.SaveChanges();
            }
        }

        private void CreateIssueType(OtherItem item)
        {
            using (var db = new OMEEntities())
            {
                var userId = GetUserId();

                var ist = new IssueTypeN
                {
                    IssueType = item.Value,
                    Description = item.Other,
                    IsDeleted = false,
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = userId,
                    ModifiedOn = DateTime.UtcNow,
                    SysId = Guid.NewGuid()
                };

                db.IssueTypeNs.Add(ist);

                db.SaveChanges();
            }
        }

        private void CreatePartyEntityRegion(StandardItem item)
        {
            using (var db = new OMEEntities())
            {
                var userId = GetUserId();

                var per = new PartyEntityRegionN
                {
                    PartyEntityRegion = item.Value,
                    IsDeleted = false,
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = userId,
                    ModifiedOn = DateTime.UtcNow,
                    SysId = Guid.NewGuid()
                };

                db.PartyEntityRegionNs.Add(per);

                db.SaveChanges();
            }
        }

        private void CreateRegion(StandardItem item)
        {
            using (var db = new OMEEntities())
            {
                var userId = GetUserId();

                var reg = new RegionN
                {
                    Region = item.Value,
                    IsDeleted = false,
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = userId,
                    ModifiedOn = DateTime.UtcNow,
                    SysId = Guid.NewGuid()
                };

                db.RegionNs.Add(reg);

                db.SaveChanges();
            }
        }
        #endregion

        #region Update
        private void UpdateActivity(StandardItem item)
        {
            using (var db = new OMEEntities())
            {
                var userId = GetUserId();

                var data = (from act in db.ActivityNs
                            where act.ActivityId == item.Id
                            select act).Single();

                data.Activity = item.Value;
                data.IsDeleted = item.IsDeleted;
                data.ModifiedBy = userId;
                data.ModifiedOn = DateTime.UtcNow;

                db.SaveChanges();
            }
        }

        private void UpdateBudgetStatus(StandardItem item)
        {
            using (var db = new OMEEntities())
            {
                var userId = GetUserId();

                var data = (from bgs in db.BudgetStatusNs
                            where bgs.BudgetStatusId == item.Id
                            select bgs).Single();

                data.BudgetStatus = item.Value;
                data.IsDeleted = item.IsDeleted;
                data.ModifiedBy = userId;
                data.ModifiedOn = DateTime.UtcNow;

                db.SaveChanges();
            }
        }

        private void UpdateCountry(StandardItem item)
        {
            using (var db = new OMEEntities())
            {
                var userId = GetUserId();

                var data = (from cnt in db.CountryNs
                            where cnt.CountryId == item.Id
                            select cnt).Single();

                data.Country = item.Value;
                data.IsDeleted = item.IsDeleted;
                data.ModifiedBy = userId;
                data.ModifiedOn = DateTime.UtcNow;

                db.SaveChanges();
            }
        }

        private void UpdateCurrency(OtherItem item)
        {
            using (var db = new OMEEntities())
            {
                var userId = GetUserId();

                var data = (from cur in db.CurrencyNs
                            where cur.CurrencyId == item.Id
                            select cur).Single();

                data.Currency = item.Value;
                data.Code = item.Other;
                data.IsDeleted = item.IsDeleted;
                data.ModifiedBy = userId;
                data.ModifiedOn = DateTime.UtcNow;

                db.SaveChanges();
            }
        }

        private void UpdateIncidentSource(StandardItem item)
        {
            using (var db = new OMEEntities())
            {
                var userId = GetUserId();

                var data = (from ins in db.IncidentSourceNs
                            where ins.IncidentSourceId == item.Id
                            select ins).Single();

                data.IncidentSource = item.Value;
                data.IsDeleted = item.IsDeleted;
                data.ModifiedBy = userId;
                data.ModifiedOn = DateTime.UtcNow;

                db.SaveChanges();
            }
        }

        private void UpdateInherentRiskCategory(StandardItem item)
        {
            using (var db = new OMEEntities())
            {
                var userId = GetUserId();

                var data = (from irc in db.InherentRiskCategoryNs
                            where irc.InherentRiskCategoryId == item.Id
                            select irc).Single();

                data.InherentRiskCategory = item.Value;
                data.IsDeleted = item.IsDeleted;
                data.ModifiedBy = userId;
                data.ModifiedOn = DateTime.UtcNow;

                db.SaveChanges();
            }
        }

        private void UpdateIssueSource(StandardItem item)
        {
            using (var db = new OMEEntities())
            {
                var userId = GetUserId();

                var data = (from iss in db.IssueSourceNs
                            where iss.IssueSourceId == item.Id
                            select iss).Single();

                data.IssueSource = item.Value;
                data.IsDeleted = item.IsDeleted;
                data.ModifiedBy = userId;
                data.ModifiedOn = DateTime.UtcNow;

                db.SaveChanges();
            }
        }

        private void UpdateIssueType(OtherItem item)
        {
            using (var db = new OMEEntities())
            {
                var userId = GetUserId();

                var data = (from ist in db.IssueTypeNs
                            where ist.IssueTypeId == item.Id
                            select ist).Single();

                data.IssueType = item.Value;
                data.Description = item.Other;
                data.IsDeleted = item.IsDeleted;
                data.ModifiedBy = userId;
                data.ModifiedOn = DateTime.UtcNow;

                db.SaveChanges();
            }
        }

        private void UpdatePartyEntityRegion(StandardItem item)
        {
            using (var db = new OMEEntities())
            {
                var userId = GetUserId();

                var data = (from per in db.PartyEntityRegionNs
                            where per.PartyEntityRegionId == item.Id
                            select per).Single();

                data.PartyEntityRegion = item.Value;
                data.IsDeleted = item.IsDeleted;
                data.ModifiedBy = userId;
                data.ModifiedOn = DateTime.UtcNow;

                db.SaveChanges();
            }
        }

        private void UpdateRegion(StandardItem item)
        {
            using (var db = new OMEEntities())
            {
                var userId = GetUserId();

                var data = (from reg in db.RegionNs
                            where reg.RegionId == item.Id
                            select reg).Single();

                data.Region = item.Value;
                data.IsDeleted = item.IsDeleted;
                data.ModifiedBy = userId;
                data.ModifiedOn = DateTime.UtcNow;

                db.SaveChanges();
            }
        }
        #endregion

        private int GetUserId()
        {
            return 1;
        }

        private List<TableDef> GetTableDefs()
        {
            var tableDefs = new List<TableDef>
            {
                new TableDef("Activity", true),
                new TableDef("Budget Status", true),
                new TableDef("Country", true),
                new TableDef("Currency", false),
                new TableDef("Incident Source", true),
                new TableDef("Inherent Risk Category", true),
                new TableDef("Issue Source", true),
                new TableDef("Issue Type", false),
                new TableDef("Party Entity Region", true),
                new TableDef("Region", true)
            };

            return tableDefs;
        }

        private class TableDef
        {
            public string Table;
            public bool Standard;

            public TableDef(string table, bool standard)
            {
                Table = table;
                Standard = standard;
            }
        }

        private class StandardItem
        {
            public string Table { get; set; }
            public int Id { get; set; }
            public string Value { get; set; }
            public bool IsDeleted { get; set; }
        }

        private class OtherItem
        {
            public string Table { get; set; }
            public int Id { get; set; }
            public string Value { get; set; }
            public string Other { get; set; }
            public bool IsDeleted { get; set; }
        }
    }
}