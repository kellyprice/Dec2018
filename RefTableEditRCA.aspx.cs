using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using Lab.Data0;

namespace Lab
{
    public partial class RefTableEditRCA : System.Web.UI.Page
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
                case "SaveICRA":
                    SaveICRA();
                    break;
                case "SaveTestingApproach":
                    SaveTestingApproach();
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

        #region SetHTML
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
            else if (table == "Control Type" || table == "Level of Automation" || table == "Reference Framework")
            {
                s.Append("<tr>");
                s.AppendFormat("<th style=\"width:180px\">{0}</th>", table);
                s.Append("<th class=\"center\" style=\"width:80px\">ICRA Score</th>");
                s.Append("<th style=\"width:25px\"></td>");
                s.Append("<th style=\"width:25px\"></td>");
                s.Append("</tr>");
            }
            else if (table == "Testing Approach")
            {
                s.Append("<tr>");
                s.Append("<th style=\"width:180px\">Testing Approach</th>");
                s.Append("<th class=\"center\" style=\"width:40px\">OET</th>");
                s.Append("<th class=\"center\" style=\"width:40px\">DET</th>");
                s.Append("<th style=\"width:25px\"></td>");
                s.Append("<th style=\"width:25px\"></td>");
                s.Append("</tr>");
            }

            TableHead.Text = s.ToString();
        }

        private void SetTableBody()
        {
            var table = Tables.Items[Tables.SelectedIndex].Value;

            if (table == "Allocation Type")
                GetAllocationType();
            else if (table == "Control Type")
                GetControlType();
            else if (table == "Frequency")
                GetFrequency();
            else if (table == "Level of Automation")
                GetLevelOfAutomation();
            else if (table == "Line of Defense")
                GetLineOfDefence();
            else if (table == "Objective Category")
                GetObjectiveCategory();
            else if (table == "Reference Framework")
                GetReferenceFramework();
            else if (table == "Reference Workstream")
                GetReferenceWorkstream();
            else if (table == "Scope Region")
                GetScopeRegion();
            else if (table == "Testing Approach")
                GetTestingApproach();
            else if (table == "Testing Phase")
                GetTestingPhase();
            else if (table == "Trade Lifecycle")
                GetTradeLifecycle();
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

        private string TestingApproachFirstRow()
        {
            var s = new StringBuilder();

            s.Append("<tr data-id=\"0\">");
            s.Append("<td style=\"width:180px\"><input type=\"text\" class=\"form-control val\" maxlength=\"100\" /></td>");
            s.Append("<td class=\"center\" style=\"width:25px\"><span class=\"glyphicon glyphicon-ok text-verymuted oet pointer\" onclick=\"toggleApproach(this)\"></span></td>");
            s.Append("<td class=\"center\" style=\"width:25px\"><span class=\"glyphicon glyphicon-ok text-verymuted det pointer\" onclick=\"toggleApproach(this)\"></span></td>");
            s.Append("<td class=\"center\" style=\"width:25px\"><span class=\"glyphicon glyphicon-floppy-disk text-primary pointer\" onclick=\"saveTestingApproach(this)\"></span></td>");
            s.Append("<td class=\"center\" style=\"width:25px\"></td>");
            s.Append("</tr>");

            return s.ToString();
        }

        private string ICRAFirstRow()
        {
            var s = new StringBuilder();

            s.Append("<tr data-id=\"0\">");
            s.Append("<td style=\"width:180px\"><input type=\"text\" class=\"form-control val\" maxlength=\"100\" /></td>");
            s.Append("<td style=\"width:80px\"><input type=\"text\" class=\"form-control icra\" /></td>");
            s.Append("<td class=\"center\" style=\"width:25px\"><span class=\"glyphicon glyphicon-floppy-disk text-primary pointer\" onclick=\"saveICRA(this)\"></span></td>");
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

        private string TestingApproachRow(int id, string val, bool oet, bool det, bool isDeleted)
        {
            var s = new StringBuilder();

            s.AppendFormat("<tr data-id=\"{0}\">", id);
            s.AppendFormat("<td style=\"width:180px\"><input type=\"text\" class=\"form-control val {0}\" maxlength=\"100\" value=\"{1}\" /></td>", isDeleted ? "item-deleted" : "", val);
            s.AppendFormat("<td class=\"center\" style=\"width:25px\"><span class=\"glyphicon glyphicon-ok text-{0} oet pointer\" onclick=\"toggleApproach(this)\"></span></td>", (oet ? "success" : "verymuted"));
            s.AppendFormat("<td class=\"center\" style=\"width:25px\"><span class=\"glyphicon glyphicon-ok text-{0} det pointer\" onclick=\"toggleApproach(this)\"></span></td>", (det ? "success" : "verymuted"));
            s.Append("<td class=\"center\" style=\"width:25px\"><span class=\"glyphicon glyphicon-floppy-disk text-primary pointer\" onclick=\"saveTestingApproach(this)\"></span></td>");
            if (!isDeleted) s.Append("<td class=\"center\" style=\"width:25px\"><span class=\"glyphicon glyphicon-minus text-danger pointer\" onclick=\"saveTestingApproach(this)\"></span></td>");
            else s.Append("<td class=\"center\" style=\"width:25px\"></td>");
            s.Append("</tr>");

            return s.ToString();
        }

        private string ICRARow(int id, string val, int? icra, bool isDeleted)
        {
            var s = new StringBuilder();

            s.AppendFormat("<tr data-id=\"{0}\">", id);
            s.AppendFormat("<td style=\"width:180px\"><input type=\"text\" class=\"form-control val {0}\" maxlength=\"100\" value=\"{1}\" /></td>", isDeleted ? "item-deleted" : "", val);
            s.AppendFormat("<td style=\"width:80px\"><input type=\"text\" class=\"form-control icra {0}\" value=\"{1}\" /></td>", isDeleted ? "item-deleted" : "", icra != null ? icra.ToString() : "");
            s.Append("<td class=\"center\" style=\"width:25px\"><span class=\"glyphicon glyphicon-floppy-disk text-primary pointer\" onclick=\"saveICRA(this)\"></span></td>");
            if (!isDeleted) s.Append("<td class=\"center\" style=\"width:25px\"><span class=\"glyphicon glyphicon-minus text-danger pointer\" onclick=\"saveICRA(this)\"></span></td>");
            else s.Append("<td class=\"center\" style=\"width:25px\"></td>");
            s.Append("</tr>");

            return s.ToString();
        }
        #endregion

        #region Get
        private void GetAllocationType()
        {
            using (var db = new RCAEntities())
            {
                var s = new StringBuilder();
                var dt = DateTime.UtcNow.AddSeconds(-10);

                var data = from alt in db.ctpAllocationTypes
                           orderby alt.ModifiedOn > dt ? 0 : 1, alt.IsDeleted, alt.AllocationType
                           select new
                           {
                               alt.AllocationTypeId,
                               alt.AllocationType,
                               alt.IsDeleted
                           };

                s.Append(StandardFirstRow());

                foreach (var t in data)
                {
                    s.Append(StandardRow(t.AllocationTypeId, t.AllocationType, t.IsDeleted));
                }

                TableBody.Text = s.ToString();
            }
        }

        private void GetControlType()
        {
            using (var db = new RCAEntities())
            {
                var s = new StringBuilder();
                var dt = DateTime.UtcNow.AddSeconds(-10);
                var icraScores = GetICRAScores(3);

                var data = from ctt in db.ctpControlTypes
                           orderby ctt.ModifiedOn > dt ? 0 : 1, ctt.IsDeleted, ctt.ControlType
                           select new
                           {
                               ctt.ControlTypeId,
                               ctt.ControlType,
                               ctt.IsDeleted
                           };

                s.Append(ICRAFirstRow());

                foreach (var t in data)
                {
                    var icraScore = icraScores.Any(x => x.Key == t.ControlType) ?
                        (int?)icraScores[t.ControlType] : null;

                    s.Append(ICRARow(t.ControlTypeId, t.ControlType, icraScore, t.IsDeleted));
                }

                TableBody.Text = s.ToString();
            }
        }

        private void GetFrequency()
        {
            using (var db = new RCAEntities())
            {
                var s = new StringBuilder();
                var dt = DateTime.UtcNow.AddSeconds(-10);

                var data = from frq in db.ctpFrequencies
                           orderby frq.ModifiedOn > dt ? 0 : 1, frq.IsDeleted, frq.Frequency
                           select new
                           {
                               frq.FrequencyId,
                               frq.Frequency,
                               frq.IsDeleted
                           };

                s.Append(StandardFirstRow());

                foreach (var t in data)
                {
                    s.Append(StandardRow(t.FrequencyId, t.Frequency, t.IsDeleted));
                }

                TableBody.Text = s.ToString();
            }
        }

        private void GetLevelOfAutomation()
        {
            using (var db = new RCAEntities())
            {
                var s = new StringBuilder();
                var dt = DateTime.UtcNow.AddSeconds(-10);
                var icraScores = GetICRAScores(2);

                var data = from loa in db.ctpLevelOfAutomations
                           orderby loa.ModifiedOn > dt ? 0 : 1, loa.IsDeleted, loa.LevelOfAutomation
                           select new
                           {
                               loa.LevelOfAutomationId,
                               loa.LevelOfAutomation,
                               loa.IsDeleted
                           };

                s.Append(ICRAFirstRow());

                foreach (var t in data)
                {
                    var icraScore = icraScores.Any(x => x.Key == t.LevelOfAutomation) ?
                        (int?)icraScores[t.LevelOfAutomation] : null;

                    s.Append(ICRARow(t.LevelOfAutomationId, t.LevelOfAutomation, icraScore, t.IsDeleted));
                }

                TableBody.Text = s.ToString();
            }
        }

        private void GetLineOfDefence()
        {
            using (var db = new RCAEntities())
            {
                var s = new StringBuilder();
                var dt = DateTime.UtcNow.AddSeconds(-10);

                var data = from lod in db.ctpLineOfDefenses
                           orderby lod.ModifiedOn > dt ? 0 : 1, lod.IsDeleted, lod.LineOfDefense
                           select new
                           {
                               lod.LineOfDefenseId,
                               lod.LineOfDefense,
                               lod.IsDeleted
                           };

                s.Append(StandardFirstRow());

                foreach (var t in data)
                {
                    s.Append(StandardRow(t.LineOfDefenseId, t.LineOfDefense, t.IsDeleted));
                }

                TableBody.Text = s.ToString();
            }
        }

        private void GetObjectiveCategory()
        {
            using (var db = new RCAEntities())
            {
                var s = new StringBuilder();
                var dt = DateTime.UtcNow.AddSeconds(-10);

                var data = from obc in db.ctpObjectiveCategories
                           orderby obc.ModifiedOn > dt ? 0 : 1, obc.IsDeleted, obc.ObjectiveCategory
                           select new
                           {
                               obc.ObjectiveCategoryId,
                               obc.ObjectiveCategory,
                               obc.IsDeleted
                           };

                s.Append(StandardFirstRow());

                foreach (var t in data)
                {
                    s.Append(StandardRow(t.ObjectiveCategoryId, t.ObjectiveCategory, t.IsDeleted));
                }

                TableBody.Text = s.ToString();
            }
        }

        private void GetReferenceFramework()
        {
            using (var db = new RCAEntities())
            {
                var s = new StringBuilder();
                var dt = DateTime.UtcNow.AddSeconds(-10);
                var icraScores = GetICRAScores(1);

                var data = from rff in db.ctpReferenceFrameworks
                           orderby rff.ModifiedOn > dt ? 0 : 1, rff.IsDeleted, rff.ReferenceFramework
                           select new
                           {
                               rff.ReferenceFrameworkId,
                               rff.ReferenceFramework,
                               rff.IsDeleted
                           };

                s.Append(ICRAFirstRow());

                foreach (var t in data)
                {
                    var icraScore = icraScores.Any(x => x.Key == t.ReferenceFramework) ?
                        (int?)icraScores[t.ReferenceFramework] : null;

                    s.Append(ICRARow(t.ReferenceFrameworkId, t.ReferenceFramework, icraScore, t.IsDeleted));
                }

                TableBody.Text = s.ToString();
            }
        }

        private void GetReferenceWorkstream()
        {
            using (var db = new RCAEntities())
            {
                var s = new StringBuilder();
                var dt = DateTime.UtcNow.AddSeconds(-10);

                var data = from rfw in db.ctpReferenceWorkstreams
                           orderby rfw.ModifiedOn > dt ? 0 : 1, rfw.IsDeleted, rfw.ReferenceWorkstream
                           select new
                           {
                               rfw.ReferenceWorkstreamId,
                               rfw.ReferenceWorkstream,
                               rfw.IsDeleted
                           };

                s.Append(StandardFirstRow());

                foreach (var t in data)
                {
                    s.Append(StandardRow(t.ReferenceWorkstreamId, t.ReferenceWorkstream, t.IsDeleted));
                }

                TableBody.Text = s.ToString();
            }
        }

        private void GetScopeRegion()
        {
            using (var db = new RCAEntities())
            {
                var s = new StringBuilder();
                var dt = DateTime.UtcNow.AddSeconds(-10);

                var data = from scr in db.ctpScopeRegions
                           orderby scr.ModifiedOn > dt ? 0 : 1, scr.IsDeleted, scr.ScopeRegion
                           select new
                           {
                               scr.ScopeRegionId,
                               scr.ScopeRegion,
                               scr.IsDeleted
                           };

                s.Append(StandardFirstRow());

                foreach (var t in data)
                {
                    s.Append(StandardRow(t.ScopeRegionId, t.ScopeRegion, t.IsDeleted));
                }

                TableBody.Text = s.ToString();
            }
        }

        private void GetTestingApproach()
        {
            using (var db = new RCAEntities())
            {
                var s = new StringBuilder();
                var dt = DateTime.UtcNow.AddSeconds(-10);

                var data = from tea in db.ctpTestingApproaches
                           orderby tea.ModifiedOn > dt ? 0 : 1, tea.IsDeleted, tea.TestingApproach
                           select new
                           {
                               tea.TestingApproachId,
                               tea.TestingApproach,
                               tea.OET,
                               tea.DET,
                               tea.IsDeleted
                           };

                s.Append(TestingApproachFirstRow());

                foreach (var t in data)
                {
                    s.Append(TestingApproachRow(t.TestingApproachId, t.TestingApproach, t.OET, t.DET, t.IsDeleted));
                }

                TableBody.Text = s.ToString();
            }
        }

        private void GetTestingPhase()
        {
            using (var db = new RCAEntities())
            {
                var s = new StringBuilder();
                var dt = DateTime.UtcNow.AddSeconds(-10);

                var data = from tep in db.ctpTestingPhases
                           orderby tep.ModifiedOn > dt ? 0 : 1, tep.IsDeleted, tep.TestingPhase
                           select new
                           {
                               tep.TestingPhaseId,
                               tep.TestingPhase,
                               tep.IsDeleted
                           };

                s.Append(StandardFirstRow());

                foreach (var t in data)
                {
                    s.Append(StandardRow(t.TestingPhaseId, t.TestingPhase, t.IsDeleted));
                }

                TableBody.Text = s.ToString();
            }
        }

        private void GetTradeLifecycle()
        {
            using (var db = new RCAEntities())
            {
                var s = new StringBuilder();
                var dt = DateTime.UtcNow.AddSeconds(-10);

                var data = from tlc in db.ctpTradeLifecycles
                           orderby tlc.ModifiedOn > dt ? 0 : 1, tlc.IsDeleted, tlc.TradeLifecycle
                           select new
                           {
                               tlc.TradeLifecycleId,
                               tlc.TradeLifecycle,
                               tlc.IsDeleted
                           };

                s.Append(StandardFirstRow());

                foreach (var t in data)
                {
                    s.Append(StandardRow(t.TradeLifecycleId, t.TradeLifecycle, t.IsDeleted));
                }

                TableBody.Text = s.ToString();
            }
        }

        private Dictionary<string, int> GetICRAScores(int question)
        {
            using (var db = new RCAEntities())
            {
                var icraScores = new Dictionary<string, int>();

                var data = from ics in db.ctpICRAScores
                           where ics.Question == question
                           select new
                           {
                               ics.Answer,
                               ics.Score
                           };

                foreach (var t in data)
                {
                    icraScores.Add(t.Answer, t.Score);
                }

                return icraScores;
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
                    if (item.Table == "Allocation Type")
                    {
                        if (!ExistsAllocationType(0, item.Value))
                            CreateAllocationType(item);
                    }
                    else if (item.Table == "Frequency")
                    {
                        if (!ExistsFrequency(0, item.Value))
                            CreateFrequency(item);
                    }
                    else if (item.Table == "Line of Defense")
                    {
                        if (!ExistsLineOfDefense(0, item.Value))
                            CreateLineOfDefense(item);
                    }
                    else if (item.Table == "Objective Category")
                    {
                        if (!ExistsObjectiveCategory(0, item.Value))
                            CreateObjectiveCategory(item);
                    }
                    else if (item.Table == "Reference Workstream")
                    {
                        if (!ExistsReferenceWorkstream(0, item.Value))
                            CreateReferenceWorkstream(item);
                    }
                    else if (item.Table == "Scope Region")
                    {
                        if (!ExistsScopeRegion(0, item.Value))
                            CreateScopeRegion(item);
                    }
                    else if (item.Table == "Testing Phase")
                    {
                        if (!ExistsTestingPhase(0, item.Value))
                            CreateTestingPhase(item);
                    }
                    else if (item.Table == "Trade Lifecycle")
                    {
                        if (!ExistsTradeLifecycle(0, item.Value))
                            CreateTradeLifecycle(item);
                    }
                }
                else
                {
                    if (item.Table == "Allocation Type")
                    {
                        if (!ExistsAllocationType(item.Id, item.Value))
                            UpdateAllocationType(item);
                    }
                    else if (item.Table == "Frequency")
                    {
                        if (!ExistsFrequency(item.Id, item.Value))
                            UpdateFrequency(item);
                    }
                    else if (item.Table == "Line of Defense")
                    {
                        if (!ExistsLineOfDefense(item.Id, item.Value))
                            UpdateLineOfDefense(item);
                    }
                    else if (item.Table == "Objective Category")
                    {
                        if (!ExistsObjectiveCategory(item.Id, item.Value))
                            UpdateObjectiveCategory(item);
                    }
                    else if (item.Table == "Reference Workstream")
                    {
                        if (!ExistsReferenceWorkstream(item.Id, item.Value))
                            UpdateReferenceWorkstream(item);
                    }
                    else if (item.Table == "Scope Region")
                    {
                        if (!ExistsScopeRegion(item.Id, item.Value))
                            UpdateScopeRegion(item);
                    }
                    else if (item.Table == "Testing Phase")
                    {
                        if (!ExistsTestingPhase(item.Id, item.Value))
                            UpdateTestingPhase(item);
                    }
                    else if (item.Table == "Trade Lifecycle")
                    {
                        if (!ExistsTradeLifecycle(item.Id, item.Value))
                            UpdateTradeLifecycle(item);
                    }
                }
            }

            SetTableHeader();
            SetTableBody();
        }

        private void SaveICRA()
        {
            var ser = new JavaScriptSerializer();
            var item = new ICRAItem();
            item = ser.Deserialize<ICRAItem>(Value__.Value);
            item.Value = HttpUtility.UrlDecode(item.Value);

            if (item.Value.Length > 0)
            {
                if (item.Id == 0)
                {
                    if (item.Table == "Reference Framework")
                    {
                        if (!ExistsReferenceFramework(0, item.Value))
                        {
                            CreateReferenceFramework(item);

                            if (ExistsICRA(1, item.Value))
                            {
                                if (item.ICRA < 1)
                                    DeleteICRA(1, item.Value);
                                else
                                    UpdateICRA(1, item.Value, item.ICRA);
                            }
                            else if (item.ICRA > 0)
                                CreateICRA(1, item.Value, item.ICRA);
                        }
                    }
                    else if (item.Table == "Level of Automation")
                    {
                        if (!ExistsLevelOfAutomation(0, item.Value))
                        {
                            CreateLevelOfAutomation(item);

                            if (ExistsICRA(2, item.Value))
                            {
                                if (item.ICRA < 1)
                                    DeleteICRA(2, item.Value);
                                else
                                    UpdateICRA(2, item.Value, item.ICRA);
                            }
                            else if (item.ICRA > 0)
                                CreateICRA(2, item.Value, item.ICRA);
                        }
                    }
                    else if (item.Table == "Control Type")
                    {
                        if (!ExistsControlType(0, item.Value))
                        {
                            CreateControlType(item);

                            if (ExistsICRA(3, item.Value))
                            {
                                if (item.ICRA < 1)
                                    DeleteICRA(3, item.Value);
                                else
                                    UpdateICRA(3, item.Value, item.ICRA);
                            }
                            else if (item.ICRA > 0)
                                CreateICRA(3, item.Value, item.ICRA);
                        }
                    }
                }
                else
                {
                    if (item.Table == "Reference Framework")
                    {
                        if (!ExistsReferenceFramework(item.Id, item.Value))
                        {
                            UpdateReferenceFramework(item);

                            if (ExistsICRA(1, item.Value))
                            {
                                if (item.ICRA < 1)
                                    DeleteICRA(1, item.Value);
                                else
                                    UpdateICRA(1, item.Value, item.ICRA);
                            }
                            else if (item.ICRA > 0)
                                CreateICRA(1, item.Value, item.ICRA);
                        }
                    }
                    else if (item.Table == "Level of Automation")
                    {
                        if (!ExistsLevelOfAutomation(item.Id, item.Value))
                        {
                            UpdateLevelOfAutomation(item);

                            if (ExistsICRA(2, item.Value))
                            {
                                if (item.ICRA < 1)
                                    DeleteICRA(2, item.Value);
                                else
                                    UpdateICRA(2, item.Value, item.ICRA);
                            }
                            else if (item.ICRA > 0)
                                CreateICRA(2, item.Value, item.ICRA);
                        }
                    }
                    else if (item.Table == "Control Type")
                    {
                        if (!ExistsControlType(item.Id, item.Value))
                        {
                            UpdateControlType(item);

                            if (ExistsICRA(3, item.Value))
                            {
                                if (item.ICRA < 1)
                                    DeleteICRA(3, item.Value);
                                else
                                    UpdateICRA(3, item.Value, item.ICRA);
                            }
                            else if (item.ICRA > 0)
                                CreateICRA(3, item.Value, item.ICRA);
                        }
                    }
                }
            }

            SetTableHeader();
            SetTableBody();
        }

        private void SaveTestingApproach()
        {
            var ser = new JavaScriptSerializer();
            var item = new TestingApproachItem();
            item = ser.Deserialize<TestingApproachItem>(Value__.Value);
            item.Value = HttpUtility.UrlDecode(item.Value);

            if (item.Value.Length > 0)
            {
                if (item.Id == 0)
                {
                    if (!ExistsTestingApproach(0, item.Value))
                        CreateTestingApproach(item);
                }
                else
                {
                    if (!ExistsTestingApproach(item.Id, item.Value))
                        UpdateTestingApproach(item);
                }
            }

            SetTableHeader();
            SetTableBody();
        }
        #endregion

        #region Exists
        private bool ExistsAllocationType(int id, string val)
        {
            using (var db = new RCAEntities())
            {
                return db.ctpAllocationTypes.Any(x => x.AllocationTypeId != id && x.AllocationType == val);
            }
        }

        private bool ExistsControlType(int id, string val)
        {
            using (var db = new RCAEntities())
            {
                return db.ctpControlTypes.Any(x => x.ControlTypeId != id && x.ControlType == val);
            }
        }

        private bool ExistsICRA(int question, string val)
        {
            using (var db = new RCAEntities())
            {
                return db.ctpICRAScores.Any(x => x.Answer == val && x.Question == question);
            }
        }

        private bool ExistsFrequency(int id, string val)
        {
            using (var db = new RCAEntities())
            {
                return db.ctpFrequencies.Any(x => x.FrequencyId != id && x.Frequency == val);
            }
        }

        private bool ExistsLevelOfAutomation(int id, string val)
        {
            using (var db = new RCAEntities())
            {
                return db.ctpLevelOfAutomations.Any(x => x.LevelOfAutomationId != id && x.LevelOfAutomation == val);
            }
        }

        private bool ExistsLineOfDefense(int id, string val)
        {
            using (var db = new RCAEntities())
            {
                return db.ctpLineOfDefenses.Any(x => x.LineOfDefenseId != id && x.LineOfDefense == val);
            }
        }

        private bool ExistsObjectiveCategory(int id, string val)
        {
            using (var db = new RCAEntities())
            {
                return db.ctpObjectiveCategories.Any(x => x.ObjectiveCategoryId != id && x.ObjectiveCategory == val);
            }
        }

        private bool ExistsReferenceFramework(int id, string val)
        {
            using (var db = new RCAEntities())
            {
                return db.ctpReferenceFrameworks.Any(x => x.ReferenceFrameworkId != id && x.ReferenceFramework == val);
            }
        }

        private bool ExistsReferenceWorkstream(int id, string val)
        {
            using (var db = new RCAEntities())
            {
                return db.ctpReferenceWorkstreams.Any(x => x.ReferenceWorkstreamId != id && x.ReferenceWorkstream == val);
            }
        }

        private bool ExistsScopeRegion(int id, string val)
        {
            using (var db = new RCAEntities())
            {
                return db.ctpScopeRegions.Any(x => x.ScopeRegionId != id && x.ScopeRegion == val);
            }
        }

        private bool ExistsTestingApproach(int id, string val)
        {
            using (var db = new RCAEntities())
            {
                return db.ctpTestingApproaches.Any(x => x.TestingApproachId != id && x.TestingApproach == val);
            }
        }

        private bool ExistsTestingPhase(int id, string val)
        {
            using (var db = new RCAEntities())
            {
                return db.ctpTestingPhases.Any(x => x.TestingPhaseId != id && x.TestingPhase == val);
            }

        }

        private bool ExistsTradeLifecycle(int id, string val)
        {
            using (var db = new RCAEntities())
            {
                return db.ctpTradeLifecycles.Any(x => x.TradeLifecycleId != id && x.TradeLifecycle == val);
            }
        }
        #endregion

        #region Create
        private void CreateAllocationType(StandardItem item)
        {
            using (var db = new RCAEntities())
            {
                var userId = GetUserId();

                var alt = new ctpAllocationType
                {
                    AllocationType = item.Value,
                    IsDeleted = false,
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = userId,
                    ModifiedOn = DateTime.UtcNow,
                    SysId = Guid.NewGuid()
                };

                db.ctpAllocationTypes.Add(alt);

                db.SaveChanges();
            }
        }

        private void CreateControlType(ICRAItem item)
        {
            using (var db = new RCAEntities())
            {
                var userId = GetUserId();

                var ctt = new ctpControlType
                {
                    ControlType = item.Value,
                    IsDeleted = false,
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = userId,
                    ModifiedOn = DateTime.UtcNow,
                    SysId = Guid.NewGuid()
                };

                db.ctpControlTypes.Add(ctt);

                db.SaveChanges();
            }
        }

        private void CreateICRA(int question, string val, int score)
        {
            using (var db = new RCAEntities())
            {
                var ics = new ctpICRAScore
                {
                    Question = question,
                    Answer = val,
                    Score = score
                };

                db.ctpICRAScores.Add(ics);

                db.SaveChanges();
            }
        }

        private void CreateFrequency(StandardItem item)
        {
            using (var db = new RCAEntities())
            {
                var userId = GetUserId();

                var frq = new ctpFrequency
                {
                    Frequency = item.Value,
                    IsDeleted = false,
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = userId,
                    ModifiedOn = DateTime.UtcNow,
                    SysId = Guid.NewGuid()
                };

                db.ctpFrequencies.Add(frq);

                db.SaveChanges();
            }
        }

        private void CreateLevelOfAutomation(ICRAItem item)
        {
            using (var db = new RCAEntities())
            {
                var userId = GetUserId();

                var loa = new ctpLevelOfAutomation
                {
                    LevelOfAutomation = item.Value,
                    IsDeleted = false,
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = userId,
                    ModifiedOn = DateTime.UtcNow,
                    SysId = Guid.NewGuid()
                };

                db.ctpLevelOfAutomations.Add(loa);

                db.SaveChanges();
            }
        }

        private void CreateLineOfDefense(StandardItem item)
        {
            using (var db = new RCAEntities())
            {
                var userId = GetUserId();

                var lod = new ctpLineOfDefense
                {
                    LineOfDefense = item.Value,
                    IsDeleted = false,
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = userId,
                    ModifiedOn = DateTime.UtcNow,
                    SysId = Guid.NewGuid()
                };

                db.ctpLineOfDefenses.Add(lod);

                db.SaveChanges();
            }
        }

        private void CreateObjectiveCategory(StandardItem item)
        {
            using (var db = new RCAEntities())
            {
                var userId = GetUserId();

                var ojc = new ctpObjectiveCategory
                {
                    ObjectiveCategory = item.Value,
                    IsDeleted = false,
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = userId,
                    ModifiedOn = DateTime.UtcNow,
                    SysId = Guid.NewGuid()
                };

                db.ctpObjectiveCategories.Add(ojc);

                db.SaveChanges();
            }
        }

        private void CreateReferenceFramework(ICRAItem item)
        {
            using (var db = new RCAEntities())
            {
                var userId = GetUserId();

                var rfw = new ctpReferenceFramework
                {
                    ReferenceFramework = item.Value,
                    IsDeleted = false,
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = userId,
                    ModifiedOn = DateTime.UtcNow,
                    SysId = Guid.NewGuid()
                };

                db.ctpReferenceFrameworks.Add(rfw);

                db.SaveChanges();
            }
        }

        private void CreateReferenceWorkstream(StandardItem item)
        {
            using (var db = new RCAEntities())
            {
                var userId = GetUserId();

                var rws = new ctpReferenceWorkstream
                {
                    ReferenceWorkstream = item.Value,
                    IsDeleted = false,
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = userId,
                    ModifiedOn = DateTime.UtcNow,
                    SysId = Guid.NewGuid()
                };

                db.ctpReferenceWorkstreams.Add(rws);

                db.SaveChanges();
            }
        }

        private void CreateScopeRegion(StandardItem item)
        {
            using (var db = new RCAEntities())
            {
                var userId = GetUserId();

                var scr = new ctpScopeRegion
                {
                    ScopeRegion = item.Value,
                    IsDeleted = false,
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = userId,
                    ModifiedOn = DateTime.UtcNow,
                    SysId = Guid.NewGuid()
                };

                db.ctpScopeRegions.Add(scr);

                db.SaveChanges();
            }
        }

        private void CreateTestingApproach(TestingApproachItem item)
        {
            using (var db = new RCAEntities())
            {
                var userId = GetUserId();

                var tea = new ctpTestingApproach()
                {
                    TestingApproach = item.Value,
                    OET = item.OET,
                    DET = item.DET,
                    IsDeleted = false,
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = userId,
                    ModifiedOn = DateTime.UtcNow,
                    SysId = Guid.NewGuid()
                };

                db.ctpTestingApproaches.Add(tea);

                db.SaveChanges();
            }
        }

        private void CreateTestingPhase(StandardItem item)
        {
            using (var db = new RCAEntities())
            {
                var userId = GetUserId();

                var tep = new ctpTestingPhase
                {
                    TestingPhase = item.Value,
                    IsDeleted = false,
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = userId,
                    ModifiedOn = DateTime.UtcNow,
                    SysId = Guid.NewGuid()
                };

                db.ctpTestingPhases.Add(tep);

                db.SaveChanges();
            }
        }

        private void CreateTradeLifecycle(StandardItem item)
        {
            using (var db = new RCAEntities())
            {
                var userId = GetUserId();

                var tlc = new ctpTradeLifecycle
                {
                    TradeLifecycle = item.Value,
                    IsDeleted = false,
                    CreatedBy = userId,
                    CreatedOn = DateTime.UtcNow,
                    ModifiedBy = userId,
                    ModifiedOn = DateTime.UtcNow,
                    SysId = Guid.NewGuid()
                };

                db.ctpTradeLifecycles.Add(tlc);

                db.SaveChanges();
            }
        }
        #endregion

        #region Update
        private void UpdateAllocationType(StandardItem item)
        {
            using (var db = new RCAEntities())
            {
                var userId = GetUserId();

                var data = (from alt in db.ctpAllocationTypes
                            where alt.AllocationTypeId == item.Id
                            select alt).Single();

                data.AllocationType = item.Value;
                data.IsDeleted = item.IsDeleted;
                data.ModifiedBy = userId;
                data.ModifiedOn = DateTime.UtcNow;

                db.SaveChanges();
            }
        }

        private void UpdateControlType(ICRAItem item)
        {
            using (var db = new RCAEntities())
            {
                var userId = GetUserId();

                var data = (from ctt in db.ctpControlTypes
                            where ctt.ControlTypeId == item.Id
                            select ctt).Single();

                data.ControlType = item.Value;
                data.IsDeleted = item.IsDeleted;
                data.ModifiedBy = userId;
                data.ModifiedOn = DateTime.UtcNow;

                db.SaveChanges();
            }
        }

        private void UpdateICRA(int question, string val, int score)
        {
            using (var db = new RCAEntities())
            {
                var userId = GetUserId();

                var data = from ics in db.ctpICRAScores
                           where ics.Question == question && ics.Answer == val
                           select ics;

                foreach (var t in data)
                {
                    t.Score = score;
                }

                db.SaveChanges();
            }
        }

        private void UpdateFrequency(StandardItem item)
        {
            using (var db = new RCAEntities())
            {
                var userId = GetUserId();

                var data = (from frq in db.ctpFrequencies
                            where frq.FrequencyId == item.Id
                            select frq).Single();

                data.Frequency = item.Value;
                data.IsDeleted = item.IsDeleted;
                data.ModifiedBy = userId;
                data.ModifiedOn = DateTime.UtcNow;

                db.SaveChanges();
            }
        }

        private void UpdateLevelOfAutomation(ICRAItem item)
        {
            using (var db = new RCAEntities())
            {
                var userId = GetUserId();

                var data = (from loa in db.ctpLevelOfAutomations
                            where loa.LevelOfAutomationId == item.Id
                            select loa).Single();

                data.LevelOfAutomation = item.Value;
                data.IsDeleted = item.IsDeleted;
                data.ModifiedBy = userId;
                data.ModifiedOn = DateTime.UtcNow;

                db.SaveChanges();
            }
        }

        private void UpdateLineOfDefense(StandardItem item)
        {
            using (var db = new RCAEntities())
            {
                var userId = GetUserId();

                var data = (from lod in db.ctpLineOfDefenses
                            where lod.LineOfDefenseId == item.Id
                            select lod).Single();

                data.LineOfDefense = item.Value;
                data.IsDeleted = item.IsDeleted;
                data.ModifiedBy = userId;
                data.ModifiedOn = DateTime.UtcNow;

                db.SaveChanges();
            }
        }

        private void UpdateObjectiveCategory(StandardItem item)
        {
            using (var db = new RCAEntities())
            {
                var userId = GetUserId();

                var data = (from obc in db.ctpObjectiveCategories
                            where obc.ObjectiveCategoryId == item.Id
                            select obc).Single();

                data.ObjectiveCategory = item.Value;
                data.IsDeleted = item.IsDeleted;
                data.ModifiedBy = userId;
                data.ModifiedOn = DateTime.UtcNow;

                db.SaveChanges();
            }
        }

        private void UpdateReferenceFramework(ICRAItem item)
        {
            using (var db = new RCAEntities())
            {
                var userId = GetUserId();

                var data = (from rfw in db.ctpReferenceFrameworks
                            where rfw.ReferenceFrameworkId == item.Id
                            select rfw).Single();

                data.ReferenceFramework = item.Value;
                data.IsDeleted = item.IsDeleted;
                data.ModifiedBy = userId;
                data.ModifiedOn = DateTime.UtcNow;

                db.SaveChanges();
            }
        }

        private void UpdateReferenceWorkstream(StandardItem item)
        {
            using (var db = new RCAEntities())
            {
                var userId = GetUserId();

                var data = (from rws in db.ctpReferenceWorkstreams
                            where rws.ReferenceWorkstreamId == item.Id
                            select rws).Single();

                data.ReferenceWorkstream = item.Value;
                data.IsDeleted = item.IsDeleted;
                data.ModifiedBy = userId;
                data.ModifiedOn = DateTime.UtcNow;

                db.SaveChanges();
            }
        }

        private void UpdateScopeRegion(StandardItem item)
        {
            using (var db = new RCAEntities())
            {
                var userId = GetUserId();

                var data = (from scr in db.ctpScopeRegions
                            where scr.ScopeRegionId == item.Id
                            select scr).Single();

                data.ScopeRegion = item.Value;
                data.IsDeleted = item.IsDeleted;
                data.ModifiedBy = userId;
                data.ModifiedOn = DateTime.UtcNow;

                db.SaveChanges();
            }
        }

        private void UpdateTestingApproach(TestingApproachItem item)
        {
            using (var db = new RCAEntities())
            {
                var userId = GetUserId();

                var data = (from tea in db.ctpTestingApproaches
                            where tea.TestingApproachId == item.Id
                            select tea).Single();

                data.TestingApproach = item.Value;
                data.OET = item.OET;
                data.DET = item.DET;
                data.IsDeleted = item.IsDeleted;
                data.ModifiedBy = userId;
                data.ModifiedOn = DateTime.UtcNow;

                db.SaveChanges();
            }
        }

        private void UpdateTestingPhase(StandardItem item)
        {
            using (var db = new RCAEntities())
            {
                var userId = GetUserId();

                var data = (from tep in db.ctpTestingPhases
                            where tep.TestingPhaseId == item.Id
                            select tep).Single();

                data.TestingPhase = item.Value;
                data.IsDeleted = item.IsDeleted;
                data.ModifiedBy = userId;
                data.ModifiedOn = DateTime.UtcNow;

                db.SaveChanges();
            }
        }

        private void UpdateTradeLifecycle(StandardItem item)
        {
            using (var db = new RCAEntities())
            {
                var userId = GetUserId();

                var data = (from tlc in db.ctpTradeLifecycles
                            where tlc.TradeLifecycleId == item.Id
                            select tlc).Single();

                data.TradeLifecycle = item.Value;
                data.IsDeleted = item.IsDeleted;
                data.ModifiedBy = userId;
                data.ModifiedOn = DateTime.UtcNow;

                db.SaveChanges();
            }
        }
        #endregion

        private void DeleteICRA(int question, string val)
        {
            using (var db = new RCAEntities())
            {
                var data = from ics in db.ctpICRAScores
                           where ics.Question == question && ics.Answer == val
                           select ics;

                foreach (var t in data)
                {
                    db.ctpICRAScores.Remove(t);
                }

                db.SaveChanges();
            }
        }

        private int GetUserId()
        {
            return 1;
        }

        private List<TableDef> GetTableDefs()
        {
            var tableDefs = new List<TableDef>
            {
                new TableDef("Allocation Type", true),
                new TableDef("Control Type", false),
                new TableDef("Frequency", true),
                new TableDef("Level of Automation", false),
                new TableDef("Line of Defense", true),
                new TableDef("Objective Category", true),
                new TableDef("Reference Framework", false),
                new TableDef("Reference Workstream", true),
                new TableDef("Scope Region", true),
                new TableDef("Testing Approach", false),
                new TableDef("Testing Phase", true),
                new TableDef("Trade Lifecycle", true)
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

        private class ICRAItem
        {
            public string Table { get; set; }
            public int Id { get; set; }
            public string Value { get; set; }
            public int ICRA { get; set; }
            public bool IsDeleted { get; set; }
        }

        private class TestingApproachItem
        {
            public int Id { get; set; }
            public string Value { get; set; }
            public bool OET { get; set; }
            public bool DET { get; set; }
            public bool IsDeleted { get; set; }
        }
    }
}