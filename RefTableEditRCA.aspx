<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="RefTableEditRCA.aspx.cs" Inherits="Lab.RefTableEditRCA" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <style>
        body {
            font-size: 12px;
        }
        input {
            font-size: 12px !important;
        }
        .table > tbody > tr > td {
            vertical-align: middle;
        }
        .pointer {
            cursor: pointer;
        }
        .center {
            text-align: center;
        }
        .text-verymuted {
            color: #e0e0e0;
        }
        .item-deleted {
            border-color: red;
        }
    </style>

    <link rel="stylesheet" href="Content/bootstrap-select.css" />
    <script src="Scripts/umd/popper.js"></script>
    <script src="Scripts/bootstrap-select.js"></script>
    
    <script>
        function fixedEncodeURIComponent (str) {
            return encodeURIComponent(str).replace(/[!'()]/g, escape).replace(/\*/g, "%2A");
        } 
        function postback(ev, val) {
            $('#MainContent_Event__').val(ev);
            $('#MainContent_Value__').val(val);
            document.forms[0].submit();
        }
        function saveStandard(e) {
            var data = {
                'table': $("#MainContent_Tables").val(),
                'id': $(e).closest('tr').data('id'),
                'value': fixedEncodeURIComponent(jQuery.trim($(e).closest('tr').find('.val').val())),
                'isDeleted': $(e).hasClass('glyphicon-minus')
            };
            postback('SaveStandard', JSON.stringify(data));
        }
        function saveTestingApproach(e) {
            var data = {
                'id': $(e).closest('tr').data('id'),
                'value': fixedEncodeURIComponent(jQuery.trim($(e).closest('tr').find('.val').val())),
                'oet': $(e).closest('tr').find('.oet').hasClass('text-success'),
                'det': $(e).closest('tr').find('.det').hasClass('text-success'),
                'isDeleted': $(e).hasClass('glyphicon-minus')
            };
            postback('SaveTestingApproach', JSON.stringify(data));
        }
        function toggleApproach(e) {
            var oth = $(e).hasClass('oet') ? $(e).closest('tr').find('.det') : $(e).closest('tr').find('.oet');
            if ($(e).hasClass('text-success')) {
                $(e).removeClass('text-success').removeClass('text-verymuted').addClass('text-verymuted');
                $(oth).removeClass('text-success').removeClass('text-verymuted').addClass('text-success');
            }
            else {
                $(e).removeClass('text-success').removeClass('text-verymuted').addClass('text-success');
                $(oth).removeClass('text-success').removeClass('text-verymuted').addClass('text-verymuted');
            }
        }
        function saveICRA(e) {
            var icra = fixedEncodeURIComponent(jQuery.trim($(e).closest('tr').find('.icra').val()));
            if (!checkICRA(icra) || icra.length === 0)
                icra = -1;
            var data = {
                'table': $("#MainContent_Tables").val(),
                'id': $(e).closest('tr').data('id'),
                'value': fixedEncodeURIComponent(jQuery.trim($(e).closest('tr').find('.val').val())),
                'icra': icra,
                'isDeleted': $(e).hasClass('glyphicon-minus')
            };
            postback('SaveICRA', JSON.stringify(data));
        }
        function checkICRA(icra) {
            if (icra.length === 0)
                return true;
            return ($.isNumeric(icra) && Math.floor(icra) == icra);
        }
    </script>
    
    <table class="table table-condensed table-nonfluid">
        <tbody>
            <tr>
                <td><select id="Tables" onchange="postback('Tables_Changed','')" runat="server"></select></td>
            </tr>
        </tbody>
    </table>

    <table class="table table-condensed table-nonfluid">
        <thead>
            <asp:Literal ID="TableHead" runat="server" />
        </thead>
        <tbody>
            <asp:Literal ID="TableBody" runat="server" />    
        </tbody>
    </table>
    
    <asp:HiddenField id="Event__" runat="server"/>
    <asp:HiddenField id="Value__" runat="server"/>

</asp:Content>
