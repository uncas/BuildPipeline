﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Uncas.BuildPipeline.Web.ViewModels.EnvironmentIndexViewModel>>" %>
<%@ Import Namespace="Uncas.BuildPipeline.Web.ViewModels" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Environments
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table>
        <tr>
            <th>
                Environment
            </th>
            <th>
                Current revision
            </th>
        </tr>
        <% foreach (EnvironmentIndexViewModel item in Model)
           { %>
        <tr>
            <td>
                <%:item.EnvironmentName%>
            </td>
            <td>
                <%:item.CurrentRevision%>
            </td>
        </tr>
        <% } %>
    </table>
</asp:Content>