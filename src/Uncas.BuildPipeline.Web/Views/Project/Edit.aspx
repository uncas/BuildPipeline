<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Uncas.BuildPipeline.Models.ProjectReadModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Edit project
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Edit project '<%: Model.ProjectName %>'</h2>

    <script src="<%: Url.Content("~/Scripts/jquery.validate.min.js") %>" type="text/javascript"> </script>
    <script src="<%: Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js") %>" type="text/javascript"> </script>

    <% using (Html.BeginForm())
       {%>
        <%: Html.ValidationSummary(true) %>
        <fieldset>
            <%: Html.HiddenFor(model => model.ProjectId) %>

            <div class="editor-label">
                <%: Html.LabelFor(model => model.GitRemoteUrl) %>
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.GitRemoteUrl) %>
                <%: Html.ValidationMessageFor(model => model.GitRemoteUrl) %>
            </div>

            <div class="editor-label">
                <%: Html.LabelFor(model => model.GithubUrl) %>
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.GithubUrl) %>
                <%: Html.ValidationMessageFor(model => model.GithubUrl) %>
            </div>

            <div class="editor-label">
                <%: Html.LabelFor(model => model.DeploymentScript) %>
            </div>
            <div class="editor-field">
                <%: Html.TextAreaFor(model => model.DeploymentScript, new {rows = 8, cols = 80}) %>
                <%: Html.ValidationMessageFor(model => model.DeploymentScript) %>
            </div>

            <p>
                <input type="submit" value="Save" />
            </p>
        </fieldset>
    <% } %>

    <div>
        <%: Html.ActionLink("Back to List", "Index") %>
    </div>

</asp:Content>