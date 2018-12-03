<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="RefTableEditOME.aspx.cs" Inherits="Lab.RefTableEditOME" %>

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
        function saveOther(e) {
            var data = {
                'database': $("#MainContent_Databases").val(),
                'table': $("#MainContent_Tables").val(),
                'id': $(e).closest('tr').data('id'),
                'value': fixedEncodeURIComponent(jQuery.trim($(e).closest('tr').find('.val').val())),
                'other': fixedEncodeURIComponent(jQuery.trim($(e).closest('tr').find('.other').val())),
                'isDeleted': $(e).hasClass('glyphicon-minus')
            };
            postback('SaveOther', JSON.stringify(data));
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